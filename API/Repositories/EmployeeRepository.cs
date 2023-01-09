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
    public class EmployeeRepository : IRepository<Employee, string>
    {
        public IConfiguration _configuration;
        private readonly MyContext context;
        public EmployeeRepository(IConfiguration configuration, MyContext context)
        {
            _configuration = configuration;
            this.context = context;
        }

        DynamicParameters parameters = new DynamicParameters();

        public IEnumerable<Employee> Get()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_EmployeesGetAll";
                var res = connection.Query<Employee>(spName, parameters, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        public IEnumerable<Employee> GetByRole(int role)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_EmployeesGetAll";
                parameters.Add("@RoleId", role);
                var res = connection.Query<Employee>(spName, parameters, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        public Employee Get(string key)
        {
            throw new System.NotImplementedException();
        }

        public int Insert(Employee entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var procName = "SP_EmployeesUpdate";
                parameters.Add("@NIK", employee.NIK);
                parameters.Add("@Name", employee.Name);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@BirthDate", employee.BirthDate);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@Phone", employee.Phone);
                parameters.Add("@Address", employee.Address);
                parameters.Add("@UpdatedAt", time);
                var update = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return update;
            }
        }

        public int Delete(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
