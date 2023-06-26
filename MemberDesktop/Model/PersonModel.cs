using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberDesktop.Model
{
    public class PersonModel
    {
        public int Id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string Name { get {
                return first_name + " " + last_name;
            }
            
            set { } }


    }
}
