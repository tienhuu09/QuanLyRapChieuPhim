using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tien_C2_B1
{
    public class ScheduleRepository : IRepository<Schedule>
    {
        private string pathData = "Data/Schedules.xml";
        public List<Schedule> lstSchedule { get; set; }

        public ScheduleRepository()
        {
            lstSchedule = new List<Schedule>();
            Load();
        }

        public void Load()
        {
            DataProvider.Instance.Open(pathData);

            XmlNodeList lstNode = DataProvider.Instance.getDsNode("//Schedule");
            Schedule schedule = null;
            foreach (XmlNode item in lstNode)
            {
                schedule = new Schedule();
                schedule.Id = item.Attributes["Id"].Value;
                schedule.IdMovie = item.Attributes["IdMovie"].Value;
                schedule.IdCinema = item.Attributes["IdCinema"].Value;

                int check = Int32.Parse(item.Attributes["Status"].Value);
                if (check == 1)
                    schedule.Status = true;
                else
                    schedule.Status = false;

                schedule.AirDate = DateTime.ParseExact(item.Attributes["AirDate"].Value, Ulti.date2, System.Globalization.CultureInfo.InvariantCulture);

                lstSchedule.Add(schedule);
            }

            DataProvider.Instance.Close(pathData);
        }

        public void Add(Schedule schedule)
        {
            lstSchedule.Add(schedule);

            DataProvider.Instance.Open(pathData);

            XmlNode newNode = DataProvider.Instance.createNode("Schedule");
            XmlAttribute attr1 = DataProvider.Instance.createAttr("Id");
            attr1.Value = schedule.Id;
            XmlAttribute attr2 = DataProvider.Instance.createAttr("IdMovie");
            attr2.Value = schedule.IdMovie;
            XmlAttribute attr3 = DataProvider.Instance.createAttr("IdCinema");
            attr3.Value = schedule.IdCinema;
            XmlAttribute attr4 = DataProvider.Instance.createAttr("AirDate");
            attr4.Value = schedule.AirDate.ToString(Ulti.date2);
            XmlAttribute attr5 = DataProvider.Instance.createAttr("Status");
            attr5.Value = "1";


            newNode.Attributes.Append(attr1);
            newNode.Attributes.Append(attr2);
            newNode.Attributes.Append(attr3);
            newNode.Attributes.Append(attr4);
            newNode.Attributes.Append(attr5);

            string xPath = string.Format("//Schedules");
            XmlNode node = DataProvider.Instance.getNode(xPath);
            DataProvider.Instance.AppendNode(node, newNode);

            DataProvider.Instance.Close(pathData);
        }

        public void Update(Schedule entity)
        {

        }

        public void Delete(Schedule entity)
        {
            throw new NotImplementedException();
        }

        public Schedule Get(string id)
        {
            foreach (var item in lstSchedule)
                if (string.Compare(item.Id, id, true) == 0)
                    return item;
            return null;
        }

        public List<Schedule> Gets()
        {
            return lstSchedule;
        }
    }
}
