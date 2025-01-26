using System.Globalization;

namespace BeerApp.Converters;
public class BrandNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int idBrand = (int)value;
        string name = "";

        foreach (var item in mdlVariablesGlobales.llBeerBrand) 
        {
            if (item.Id == idBrand)
            {
                name = item.Name;
                break;
            }
        }

        if(name != "") return name;
        else return "Marca desconocida";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
