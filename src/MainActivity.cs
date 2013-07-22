using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;
using Android.Graphics;


namespace CustomActionBar
{
	[Activity (Theme="@style/Theme.Example", Label = "CustomActionBar", MainLauncher = true)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//RequestWindowFeature (WindowFeatures.ActionBarOverlay);
			SetContentView (Resource.Layout.Main);

			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			var tab1 = ActionBar.NewTab ();
			tab1.SetText ("Tab1");
			tab1.TabSelected += (sender, e) => e.FragmentTransaction.Replace (Resource.Id.fragmentContainer, new Fragment1 ());
			ActionBar.AddTab (tab1);

			var tab2 = ActionBar.NewTab ();
			tab2.SetText ("Tab2");
			tab2.TabSelected += (sender, e) => e.FragmentTransaction.Replace (Resource.Id.fragmentContainer, new Fragment2 ());
			ActionBar.AddTab (tab2);

			var tab3 = ActionBar.NewTab ();
			tab3.SetText ("Tab3");
			tab3.TabSelected += (sender, e) => e.FragmentTransaction.Replace (Resource.Id.fragmentContainer, new Fragment3 ());
			ActionBar.AddTab (tab3);

			ActionBar.SetSelectedNavigationItem (0);
			ActionBar.SetBackgroundDrawable (new ColorDrawable (Color.Transparent));
		}
	}
}


