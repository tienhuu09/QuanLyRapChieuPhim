using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tien_C2_B1
{
    internal class UnitOfWork
    {
        private static IRepository<Account> _accountRepository;
        private static IRepository<Order> _orderRepository;
        private static IRepository<Movie> _movieRepository;
        private static IRepository<Cinema> _cinemaRepository;
        private static IRepository<Schedule> _scheduleRepository;
        private static IRepository<ScheduleShowTime> _scheduleSTRepo;

        public IRepository<Account> AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                    _accountRepository = new AccountRepository();
                return _accountRepository;
            }
            set
            {
                _accountRepository = value;
            }
        }

        public IRepository<Order> OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = new OrderRepository();
                return _orderRepository;
            }
            set
            {
                _orderRepository = value;
            }
        }

        public IRepository<Movie> MovieRepository
        {
            get
            {
                if (_movieRepository == null)
                    _movieRepository = new MovieRepository();
                return _movieRepository;
            }
            set
            {
                _movieRepository = value;
            }
        }

        public IRepository<Cinema> CinemaRepository
        {
            get
            {
                if (_cinemaRepository  == null)
                    _cinemaRepository = new CinemaRepository();
                return _cinemaRepository;
            }
            set
            {
                _cinemaRepository = value;
            }
        }

        public IRepository<Schedule> ScheduleRepository
        {
            get
            {
                if (_scheduleRepository == null)
                    _scheduleRepository = new ScheduleRepository();
                return _scheduleRepository;
            }
            set
            {
                _scheduleRepository = value;
            }
        }

        public IRepository<ScheduleShowTime> ScheduleSTRepository
        {
            get
            {
                if (_scheduleSTRepo == null)
                    _scheduleSTRepo = new ScheduleSTRepository();
                return _scheduleSTRepo;
            }
            set
            {
                _scheduleSTRepo = value;
            }
        }

        public void Load()
        {
            foreach (var item in ScheduleRepository.Gets())
            {
                item.Movie = MovieRepository.Get(item.IdMovie);
                item.Cinema = CinemaRepository.Get(item.IdCinema);

                DateTime date1 = item.AirDate;
                DateTime date2 = DateTime.Now;
                int result = DateTime.Compare(date1, date2);
                if (result < 0)
                    item.Status = false;
            }

            foreach (var item in ScheduleSTRepository.Gets())
            {
                item.Schedule = ScheduleRepository.Get(item.IdSchedule);
            }

            foreach (var item in OrderRepository.Gets())
            {
                item.ScheduleST = ScheduleSTRepository.Get(item.IdScheduleShowTime);
                foreach (var itemDetail in item.lstDetailOrder)
                {
                    foreach (var itemSeat in item.ScheduleST.Schedule.Cinema.lstSeat)
                    {
                        if (string.Compare(itemDetail.SeatId, itemSeat.Id, true) == 0)
                        {
                            itemDetail.Seat = itemSeat;
                            itemDetail.SeatName = itemSeat.Name;
                        }
                    }
                }
            }
        }

        public void LoadMovieStatus()
        {
            foreach (var item in _movieRepository.Gets())
            {
                if (item.Status == false)
                {
                    foreach (var itemSche in _scheduleRepository.Gets())
                    {
                        if (string.Compare(item.Id, itemSche.Movie.Id, true) == 0)
                            itemSche.Status = item.Status;
                    }
                    foreach (var itemScheST in _scheduleSTRepo.Gets())
                    {
                        if (string.Compare(item.Id, itemScheST.Schedule.Movie.Id, true) == 0)
                            itemScheST.Status = item.Status;
                    }
                }
            }
        }

        public void UpdateStatus(Movie item)
        {
            if (item.Status == false)
            {
                foreach (var itemSche in _scheduleRepository.Gets())
                {
                    if (string.Compare(item.Id, itemSche.IdMovie, true) == 0)
                        itemSche.Status = false;
                }
                foreach (var itemScheST in _scheduleSTRepo.Gets())
                {
                    if (string.Compare(item.Id, itemScheST.Schedule.IdMovie, true) == 0)
                        itemScheST.Status = false;
                }
            }
        }

        public void UpdateSttSchedule(Schedule schedule)
        {
            if (schedule.Status == false)
            {
                foreach (var item in _scheduleSTRepo.Gets())
                {
                    if (string.Compare(item.IdSchedule, schedule.Id, true) == 0)
                        item.Status = false;
                }
            }
        }

        public UnitOfWork()
        {
            Load();
            LoadMovieStatus();
        }
    }
}
