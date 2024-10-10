using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class BoughtSeat
    {
        public string SeatId { get; set; }
        public string Type { get; set; }
        public Seat Seat { get; set; }

        public BoughtSeat()
        {
            Seat = new Seat();
        }

        public BoughtSeat(string seatNo)
        {
            this.SeatId = seatNo;
        }

        public BoughtSeat(Seat seat, string seatNo)
        {
            this.Seat = seat;
            this.SeatId = seatNo;
        }

    }
}
