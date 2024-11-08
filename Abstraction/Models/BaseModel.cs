using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public abstract class BaseModel
    {
        protected BaseModel()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModel"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the model.</param>
        protected BaseModel(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the model.
        /// </summary>
        public int Id { get; set; }
    }
}
