using MyScullion.Features.Main;
using System.Collections.Generic;

namespace MyScullion.Services
{
    public interface IMenuService
    {
        List<MasterDetailPageMenuItem> GetMenuItems();
    }
}
