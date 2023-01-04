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
using System.Xml.Linq;

namespace API.Repositories
{
    public class DepartmentRepository : IRepository<Department, int>
    {
        public IConfiguration _configuration;
        private readonly MyContext context;
        public DepartmentRepository(IConfiguration configuration, MyContext context)
        {
            _configuration = configuration;
            this.context = context;
        }

        DynamicParameters parameters = new DynamicParameters();

        public IEnumerable<Department> Get()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_DepartmentsGetAll";
                var res = connection.Query<Department>(spName, commandType: CommandType.StoredProcedure);
                return res;
                //throw new System.NotImplementedException();
            }
        }

        public IEnumerable<DepartmentVM> GetDetailDepartment()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_DepartmentsGetAll";
                var res = connection.Query<DepartmentVM>(spName, commandType: CommandType.StoredProcedure);
                return res;
                //throw new System.NotImplementedException();
            }
        }

        //public virtual IEnumerable<DepartmentVM> GetByRole(int Id)
        //{
        //    using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
        //    {
        //        var spName = "SP_DepartmentsGetAll";
        //        parameters.Add("@RoleId", Id);
        //        var res = connection.Query<DepartmentVM>(spName, parameters, commandType: CommandType.StoredProcedure);
        //        return res;
        //    }
        //}

        public Department Get(int key)
        {
            throw new System.NotImplementedException();
        }

        public int Insert(Department department)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_DepartmentsInsert";
                parameters.Add("@DepartmentName", department.DepartmentName);
                parameters.Add("@SupervisorNIK", department.SupervisorNIK);
                var insert = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
                return insert;
            }
        }

        public int Update(Department department)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_DepartmentsUpdate";
                parameters.Add("@Id", department.Id);
                parameters.Add("@DepartmentName", department.DepartmentName);
                parameters.Add("@SupervisorNIK", department.SupervisorNIK);
                var update = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
                return update;
            }
        }

        public int Delete(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}
