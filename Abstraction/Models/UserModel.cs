namespace Abstraction.Models
{
    public class UserModel : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int PersonId { get; set; }
    }
}