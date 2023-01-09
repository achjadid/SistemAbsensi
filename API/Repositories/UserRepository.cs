using API.Contexts;
using API.Models;
using API.Repositories.Interface;
using API.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Repositories
{
    public class UserRepository : IRepository<User, string>
    {
        public IConfiguration _configuration;
        private readonly MyContext context;
        public UserRepository(IConfiguration configuration, MyContext context)
        {
            _configuration = configuration;
            this.context = context;
        }

        DynamicParameters parameters = new DynamicParameters();

        public virtual IEnumerable<UserEmployeeVM> GetAllUserEmployee()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_UsersEmployeeGetAll";
                var res = connection.Query<UserEmployeeVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        public virtual IEnumerable<UserEmployeeVM> GetUserEmployeeByDepartment(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_UsersEmployeeGetByDepartment";
                parameters.Add("@DepartmentId", Id);
                var res = connection.Query<UserEmployeeVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        public virtual UserEmployeeVM GetUserEmployeeByNIK(string NIK)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_UsersEmployeeGetByNIK";
                parameters.Add("@NIK", NIK);
                var res = connection.QuerySingleOrDefault<UserEmployeeVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        private int CheckDuplicate(UserEmployeeVM userEmployeeVM, string action)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spCheckNIK = "SP_UsersCheckNIK";
                var spCheckUsername = "SP_UsersCheckUsername";
                if(action == "insert")
                {
                    parameters.Add("@Username", userEmployeeVM.Username);
                    var checkUsername = connection.Query<User>(spCheckUsername, parameters, commandType: CommandType.StoredProcedure);
                    if (checkUsername.Count() >= 1)
                    {
                        return -11;
                    }
                }
                else
                {
                    parameters.Add("@NIK", userEmployeeVM.NIK);
                    var checkNIK = connection.QuerySingleOrDefault<User>(spCheckNIK, parameters, commandType: CommandType.StoredProcedure);

                    parameters = new DynamicParameters();
                    parameters.Add("@Username", userEmployeeVM.Username);
                    var checkUsername = connection.QuerySingleOrDefault<User>(spCheckUsername, parameters, commandType: CommandType.StoredProcedure);
                    if (checkUsername != null && checkUsername.Username != checkNIK.Username)
                    {
                        return -11;
                    }
                }

                return 1;
            }
        }

        public virtual int InsertUserEmployee(UserEmployeeVM userEmployeeVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string generatedNIK = GenerateNIK();
                string generatedUsername = GenerateUsername(userEmployeeVM.Name);
                string generatedPassword = GeneratePassword(generatedUsername);
                if (generatedUsername == "" || generatedPassword == "") return -1;
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(generatedPassword);

                var spName = "SP_UsersEmployeeInsert";
                parameters = new DynamicParameters();
                parameters.Add("@NIK", generatedNIK);
                parameters.Add("@Username", generatedUsername);
                parameters.Add("@Password", passwordHash);
                parameters.Add("@Name", userEmployeeVM.Name);
                parameters.Add("@Email", userEmployeeVM.Email);
                parameters.Add("@BirthDate", userEmployeeVM.BirthDate);
                parameters.Add("@Gender", userEmployeeVM.Gender);
                parameters.Add("@Phone", userEmployeeVM.Phone);
                parameters.Add("@Address", userEmployeeVM.Address);
                parameters.Add("@DepartmentId", userEmployeeVM.DepartmentId);
                parameters.Add("@CreatedAt", time);
                var insert = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
                return insert;
            }
        }

        public virtual int UpdateUserEmployee(UserEmployeeVM userEmployeeVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var spName = "SP_UsersEmployeeUpdate";
                parameters.Add("@NIK", userEmployeeVM.NIK);
                //parameters.Add("@Username", userEmployeeVM.Username);
                //if (userEmployeeVM.Password != null && userEmployeeVM.Password != "")
                //{
                //    string passwordHash = BCrypt.Net.BCrypt.HashPassword(userEmployeeVM.Password);
                //    parameters.Add("@Password", passwordHash);
                //}
                parameters.Add("@Name", userEmployeeVM.Name);
                parameters.Add("@Email", userEmployeeVM.Email);
                parameters.Add("@BirthDate", userEmployeeVM.BirthDate);
                parameters.Add("@Gender", userEmployeeVM.Gender);
                parameters.Add("@Phone", userEmployeeVM.Phone);
                parameters.Add("@Address", userEmployeeVM.Address);
                parameters.Add("@DepartmentId", userEmployeeVM.DepartmentId);
                parameters.Add("@UpdatedAt", time);
                var insert = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
                return insert;
            }
        }

        public int UpdatePassword(User user, bool reset = false)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string password = user.Password;
                if (reset)
                {
                    password = GeneratePassword(user.Username);
                }

                if (password == "") return -1;
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var spName = "SP_UsersUpdatePassword";
                parameters.Add("@NIK", user.NIK);
                parameters.Add("@Password", passwordHash);
                parameters.Add("@UpdatedAt", time);
                var update = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
                return update;
            }
        }

        public IEnumerable<User> Get()
        {
            throw new System.NotImplementedException();
        }

        public User Get(string key)
        {
            throw new System.NotImplementedException();
        }

        public int Insert(User entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(User entity)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(string NIK)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var procName = "SP_UsersEmployeeDelete";
                parameters.Add("@NIK", NIK);
                parameters.Add("@DeletedAt", time);
                var insert = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return insert;
            }
        }

        private string GenerateNIK()
        {
            var lastId = context.Users.FromSqlRaw(
                "SELECT TOP 1 * " +
                "FROM Users " +
                "WHERE len(NIK) = 12 " +
                "ORDER BY RIGHT(NIK, 4) desc"
                ).ToList();
            int highestId = 0;
            if (lastId.Any())
            {
                var newId = lastId[0].NIK;
                newId = newId.Substring(newId.Length - 4);
                highestId = Convert.ToInt32(newId);
            }

            int increamentId = highestId + 1;
            string generatedNIK = increamentId.ToString().PadLeft(4, '0');
            DateTime today = DateTime.Today;
            var dateNow = today.ToString("yyyyddMM");
            generatedNIK = dateNow + generatedNIK;

            return generatedNIK;
        }

        private string GenerateUsername(string name)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string generatedUsername = "";
                string[] splittedName = name.Split(' ');
                if(splittedName.Length > 1)
                {
                    generatedUsername = (splittedName[0] + "." + splittedName[splittedName.Length - 1]).ToLower();

                }
                else if(splittedName.Length == 1)
                {
                    generatedUsername = splittedName[0].ToLower();
                }
                else
                {
                    return generatedUsername;
                }
                var spCheckUsername = "SP_UsersCheckUsername";
                parameters.Add("@Username", generatedUsername);
                parameters.Add("@Generated", 1);
                var checkUsername = connection.Query<User>(spCheckUsername, parameters, commandType: CommandType.StoredProcedure);
                if (checkUsername.Count() > 0)
                {
                    generatedUsername += checkUsername.Count();
                }

                return generatedUsername;
            }
        }

        private string GeneratePassword(string username)
        {
            string generatedPassword = "";
            string number = "654321";
            Match match = Regex.Match(username, @"\d+");
            if(match.Success)
            {
                number = match.Value.ToString() + number;
            }

            string[] splittedName = username.Split('.');
            if (splittedName.Length > 1)
            {
                generatedPassword = splittedName[0].Substring(0, 1)[0].ToString().ToUpper() + splittedName[1].Substring(0, 1)[0].ToString().ToUpper() + number;
            }
            else if(splittedName.Length == 1) 
            {
                generatedPassword = splittedName[0].Substring(0, 1)[0].ToString().ToUpper() + number;
            }

            return generatedPassword;
        }
    }
}
