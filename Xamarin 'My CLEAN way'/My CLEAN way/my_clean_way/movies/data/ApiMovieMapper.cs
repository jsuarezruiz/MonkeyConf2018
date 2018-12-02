using System;
using my_clean_way.data;
using my_clean_way.movies.domain;

namespace my_clean_way.movies.data
{
    public static class ApiMovieMapper
    {

        public static Movie toDomain(this ApiMovie movie)
        {
            return new Movie()
            {
                IsFavorite = false,
                BackdropPath = ApiUtils.API_IMAGE_BASE_URL + movie.BackdropPath,
                Id = movie.Id,
                Overview = movie.Overview,
                Popularity = movie.Popularity,
                PosterPath = ApiUtils.API_IMAGE_BASE_URL + movie.PosterPath,
                Title = movie.Title,
                VoteAverage = movie.VoteAverage,
                VoteCount = movie.VoteCount
            };
        }

    }
}
