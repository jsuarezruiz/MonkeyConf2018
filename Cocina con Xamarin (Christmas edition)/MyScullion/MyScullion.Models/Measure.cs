namespace MyScullion.Models
{
    public class Measure : BaseModel
    {
        public Measure()
        { }

        public Measure(string measureLine)
        {
            var raw = measureLine.Split(';');

            Id = int.Parse(raw[0]);
            IngredientId = int.Parse(raw[1]);
            Description = raw[2];
            Amount = double.Parse(raw[3]);
            Grams = double.Parse(raw[4]);
        }

        public int IngredientId { get; set; }
       
        public string Description { get; set; }

        public double Amount { get; set; }

        public double Grams { get; set; }

        public override string ToString() => $"{Amount} {Description} {Grams}";

        public string ToCSV()
        {
            return $"{IngredientId};{Id};{Description.Trim()};{Amount};{Grams}";
        }
        
    }
}
