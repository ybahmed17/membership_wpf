using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberDesktop.Model
{
    public class Category
    {
        public Category(string v1, string v2)
        {
            Id = v1;
            Name = v2;
        }

        public string Id { get; set; }
        public string Name { get; set; }
  
    }
}
