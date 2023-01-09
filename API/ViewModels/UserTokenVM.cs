namespace API.ViewModels
{
    public class UserTokenVM
    {
        public string Token { get; set; }
        public string NIK { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool DefaultPassword { get; set; }
    }
}
