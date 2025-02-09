namespace BeerApp.Views;

public partial class pageResume : ContentPage
{
    #region Variables

    private List<BeerData> beerData = new List<BeerData>();
    private float totalBeerMesure = 0;
    private int totalDrinkedBeers = 0;

    #endregion Variables

    #region Constructor

    public pageResume()
    {
        InitializeComponent();
    }

    #endregion Constructor

    #region Eventos

    #region "Eventos de Pagina"

    protected override void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            LoadData();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        totalBeerMesure = 0;
        totalDrinkedBeers = 0;
        beerData.Clear();
    }

    #endregion "Eventos de Pagina"

    #region "Eventos de CheckBox"

    private void chkFiltrer_CheckChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkFilter.IsChecked)
            {
                hslFilter.IsVisible = true;

                if (pickerBrand.Items.Count <= 0)
                {
                    pickerBrand.ItemsSource = mdlVariablesGlobales.llBeerBrand.OrderBy<BeerBrand, string>(x => x.Name).ToList();
                    pickerBrand.ItemDisplayBinding = new Binding("Name");

                    if (pickerBrand.Items.Count > 0) pickerBrand.SelectedIndex = 0;
                }
            }
            else
            {
                hslFilter.IsVisible = false;
                LoadData();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    #endregion "Eventos de CheckBox"

    #region "Eventos de Boton"

    private void btnAplicar_Clicked(object sender, EventArgs e)
    {
        try
        {
            List<BeerData> beerDataCopy = beerData;
            DateTime desde = dpDesde.Date;
            DateTime hasta = dpHasta.Date.AddDays(1).AddTicks(-1);
            BeerBrand beerBrand = pickerBrand.SelectedItem as BeerBrand;

            if (beerBrand != null)
            {
                var filteredBeerData = beerDataCopy
                .Where(beer =>
                    (beer.Created >= desde && beer.Created <= hasta) &&
                    (beerBrand == null || beer.IdBrand == beerBrand.Id))
                .ToList();

                BeerListView.ItemsSource = filteredBeerData;
                LoadNums(filteredBeerData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    #endregion "Eventos de Boton"

    #endregion Eventos

    #region Funciones

    private void LoadData()
    {
        try
        {
            if (beerData.Count == 0) beerData = mdlVariablesGlobales.db.Table<BeerData>().ToList();
           
            BeerListView.ItemsSource = beerData;

            LoadNums(beerData);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void LoadNums(List<BeerData> list)
    {
        try
        {
            totalBeerMesure = 0; totalDrinkedBeers = 0;

            foreach (BeerData beer in list)
            {
                if (beer.TypeMesure == "l") totalBeerMesure += (beer.Mesure * beer.Qtt);
                else if (beer.TypeMesure == "dl") totalBeerMesure += ConvertDlToL(beer.Mesure * beer.Qtt);
                else if (beer.TypeMesure == "cl") totalBeerMesure += ConvertClToL(beer.Mesure * beer.Qtt);
                else if (beer.TypeMesure == "ml") totalBeerMesure += ConvertMlToL(beer.Mesure * beer.Qtt); ;

                totalDrinkedBeers += beer.Qtt;
            }

            lblTotalBebidos.Text = totalDrinkedBeers.ToString();
            lblLitrosConsumidos.Text = totalBeerMesure.ToString("F2");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private float ConvertDlToL(float dl)
    {
        float result = 0;

        try
        {
            result = dl * 0.1f;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }

        return result;
    }

    private float ConvertClToL(float cl)
    {
        float result = 0;

        try
        {
            result = cl * 0.01f;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }

        return result;
    }

    private float ConvertMlToL(float ml)
    {
        float result = 0;

        try
        {
            result = ml * 0.001f;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }

        return result;
    }

    #endregion Functiones
}