using MyScullion.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScullion.Maintanance
{
    class Program
    {
        private const string Database = @"D:\Projects\Namocode\MyScullion\MyScullion\MyScullion.Maintanance\bin\Debug\SR_Legacy.accdb";

        static void Main(string[] args)
        {
            var rawIngredients = new List<Tuple<int,string>>();
            var measures = new List<Measure>();

            if(File.Exists(Database))
            {
                rawIngredients = ReadRawIngredients();
                measures = ReadRawMeasures();
            }

            var ingredients = ConvertIngredients(rawIngredients);

            ConvertToCSVIngredients(ingredients);
            ConvertToCSVMeasures(measures);

            

            Console.WriteLine("Finito");
            Console.ReadKey();
        }

        private static OleDbConnection CreateConnection() => new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Database}");


        private static List<Tuple<int,string>> ReadRawIngredients()
        {
            var longDescriptionNameProducts = new List<Tuple<int, string>>();

            OleDbConnection connection = CreateConnection();

            connection.Open();

            var command = new OleDbCommand("SELECT NDB_No, Long_Desc From FOOD_DES WHERE IsNull(ManufacName)");
            command.Connection = connection;
            var reader = command.ExecuteReader();
            while(reader.Read())
            {                
                longDescriptionNameProducts.Add(new Tuple<int,string>(int.Parse(reader.GetString(0)), reader.GetString(1)));
            }
            
            connection.Close();

            return longDescriptionNameProducts;
        }

        private static List<Measure> ReadRawMeasures()
        {
            var measures = new List<Measure>();

            var connection = CreateConnection();
            connection.Open();

            var command = new OleDbCommand("SELECT NDB_No, Seq, Amount, Msre_Desc, Gm_Wgt FROM Weight");
            command.Connection = connection;

            var reader = command.ExecuteReader();

            while(reader.Read())
            {
                measures.Add(new Measure()
                {
                    IngredientId = int.Parse(reader.GetString(0)),
                    Id = int.Parse(reader.GetString(1)),
                    Amount = double.Parse(reader.GetValue(2).ToString()),
                    Description = reader.GetString(3),
                    Grams = reader.GetDouble(4)
                });
            }
                        
            connection.Close();
            return measures;
        }

        private static List<Ingredient> ConvertIngredients(List<Tuple<int,string>> rawIngredients)
        {
            var ingredients = new List<Ingredient>();

            foreach (var rawIngredient in rawIngredients)
            {
                var ingredient = rawIngredient.ToRawIngredientToIngredient();
                if(ingredient != null)
                {
                    ingredients.Add(ingredient);
                }                
            }

            return ingredients;
        }

        private static void PrintIngredients(List<Ingredient> ingredients)
        {
            foreach(var ing in ingredients)
            {
                Console.WriteLine(ing);
            }
        }

        private static void PrintMeasures(List<Measure> measures)
        {
            foreach(var measure in measures)
            {
                Console.WriteLine(measure);
            }
        }

        private static void ConvertToCSVIngredients(List<Ingredient> ingredients)
        {
            StreamWriter writer = new StreamWriter("ingredients.csv");

            foreach(var ing in ingredients)
            {
                writer.WriteLine(ing.ToCSV());
            }

            writer.Close();
        }

        private static void ConvertToCSVMeasures(List<Measure> measures)
        {
            StreamWriter writer = new StreamWriter("measures.csv");

            foreach(var measure in measures)
            {
                writer.WriteLine(measure.ToCSV());
            }

            writer.Close();
        }
    }
}
