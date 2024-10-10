using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tien_C2_B1
{
    public class OrderRepository : IRepository<Order>
    {
        private string pathData { get; } = "data/Orders.xml";
        public List<Order> lstOrder { get; set; }

        public OrderRepository()
        {
            lstOrder = new List<Order>();
            Load();
        }

        public void Load()
        {
            DataProvider.Instance.Open(pathData);

            XmlNodeList lstNode = DataProvider.Instance.getDsNode("//Order");

            Order order = null;
            foreach (XmlNode item in lstNode)
            {
                order = new Order();
                order.Id = item.Attributes["Id"].Value.ToString();
                order.IdScheduleShowTime = item.Attributes["IdScheduleShowTime"].Value;
                order.CinemaType = item.Attributes["CinemaType"].Value;
                order.CustomerName = item.Attributes["CustomerName"].Value;
                order.PhoneNumber = item.Attributes["PhoneNumber"].Value;
                order.Date = DateTime.ParseExact(item.Attributes["Date"].Value, Ulti.date, System.Globalization.CultureInfo.InvariantCulture);
                order.Total = double.Parse(item.Attributes["Total"].Value);

                DetailOrder detail = null;
                foreach (XmlNode node in item)
                {
                    detail = new DetailOrder();
                    detail.Age = node.Attributes["Age"].Value;
                    detail.SeatId = node.Attributes["SeatId"].Value;
                    detail.Price = double.Parse(node.Attributes["Price"].Value);
                    detail.Discount = double.Parse(node.Attributes["Discount"].Value);
                    order.lstDetailOrder.Add(detail);
                }

                lstOrder.Add(order);
            }

            DataProvider.Instance.Close(pathData);
        }

        public void Add(Order order)
        {
            lstOrder.Add(order);

            DataProvider.Instance.Open(pathData);

            XmlNode newNode = DataProvider.Instance.createNode("Order");
            XmlAttribute attr1 = DataProvider.Instance.createAttr("Id");
            attr1.Value = order.Id;
            XmlAttribute attr2 = DataProvider.Instance.createAttr("IdScheduleShowTime");
            attr2.Value = order.IdScheduleShowTime;
            XmlAttribute attr3 = DataProvider.Instance.createAttr("CinemaType");
            attr3.Value = order.CinemaType;
            XmlAttribute attr4 = DataProvider.Instance.createAttr("CustomerName");
            attr4.Value = order.CustomerName;
            XmlAttribute attr5 = DataProvider.Instance.createAttr("PhoneNumber");
            attr5.Value = order.PhoneNumber;
            XmlAttribute attr6 = DataProvider.Instance.createAttr("Date");
            attr6.Value = order.Date.ToString(Ulti.date);
            XmlAttribute attr7 = DataProvider.Instance.createAttr("Total");
            attr7.Value = order.Total.ToString();

            newNode.Attributes.Append(attr1);
            newNode.Attributes.Append(attr2);
            newNode.Attributes.Append(attr3);
            newNode.Attributes.Append(attr4);
            newNode.Attributes.Append(attr5);
            newNode.Attributes.Append(attr6);
            newNode.Attributes.Append(attr7);

            XmlNode DetailOrder = null;
            foreach (var item in order.lstDetailOrder)
            {
                DetailOrder = DataProvider.Instance.createNode("DetailOrder");
                attr1 = DataProvider.Instance.createAttr("Age");
                attr1.Value = item.Age;
                attr2 = DataProvider.Instance.createAttr("SeatId");
                attr2.Value = item.SeatId.ToString();
                attr3 = DataProvider.Instance.createAttr("Price");
                attr3.Value = item.Price.ToString();
                attr4 = DataProvider.Instance.createAttr("Discount");
                attr4.Value = item.Discount.ToString();

                DetailOrder.Attributes.Append(attr1);
                DetailOrder.Attributes.Append(attr2);
                DetailOrder.Attributes.Append(attr3);
                DetailOrder.Attributes.Append(attr4);

                DataProvider.Instance.AppendNode(newNode, DetailOrder);
            }

            string xPath = string.Format("//Orders");
            XmlNode node = DataProvider.Instance.getNode(xPath);
            DataProvider.Instance.AppendNode(node, newNode);

            DataProvider.Instance.Close(pathData);
        }

        public void Update(Order entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Order entity)
        {
            throw new NotImplementedException();
        }

        public Order Get(string id)
        {
            foreach (var item in lstOrder)
                if (string.Compare(item.Id, id, true) == 0)
                    return item;
            return null;
        }

        public List<Order> Gets()
        {
            return lstOrder;
        }
    }
}
