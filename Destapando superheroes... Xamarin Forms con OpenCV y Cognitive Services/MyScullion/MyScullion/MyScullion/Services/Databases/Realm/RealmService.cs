using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MyScullion.Models;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reactive;

namespace MyScullion.Services.Databases.Realm
{
    public class RealmService : IDatabaseService
    {
        private Realms.Realm database;

        public RealmService()
        {
            database = Realms.Realm.GetInstance(CustomDependencyService.Get<IPathService>().GetDatabasePath("Realm"));            
        }

        public Task<bool> Delete<T>(T item) where T : BaseModel, new()
        {
            var element = database.Find<WrapRealm<T>>(item.Id);

            if(element != null)
            {
                database.Remove(element);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<T> Get<T>(int id) where T : BaseModel, new()
        {
            return Task.FromResult(database.Find<WrapRealm<T>>(id).Model);
        }

        public Task<IEnumerable<T>> GetAll<T>() where T : BaseModel, new()
        {            
            return Task.FromResult(database.All<WrapRealm<T>>().ToList().Select(x => x.Model));
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

        public Task Insert<T>(T item) where T : BaseModel, new()
        {
            database.Add(new WrapRealm<T>(item));
            return Task.FromResult(Unit.Default);
        }

        public Task InsertAll<T>(List<T> items) where T : BaseModel, new()
        {
            foreach(var element in items)
            {
                Insert(element);
            }
            return Task.FromResult(Unit.Default);
        }                
    }
}
