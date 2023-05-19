namespace DiplomWeb_Service.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Group Group { get; set; }
    }
}
