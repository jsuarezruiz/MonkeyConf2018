using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Data.Sqlite;
using System.Runtime.InteropServices;
using MyScullion.Utils;


#if DROID
namespace MyScullion.Droid.Services
#elif __IOS__
namespace MyScullion.iOS.Services
#endif
{
    public class DatabaseBase
    {
#if DROID
        [DllImport("libsqlite.so")]
        internal static extern int sqlite3_shutdown();

        [DllImport("libsqlite.so")]
        internal static extern int sqlite3_initialize();
#endif


        protected void ApplySerialized()
        {
#if DROID
            sqlite3_shutdown();
            SqliteConnection.SetConfig(SQLiteConfig.Serialized);
            sqlite3_initialize();
#endif
        }

        private SqliteConnection _con;

        protected SqliteConnection Connection
        {
            get
            {
                return _con;
            }
            set
            {
                _con = value;
            }
        }
        protected void CreateTable(string query)
        {
            try
            {
                LaunchSQL(query);
            }
            catch (Exception e)
            {
                if (e is SqliteException)
                {
                    if (!e.Message.Contains("already exists"))
                    {
                        throw e;
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        protected void DropTable(string tableName)
        {
            try
            {
                LaunchSQL($"DROP TABLE IF EXISTS {tableName}");
            }
            catch(Exception e)
            {
                Log.Trace(e.Message);
            }
        }
        
        protected List<TModel> Select<TModel>(string sql, Func<object, List<TModel>> serializeAction) 
        {
            Connection.Open();
            var com = PrepareCommand(sql);
            var reader = com.ExecuteReader();
            com.Dispose();

            var list = serializeAction.Invoke(reader);
            reader.Close();
            reader.Dispose();
            Connection.Close();
            return list;
        }
        
        protected List<TModel> Select<TModel>(string sql, List<object> parameters, Func<object, List<TModel>> serializeAction) 
        {
            Connection.Open();
            var com = PrepareCommand(sql, parameters.Select(x => (SqliteParameter)x).ToList());
            var reader = com.ExecuteReader();
            com.Dispose();

            var list = serializeAction.Invoke(reader);
            reader.Close();
            reader.Dispose();
            Connection.Close();
            return list;
        }
        
        protected TElement SelectFirst<TElement>(string sql, List<object> parameters, Func<object, TElement> serializeAction)
        {
            var com = PrepareCommand(sql, parameters.Select(x => (SqliteParameter)x).ToList());
            var reader = com.ExecuteReader();
            com.Dispose();

            var list = serializeAction.Invoke(reader);
            reader.Close();
            reader.Dispose();
            return list;
        }

        protected void Insert(string sql, List<object> parameters)
        {            
            DoTransaction(sql, parameters);
        }
        
        public void Update(string sql, List<object> parameters)
        {            
            DoTransaction(sql, parameters);
        }

        public void Delete(string sql, List<object> parameters)
        {            
            DoTransaction(sql, parameters);
        }

        public void InsertAll(string sql, List<List<object>> parameters)
        {
            Connection.Open();
            var command = _con.CreateCommand();
            var transaction = _con.BeginTransaction();
            command.CommandText = sql;

            try
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddRange(parameter.Select(x => (SqliteParameter)x).ToArray());
                    command.ExecuteNonQuery();
                    Log.Trace($"Execute Insert {sql}");
                }

                transaction.Commit();
            }
            catch (Exception e)
            {                
                transaction.Rollback();
            }
            finally
            {
                command.Dispose();
                Connection.Close();
            }            
        }

        public void InsertAllWOTransaction(string sql, List<List<object>> parameters)
        {
            var command = _con.CreateCommand();            
            command.CommandText = sql;

            try
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddRange(parameter.Select(x => (SqliteParameter)x).ToArray());
                    command.ExecuteNonQuery();
                }                
            }
            catch (Exception e)
            {                
                throw e;           
            }
            finally
            {
                command.Dispose();
            }
        }

        public void LaunchSQL(string sql)
        {
            DoTransaction(sql, new List<object>());
        }

        private void DoTransaction(string sql, List<object> parameters)
        {
            Connection.Open();
            var command = PrepareCommand(sql, parameters.Select(x => (SqliteParameter)x).ToList());

            try
            {                
                command.ExecuteNonQuery();                
            }
            catch (Exception e)
            {                
                throw e;
            }
            finally
            {
                command.Dispose();
                Connection.Close();
            }            
        }

        private SqliteCommand PrepareCommand(string sql)
        {
            return PrepareCommand(sql, new List<SqliteParameter>());
        }

        private SqliteCommand PrepareCommand(string sql, List<SqliteParameter> parameters)
        {
            var com = _con.CreateCommand();            
            com.CommandText = sql;
            com.Parameters.AddRange(parameters.ToArray());

            return com;
        }

        
    }
}