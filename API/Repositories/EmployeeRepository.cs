using API.Contexts;
using API.Models;
using API.Repositories.Interface;
using API.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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

        public int Update(Employee entity)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
