using API.Models;
using System;

namespace API.ViewModels
{
    public class AttendanceHistoryVM
    {
        public int Id { get; set; }
        public string NIK { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Reason { get; set; }
        public DateTime? ResponseDate { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int? LateStatus { get; set; }
        public DateTime? ReviseCheckIn { get; set; }
        public DateTime? ReviseCheckOut { get; set; }
        public DateTime? ReviseDate { get; set; }
    }
}
