using System;
using my_clean_way.movies.domain;
using my_clean_way.domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using my_clean_way.movie_list.repository;
using my_clean_way.movie_favorites.repository;
using System.Linq;

namespace my_clean_way.movies.domain
{
    public class GetPopularMoviesUseCase : IUseCase<List<Movie>,bool>
    {
        readonly IMovieListRepository _movieListRepository;
        public GetPopularMoviesUseCase(IMovieListRepository movieListRepository)
        {
            _movieListRepository = movieListRepository;
        }

        public async Task<Transaction<List<Movie>>> execute(bool needMoreItems)
        {
            var transaction = new Transaction<List<Movie>>();
            try{
                var storedList = await _movieListRepository.GetPopularMovies(needMoreItems);
                transaction.Result = storedList;
                transaction.Status = TransactionStatus.Success;
            }catch(Exception ex) {
                transaction.ErrorMessage = ex.Message;
                transaction.Status = TransactionStatus.Failure;
            }

            return transaction;
        }
    }
}
