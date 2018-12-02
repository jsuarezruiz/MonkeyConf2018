using System;
using System.Threading.Tasks;
using my_clean_way.domain;
using my_clean_way.movie_favorites.repository;
using my_clean_way.movies.domain;

namespace my_clean_way.movie_favorites.domain
{
    public class ChangeFavoriteStatusUseCase : IUseCase<Movie, Movie>
    {
        readonly IFavoriteMovieRepository _favoriteMovieRepository;
        public ChangeFavoriteStatusUseCase(IFavoriteMovieRepository favoriteMovieRepository)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
        }

        public async Task<Transaction<Movie>> execute(Movie parameter)
        {
            var transaction = new Transaction<Movie>();
            try
            {
                parameter.IsFavorite = !parameter.IsFavorite;
                if(parameter.IsFavorite){
                   await _favoriteMovieRepository.AddToFavorites(parameter);
                } else {
                    await _favoriteMovieRepository.RemoveFavorite(parameter);
                }
                transaction.Result = parameter;
                transaction.Status = TransactionStatus.Success;
            }
            catch (Exception ex)
            {
                transaction.Status = TransactionStatus.Failure;
                transaction.ErrorMessage = ex.Message;
            }

            return transaction;
        }
    }
}
