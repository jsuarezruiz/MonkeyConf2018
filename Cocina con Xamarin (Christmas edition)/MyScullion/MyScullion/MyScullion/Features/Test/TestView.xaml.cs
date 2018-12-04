using MyScullion.Models;
using MyScullion.Services;
using MyScullion.Services.Databases;
using MyScullion.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyScullion.Features.Test
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestView : ContentPage
	{

        private readonly IDatabaseService databaseService;
        private readonly IRandomService randomService;
        private readonly IFileEmbeddedService fileEmbeddedService;

		public TestView ()
		{
			InitializeComponent ();

            databaseService = CustomDependencyService.Get<IDatabaseService>();
            randomService = CustomDependencyService.Get<IRandomService>();
            fileEmbeddedService = CustomDependencyService.Get<IFileEmbeddedService>();
		}

        private void InsertIngredientsClicked(object sender, ItemTappedEventArgs args)
        {
            var data = fileEmbeddedService.GetFile("ingredients.csv")
                            .Select(x => new Ingredient(x)).ToList();
            InsertAll<Ingredient>(data);
        }

        private void GetIngredientsClicked(object sender, EventArgs e)
        {
            GetAll<Ingredient>();
        }

        private void InsertMeasuresClicked(object sender, ItemTappedEventArgs args)
        {
            var data = fileEmbeddedService.GetFile("measures.csv")
                            .Select(x => new Measure(x)).ToList();
            InsertAll<Measure>(data);
        }

        private void GetMeasuresClicked(object sender, EventArgs e)
        {
            GetAll<Measure>();
        }

        private void InsertRandomTest(object sender, ItemTappedEventArgs args)
        {
            var rows = 0;
            int.TryParse(EntryRows.Text, out rows);

            var randomData = randomService.CreateRandomData(rows);            
            InsertAll<RandomModel>(randomData);
            
        }

        private void GetRandomData(object sender, EventArgs e)
        {
            GetAll<RandomModel>();
        }
        
        private async void InsertAll<T>(List<T> data) where T : BaseModel, new()
        {
            Log.Start($"InsertRandomData{databaseService.GetType().Name}");
            var stopWatch = new Stopwatch();            
            stopWatch.Start();            
            await databaseService.InsertAll<T>(data);            
            stopWatch.Stop();            
            LabelTimeWorking.Text = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds).ToString();

            Log.Stop($"InsertRandomData{databaseService.GetType().Name}");
        }
        
        private async void GetAll<T>() where T : BaseModel, new()
        {
            Log.Start($"InsertRandomData{databaseService.GetType().Name}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            GridWaiting.IsVisible = true;
            await databaseService.GetAll<T>();
            GridWaiting.IsVisible = false;
            stopWatch.Stop();

            LabelTimeWorking.Text = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds).ToString();

            Log.Stop($"InsertRandomData{databaseService.GetType().Name}");
        }

        private Task ShowBusy()
        {
            var contentView = (Layout<View>)Content;
            Device.BeginInvokeOnMainThread(() =>
            {
                var busyView = new BusyView();
                busyView.WidthRequest = this.Width;
                busyView.HeightRequest = this.Height;
                contentView.Children.Add(busyView);
            });

            return Task.FromResult(Unit.Default);
        }

        private Task RemoveBusy()
        {
            var contentView = (Layout<View>)Content;
            if (contentView != null)
            {
                var busyViews = contentView.Children.Where(x => x.GetType() == typeof(BusyView)).ToList();

                foreach (var busyView in busyViews)
                {
                    contentView.Children.Remove(busyView);
                }
            }

            return Task.FromResult(Unit.Default);
        }        
    }
}