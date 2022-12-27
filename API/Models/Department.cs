using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        [ForeignKey("EmployeeSupervisor")]
        public string SupervisorNIK { get; set; }
        public virtual Employee EmployeeSupervisor { get; set; }
        public virtual ICollection<Employee> EmployeeDepartments { get; set; }
    }
}
