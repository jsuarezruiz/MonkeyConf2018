using System;
using Prism;

namespace my_clean_way.ui
{
    public class TabPageViewModelBase : ViewModelBase, IActiveAware
    {
        bool _IsActive;
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                SetProperty(ref _IsActive, value, RaiseIsActiveChanged);
            }
        }

        public event EventHandler IsActiveChanged;


        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
        public virtual void Destroy()
        {

        }
    }
}
