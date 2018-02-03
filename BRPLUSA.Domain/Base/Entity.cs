using BRPLUSA.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Domain.Base
{
    public class Entity : IEntity
    {
        public virtual string InternalId { get;  set; }
    }
}
