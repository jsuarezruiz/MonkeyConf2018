using MyScullion.Features.Main;
using MyScullion.Services;
using MyScullion.Styles;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace MyScullion
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            CustomDependencyService.Register<MenuService>();
            CustomDependencyService.Register<RandomService>();
            CustomDependencyService.Register<FileEmbedddedService>();

            LoadStyles();
            
            MainPage = new MasterDetailPage() { Master = new MasterDetailMenu(), Detail = new NavigationPage(new MainView()) };
		}

        private void LoadStyles()
        {
            var masterStyle = new MasterDetailStyle();
            
            foreach(var element in masterStyle.Resources)
            {
                App.Current.Resources.Add(element.Key, element.Value);
            }
        }

        public static void ChangePresented()
        {
            var masterDetail = (MasterDetailPage)App.Current.MainPage;
            masterDetail.IsPresented = !masterDetail.IsPresented;
        }

        public static void ChangeDetail(Page page)
        {
            var masterDetail = (MasterDetailPage)App.Current.MainPage;
            masterDetail.Detail = page;
            ChangePresented();
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}        
    }
}
