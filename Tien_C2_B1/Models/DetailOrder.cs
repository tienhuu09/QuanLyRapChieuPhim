using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class DetailOrder
    {
        public string Age { get; set; }
        public string SeatName { get; set; }
        public string SeatId { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public Seat Seat { get; set; }

        public DetailOrder() { }

        public DetailOrder(string age, string seatNo, double price, double discount)
        {
            this.Age = age;
            this.SeatId = seatNo;
            this.Price = price;
            this.Discount = discount;
        }
    }
}
