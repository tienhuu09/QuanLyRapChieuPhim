using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tien_C2_B1;

namespace Tien_C2_B1
{
    public class ScheduleService
    {
        private static UnitOfWork _unitOfWork = new UnitOfWork();
        public IRepository<Schedule> ScheduleRepository { get; set; }

        public ScheduleService()
        {
            ScheduleRepository = _unitOfWork.ScheduleRepository;
        }

        public bool isExist(string id)
        {
            if (ScheduleRepository.Get(id) != null)
                return true;
            return false;
        }

        public bool ScheduleIsExist(Schedule schedule)
        {
            foreach (var item in ScheduleRepository.Gets())
                if (string.Compare(item.IdCinema, schedule.IdCinema, true) == 0
                    && string.Compare(item.IdMovie, schedule.IdMovie, true) == 0
                    && DateTime.Compare(item.AirDate, schedule.AirDate) == 0)
                    return true;
            return false;
        } 

        public void Add(Schedule schedule)
        {
            ScheduleRepository.Add(schedule);
        }

        public Schedule Get(string id)
        {
            return ScheduleRepository.Get(id);
        }

        public List<Schedule> Gets()
        {
            return ScheduleRepository.Gets();
        } 

        public void UpdateStatus(Schedule schedule)
        {
            _unitOfWork.UpdateSttSchedule(schedule);
            ScheduleRepository.Update(schedule);
        }
    }
}
