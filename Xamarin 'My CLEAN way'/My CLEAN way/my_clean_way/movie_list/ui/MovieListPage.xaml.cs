using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace my_clean_way.movie_list.ui
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieListPage : ContentPage
    {
        public readonly static String Route = "MovieListPageRoute";
        public MovieListPage()
        {
            InitializeComponent();
        }
    }
}
