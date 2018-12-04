using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using MyScullion.Models;
using SQLite;

namespace MyScullion.Services.Databases.OrmSqlite
{
    public class OrmSqliteService : IDatabaseService
    {
        private SQLite.SQLiteAsyncConnection connection;

        public OrmSqliteService()
        {
            connection = new SQLite.SQLiteAsyncConnection(CustomDependencyService.Get<IPathService>().GetDatabasePath("sqlite.db"));

            CreateTables();
        }

        private void CreateTables()
        {
            connection.CreateTableAsync<RandomModel>();
            connection.CreateTableAsync<Ingredient>();
            connection.CreateTableAsync<Measure>();
        }

        public async Task<bool> Delete<T>(T item) where T : BaseModel, new()
        {
            return await connection.DeleteAsync(item) > 0;
        }

        public Task<T> Get<T>(int id) where T : BaseModel, new()
        {            
            return connection.GetAsync<T>(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : BaseModel, new()
        {
            return await connection.Table<T>().ToListAsync();
        }

        public IObservable<T> GetAndFetch<T>(Func<Task<T>> restAction) where T : BaseModel, new()
        {
            var fetch = Observable.Defer(() => GetAll<T>().ToObservable())
                .SelectMany(_ =>
                {
                    var fetchObs = restAction().ToObservable().Catch<T, Exception>(ex =>
                    {
                        return Observable.Return(Unit.Default).SelectMany(x => Observable.Throw<T>(ex));
                    });
                    return fetchObs;
                });

            return fetch;
        }

        public async Task Insert<T>(T item) where T : BaseModel, new()
        {
            await connection.InsertAsync(item);            
        }

        public async Task InsertAll<T>(List<T> items) where T : BaseModel, new()
        {            
            await connection.InsertAllAsync(items);
        }
    }
}
