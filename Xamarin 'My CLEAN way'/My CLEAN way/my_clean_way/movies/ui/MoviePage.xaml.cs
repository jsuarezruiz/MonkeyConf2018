using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace my_clean_way.movies.ui
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviePage : ContentPage
    {
        public readonly static String Route = "MoviePageRoute";
        public MoviePage()
        {
            InitializeComponent();
        }
    }
}
