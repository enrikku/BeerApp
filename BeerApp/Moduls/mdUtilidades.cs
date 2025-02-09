using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BeerApp.Moduls
{
    public class mdUtilidades
    {
        public static float GetTotalLitros(List<BeerData> list)
        {
            float totalBeerMesure = 0;

            try
            {
                if (list.Count <= 0) totalBeerMesure = 0;

                foreach (BeerData beer in list)
                {
                    if (beer.TypeMesure == "l") totalBeerMesure += (beer.Mesure * beer.Qtt);
                    else if (beer.TypeMesure == "dl") totalBeerMesure += ConvertDlToL(beer.Mesure * beer.Qtt);
                    else if (beer.TypeMesure == "cl") totalBeerMesure += ConvertClToL(beer.Mesure * beer.Qtt);
                    else if (beer.TypeMesure == "ml") totalBeerMesure += ConvertMlToL(beer.Mesure * beer.Qtt); ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0.0f;
            }

            return totalBeerMesure;
        }

        public static int GetTotalBeers(List<BeerData> list)
        {
            int total = 0;

            try
            {
                foreach (BeerData beer in list)
                {
                    total += beer.Qtt ;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

            return total;
        }

        private static float ConvertDlToL(float dl)
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

        private static float ConvertClToL(float cl)
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

        private static float ConvertMlToL(float ml)
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
    }
}
