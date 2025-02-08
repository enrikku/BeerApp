using BeerApp.Models;

namespace BeerApp
{
    public static class mdlVariablesGlobales
    {
        public static string dbPath = Path.Combine(FileSystem.AppDataDirectory, "database.db");
        public static SQLiteConnection db = null;
        public static List<BeerBrand> llBeerBrand = null;
        public static List<string> llMesures = new List<string>
        {
            "l",
            "dl",
            "cl",
            "ml"
        };

        public static float totalL = 0.0f;
    } 
}