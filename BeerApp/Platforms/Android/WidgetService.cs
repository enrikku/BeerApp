using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.LocalBroadcastManager.Content;
using BeerApp.Moduls;
using BeerApp.Platforms.Android.Moduls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerApp.Platforms.Android
{
    [Service(Permission = "android.permission.BIND_REMOTEVIEWS")]
    public class WidgetService : RemoteViewsService
    {
        public override IRemoteViewsFactory OnGetViewFactory(Intent intent)
        {
            return new WidgetFactory(this.ApplicationContext);
        }

        public class WidgetFactory : Java.Lang.Object, RemoteViewsService.IRemoteViewsFactory
        {
            private readonly Context context;
            private List<string> items = new List<string>();

            public WidgetFactory(Context context)
            {
                this.context = context;
            }

            public void OnCreate() { }

            public void OnDestroy() { }

            public int Count => items.Count;

            public long GetItemId(int position) => position;

            public RemoteViews GetViewAt(int position)
            {
                var views = new RemoteViews(context.PackageName, Resource.Layout.widget_list_item);
                views.SetTextViewText(Resource.Id.widgetItemText, items[position]);

                return views;
            }

            public RemoteViews LoadingView => null;

            public int ViewTypeCount => 1;

            public bool HasStableIds => true;

            public void OnDataSetChanged()
            {
                try
                {
                    items.Clear();

                    var llBeerData = mdlVariablesGlobales.db.Table<BeerData>().ToList();
                    var brands = mdlVariablesGlobales.db.Table<BeerBrand>();
                    var beerGroup = llBeerData.GroupBy(x => x.IdBrand).Select(x => new { x.Key, Qtt = x.Sum(y => y.Qtt) }).OrderByDescending(x => x.Qtt).ToList();

                    foreach (var item in beerGroup)
                    {
                        var brand = brands.Where(x => x.Id == item.Key).FirstOrDefault()?.Name;
                        var quantity = item.Qtt;

                        this.items.Add($"{brand} {quantity}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}
