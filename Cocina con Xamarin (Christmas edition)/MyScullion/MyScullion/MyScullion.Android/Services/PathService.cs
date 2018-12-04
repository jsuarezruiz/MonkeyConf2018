
using System.IO;
using MyScullion.Services;

namespace MyScullion.Droid.Services
{
    public class PathService : IPathService
    {
        public string GetDatabasePath(string suffix)
        {
            var databasePath = Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), 
                    $"{typeof(App).Namespace}{suffix}");
            
            if (!File.Exists(databasePath))
            {
                
                File.Create(databasePath).Dispose();
            }

            return databasePath;
        }
        
    }
}