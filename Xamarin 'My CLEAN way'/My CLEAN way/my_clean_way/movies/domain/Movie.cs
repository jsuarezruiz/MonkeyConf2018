using System;
namespace my_clean_way.movies.domain
{
    // Domain Entity 
    public class Movie
    {
        public bool IsFavorite { get; set; }

        public string PosterPath { get; set; }

        public string Overview { get; set; }

        public long Id { get; set; }

        public string Title { get; set; }

        public string BackdropPath { get; set; }

        public double Popularity { get; set; }

        public long VoteCount { get; set; }

        public double VoteAverage { get; set; }
    }
}
