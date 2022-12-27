using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [ForeignKey("Departments")]
        public int DepartmentId { get; set; }
        public virtual User User { get; set; }
        public virtual Department DepartmentSupervisor { get; set; }
        public virtual Department Departments { get; set; }
        public virtual ICollection<AttendanceHistory> AttendanceHistories { get; set; }
    }

    public enum Gender { Male, Female }
}
