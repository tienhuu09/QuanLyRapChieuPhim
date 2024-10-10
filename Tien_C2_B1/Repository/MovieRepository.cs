using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tien_C2_B1
{
    public class MovieRepository : IRepository<Movie>
    {
        private string pathData = "Data/Movies.xml";
        public List<Movie> lstMovie { get; set; }

        public MovieRepository()
        {
            lstMovie = new List<Movie>();
            Load();
        }

        public void Load()
        {
            DataProvider.Instance.Open(pathData);

            XmlNodeList lstNode = DataProvider.Instance.getDsNode("//Movie");

            Movie movie = null;
            foreach (XmlNode item in lstNode)
            {
                movie = new Movie();
                movie.Id = item.Attributes["Id"].Value;
                movie.Name = item.Attributes["Name"].Value;
                movie.Description = item.Attributes["Description"].Value;
                movie.Duration = item.Attributes["Duration"].Value;
                movie.StartAirDate = DateTime.ParseExact(item.Attributes["StartAirDate"].Value, Ulti.date, System.Globalization.CultureInfo.InvariantCulture);
                movie.EndAirDate = DateTime.ParseExact(item.Attributes["EndAirDate"].Value, Ulti.date, System.Globalization.CultureInfo.InvariantCulture);
                int num = Int32.Parse(item.Attributes["Status"].Value.ToString());
                if (num == 1 && DateTime.Compare(movie.EndAirDate, DateTime.Now) > 0)
                    movie.Status = true;
                else
                    movie.Status = false;

                lstMovie.Add(movie);
            }
            DataProvider.Instance.Close(pathData);
        }

        public void Add(Movie movie)
        {
            lstMovie.Add(movie);

            DataProvider.Instance.Open(pathData);

            XmlNode newNode = DataProvider.Instance.createNode("Movie");
            XmlAttribute attr1 = DataProvider.Instance.createAttr("Id");
            attr1.Value = movie.Id;
            XmlAttribute attr2 = DataProvider.Instance.createAttr("Name");
            attr2.Value = movie.Name;
            XmlAttribute attr3 = DataProvider.Instance.createAttr("Description");
            attr3.Value = movie.Description;
            XmlAttribute attr4 = DataProvider.Instance.createAttr("Duration");
            attr4.Value = movie.Duration;
            XmlAttribute attr5 = DataProvider.Instance.createAttr("StartAirDate");
            attr5.Value = movie.StartAirDate.ToString(Ulti.date);
            XmlAttribute attr6 = DataProvider.Instance.createAttr("EndAirDate");
            attr6.Value = movie.EndAirDate.ToString(Ulti.date);
            XmlAttribute attr7 = DataProvider.Instance.createAttr("Status");
            attr7.Value = "1";

            newNode.Attributes.Append(attr1);
            newNode.Attributes.Append(attr2);
            newNode.Attributes.Append(attr3);
            newNode.Attributes.Append(attr4);
            newNode.Attributes.Append(attr5);
            newNode.Attributes.Append(attr6);
            newNode.Attributes.Append(attr7);

            string xPath = string.Format("//Movies");
            XmlNode node = DataProvider.Instance.getNode(xPath);
            DataProvider.Instance.AppendNode(node, newNode);
            DataProvider.Instance.Close(pathData);
        }

        public void Delete(Movie entity)
        {
            lstMovie.Remove(entity);

            DataProvider.Instance.Open(pathData);

            string xPath = string.Format("//Movie[@Id='{0}']", entity.Id);
            XmlNode refNode = DataProvider.Instance.getNode(xPath);
            DataProvider.Instance.RemoveNode(refNode);

            DataProvider.Instance.Close(pathData);
        }

        public Movie Get(string id)
        {
            foreach (var item in lstMovie)
                if (string.Compare(item.Id, id, true) == 0)
                    return item;
            return null;
        }

        public List<Movie> Gets()
        {
            return lstMovie;
        }

        public void Update(Movie movie)
        {
            DataProvider.Instance.Open(pathData);
            string xPath = string.Format("//Movie[@Id='{0}']", movie.Id);

            XmlNode node = DataProvider.Instance.getNode(xPath);
            node.Attributes["Name"].InnerText = movie.Name;
            node.Attributes["Description"].InnerText = movie.Description;
            node.Attributes["Duration"].InnerText = movie.Duration;
            node.Attributes["StartAirDate"].InnerText = movie.StartAirDate.ToString(Ulti.date);
            node.Attributes["EndAirDate"].InnerText = movie.EndAirDate.ToString(Ulti.date);

            if (movie.Status)
                node.Attributes["Status"].InnerText = "1";
            else
                node.Attributes["Status"].InnerText = "0";

            DataProvider.Instance.Close(pathData);
        }


    }
}
