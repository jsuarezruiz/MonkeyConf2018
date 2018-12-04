using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;

namespace MyScullion.Services
{
    public class FileEmbedddedService : IFileEmbeddedService
    {
        public List<string> GetFile(string name)
        {
            var data = new List<string>();

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

            if (stream !=  null)
            {
                var reader = new StreamReader(stream);

                while (reader.EndOfStream)
                {
                    data.Add(reader.ReadLine());
                }

                stream.Close();
            }
            
            return data;
        }
    }
}
