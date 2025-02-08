using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using BeerApp.Moduls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerApp.Platforms.Android
{
    [BroadcastReceiver(Label = "Mi Widget", Enabled = true, Exported = true)]
    [IntentFilter(new [] { "android.appwidget.action.APPWIDGET_UPDATE", "com.enrikku.beerapp.UPDATE_WIDGET" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/widget_info")]
    public class CircleWidget : AppWidgetProvider
    {
        #region "Eventos"

        /// <summary>
        /// Se ejecuta cuando el widget se agrega por primera vez a la pantalla de inicio.
        /// </summary>
        /// <param name="context"></param>
        public override void OnEnabled(Context context)
        {
            base.OnEnabled(context);
            ScheduleUpdate(context);
        }

        /// <summary>
        /// Este es el método principal de actualización. Se ejecuta cuando el widget necesita ser actualizado.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appWidgetManager"></param>
        /// <param name="appWidgetIds"></param>
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            foreach (int widgetId in appWidgetIds)
            {
                RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.widget_layout);

                Intent serviceIntent = new Intent(context, typeof(WidgetService));
                views.SetRemoteAdapter(Resource.Id.widgetListView, serviceIntent);

                var llBeerData = mdlVariablesGlobales.db.Table<BeerData>().ToList();
                views.SetTextViewText(Resource.Id.totalLitrosTextView, "Total: " + mdUtilidades.GetTotalLitros(llBeerData).ToString("F2") + "L");
                views.SetTextViewText(Resource.Id.totalBeers, $"Total: {llBeerData.Count}");

                appWidgetManager.UpdateAppWidget(widgetId, views);
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            try
            {
                if (intent.Action == AppWidgetManager.ActionAppwidgetUpdate || intent.Action == "com.tuapp.ACTUALIZAR_WIDGET")
                {
                    var appWidgetManager = AppWidgetManager.GetInstance(context);
                    var widgetIds = intent.GetIntArrayExtra(AppWidgetManager.ExtraAppwidgetIds);

                    foreach (var widgetId in widgetIds)
                    {
                        var views = new RemoteViews(context.PackageName, Resource.Layout.widget_layout);
                        var llBeerData = mdlVariablesGlobales.db.Table<BeerData>().ToList();
                        views.SetTextViewText(Resource.Id.totalLitrosTextView, "Total: " + mdUtilidades.GetTotalLitros(llBeerData).ToString("F2") + "L");
                        views.SetTextViewText(Resource.Id.totalBeers, $"Total: {llBeerData.Count}");
                        appWidgetManager.UpdateAppWidget(widgetId, views);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion "Eventos"

        #region "Funciones"

        private void ScheduleUpdate(Context context)
        {
            try
            {
                if (context != null)
                {
                    // Obtener el servicio AlarmManager del sistema para programar alarmas
                    AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

                    // Crear un Intent que apunta al BroadcastReceiver del widget (CircleWidget)
                    Intent intent = new Intent(context, typeof(CircleWidget));

                    // Especificar la acción de actualización del widget
                    intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);

                    // Crear un PendingIntent que se enviará cuando la alarma se dispare
                    PendingIntent pendingIntent = PendingIntent.GetBroadcast(
                        context, // Contexto de la aplicación
                        0,       // Identificador de la solicitud (0 porque no necesitamos diferenciarlas)
                        intent,  // Intent que se ejecutará cuando la alarma se active
                        PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable // Flags de configuración
                    );

                    // Programar la alarma para que se ejecute periódicamente cada 30 minutos
                    alarmManager.SetRepeating(
                        AlarmType.ElapsedRealtime, // Usar el tiempo relativo al tiempo de actividad del sistema
                        SystemClock.ElapsedRealtime() + 60000, // La primera ejecución será después de 1 minuto
                        30 * 60 * 1000, // Intervalo de repetición: cada 30 minutos (30 min * 60 seg * 1000 ms)
                        pendingIntent // PendingIntent que activará la actualización del widget
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion "Funciones"
    }
}