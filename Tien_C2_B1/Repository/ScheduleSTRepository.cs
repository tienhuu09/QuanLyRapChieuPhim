using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tien_C2_B1
{
    public class ScheduleSTRepository : IRepository<ScheduleShowTime>
    {
        private string pathData = "Data/ScheduleShowTimes.xml";
        public List<ScheduleShowTime> lstScheduleST { get; set; }

        public ScheduleSTRepository()
        {
            lstScheduleST = new List<ScheduleShowTime>();
            Load();
        }

        public void Load()
        {
            DataProvider.Instance.Open(pathData);

            XmlNodeList lstNode = DataProvider.Instance.getDsNode("//ScheduleShowTime");
            ScheduleShowTime scheduleST = null;
            foreach (XmlNode item in lstNode)
            {
                scheduleST = new ScheduleShowTime();
                scheduleST.Id = item.Attributes["Id"].Value;
                scheduleST.IdSchedule = item.Attributes["IdSchedule"].Value;
                int check = Int32.Parse(item.Attributes["Status"].Value);
                if (check == 1)
                    scheduleST.Status = true;
                else
                    scheduleST.Status = false;

                scheduleST.AirDate = DateTime.ParseExact(item.Attributes["AirDate"].Value, Ulti.date, System.Globalization.CultureInfo.InvariantCulture);
                if (DateTime.Compare(scheduleST.AirDate, DateTime.Now) < 0)
                    scheduleST.Status = false;
                BoughtSeat boughtSeat = null;
                foreach (XmlNode node in item)
                {
                    boughtSeat = new BoughtSeat();
                    boughtSeat.SeatId = node.Attributes["SeatId"].Value;
                    scheduleST.lstBoughtSeat.Add(boughtSeat);
                }

                lstScheduleST.Add(scheduleST);
            }

            DataProvider.Instance.Close(pathData);
        }

        public void Add(ScheduleShowTime scheduleST)
        {
            lstScheduleST.Add(scheduleST);

            DataProvider.Instance.Open(pathData);
            XmlNode newNode = DataProvider.Instance.createNode("ScheduleShowTime");

            XmlAttribute attr1 = DataProvider.Instance.createAttr("Id");
            attr1.Value = scheduleST.Id;
            XmlAttribute attr2 = DataProvider.Instance.createAttr("IdSchedule");
            attr2.Value = scheduleST.IdSchedule.ToString();
            XmlAttribute attr3 = DataProvider.Instance.createAttr("AirDate");
            attr3.Value = scheduleST.AirDate.ToString(Ulti.date);
            XmlAttribute attr4 = DataProvider.Instance.createAttr("Status");
            if (scheduleST.Status)
                attr4.Value = "1";
            else
                attr4.Value = "0";
            newNode.Attributes.Append(attr1);
            newNode.Attributes.Append(attr2);
            newNode.Attributes.Append(attr3);
            newNode.Attributes.Append(attr4);

            XmlNode boughtSeatNode = null;
            foreach (var item in scheduleST.lstBoughtSeat)
            {
                boughtSeatNode = DataProvider.Instance.createNode("Bought Seat");
                attr1 = DataProvider.Instance.createAttr("SeatId");
                attr1.Value = item.SeatId;

                boughtSeatNode.Attributes.Append(attr1);

                DataProvider.Instance.AppendNode(newNode, boughtSeatNode);
            }
            string xPath = string.Format("//ScheduleShowTimes");
            XmlNode node = DataProvider.Instance.getNode(xPath);
            DataProvider.Instance.AppendNode(node, newNode);

            DataProvider.Instance.Close(pathData);
        }

        public void AddBoughtSeat(ScheduleShowTime scheduleST, BoughtSeat boughtSeat)
        {
            DataProvider.Instance.Open(pathData);

            XmlNode newNode = DataProvider.Instance.createNode("BoughtSeat");
            XmlAttribute attr1 = DataProvider.Instance.createAttr("SeatId");
            attr1.Value = boughtSeat.SeatId;
            newNode.Attributes.Append(attr1);

            string xPath = string.Format("//ScheduleShowTime[@Id='{0}']", scheduleST.Id);
            XmlNode node = DataProvider.Instance.getNode(xPath);
            DataProvider.Instance.AppendNode(node, newNode);

            DataProvider.Instance.Close(pathData);
        }

        public void Delete(ScheduleShowTime entity)
        {
            throw new NotImplementedException();
        }

        public ScheduleShowTime Get(string id)
        {
            foreach (var item in lstScheduleST)
                if (string.Compare(item.Id, id, true) == 0)
                    return item;
            return null;
        }

        public List<ScheduleShowTime> Gets()
        {
            return lstScheduleST;
        }

        public void Update(ScheduleShowTime entity)
        {
            throw new NotImplementedException();
        }
    }
}
