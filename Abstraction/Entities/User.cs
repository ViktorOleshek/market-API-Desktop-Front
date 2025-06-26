using System.ComponentModel.DataAnnotations.Schema;

namespace Abstraction.Entities;

[Table("User")]
public class User
    : BaseEntity
{
    public User()
        : base()
    {
    }

    public User(int id)
        : base(id)
    {
    }

    public string Username { get; set; }

    public string? Password { get; set; }
    public string Email { get; set; }

    public string Role { get; set; }

    public int PersonId { get; set; }

    [ForeignKey("PersonId")]
    public virtual Person Person { get; set; }
}
