using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Animation;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.OS;

namespace CustomActionBar
{
	public class Fragment1 : Fragment
	{
		LinearLayout loadingBars;
		ProgressBar bar1;
		ProgressBar bar2;
		TextView swipeText;
		FrameLayout fakeActionBar;

		bool setup = false;
		int accumulatedDeltaY = 0;

		ObjectAnimator bar1Fade, bar2Fade;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			// Create your fragment here
		}

		void ShowSwipeDown ()
		{
			if (!setup) {
				//Activity.ActionBar.Hide ();
				if (bar1Fade != null) {
					bar1Fade.Cancel ();
					bar1Fade = null;
				}
				if (bar2Fade != null) {
					bar2Fade.Cancel ();
					bar2Fade = null;
				}
				loadingBars.Visibility = ViewStates.Visible;
				fakeActionBar.Visibility = ViewStates.Visible;
				swipeText.TranslationY = -(Activity.ActionBar.Height + swipeText.Height + 4);
				swipeText.Visibility = ViewStates.Visible;
				swipeText.Animate ().TranslationY (0).SetStartDelay (50).Start ();
				accumulatedDeltaY = 0;
				setup = true;
			}
		}

		void HideSwipeDown ()
		{
			//Activity.ActionBar.Show ();
			swipeText.Visibility = ViewStates.Invisible;
			fakeActionBar.Visibility = ViewStates.Invisible;
			bar1Fade = ObjectAnimator.OfInt (bar1, "progress", bar1.Progress, 0);
			bar1Fade.SetDuration (250);
			bar1Fade.Start ();
			bar2Fade = ObjectAnimator.OfInt (bar2, "progress", bar2.Progress, 0);
			bar2Fade.SetDuration (250);
			bar2Fade.Start ();
			bar2Fade.AnimationEnd += (sender, e) => loadingBars.Visibility = ViewStates.Gone;
			setup = false;
		}
	
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.Tab1, container, false);

			var list = view.FindViewById<OverscrollListView> (Resource.Id.listView1);
			loadingBars = view.FindViewById<LinearLayout> (Resource.Id.loadingBars);
			bar1 = view.FindViewById<ProgressBar> (Resource.Id.loadingBar1);
			bar2 = view.FindViewById<ProgressBar> (Resource.Id.loadingBar2);
			swipeText = view.FindViewById<TextView> (Resource.Id.swipeToRefreshText);
			fakeActionBar = view.FindViewById<FrameLayout> (Resource.Id.fakeActionBar);

			// Remove progress bar background
			foreach (var p in new[] { bar1, bar2 }) {
				var layer = p.ProgressDrawable as LayerDrawable;
				if (layer != null)
					layer.SetDrawableByLayerId (Android.Resource.Id.Background,
					                            new ColorDrawable (Color.Transparent));
			}

			list.OverScrolled += deltaY => {
				ShowSwipeDown ();

				accumulatedDeltaY += -deltaY;
				bar1.Progress = bar2.Progress = accumulatedDeltaY;
				if (accumulatedDeltaY == 0)
					HideSwipeDown ();
			};
			list.OverScrollCanceled += HideSwipeDown;

			return view;
		}
	}
}

