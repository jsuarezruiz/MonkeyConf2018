using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyScullion.Models;

namespace MyScullion.Services.Databases.RawSqlite
{
    public class RawSQLiteService : IDatabaseService
    {
        private IRawSQLitePlatformService platformService;

        public RawSQLiteService()
        {
            platformService = CustomDependencyService.Get<IRawSQLitePlatformService>();
        }

        public Task<bool> Delete<T>(T item) where T : BaseModel, new()
        {
            return platformService.Delete<T>(item);
        }

        public Task<T> Get<T>(int id) where T : BaseModel, new()
        {
            return platformService.Get<T>(id);
        }

        public Task<IEnumerable<T>> GetAll<T>() where T : BaseModel, new()
        {
            return platformService.GetAll<T>();
        }

        public IObservable<T> GetAndFetch<T>(Func<Task<T>> restAction) where T : BaseModel, new()
        {
            return platformService.GetAndFetch<T>(restAction);
        }

        public Task Insert<T>(T item) where T : BaseModel, new()
        {
            return platformService.Insert<T>(item);
        }

        public Task InsertAll<T>(List<T> items) where T : BaseModel, new()
        {
            return platformService.InsertAll<T>(items);
        }
    }
}
