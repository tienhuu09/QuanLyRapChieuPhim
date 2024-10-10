using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tien_C2_B1
{
    public class CinemaRepository : IRepository<Cinema>, IEnumerable
    {
        public string pathData = "Data/Cinemas.xml";
        public List<Cinema> lstCinema { get; set; }

        public CinemaRepository()
        {
            lstCinema = new List<Cinema>();
            Load();
        }
        
        public void Load()
        {
            DataProvider.Instance.Open(pathData);

            XmlNodeList lstNode = DataProvider.Instance.getDsNode("//Cinema");

            Cinema cinema = null;
            foreach (XmlNode node in lstNode)
            {
                string str = node.Attributes["Type"].Value;
                if (string.Compare("Vip", str, true) == 0)
                    cinema = new CinemaVip();
                else
                    cinema = new CinemaStandard();

                cinema.Id = node.Attributes["IdCinema"].Value;
                cinema.Name = node.Attributes["Name"].Value;
                cinema.Type = node.Attributes["Type"].Value;
                cinema.PriceCenter = double.Parse(node.Attributes["PriceCenter"].Value);
                cinema.Description = node.Attributes["Description"].Value;
                int check = Int32.Parse(node.Attributes["Status"].Value);
                if (check == 1)
                    cinema.Status = true;
                else
                    cinema.Status = false;

                Seat seat = null;
                foreach (XmlNode item in node)
                {
                    seat = new Seat();
                    seat.Id = item.Attributes["Id"].Value;
                    seat.Name = item.Attributes["Name"].Value;
                    check = Int32.Parse(item.Attributes["Status"].Value);
                    if (check == 1)
                        seat.Status = true;
                    else
                        seat.Status = false;
                    cinema.lstSeat.Add(seat);
                }
                lstCinema.Add(cinema);
            }

            DataProvider.Instance.Close(pathData);
        }

        public void Add(Cinema entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Cinema entity)
        {
            throw new NotImplementedException();
        }

        public Cinema Get(string id)
        {
            foreach (var item in lstCinema)
                if (string.Compare(item.Id, id, true) == 0)
                    return item;
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public List<Cinema> Gets()
        {
            return lstCinema;
        }

        public void Update(Cinema entity)
        {
            throw new NotImplementedException();
        }
    }
}
