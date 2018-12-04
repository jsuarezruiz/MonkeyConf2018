using System;
using System.Linq;
using System.Collections.Generic;

namespace MyScullion.Models
{
    public class Ingredient : BaseModel
    {
        public Ingredient()
        { }

        public Ingredient(string lineCsv)
        {
            var raw = lineCsv.Split(';').ToList();

            Id = int.Parse(raw[0]);
            Family = raw[1];
            Tags = raw.Skip(2).ToList();

        }

        public string Name { get; set; }

        public string Family { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{Name} | {Family}";
        }

        public string ToCSV()
        {
            string tagsDelimited = string.Empty;
            foreach (var tag in Tags)
            {
                tagsDelimited += tag.Trim() + ";";                
            }

            tagsDelimited.Remove(tagsDelimited.Length - 1);

            return $"{Id};{Name.Trim()};{Family.Trim()};{tagsDelimited}";
        }
    }
}
