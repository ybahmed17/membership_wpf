using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace MemberDesktop.Model
{
    public class MembershipRenewal
    {
        public int MemberID { get; set; }

        public int FamilyID { get; set; }

        public short Year { get; set; }

        public DateTime ApplicationDate { get; set; }

        public ushort RenewedBy { get; set; }

        public string Comment { get; set; }
    }

}
