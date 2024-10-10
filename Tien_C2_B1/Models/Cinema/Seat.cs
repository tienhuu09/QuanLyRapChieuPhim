using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class Seat : IComparable<Seat>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public Seat() { }

        public int CompareTo(Seat other)
        {
            throw new NotImplementedException();
        }
    }
}
