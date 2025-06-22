using System.ComponentModel.DataAnnotations.Schema;

namespace Abstraction.Entities;

public abstract class BaseEntity
    : IBaseEntity
{
    protected BaseEntity()
    {
        this.Id = 0;
    }

    protected BaseEntity(int id)
    {
        this.Id = id;
    }

    [Column("id")]
    public int Id { get; set; }
}
