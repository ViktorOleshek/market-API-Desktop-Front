using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abstraction.IEntities;

namespace Data.Entities
{
    public abstract class BaseEntity : IBaseEntity
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
}
