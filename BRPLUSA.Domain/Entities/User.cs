using BRPLUSA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string ComputerName { get; set; }
        public string ModelAccesed { get; set; }
    }
}
