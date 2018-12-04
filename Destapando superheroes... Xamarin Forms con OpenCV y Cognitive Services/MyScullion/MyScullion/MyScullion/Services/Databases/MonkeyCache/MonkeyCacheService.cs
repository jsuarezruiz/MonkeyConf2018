using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using MyScullion.Models;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reactive;

namespace MyScullion.Services.Databases.MonkeyCache
{
    public class MonkeyCacheService : IDatabaseService
    {
        public MonkeyCacheService()
        {
            Barrel.ApplicationId = typeof(App).Namespace;
        }

        public Task<bool> Delete<T>(T item) where T : BaseModel, new()
        {
            var collection = Barrel.Current.Get<IEnumerable<T>>(typeof(T).Name).ToList();
            var element = collection.FirstOrDefault(x => x.Id == item.Id);

            if(element != null)
            {
                collection.Remove(element);
                Barrel.Current.Empty(typeof(T).Name);
                InsertAll(collection);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<T> Get<T>(int id) where T : BaseModel, new()
        {
            return Task.FromResult(Barrel.Current.Get<IEnumerable<T>>(typeof(T).Name).FirstOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<T>> GetAll<T>() where T : BaseModel, new()
        {
            return Task.FromResult(Barrel.Current.Get<IEnumerable<T>>(typeof(T).Name));
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
            //Probably not working.
            Barrel.Current.Add<T>(typeof(T).Name, item, TimeSpan.FromDays(30));
            return Task.FromResult(Unit.Default);
        }

        public Task InsertAll<T>(List<T> items) where T : BaseModel, new()
        {
            Barrel.Current.Add<IEnumerable<T>>(typeof(T).Name, items, TimeSpan.FromDays(30));
            return Task.FromResult(Unit.Default);
        }


    }
}
