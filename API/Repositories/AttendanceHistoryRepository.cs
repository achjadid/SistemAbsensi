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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Repositories
{
    public class AttendanceHistoryRepository
    {
        public IConfiguration _configuration;
        private readonly MyContext context;
        public AttendanceHistoryRepository(IConfiguration configuration, MyContext context)
        {
            _configuration = configuration;
            this.context = context;
        }

        DynamicParameters parameters = new DynamicParameters();

        public virtual AttendanceHistoryVM ScanQR(string NIK)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string dateToday = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                var spName = "SP_AttendanceHistoriesScanQR";
                parameters.Add("@NIK", NIK);
                parameters.Add("@TimeScan", time);
                parameters.Add("@DateToday", dateToday);
                var res = connection.QuerySingleOrDefault<AttendanceHistoryVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        public virtual IEnumerable<AttendanceHistoryVM> GetByNIK(string NIK)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_AttendanceHistoriesGetByNIK";
                parameters.Add("@NIK", NIK);
                var res = connection.Query<AttendanceHistoryVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                foreach(AttendanceHistoryVM item in res)
                {
                    var recordDateTime = item.Date;
                    string recordDate = recordDateTime.ToString("yyyy/MM/dd");
                    DateTime checkInUser = item.CheckIn.HasValue ? DateTime.Parse(item.CheckIn.ToString()) : recordDateTime.Date.AddHours(10).AddMinutes(0).AddSeconds(1);
                    DateTime checkInTime = DateTime.Parse(recordDate + " 10:00:00");

                    DateTime checkOutUser = item.CheckOut.HasValue ? DateTime.Parse(item.CheckOut.ToString()) : recordDateTime.Date.AddHours(14).AddMinutes(59).AddSeconds(59);
                    DateTime checkOutTime = DateTime.Parse(recordDate + " 15:00:00");

                    if (checkInUser.TimeOfDay > checkInTime.TimeOfDay || checkOutUser.TimeOfDay < checkOutTime.TimeOfDay)
                    {
                        item.LateStatus = 1;
                    }
                    else
                    {
                        item.LateStatus = 0;
                    }
                }
                return res;
            }
        }

        public virtual IEnumerable<AttendanceHistoryVM> GetRevise(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                var spName = "SP_AttendanceHistoriesGetRevise";
                parameters.Add("@DepartmentId", Id);
                var res = connection.Query<AttendanceHistoryVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                foreach (AttendanceHistoryVM item in res)
                {
                    var recordDateTime = item.Date;
                    string recordDate = recordDateTime.ToString("yyyy/MM/dd");
                    DateTime checkInUser = item.CheckIn.HasValue ? DateTime.Parse(item.CheckIn.ToString()) : recordDateTime.Date.AddHours(10).AddMinutes(0).AddSeconds(1);
                    DateTime checkInTime = DateTime.Parse(recordDate + " 10:00:00");

                    DateTime checkOutUser = item.CheckOut.HasValue ? DateTime.Parse(item.CheckOut.ToString()) : recordDateTime.Date.AddHours(14).AddMinutes(59).AddSeconds(59);
                    DateTime checkOutTime = DateTime.Parse(recordDate + " 15:00:00");

                    if (checkInUser.TimeOfDay > checkInTime.TimeOfDay || checkOutUser.TimeOfDay < checkOutTime.TimeOfDay)
                    {
                        item.LateStatus = 1;
                    }
                    else
                    {
                        item.LateStatus = 0;
                    }
                }
                return res;
            }
        }

        public virtual int Revise(AttendanceHistory attendanceHistory)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var spName = "SP_AttendanceHistoriesRevise";
                parameters.Add("@Id", attendanceHistory.Id);
                parameters.Add("@CheckIn", attendanceHistory.CheckIn);
                parameters.Add("@CheckOut", attendanceHistory.CheckOut);
                parameters.Add("@Reason", attendanceHistory.Reason);
                parameters.Add("@ReviseDate", time);
                var revise = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
                return revise;
            }
        }

        public virtual int ResponseRevise(AttendanceHistory attendanceHistory)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:APISistemAbsensi"]))
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (!(attendanceHistory.ResponseStatus == ResponseStatus.Approve || attendanceHistory.ResponseStatus == ResponseStatus.Reject))
                {
                    return 0;
                }

                var spName = "SP_AttendanceHistoriesResponse";
                parameters.Add("@Id", attendanceHistory.Id);
                parameters.Add("@ResponseStatus", attendanceHistory.ResponseStatus);
                parameters.Add("@ResponseDate", time);
                var response = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }
    }
}
