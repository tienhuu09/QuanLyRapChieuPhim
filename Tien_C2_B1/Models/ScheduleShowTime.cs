using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class ScheduleShowTime
    {
        public string Id { get; set; }
        public string IdSchedule { get; set; }
        public bool Status { get; set; }

        public Schedule Schedule { get; set; }
        public DateTime AirDate { get; set; }
        public List<BoughtSeat> lstBoughtSeat { get; set; }

        public ScheduleShowTime()
        {
            Schedule = new Schedule();
            lstBoughtSeat = new List<BoughtSeat>();
            //AirDate = new DateTime();
            this.Status = true;
        }
    }
}
