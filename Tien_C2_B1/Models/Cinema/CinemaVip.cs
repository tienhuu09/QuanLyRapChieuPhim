using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class CinemaVip : Cinema, IDiscountThursDay, IServiceCharge
    {
        public CinemaVip() : base()
        {
            //this.PriceCenter = 100000;
        }

        public CinemaVip(string id, string name, string description) : base(id, name, description)
        {
            //this.PriceCenter = 100000;
        }

        public override string getType()
        {
            return "Vip";
        }

        public double Discount(int day, int month, int year)
        {
            DateTime date = new DateTime(year, month, day);
            DayOfWeek dwk = date.DayOfWeek;
            if (string.Compare(dwk.ToString(), "ThursDay", true) == 0)
                return (this.PriceCenter + ServiceChange()) * 0.1;
            return 0;
        }

        public override double getDiscount(string age, int day, int month, int year)
        {
            return Discount(day, month, year);
        }

        public double ServiceChange()
        {
            return 40000;
        }

        public override double getPrice()
        {
            return (this.PriceCenter + ServiceChange());
        }
    }
}
