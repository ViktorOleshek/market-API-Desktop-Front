using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public abstract class BaseEntity
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
