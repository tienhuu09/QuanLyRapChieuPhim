using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class Order
    {
        public string Id { get; set; }
        public string IdScheduleShowTime { get; set; }
        public string CinemaType { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public double Total { get; set; }

        public DateTime Date { get; set; }
        public ScheduleShowTime ScheduleST { get; set; }
        public List<DetailOrder> lstDetailOrder { get; set; }

        public Order()
        {
            Date = new DateTime();
            lstDetailOrder = new List<DetailOrder>();
        }

        public Order(string id, string idScheduleST, string cinemaType, string customer, string phone, double total, DateTime date, ScheduleShowTime schedule)
        {
            this.Id = id;
            this.IdScheduleShowTime = idScheduleST;
            this.CinemaType = cinemaType;
            this.CustomerName = customer;
            this.PhoneNumber = phone;
            this.Total = total;
            this.Date = date;
            this.ScheduleST = schedule;
            this.lstDetailOrder = new List<DetailOrder>();
        }

        public double getTotal()
        {
            return this.Total;
        }
    }
}
