using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class User
    {
        [Key]
        public string NIK { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Role Role { get; set; }
    }
}
