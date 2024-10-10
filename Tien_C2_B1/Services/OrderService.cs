using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tien_C2_B1;

namespace Tien_C2_B1
{
    public class OrderService
    {
        private static UnitOfWork _unitOfWork = new UnitOfWork();
        public IRepository<Order> OrderRepository { get; set; }

        public OrderService()
        {
            OrderRepository = _unitOfWork.OrderRepository;
        }

        public List<Order> Gets()
        {
            return OrderRepository.Gets();
        }

        public Order Get(string id)
        {
            return OrderRepository.Get(id);
        }

        public void Add(Order order)
        {
            OrderRepository.Add(order);
        }
    }
}
