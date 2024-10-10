using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tien_C2_B1;

namespace Tien_C2_B1
{
    public class ScheduleSTService
    {
        private static UnitOfWork _unitOfWork = new UnitOfWork();
        public IRepository<ScheduleShowTime> ScheduleSTRepo { get; set; }
        private CinemaService cinemaService { get; set; }   

        private ScheduleSTRepository RepoTemp { get; set; }

        public ScheduleSTService()
        {
            ScheduleSTRepo = _unitOfWork.ScheduleSTRepository;
            RepoTemp = new ScheduleSTRepository();
            cinemaService = new CinemaService();
            LoadSeat();
        }

        public void LoadSeat()
        {
            foreach (var item in _unitOfWork.ScheduleSTRepository.Gets())
            {
                foreach (var item2 in item.lstBoughtSeat)
                    item2.Seat = cinemaService.getSeatById(item.Schedule.Cinema, item2.SeatId);
            }
        }

        public ScheduleShowTime Get(string id)
        {
            return ScheduleSTRepo.Get(id);
        }

        public List<ScheduleShowTime> Gets()
        {
            return ScheduleSTRepo.Gets();
        }

        public bool isExisted(string id)
        {
            if (Get(id) != null)
                return true;
            return false;
        }

        public void Add(ScheduleShowTime item)
        {
            ScheduleSTRepo.Add(item);
        }

        // Kiểm tra phim còn trong thời gian khởi chiếu ?
        public bool CheckDateShowTime(ScheduleShowTime ScheduleST)
        {
            int result1 = DateTime.Compare(ScheduleST.Schedule.Movie.StartAirDate, ScheduleST.AirDate);
            int result2 = DateTime.Compare(ScheduleST.Schedule.Movie.EndAirDate, ScheduleST.AirDate);
            if (result1 < 0 && result2 > 0)
                return true;
            return false;
        }

        // Schedule.AirDate: thời gian nhập
        // item.AirDate: thời gian lịch chiếu đã có
        public bool OverlapTime(ScheduleShowTime ScheduleST)
        {
            foreach (var item in Gets())
            {
                if (item.IdSchedule == ScheduleST.IdSchedule)
                {
                    if (item.AirDate.Day == ScheduleST.AirDate.Day
                        && item.AirDate.Month == ScheduleST.AirDate.Month
                        && item.AirDate.Year == ScheduleST.AirDate.Year)
                    {
                        // thời gian nhập lớn hơn thời gian chiếu đã có
                        if (DateTime.Compare(ScheduleST.AirDate, item.AirDate) > 0)
                        {
                            // thời gian có sẵn + ...
                            DateTime date = item.AirDate.AddMinutes(Int32.Parse(item.Schedule.Movie.Duration) + 45);
                            int result = DateTime.Compare(ScheduleST.AirDate, date);
                            if (result < 0)
                                return true;
                        }
                        else
                        {
                            // thời gian nhập + ...
                            DateTime date = ScheduleST.AirDate.AddMinutes(Int32.Parse(item.Schedule.Movie.Duration) + 45);
                            int result = DateTime.Compare(date, item.AirDate);
                            if (result > 0)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckThePresentTime(ScheduleShowTime scheduleShowTime)
        {
            DateTime date = scheduleShowTime.AirDate.AddDays(- 1);
            int result = DateTime.Compare(DateTime.Now, date);
            if (result < 0)
                return true;
            return false;
        }

        public void AddBoughtSeat(ScheduleShowTime scheduleSt, List<BoughtSeat> lstBoughtSeat)
        {
            foreach (var itemScheduleST in ScheduleSTRepo.Gets())
            {
                if (string.Compare(scheduleSt.Id, itemScheduleST.Id, true) == 0)
                {
                    foreach (var itemBSeat in lstBoughtSeat)
                    {
                        itemScheduleST.lstBoughtSeat.Add(itemBSeat);
                        RepoTemp.AddBoughtSeat(itemScheduleST, itemBSeat);
                    }
                }
            }
        }
    }
}
