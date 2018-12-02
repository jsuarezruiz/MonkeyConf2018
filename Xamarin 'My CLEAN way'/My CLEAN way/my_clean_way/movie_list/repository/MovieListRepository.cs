using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using my_clean_way.data;
using my_clean_way.movie_favorites.repository;
using my_clean_way.movies.data;
using my_clean_way.movies.domain;

namespace my_clean_way.movie_list.repository
{
    public class MovieListRepository : IMovieListRepository
    {
        readonly IApiMovieDataSource _apiMovieDataSource;
        readonly IFavoriteMovieRepository _favoriteMovieRepository;
        int lastPage;
        public MovieListRepository(IApiMovieDataSource apiMovieDataSource,
                                   IFavoriteMovieRepository favoriteMovieRepository)
        {
            lastPage = 1;
            _apiMovieDataSource = apiMovieDataSource;
            _favoriteMovieRepository = favoriteMovieRepository;
        }


        public async Task<List<Movie>> GetPopularMovies(bool needMore)
        {
            if(needMore){
                lastPage++;
            } else {
                lastPage = 1;
            }
            var Apiresponse = await _apiMovieDataSource.GetPopularMovies(lastPage, ApiUtils.API_TOKEN);
            var storedFavorites = await _favoriteMovieRepository.GetFavoritesMovies();
            return Apiresponse.Results.Select((x) => {
                var domain = x.toDomain();
               // domain.IsFavorite = _favoriteMovieRepository.IsMovieFavorite(domain).Result;
                return domain;
            }).ToList();
        }

    }
}
