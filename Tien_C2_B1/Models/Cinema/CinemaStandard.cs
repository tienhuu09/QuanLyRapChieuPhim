using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class CinemaStandard : Cinema, IDiscountChildren
    {
        public double DoDiscount(string age)
        {
            if (string.Compare(age, "Child", true) == 0)
                return this.PriceCenter * 0.5;
            return 0;
        }

        public override double getDiscount(string age, int day, int month, int year)
        {
            return DoDiscount(age);
        }

        public CinemaStandard() : base()
        {
            //this.PriceCenter = 60000;
        }

        public CinemaStandard(string id, string name, string description) : base(id, name, description)
        {
            //this.PriceCenter = 60000;
        }

        public override string getType()
        {
            return "Standard";
        }

        public override double getPrice()
        {
            return this.PriceCenter;
        }
    }
}
