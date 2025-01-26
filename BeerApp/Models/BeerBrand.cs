namespace BeerApp.Models
{
    public class BeerBrand
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}