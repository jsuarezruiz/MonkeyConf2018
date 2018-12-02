using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using my_clean_way.domain;
using my_clean_way.movie_favorites.repository;
using my_clean_way.movies.domain;

namespace my_clean_way.movie_favorites.domain
{
	public class GetFavoritesMoviesUseCase : IUseCase<List<Movie>,EmptyParameter>
    {
        readonly IFavoriteMovieRepository _favoriteMovieRepository;
        public GetFavoritesMoviesUseCase(IFavoriteMovieRepository favoriteMovieRepository)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
        }

        public async Task<Transaction<List<Movie>>> execute(EmptyParameter parameter)
        {
            var transaction = new Transaction<List<Movie>>();
            try
            {
                var favorites = await _favoriteMovieRepository.GetFavoritesMovies();
                transaction.Result = favorites;
                transaction.Status = TransactionStatus.Success;
            }
            catch (Exception ex) {
                transaction.Status = TransactionStatus.Failure;
                transaction.ErrorMessage = ex.Message;
            }
            return transaction;
        }
    }
}
