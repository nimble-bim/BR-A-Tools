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
        public string Name { get; protected set; }
        public string ComputerName { get; protected set; }
        public string ModelAccesed { get; set; }

        public User() { }

        public User(string name)
        {
            Name = name;
        }

        public User(string userName, string compName) : this(userName)
        {
            ComputerName = compName;
        }

    }

    public class CurrentUser : User
    {
        public CurrentUser()
        {
            Name = Environment.UserName;
            ComputerName = Environment.MachineName;
        }
    }
}
