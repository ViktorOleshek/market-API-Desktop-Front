using System.ComponentModel.DataAnnotations.Schema;
using Abstraction.IEntities;

namespace Data.Entities
{
    [Table("User")]
    public class User : BaseEntity, IUser
    {
        public User()
            : base()
        {
            this.Person = new Person();
        }

        public User(int id)
            : base(id)
        {
            this.Person = new Person();
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public int PersonId { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        IPerson IUser.Person
        {
            get => this.Person;
            set => this.Person = (Person)value;
        }
    }
}
