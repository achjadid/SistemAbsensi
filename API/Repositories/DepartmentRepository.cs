﻿using API.Contexts;
using API.Models;
using API.Repositories.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;

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

        public IEnumerable<Department> Get()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_DepartmentsGetAll";
                var res = connection.Query<Department>(spName, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        public Department Get(int key)
        {
            throw new System.NotImplementedException();
        }

        public int Insert(Department entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(Department entity)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}
