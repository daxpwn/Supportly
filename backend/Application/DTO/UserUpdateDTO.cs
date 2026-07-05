namespace Application.DTO
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}
