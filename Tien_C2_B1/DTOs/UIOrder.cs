using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class UIOrder : IMapFrom<Order, UIOrder>
    {
        public string Id { get; set; }
        public string IdScheduleST { get; set; }
        public string NameMovie { get; set; }
        public string NameCinema { get; set; }
        public string CinemaType { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Date { get; set; }
        public string Total { get; set; }
        public string Count { get; set; }

        public UIOrder MapFrom(Order item)
        {
            this.Id = item.Id;
            this.IdScheduleST = item.IdScheduleShowTime;
            this.NameMovie = item.ScheduleST.Schedule.Movie.Name;
            this.NameCinema = item.ScheduleST.Schedule.Cinema.Name;
            this.CinemaType = item.CinemaType;
            this.CustomerName = item.CustomerName;
            this.PhoneNumber = item.PhoneNumber;
            this.Date = item.Date.ToString(Ulti.date);
            this.Total = item.Total.ToString(Ulti.spec);
            this.Count = item.lstDetailOrder.Count.ToString();

            return this;
        }
    }
}
