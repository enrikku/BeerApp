using BeerApp.Models;

namespace BeerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try 
            {
                MainPage = new AppShell();

                CreateDatabaseIfNotExists();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateDatabaseIfNotExists()
        {
            try
            {
                // Si no existe la base de datos, la creamos y inicializamos las tablas
                if (!File.Exists(mdlVariablesGlobales.dbPath))
                {
                    mdlVariablesGlobales.db = new SQLiteConnection(mdlVariablesGlobales.dbPath);

                    InitializeBeerBrands();
                    InitializeBeerData();
                }
                else
                {
                    mdlVariablesGlobales.db = new SQLiteConnection(mdlVariablesGlobales.dbPath);
                    mdlVariablesGlobales.llBeerBrand = mdlVariablesGlobales.db.Table<BeerBrand>().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InitializeBeerBrands()
        {
            try
            {
                mdlVariablesGlobales.db.CreateTable<BeerBrand>();

                List<BeerBrand> brands = new List<BeerBrand>
                {
                    new BeerBrand { Name = "Heineken" },
                    new BeerBrand { Name = "Corona" },
                    new BeerBrand { Name = "Guinness" },
                    new BeerBrand { Name = "Paulaner" },
                    new BeerBrand { Name = "Estrella Galicia" },
                    new BeerBrand { Name = "Cruzcampo" },
                    new BeerBrand { Name = "Voll-Damm" },
                    new BeerBrand { Name = "Turia" },
                    new BeerBrand { Name = "Alhambra" },
                    new BeerBrand { Name = "Amstel" },
                    new BeerBrand { Name = "Mahou Cinco Estrellas" },
                    new BeerBrand { Name = "San Miguel" },
                    new BeerBrand { Name = "Estrella Damm" },
                    new BeerBrand { Name = "Águila" },
                    new BeerBrand { Name = "La Virgen" },
                    new BeerBrand { Name = "Montseny" },
                    new BeerBrand { Name = "Rosita" },
                    new BeerBrand { Name = "Cibeles" },
                    new BeerBrand { Name = "La Sagra" },
                    new BeerBrand { Name = "Moritz" },
                    new BeerBrand { Name = "Castelló Beer Factory" },
                    new BeerBrand { Name = "Tyris" },
                    new BeerBrand { Name = "Dougall's" },
                    new BeerBrand { Name = "Caleya" },
                    new BeerBrand { Name = "Balate" },
                    new BeerBrand { Name = "Trinitaria" },
                    new BeerBrand { Name = "Mustache" },
                    new BeerBrand { Name = "Cierzo Brewing Co." },
                    new BeerBrand { Name = "Naparbier" },
                    new BeerBrand { Name = "Guineu" },
                    new BeerBrand { Name = "Otras" }
                };

                mdlVariablesGlobales.db.InsertAll(brands);
                mdlVariablesGlobales.llBeerBrand = mdlVariablesGlobales.db.Table<BeerBrand>().ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InitializeBeerData()
        {
            try
            {
                mdlVariablesGlobales.db.CreateTable<BeerData>();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
