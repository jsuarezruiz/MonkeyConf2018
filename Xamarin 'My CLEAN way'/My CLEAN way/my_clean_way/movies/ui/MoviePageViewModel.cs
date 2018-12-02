using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using my_clean_way.domain;
using my_clean_way.movies.domain;
using my_clean_way.ui;
using Prism.Navigation;
using Xamarin.Forms;

namespace my_clean_way.movies.ui
{
    public class MoviePageViewModel : ViewModelBase
    {
        readonly IUseCase<Movie, Movie> _changeFavoriteStatusUseCase;
        readonly IUseCase<bool, Movie> _isFavoriteMovieInFavoritesUseCase;

        String NoFavIconName = "no_fav.png";
        String FavIconName = "fav.png";

        Movie movie;
        public Movie Movie { get => movie; set { movie = value; RaisePropertyChanged(); } }

        String favIcon;
        public String FavIcon { get => favIcon; set { favIcon = value; RaisePropertyChanged(); } }

        public ICommand FavCommand => new Command(ChangeFavoriteStatus, () => !IsBusy);

        public MoviePageViewModel(IUseCase<Movie, Movie> changeFavoriteStatusUseCase,
                                 IUseCase<bool, Movie> isFavoriteMovieInFavoritesUseCase)
        {
            FavIcon = NoFavIconName;
            _changeFavoriteStatusUseCase = changeFavoriteStatusUseCase;
            _isFavoriteMovieInFavoritesUseCase = isFavoriteMovieInFavoritesUseCase;
        }

        async void ChangeFavoriteStatus() {
            var transaction = await _changeFavoriteStatusUseCase.execute(Movie);
            if(transaction.IsSuccesfull) {
                Movie = transaction.Result;
                FavIcon = Movie.IsFavorite ? FavIconName : NoFavIconName;
            }
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            Debug.WriteLine("MovieListPageViewModel - OnNavigatedTo");
            Movie = parameters.GetValue<Movie>(PageParameters.MOVIE_PARAMETER);
            var transaction = await _isFavoriteMovieInFavoritesUseCase.execute(Movie);
            FavIcon = transaction.Result ? FavIconName : NoFavIconName;
            base.OnNavigatedFrom(parameters);
        }

    }
}
