using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using my_clean_way.movies.domain;

namespace my_clean_way.movie_list.repository
{
    public interface IMovieListRepository
    {
        Task<List<Movie>> GetPopularMovies(Boolean needMore);

    }
}
