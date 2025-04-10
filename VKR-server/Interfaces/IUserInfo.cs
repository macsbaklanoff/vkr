namespace VKR_server.Interfaces
{
    public interface IUserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId {  get; set; }
    }
}
