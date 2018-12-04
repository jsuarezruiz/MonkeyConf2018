using MyScullion.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyScullion.Services.Databases;
using System;

namespace MyScullion.Features.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailMenu : ContentPage
    {
        public ListView ListView;

        public MasterDetailMenu()
        {            
            InitializeComponent();
            BindingContext = new MasterDetailPageMasterViewModel();            
        }

        class MasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetailPageMenuItem> MenuItems { get; set; }
            
            public MasterDetailPageMasterViewModel()
            {                
                MenuItems = new ObservableCollection<MasterDetailPageMenuItem>(CustomDependencyService.Get<MenuService>().GetMenuItems());
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private void MenuItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if(e.Item != null)
            {
                var menuItem = (MasterDetailPageMenuItem)e.Item;
                
                if(menuItem.TargetType.GetInterfaces().First() == typeof(IDatabaseService))
                {
                    ChangeDatabaseServiceType(menuItem.TargetType);
                    MessagingCenter.Send<MessageCenterChef>(new MessageCenterChef() { Name = $"The chef is {menuItem.TargetType.Name}" }, nameof(MessageCenterChef));                    
                }
                else if(menuItem.TargetType.BaseType == typeof(ContentPage))
                {
                    NavigatePage(menuItem.TargetType);
                }

                (sender as ListView).SelectedItem = null;
            }
        }
        
        private void ChangeDatabaseServiceType(Type targetType)
        {
            CustomDependencyService.Register(targetType);
            App.ChangePresented();
        }

        private void NavigatePage(Type targetType)
        {
            var page = (Page)Activator.CreateInstance(targetType);
            App.ChangeDetail(page);           
        }
    }
}