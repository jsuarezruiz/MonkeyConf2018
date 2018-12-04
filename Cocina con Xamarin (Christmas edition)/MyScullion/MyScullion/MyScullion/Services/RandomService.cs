using System;
using System.Collections.Generic;
using System.Text;
using MyScullion.Models;

namespace MyScullion.Services
{
    public class RandomService : IRandomService
    {
        public List<RandomModel> CreateRandomData(int rows)
        {
            var randoms = new List<RandomModel>();

            for(int i = 0; i < rows; i ++)
            {
                randoms.Add(new RandomModel()
                {
                    Id = i + 1,
                    CreateDate = RandomDate(),
                    Description = RandomString(10),
                    Name = RandomString(2),
                    Price = RandomDouble(),
                    Quantity = RandomDouble()
                });
            }

            return randoms;
        }

        private string RandomString(int nWords)
        {
            var rand = new Random();

            var words = new[] {"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                               "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                               "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};


            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < nWords; i++)
            {
                builder.Append(words[rand.Next(words.Length)]);
            }

            return builder.ToString();
        }

        private DateTime RandomDate()
        {
            var rand = new Random();
            
            return DateTime.Now.AddDays(-rand.Next(365));
        }

        private double RandomDouble()
        {
            var random = new Random();
            return (random.Next(300) + random.NextDouble());
        }
    }
}
