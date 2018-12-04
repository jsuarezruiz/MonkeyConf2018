using System;
using System.Linq;

namespace MyScullion.Models
{
    public static class ExtensionMyScullion
    {
        public static Ingredient ToRawIngredientToIngredient(this Tuple<int,string> rawIngredient)
        {
            var values = rawIngredient.Item2.Split(',');

            var tags = values.ToList().Skip(2).ToList();
                
            if(values.Length > 2)
            {
                return new Ingredient()
                {
                    Id = rawIngredient.Item1,
                    Name = values[1],
                    Family = values[0],
                    Tags = tags
                };
            }
            else
            {
                return null;
            }
            
        }
    }
}
