using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akavache;
using my_clean_way.data;
using my_clean_way.movies.domain;
using System.Reactive.Linq;
using System.Linq;
using System.Diagnostics;

namespace my_clean_way.movie_favorites.repository
{
    public class FavoriteMoviesAkavacheDataSource : IFavoriteMovieRepository
    {
        public FavoriteMoviesAkavacheDataSource()
        {
            BlobCache.ApplicationName = DBUtils.DB_NAME;
        }

        public async Task<Movie> AddToFavorites(Movie movie)
        {
            await BlobCache.LocalMachine.InsertObject<Movie>(movie.Id.ToString(), movie);
            return movie;
        }

        public async Task<List<Movie>> GetFavoritesMovies()
        {
            var storedObjects = await BlobCache.LocalMachine.GetAllObjects<Movie>();
            return new List<Movie>(storedObjects);
        }

        public async Task<bool> IsMovieFavorite(Movie movie)
        {
            var result = false;
            var stored = await BlobCache.LocalMachine
                                    .GetObject<Movie>(movie.Id.ToString())
                                    .Catch(Observable.Return( default(Movie)));
            if(stored != null) {
                result = stored.IsFavorite;
            }
            
            return result;          
        }

        public async Task<bool> RemoveFavorite(Movie movie)
        {
            await BlobCache.LocalMachine.InvalidateObject<Movie>(movie.Id.ToString());
            return true;
        }
    }
}
