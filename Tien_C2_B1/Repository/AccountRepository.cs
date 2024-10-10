using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tien_C2_B1
{
    public class AccountRepository : IRepository<Account>
    {
        private string pathData = "Data/Account.xml";
        public List<Account> lstAccount { get; set; }

        public AccountRepository()
        {
            lstAccount = new List<Account>();
            Load();
        }

        public void Load()
        {
            DataProvider.Instance.Open(pathData);

            string xPath = string.Format("//Account");

            XmlNodeList lstNode = DataProvider.Instance.getDsNode(xPath);

            Account account = null;
            foreach (XmlNode item in lstNode)
            {
                account = new Account();
                account.Id = item.Attributes["Id"].Value;
                account.Name = item.Attributes["Name"].Value;
                account.Username = item.Attributes["Username"].Value;
                account.Password = item.Attributes["Password"].Value;
                account.Permission = Int32.Parse(item.Attributes["Permission"].Value);

                lstAccount.Add(account);   
            }
            DataProvider.Instance.Close(pathData);
        }

        public void Add(Account entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Account entity)
        {
            throw new NotImplementedException();
        }

        public Account Get(string id)
        {
            foreach (var item in lstAccount)
                if (string.Compare(item.Id, id, true) == 0)
                    return item;
            return null;
        }

        public List<Account> Gets()
        {
            return lstAccount;
        }

        public void Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
