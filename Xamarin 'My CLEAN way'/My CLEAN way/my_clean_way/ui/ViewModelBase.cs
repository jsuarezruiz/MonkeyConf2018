using System;
using System.Diagnostics;
using Prism.Mvvm;
using Prism.Navigation;

namespace my_clean_way.ui
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        Boolean _isBusy;
        protected Boolean IsBusy { get => _isBusy; set { _isBusy = value; RaisePropertyChanged(); }}

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            Debug.WriteLine("OnNavigatedFrom");
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            Debug.WriteLine("OnNavigatedTo");
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
            Debug.WriteLine("OnNavigatingTo");
        }
    }
}
