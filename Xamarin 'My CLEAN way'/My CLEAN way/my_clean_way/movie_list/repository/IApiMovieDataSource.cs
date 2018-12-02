using System;
using System.Threading.Tasks;
using my_clean_way.data;
using my_clean_way.movies.data;
using Refit;

namespace my_clean_way.movie_list.repository
{
    public interface IApiMovieDataSource
    {
        [Get("/movie/popular?page={pageNumber}&language=es-ES&api_key={apiKey}")]
        Task<PaginableResponse<ApiMovie>> GetPopularMovies([AliasAs("pageNumber")] int pageNumber,[AliasAs("apiKey")] string apiKey);
    }
}
