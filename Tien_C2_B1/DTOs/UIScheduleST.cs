using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class UIScheduleST : IMapFrom<ScheduleShowTime, UIScheduleST>
    {
        public string Id { get; set; }
        public string NameMovie { get; set; }
        public string NameCinema { get; set; }
        public DateTime AirDate { get; set; }
        public string SeatActive { get; set; }
        public double Price { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }

        public UIScheduleST MapFrom(ScheduleShowTime source)
        {
            this.Id = source.Id;
            this.NameMovie = source.Schedule.Movie.Name;
            this.NameCinema = source.Schedule.Cinema.Name;
            this.AirDate = source.AirDate;
            this.SeatActive = (source.lstBoughtSeat.Count.ToString() + "/" + source.Schedule.Cinema.lstSeat.Count.ToString());
            this.Price = source.Schedule.Cinema.PriceCenter;
            this.Duration = source.Schedule.Movie.Duration;
            if (source.Status)
                this.Status = "Available";
            else
                this.Status = "Unvailable";

            return this;
        }
    }
}
