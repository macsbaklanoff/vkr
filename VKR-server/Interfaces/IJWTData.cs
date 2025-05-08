namespace VKR_server.Interfaces
{
    public interface IJWTData
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }

        public string GroupName {  get; set; }
    }
}
