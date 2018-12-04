using System;
using System.Collections.Generic;
using System.Text;

namespace MyScullion.Services
{
    public interface IPathService
    {
        string GetDatabasePath(string suffix);
    }
}
