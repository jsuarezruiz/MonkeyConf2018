using System;
using System.Collections.Generic;
using System.Text;

namespace MyScullion.Services
{
    public interface IFileEmbeddedService
    {
        List<string> GetFile(string name);
    }
}
