using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Tien_C2_B1;

namespace Tien_C2_B1
{
    public class CinemaService
    {
        private static UnitOfWork _unitOfWork = new UnitOfWork();
        public IRepository<Cinema> CinemaRepository { get; set; }

        public CinemaService()
        {
            CinemaRepository = _unitOfWork.CinemaRepository;
        }

        public List<Cinema> Gets()
        {
            return CinemaRepository.Gets();
        }

        public Cinema Get(string id)
        {
            return CinemaRepository.Get(id);
        }

        public bool isExisted(string id)
        {
            if(Get(id) != null)
                return true;
            return false;
        }

        public Seat getSeatById(Cinema cinema, string id)
        {
            foreach (var item in cinema.lstSeat)
                if (string.Compare(item.Id, id, true) == 0)
                    return item;
            return null;
        }

        public BoughtSeat getSeatByName(Cinema cinema, string name)
        {
            BoughtSeat bought = new BoughtSeat();
            foreach (var item in cinema.lstSeat)
            {
                if (string.Compare(item.Name, name, true) == 0)
                {
                    bought.Seat = item;
                    bought.SeatId = item.Id;
                }
            }
            return bought;
        }

        public void DeleteSeat(List<BoughtSeat> boughtSeats, string id)
        {
            int i = 0;
            foreach (var item in boughtSeats)
            {
                if (string.Compare(item.Seat.Id, id, true) == 0)
                    break;
                else
                    i++;
            }
            boughtSeats.RemoveAt(i);
        }
    }
}
