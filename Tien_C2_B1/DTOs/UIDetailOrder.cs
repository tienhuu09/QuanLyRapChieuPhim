using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class UIDetailOrder : IMapFrom<DetailOrder, UIDetailOrder>
    {
        public string IdOrder { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Movie { get; set; }
        public string Cinema { get; set; }
        public string Age { get; set; }
        public string Type { get; set; }
        public string SeatName { get; set; }
        public string Price { get; set; }
        public string Discount { get; set; }
        public string Total { get; set; }
        public string DateBook { get; set; }
        public string ShowTime { get; set; }

        public UIDetailOrder MapFrom(DetailOrder source)
        {
            this.Age = source.Age;
            this.SeatName = source.SeatName;
            this.Price = source.Price.ToString(Ulti.spec);
            this.Discount = source.Discount.ToString(Ulti.spec);
            this.Total = (source.Price - source.Discount).ToString(Ulti.spec);

            return this;
        }
    }
}
