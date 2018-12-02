using System.Collections.Generic;
using System.Collections.ObjectModel;
using my_clean_way.domain;
using my_clean_way.movies.domain;
using my_clean_way.ui;
using Prism.Navigation;
using Xamarin.Forms;

namespace my_clean_way.movie_favorites.ui
{
    public class FavoritesPageViewModel : ViewModelBase
    {
        readonly IUseCase<List<Movie>, EmptyParameter> _getFavoriteMoviesUseCase;

        ObservableCollection<Movie> movies;
        public ObservableCollection<Movie> Movies { get => movies; set { movies = value; RaisePropertyChanged(); } }


        public FavoritesPageViewModel(IUseCase<List<Movie>, EmptyParameter> getFavoriteMoviesUseCase)
        {
            _getFavoriteMoviesUseCase = getFavoriteMoviesUseCase;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            var transaction = await _getFavoriteMoviesUseCase.execute(new EmptyParameter());
            if(transaction.IsSuccesfull){
                Device.BeginInvokeOnMainThread(()=> Movies = new ObservableCollection<Movie>(transaction.Result));
            }
            base.OnNavigatedTo(parameters);
        }

    }
}
