namespace ApiTest.Contracts
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

        public Product()
        {
            
        }

        // Copy constructor
        public Product(Product item)
        {
            Id = item.Id;
            Name = item.Name;
            Price = item.Price;
            Created = item.Created;
            LastUpdated = item.LastUpdated;
        }
    }

}
