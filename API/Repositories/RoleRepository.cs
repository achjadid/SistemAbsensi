using API.Contexts;
using API.Models;
using API.Repositories.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;

namespace API.Repositories
{
    public class RoleRepository : IRepository<Role, int>
    {
        public IConfiguration _configuration;
        private readonly MyContext context;
        public RoleRepository(IConfiguration configuration, MyContext context)
        {
            _configuration = configuration;
            this.context = context;
        }

        public IEnumerable<Role> Get()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_RolesGetAll";
                var res = connection.Query<Role>(spName, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        public Role Get(int key)
        {
            throw new System.NotImplementedException();
        }

        public int Insert(Role entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(Role entity)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}
