using CommunityToolkit.Maui.Storage;

namespace BeerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void miExportDatabase_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(mdlVariablesGlobales.dbPath))
                {
                    await DisplayAlert("Error", "No se encontró la base de datos.", "OK");
                    return;
                }

                var result = await FileSaver.Default.SaveAsync("database.db", File.OpenRead(mdlVariablesGlobales.dbPath));

                if (!string.IsNullOrEmpty(result.FilePath)) await DisplayAlert("Éxito", $"Base de datos guardada en: {result.FilePath}, no cambié el nombre del archivo.", "OK");
                else await DisplayAlert("Cancelado", "No se guardó la base de datos.", "OK");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("Error", "Ocurrió un error al guardar la base de datos.", "OK");
            }
        }

        private async void miImportDatabase_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(mdlVariablesGlobales.dbPath)) 
                { 
                    mdlVariablesGlobales.db.DeleteAll<BeerBrand>();
                    mdlVariablesGlobales.db.DeleteAll<BeerData>();

                    var result = await FilePicker.PickAsync(new PickOptions
                    {
                        PickerTitle = "Selecciona la base de datos SQLite"
                    });

                    if (result != null)
                    {
                        bool esValida = await EsBaseDeDatosValida(result.FullPath);
                        if (!esValida)
                        {
                            await DisplayAlert("Error", "El archivo seleccionado no es una base de datos SQLite válida.", "OK");
                            return;
                        }
                        
                        if (!result.FileName.EndsWith(".db"))
                        {
                            await DisplayAlert("Error", "El archivo debe ser una base de datos SQLite con extensión '.db'.", "OK");
                            return;
                        }
                        
                        if(result.FileName != "database.db")
                        {
                            DisplayAlert("Error", "La base de datos se tiene que llamar database.db", "OK");
                            return;
                        }



                        var filePath = result.FullPath;

                        var appDatabasePath = mdlVariablesGlobales.dbPath;

                        if (File.Exists(filePath))
                        {
                            if (File.Exists(appDatabasePath))
                            {
                                // Eliminar la base de datos actual
                                File.Delete(appDatabasePath); 
                            }

                            
                            File.Copy(filePath, appDatabasePath);

                            
                            await Application.Current.MainPage.DisplayAlert("Éxito", "La base de datos se importó correctamente. La aplicación se cerrara.", "OK");

                            Application.Current.Quit();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "El archivo seleccionado no existe.", "OK");
                        }
                    }
                    else
                    {
                        // El usuario no seleccionó ningún archivo
                        await Application.Current.MainPage.DisplayAlert("Error", "No se seleccionó ningún archivo.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task<bool> EsBaseDeDatosValida(string filePath)
        {
            try
            {
                const int SQLiteHeaderLength = 16;
                byte[] header = new byte[SQLiteHeaderLength];

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead = stream.Read(header, 0, SQLiteHeaderLength);

                    if (bytesRead == SQLiteHeaderLength)
                    {
                        var headerString = System.Text.Encoding.ASCII.GetString(header);
                        if (headerString.StartsWith("SQLite format 3"))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar la base de datos: {ex.Message}");
            }

            return false;
        }
    }
}
