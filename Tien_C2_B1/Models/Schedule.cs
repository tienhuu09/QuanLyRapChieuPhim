using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class Schedule
    {
        public string Id { get; set; }
        public string IdMovie { get; set; }
        public string IdCinema { get; set; }
        public bool Status { get; set; }
        public DateTime AirDate { get; set; }

        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }

        public Schedule()
        {
            Movie = new Movie();
            Cinema = new Cinema();
        }

        public Schedule(string id, Movie movieTemp, Cinema cinemaTemp)
        {
            this.Id = id;
            this.IdMovie = movieTemp.Id;
            this.IdCinema = cinemaTemp.Id;
            this.Movie = movieTemp;
            this.Cinema = cinemaTemp;
            this.Status = true;
        }
    }
}
