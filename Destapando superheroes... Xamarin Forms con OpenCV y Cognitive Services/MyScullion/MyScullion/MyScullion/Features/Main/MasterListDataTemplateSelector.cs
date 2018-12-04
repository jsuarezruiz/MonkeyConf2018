using Xamarin.Forms;

namespace MyScullion.Features.Main
{
    public class MasterListDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Cell { get; set; }
        public DataTemplate Separator { get; set; }
        
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var menuItem = (MasterDetailPageMenuItem)item;

            if(menuItem.GetType() == typeof(MasterDetailPageMenuItem))
            {
                return Cell;
            }
            else
            {
                return Separator;
            }
        }
    }
}
