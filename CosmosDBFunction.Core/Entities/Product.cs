using CosmosDBFunction.Core.Entities.Base;

namespace CosmosDBFunction.Core.Entities
{
    public class Product : EntityBase
    {
        public string Category { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }


        public int PriceWithTax(int rateParcent)
        {
            return this.Price * (rateParcent / 100) + this.Price;
        }
    }
}
