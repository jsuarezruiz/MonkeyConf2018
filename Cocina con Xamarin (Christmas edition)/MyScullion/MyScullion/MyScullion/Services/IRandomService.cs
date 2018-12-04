using MyScullion.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScullion.Services
{
    public interface IRandomService
    {
        List<RandomModel> CreateRandomData(int rows);
    }
}
