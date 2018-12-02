using System;
using System.Diagnostics;
using my_clean_way.ui;
using Prism;
using Prism.Navigation;
using my_clean_way.domain;
using System.Collections.Generic;
using my_clean_way.movies.domain;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using my_clean_way.movies.ui;
using my_clean_way.movie_favorites.ui;

namespace my_clean_way.movie_list.ui
{
    public class MovieListPageViewModel : ViewModelBase
    {
        readonly IUseCase<List<Movie>, bool> _getPopularMovieList;
        readonly INavigationService _navigationService;

        ObservableCollection<Movie> movies;
        public ObservableCollection<Movie> Movies { get => movies; set { movies = value; RaisePropertyChanged(); } }

        public ICommand MovieSelectedCommand => new Command<Movie>(MovieSelected,(movie) => !IsBusy);
        public ICommand SearchMoviesCommand => new Command(SearchMovies,() => !IsBusy);
        public ICommand MovieAppearingCommand => new Command<Movie>(MovieAppearing, (movie) => !IsBusy);
        public ICommand NavigateToFavoritesCommand => new Command(() => _navigationService.NavigateAsync(FavoritesPage.Route));

        public MovieListPageViewModel(IUseCase<List<Movie>, bool> getPopularMovieList,
                                     INavigationService navigationService)
        {
            _getPopularMovieList = getPopularMovieList;
            _navigationService = navigationService;
            Movies = new ObservableCollection<Movie>(new List<Movie>());
        }


        async void SearchMovies(){
            IsBusy = true;
            var transaction = await _getPopularMovieList.execute(false);
            if (transaction.IsSuccesfull)
            {
                Device.BeginInvokeOnMainThread(() => Movies = new ObservableCollection<Movie>(transaction.Result));
            }
            IsBusy = false;
        }

        void MovieSelected(Movie movie){
            NavigationParameters parameter = new NavigationParameters();
            parameter.Add(PageParameters.MOVIE_PARAMETER, movie);
            _navigationService.NavigateAsync(MoviePage.Route,parameter);
        }

        async void MovieAppearing(Movie movie)
        {
           var lastMovie = Movies.LastOrDefault();
            if(movie.Id.Equals(lastMovie.Id)){
                IsBusy = true;
                var transaction = await _getPopularMovieList.execute(true);
                if (transaction.IsSuccesfull)
                {

                    Device.BeginInvokeOnMainThread(() =>{
                        Movies = new ObservableCollection<Movie>(Movies.ToList().Concat(transaction.Result));
                    });
                }
                IsBusy = false;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Debug.WriteLine("MovieListPageViewModel - OnNavigatedTo");
            if (Movies.Count == 0)
            {
                SearchMoviesCommand.Execute(null);
            }

            base.OnNavigatedFrom(parameters);
        }
    }
}
