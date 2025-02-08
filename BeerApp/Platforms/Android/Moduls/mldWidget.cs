using Android.Appwidget;
using Android.Content;
using AndroidX.LocalBroadcastManager.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerApp.Platforms.Android.Moduls
{
    public class mldWidget
    {
        public static void UpdateWidget(Context context)
        {
            try
            {
                AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
                ComponentName thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(CircleWidget)));
                appWidgetManager.NotifyAppWidgetViewDataChanged(appWidgetManager.GetAppWidgetIds(thisWidget), Resource.Id.widgetListView);

                Java.Lang.Class widgetClass = Java.Lang.Class.FromType(typeof(CircleWidget));
                int[] widgetIds = appWidgetManager.GetAppWidgetIds(new ComponentName(context, widgetClass));
                Intent updateIntent = new Intent(AppWidgetManager.ActionAppwidgetUpdate);
                updateIntent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, widgetIds);
                context.SendBroadcast(updateIntent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
