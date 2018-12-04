using Akavache;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyScullion.Models;
using System.Reactive;

namespace MyScullion.Services.Databases
{
    public class AkavacheService : IDatabaseService
    {
        private readonly IBlobCache localBlobCache;

        public AkavacheService()
        {
            Akavache.BlobCache.ApplicationName = typeof(App).Namespace;
            localBlobCache = BlobCache.LocalMachine;
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : BaseModel, new()
        {
            return await localBlobCache.GetAllObjects<T>();
        }

        public Task<T> Get<T>(int id) where T : BaseModel, new()
        {
            return Task.FromResult(localBlobCache.GetObject<T>(typeof(T).Name).FirstOrDefault(x => x.Id == id));
        }

        public IObservable<T> GetAndFetch<T>(Func<Task<T>> restAction) where T : BaseModel, new()
        {
            return localBlobCache.GetAndFetchLatest(typeof(T).Name, restAction);                    
        }

        public Task Insert<T>(T item) where T : BaseModel, new()
        {
            localBlobCache.InsertObject<T>(typeof(T).Name, item);
            return Task.FromResult(Unit.Default);
        }

        public Task InsertAll<T>(List<T> items) where T : BaseModel, new()
        {
            foreach(var item in items)
            {
                Insert(item);
            }
            return Task.FromResult(Unit.Default);
        }

        public async Task<bool> Delete<T>(T item) where T : BaseModel, new()
        {
            var collection = (await GetAll<T>()).ToList();

            var element = collection.FirstOrDefault(x => x.Id == item.Id);

            if(element != null)
            {
                collection.Remove(element);
                await localBlobCache.Invalidate(typeof(T).Name);
                InsertAll(collection);
            }
            
            return false;
        }
        
    }
}
