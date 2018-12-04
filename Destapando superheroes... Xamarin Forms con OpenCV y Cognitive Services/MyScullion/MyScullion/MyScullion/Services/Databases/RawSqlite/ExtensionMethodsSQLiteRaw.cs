using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Data.Sqlite;
using MyScullion.Models;

namespace MyScullion.Services.Databases.RawSqlite
{
    public static class ExtensionMethodsSQLiteRaw
    {
        public static string ToCreateQuery(this BaseModel model)
        {
            var firstPart = $"CREATE TABLE IF NOT EXISTS [{model.GetType().Name}]";
            
            var secondPart = string.Join(",", PrepareFieldsToCreate(model));

            return $"{firstPart} ({secondPart})";
        }

        private static List<string> PrepareFieldsToCreate(BaseModel model)
        {
            var fields = new List<string>();
            var properties = model.GetType().GetProperties().ToList();
            properties.Remove(properties.FirstOrDefault(x => x.Name == "Id"));

            fields.Add("[Id] INTEGER UNIQUE NULL");

            foreach (var property in properties)
            {
                var field = $"[{property.Name}] {property.PropertyType.Name}";
                //Check it's null or not null to create de query for create table more correctly
                if(Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    field += " NULL";
                }
                else
                {
                    field += " NOT NULL";
                }

                fields.Add(field);
            }

            return fields;
        }
        
        public static T Serialize<T>(object obj) where T : BaseModel, new()
        {
            var reader = (SqliteDataReader)obj;            
            var fieldsAndOrdinals = GetOrdinals(reader, GetFieldsFromTable<T>());
            
            try
            {
                if(reader.Read())
                {
                    return ReadFromReader<T>(reader, fieldsAndOrdinals);
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            return default(T);
        }

        public static List<T> SerializeCollection<T>(object obj) where T : BaseModel, new()
        {
            var list = new List<T>();
            var reader = (SqliteDataReader)obj;
            var fieldsAndOrdinals = GetOrdinals(reader, GetFieldsFromTable<T>());

            try
            {
                while(reader.Read())
                {
                    list.Add(ReadFromReader<T>(reader, fieldsAndOrdinals));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            return list;
        }

        private static Dictionary<int, PropertyInfo> GetOrdinals(SqliteDataReader reader, List<PropertyInfo> list)
        {
            var fieldsAndOrdinals = new Dictionary<int, PropertyInfo>();
            foreach(var property in list)
            {
                var ordinal = reader.GetOrdinal(property.Name);
                if(ordinal != -1 && !fieldsAndOrdinals.ContainsKey(ordinal))
                {
                    fieldsAndOrdinals.Add(ordinal, property);
                }
            }

            return fieldsAndOrdinals;
        }

        private static T ReadFromReader<T>(SqliteDataReader reader, Dictionary<int, PropertyInfo> keyValues) where T : BaseModel, new()
        {
            //It's more slow than GetOrdinals, and create your own serialize, but. It's only one.
            var myT = new T();

            foreach (var keyValue in keyValues)
            {
                keyValue.Value.SetValue(myT, reader.GetValue(keyValue.Key));
            }
                        
            return myT;
        }

        private static string GetTypeDatabase(Type type)
        {
            if(type == typeof(string))
            {
                return "NVARCHAR(100)";
            }
            else if(type == typeof(int))
            {
                return "INTEGER";
            }
            else if(type == typeof(float))
            {
                return "FLOAT";
            }
            else if(type == typeof(double))
            {
                return "FLOAT";
            }
            else
            {
                return string.Empty;
            }
        }

        public static List<object> GetIdParameter(this BaseModel model) => new List<object> {
                                                                                new SqliteParameter(model.GetType().Name, model.Id) };
        
        public static string ToDeleteQuery(this BaseModel model)
        {
            return $"DELETE FROM {model.GetType().Name} WHERE Id = @Id";
        }

        private static List<PropertyInfo> GetFieldsFromTable<T>() where T : BaseModel, new() => GetFieldsFromTable(typeof(T));

        private static List<PropertyInfo> GetFieldsFromTable(Type t) => t.GetProperties().ToList();



        public static string PrepareFieldsToSelectOrInsert<T>(string include = "") where T : BaseModel, new() => 
                    string.Join(",", GetFieldsFromTable<T>().Select(x => $"{include}{x.Name}"));

        public static List<object> GetParameters<T>(this BaseModel item) where T : BaseModel, new()
               => GetFieldsFromTable(item.GetType()).Select(x => new SqliteParameter($"@{x.Name}", x.GetValue(item)))
                        .OfType<object>()
                        .ToList();

        public static string ToInsertQuery<T>() where T : BaseModel, new() =>         
                                  $"INSERT OR REPLACE INTO {typeof(T).Name} " +
                                  $"({ExtensionMethodsSQLiteRaw.PrepareFieldsToSelectOrInsert<T>()}, " +
                                  $"VALUES ({ExtensionMethodsSQLiteRaw.PrepareFieldsToSelectOrInsert<T>("@")}";
        


    }    
}
