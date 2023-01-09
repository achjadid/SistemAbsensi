using API.Contexts;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using System.Text.RegularExpressions;

namespace API.Repositories
{
    public class LoginRepository
    {
        public IConfiguration _configuration;
        private readonly MyContext context;
        public LoginRepository(IConfiguration configuration, MyContext context)
        {
            _configuration = configuration;
            this.context = context;
        }

        DynamicParameters parameters = new DynamicParameters();

        public UserTokenVM Login(LoginVM loginVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spCheckPassword = "SP_UsersCheckPassword";
                parameters.Add("@username", loginVM.Username);
                var userPassword = connection.QuerySingleOrDefault<User>(spCheckPassword, parameters, commandType: CommandType.StoredProcedure);
                if (userPassword == null)
                {
                    return null;
                }

                //string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginVM.Password);
                bool verified = BCrypt.Net.BCrypt.Verify(loginVM.Password, userPassword.Password);
                if (!verified)
                {
                    return null;
                }

                parameters = new DynamicParameters();
                var spUserToken = "SP_UsersGetLoginTokenData";
                parameters.Add("@NIK", userPassword.NIK);
                var userToken = connection.QuerySingleOrDefault<UserTokenVM>(spUserToken, parameters, commandType: CommandType.StoredProcedure);
                if (userPassword == null)
                {
                    return null;
                }

                string token = GenerateJwtToken(userToken);
                userToken.Token = token;

                string defaultPassword = GeneratePassword(loginVM.Username);
                if (loginVM.Password == defaultPassword)
                {
                    userToken.DefaultPassword = true;
                }

                return userToken;
            }
        }

        private string GenerateJwtToken(UserTokenVM userToken)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Sub, userToken.Username),
                //new Claim(ClaimTypes.NameIdentifier, userToken.NIK),
                //new Claim(ClaimTypes.Name, userToken.Name),
                new Claim("NIK", userToken.NIK),
                new Claim("Username", userToken.Username),
                new Claim("Name", userToken.Name),
                new Claim("RoleId", userToken.RoleId.ToString()),
                //new Claim(ClaimTypes.Role, userToken.RoleName),
                new Claim("RoleName", userToken.RoleName),
                new Claim("DepartmentId", userToken.DepartmentId.ToString()),
                new Claim("DepartmentName", userToken.DepartmentName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GeneratePassword(string username)
        {
            string generatedPassword = "";
            string number = "654321";
            Match match = Regex.Match(username, @"\d+");
            if (match.Success)
            {
                number = match.Value.ToString() + number;
            }

            string[] splittedName = username.Split('.');
            if (splittedName.Length > 1)
            {
                generatedPassword = splittedName[0].Substring(0, 1)[0].ToString().ToUpper() + splittedName[1].Substring(0, 1)[0].ToString().ToUpper() + number;
            }
            else if (splittedName.Length == 1)
            {
                generatedPassword = splittedName[0].Substring(0, 1)[0].ToString().ToUpper() + number;
            }

            return generatedPassword;
        }
    }
}
