using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class Cinema: IComparable<Cinema>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double PriceCenter { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public List<Seat> lstSeat { get; set; }

        public Cinema()
        {
            this.Status = true;
            lstSeat = new List<Seat>();
        }

        public Cinema(string id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Status = true;
            lstSeat = new List<Seat>();
        }

        public virtual string getType()
        {
            return "Standard";
        }

        public virtual double getPrice()
        {
            return 0;
        }

        public virtual double getDiscount(string age, int day, int month, int year)
        {
            return 0;
        }

        public int CompareTo(Cinema other)
        {
            return this.Id.CompareTo(other.Id);
        }
    }
}
