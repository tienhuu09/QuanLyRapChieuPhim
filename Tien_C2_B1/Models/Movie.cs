using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class Movie
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public bool Status { get; set; }

        public DateTime StartAirDate { get; set; }
        public DateTime EndAirDate { get; set; }

        public Movie()
        {
            Status = true;
        }

    }
}
