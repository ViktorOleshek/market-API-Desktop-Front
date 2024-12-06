namespace Abstraction.IEntities
{
    public interface IUser : IBaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public int PersonId { get; set; }

        public IPerson Person { get; set; }
    }
}
