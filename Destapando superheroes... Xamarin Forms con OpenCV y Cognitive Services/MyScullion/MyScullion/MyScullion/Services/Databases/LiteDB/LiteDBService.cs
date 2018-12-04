using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using MyScullion.Models;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reactive;

namespace MyScullion.Services.Databases.LiteDB
{
    public class LiteDBService : IDatabaseService
    {
        //TODO: Maybe cache collections from LiteDB.

        private LiteDatabase database;

        public LiteDBService()
        {
            database = new LiteDatabase(CustomDependencyService.Get<IPathService>().GetDatabasePath("LiteDB"));
            MapModels();
        }

        private void MapModels()
        {
            var mapper = BsonMapper.Global;
            mapper.Entity<BaseModel>().Id(x => x.Id);
        }

        public Task<bool> Delete<T>(T item) where T : BaseModel, new()
        {            
            return Task.FromResult(database.Engine.Delete(typeof(T).Name, item.Id));
        }

        public Task<T> Get<T>(int id) where T : BaseModel, new()
        {
            var coll = database.GetCollection<T>();
            return Task.FromResult(coll.FindById(new BsonValue(id)));
        }

        public Task<IEnumerable<T>> GetAll<T>() where T : BaseModel, new()
        {            
            var coll = database.GetCollection<T>();
            var collF = coll.FindAll();
            return Task.FromResult(collF);            
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
            var coll = database.GetCollection<T>();

            var existing = coll.FindById(item.Id);

            if(existing == null)
            {
                coll.Insert(item);
            }
            else
            {
                coll.Update(item);
            }

            return Task.FromResult(Unit.Default);
        }

        public Task InsertAll<T>(List<T> items) where T : BaseModel, new()
        {
            var coll = database.GetCollection<T>();

            coll.Insert(items);

            return Task.FromResult(Unit.Default);
        }

        
    }
}
