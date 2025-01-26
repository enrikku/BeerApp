namespace BeerApp.Models
{
    public class BeerData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdBrand { get; set; }
        public int Qtt { get; set; }
        public string TypeMesure { get; set; }
        public float Mesure { get; set; }
        public DateTime? Created { get; set; }
    }
}
