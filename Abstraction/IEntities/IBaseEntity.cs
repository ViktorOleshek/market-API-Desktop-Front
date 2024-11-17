using Abstraction.IEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.IEntities
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
    }
}