using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Entities
{
    public abstract class EntityBase<T>
    {
        public virtual T Id { get; init; }

        public DateTime CreateAt { get; init; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; } = DateTime.Now;

        public bool IsPublish { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public long? CreateBy { get; set; }


        public virtual T GenerateId()
        {
            return default;
        }
    }
}
