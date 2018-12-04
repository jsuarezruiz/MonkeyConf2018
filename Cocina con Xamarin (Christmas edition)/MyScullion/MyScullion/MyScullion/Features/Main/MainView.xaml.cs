using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyScullion.Features.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<MessageCenterChef>(this, nameof(MessageCenterChef), 
                (chef) =>
                {
                    LabelChef.Text = chef.Name;
                });
        }
    }
}