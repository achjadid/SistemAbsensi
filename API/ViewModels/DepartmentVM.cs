using System.ComponentModel.DataAnnotations.Schema;

namespace API.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string SupervisorNIK { get; set; }
        public string SupervisorName { get; set; }
    }
}
