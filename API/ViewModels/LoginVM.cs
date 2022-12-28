using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.ViewModels
{
    public class LoginVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
