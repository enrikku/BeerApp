namespace BeerApp
{
    public partial class MainPage : ContentPage
    {
        #region Constructor

        public MainPage()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region "Eventos"

        #region "Eventos de Pagina"

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                pTypeBeer.ItemsSource = mdlVariablesGlobales.llBeerBrand.OrderBy<BeerBrand, string>(x => x.Name).ToList();
                pTypeBeer.ItemDisplayBinding = new Binding("Name");

                if (pTypeBeer.Items.Count > 0) pTypeBeer.SelectedIndex = 0;

                pMesure.ItemsSource = mdlVariablesGlobales.llMesures;

                if (pMesure.Items.Count > 0) pMesure.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion "Eventos de Pagina"

        #region "Eventos de Stepper"

        private void sQttBeers_Changed(object sender, EventArgs e)
        {
            try
            {
                eQttBeers.Text = sQttBeers.Value.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion "Eventos de Stepper"

        #region "Eventos de Picker"

        private void pMesure_Changed(object sender, EventArgs e)
        {
            try
            {
                if (pMesure.SelectedItem != null)
                {
                    string selectedMesure = pMesure.SelectedItem.ToString();

                    switch (selectedMesure)
                    {
                        case "l":
                            eQttMesure.Text = "1";
                            break;

                        case "ml":
                            eQttMesure.Text = "330";
                            break;

                        case "cl":
                            eQttMesure.Text = "33";
                            break;

                        case "dl":
                            eQttMesure.Text = "3.3";
                            break;

                        default:
                            eQttMesure.Text = "0";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion "Eventos de Picker"

        #region "Eventos de Button"

        private void btnAceptar_Clicked(object sender, EventArgs e)
        {
            try
            {
                BeerData beerData = new BeerData();

                if (CanInsertBeer())
                {
                    BeerBrand loBeerBrand = pTypeBeer.SelectedItem as BeerBrand;

                    if (loBeerBrand != null)
                    {
                        beerData.IdBrand = loBeerBrand.Id;
                        beerData.Qtt = Convert.ToInt32(sQttBeers.Value);
                        beerData.Mesure = Convert.ToInt32(eQttMesure.Text);
                        beerData.TypeMesure = pMesure.SelectedItem.ToString();
                        beerData.Created = DateTime.Now;

                        if (mdlVariablesGlobales.db.Insert(beerData) == 1) Toast.Make("Cerveza insertada").Show();
                        else Toast.Make("Error al insertar la cerveza").Show();
                    }
                }
                else DisplayAlert("Error", "Los datos de la cerveza no son válidos", "Aceptar");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion "Eventos de Button"

        #endregion "Eventos"

        #region "Funciones"

        private bool CanInsertBeer()
        {
            try
            {
                bool isTypeBeerValid = pTypeBeer.SelectedItem != null;
                bool isQuantityValid = !string.IsNullOrWhiteSpace(eQttBeers.Text) && eQttBeers.Text != "0";
                bool isMesureValid = pMesure.SelectedItem != null;

                return isTypeBeerValid && isQuantityValid && isMesureValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CanInsertBeer: {ex.Message}");
                return false;
            }
        }

        #endregion "Funciones"
    }
}