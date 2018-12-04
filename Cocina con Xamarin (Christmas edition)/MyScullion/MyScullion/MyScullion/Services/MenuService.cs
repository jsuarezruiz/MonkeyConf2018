using System.Collections.Generic;
using MyScullion.Features.Main;
using MyScullion.Features.Test;
using MyScullion.Services;
using MyScullion.Services.Databases;
using MyScullion.Services.Databases.LiteDB;
using MyScullion.Services.Databases.MonkeyCache;
using MyScullion.Services.Databases.OrmSqlite;
using MyScullion.Services.Databases.RawJson;
using MyScullion.Services.Databases.RawSqlite;
using MyScullion.Services.Databases.Realm;

[assembly: Xamarin.Forms.Dependency(typeof(MenuService))]
namespace MyScullion.Services
{    
    public class MenuService : IMenuService
    {
        public List<MasterDetailPageMenuItem> GetMenuItems()
        {
            return new List<MasterDetailPageMenuItem>()
            {
                new MasterDetailPageMenuItem(typeof(AkavacheService))
                {
                    Id = 10,
                    Title = "Akavache"
                },
                new MasterDetailPageMenuItem(typeof(MonkeyCacheService))
                {
                    Id = 20,
                    Title = "MonkeyCache"
                },
                new MasterDetailPageMenuItem(typeof(LiteDBService))
                {
                    Id = 30,
                    Title = "LiteDB"
                },
                new MasterDetailPageMenuItem(typeof(RawJsonService))
                {
                    Id = 40,
                    Title = "RawJson"
                },
                new MasterDetailPageMenuItem(typeof(RealmService))
                {
                    Id = 50,
                    Title = "Realm"
                },
                new MasterDetailPageMenuItem(typeof(OrmSqliteService))
                {
                    Id = 60,
                    Title = "OrmSqlite"
                },
                new MasterDetailPageMenuItem(typeof(RawSQLiteService))
                {
                    Id = 70,
                    Title = "RawSQLite"
                },
                new SeparatorMasterDetail(),

                new MasterDetailPageMenuItem(typeof(TestView))
                {
                    Id = 11,
                    Title = "Test database view"
                }
            };
        }
    }
}
