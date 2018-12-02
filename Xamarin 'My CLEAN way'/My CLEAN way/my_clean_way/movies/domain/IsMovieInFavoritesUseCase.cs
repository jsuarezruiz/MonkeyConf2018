using System;
using System.Threading.Tasks;
using my_clean_way.domain;
using my_clean_way.movie_favorites.repository;

namespace my_clean_way.movies.domain
{
    public class IsMovieInFavoritesUseCase : IUseCase<bool,Movie>
    {
        readonly IFavoriteMovieRepository _favoriteMovieRepository;
        public IsMovieInFavoritesUseCase(IFavoriteMovieRepository favoriteMovieRepository)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
        }

        public async Task<Transaction<bool>> execute(Movie parameter)
        {
            var transaction = new Transaction<bool>();
            try{
                transaction.Result =await  _favoriteMovieRepository.IsMovieFavorite(parameter);
                transaction.Status = TransactionStatus.Success;
            }catch(Exception ex) {
                transaction.ErrorMessage = ex.Message;
                transaction.Status = TransactionStatus.Failure;
            }
            return transaction;
        }
    }
}
