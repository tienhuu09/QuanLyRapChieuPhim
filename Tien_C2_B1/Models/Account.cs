using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tien_C2_B1
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Permission { get; set; }

        public Account() { }

        public Account(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public Account getAccount(string userName)
        {
            return this;
        }
    }
}
