using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tien_C2_B1;

namespace Tien_C2_B1
{
    public class AccountService
    {
        private static UnitOfWork _unitOfWork = new UnitOfWork();
        public IRepository<Account> AccountRepository { get; set; }

        public AccountService()
        {
            AccountRepository = _unitOfWork.AccountRepository;
        }

        public Account GetAccount(string username, string password)
        {
            foreach (var item in AccountRepository.Gets())
            {
                if (string.Compare(item.Username, username, true) == 0
                    && string.Compare(item.Password, password, true) == 0)
                    return item;
            }
            return null;
        }
    }
}
