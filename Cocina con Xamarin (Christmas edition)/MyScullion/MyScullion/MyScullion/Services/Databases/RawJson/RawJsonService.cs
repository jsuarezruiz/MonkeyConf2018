using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MyScullion.Models;
using Newtonsoft.Json;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reactive;

namespace MyScullion.Services.Databases.RawJson
{
    public class RawJsonService : IDatabaseService
    {
        private Dictionary<Type, object> collectionJson = new Dictionary<Type, object>();
        private IPathService pathService;

        public RawJsonService()
        {
            pathService = CustomDependencyService.Get<IPathService>();
        }

        public Task<bool> Delete<T>(T item) where T : BaseModel, new()
        {
            var collection = (List<T>)GetCollection(typeof(T));

            if(collection != null)
            {
                var element = collection.FirstOrDefault(x => x.Id == item.Id);

                if(element != null)
                {
                    collection.Remove(element);
                    Write(PathJson(typeof(T)), JsonConvert.SerializeObject(collection));
                }
            }

            return Task.FromResult(false);
        }

        public Task<T> Get<T>(int id) where T : BaseModel, new()
        {
            var collection = (List<T>)GetCollection(typeof(T));

            if(collection != null)
            {
                return Task.FromResult(collection.FirstOrDefault(x => x.Id == id));
            }

            return null;
        }

        public Task<IEnumerable<T>> GetAll<T>() where T : BaseModel, new()
        {
            return Task.FromResult((IEnumerable<T>)GetCollection(typeof(T)));
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
            var collection = (List<T>)GetCollection(typeof(T));
            if (collection == null)
            {
                collection = new List<T>();
            }

            collection.Add(item);

            Write(PathJson(typeof(T)), JsonConvert.SerializeObject(collection));

            return Task.FromResult(Unit.Default);
        }

        public Task InsertAll<T>(List<T> items) where T : BaseModel, new()
        {
            var collection = (List<T>)GetCollection(typeof(T));

            if (collection == null)
            {
                collection = new List<T>();
            }

            collection.AddRange(items);

            Write(PathJson(typeof(T)), JsonConvert.SerializeObject(collection));

            return Task.FromResult(Unit.Default);
        }

        private void Store(Type type, object collection)
        {
            if(collectionJson.ContainsKey(type))
            {
                collectionJson[type] = collection;
            }
            else
            {
                collectionJson.Add(type, collection);
            }
        }
        
        private object GetCollection(Type type)
        {
            if(collectionJson.ContainsKey(type))
            {
                return collectionJson[type];
            }

            var path = PathJson(type);

            if (File.Exists(path))
            {
                var content = ReadFile(path);
                var collection = JsonConvert.DeserializeObject(content, type);

                Store(type, collection);

                return collection;
            }

            return null;
        }

        private string PathJson(Type type) => pathService.GetDatabasePath($"Json{type.Name}.json");

        private string ReadFile(string path)
        {
            string content = string.Empty;
            var fileStream = new FileStream(path, FileMode.OpenOrCreate);
            var reader = new StreamReader(fileStream);

            content = reader.ReadToEnd();
            fileStream.Close();
            fileStream.Dispose();
                                        
            return content;            
        }

        private void Write(string path, string content)
        {
            var fileStream = new FileStream(path, FileMode.OpenOrCreate);
            var writer = new StreamWriter(fileStream);
            writer.Write(content);

            writer.Close();
            writer.Dispose();
        }        
    }
}
