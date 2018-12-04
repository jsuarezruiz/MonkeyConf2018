using System;
using System.Collections.Generic;
using System.Text;

namespace MyScullion.Models
{
    public class RandomModel : BaseModel
    {        
        public string Name { get; set; }

        public string Description { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
