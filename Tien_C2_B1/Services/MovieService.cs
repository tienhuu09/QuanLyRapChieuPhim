using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tien_C2_B1;

namespace Tien_C2_B1
{
    public class MovieService
    {
        private static UnitOfWork _unitOfWork = new UnitOfWork();
        public IRepository<Movie> MovieRepository { get; set; }

        public MovieService()
        {
            MovieRepository = _unitOfWork.MovieRepository;
        }

        public Movie getMovieByid(string id)
        {
            foreach (var item in MovieRepository.Gets())
                if (string.Compare(item.Id, id, true) == 0)
                    return item;
            return null;
        }

        public void Remove(Movie movie)
        {
            MovieRepository.Delete(movie);
        }

        public void Add(Movie movie)
        {
            MovieRepository.Add(movie);

        }

        public bool isExist(string id)
        {
            if (MovieRepository.Get(id) != null)
                return true;
            return false;
        }

        public List<Movie> Gets()
        {
            return MovieRepository.Gets();
        }

        public Movie Get(string id)
        {
            return MovieRepository.Get(id);
        }

        public void Update(Movie movie)
        {
            _unitOfWork.UpdateStatus(movie);
            MovieRepository.Update(movie);
        }
    }
}
