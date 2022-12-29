using API.Contexts;
using API.Models;
using API.Repositories.Interface;
using API.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

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
                string password = "jadid1234";
                string passwordHash = BCrypt.Net.BCrypt.HashPassword("./][/'" + password + "[><]");
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

        public virtual int InsertUserEmployee(UserEmployeeVM userEmployeeVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(userEmployeeVM.Password);
                var spName = "SP_UsersEmployeeInsert";
                parameters.Add("@NIK", userEmployeeVM.NIK);
                parameters.Add("@Username", userEmployeeVM.Username);
                parameters.Add("@Password", passwordHash);
                parameters.Add("@RoleId", userEmployeeVM.RoleId);
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
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(userEmployeeVM.Password);
                var spName = "SP_UsersEmployeeUpdate";
                parameters.Add("@NIK", userEmployeeVM.NIK);
                parameters.Add("@Username", userEmployeeVM.Username);
                parameters.Add("@Password", passwordHash);
                parameters.Add("@RoleId", userEmployeeVM.RoleId);
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
    }
}
