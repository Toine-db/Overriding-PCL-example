using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MyApp.Core;

namespace MyApp.Droid
{
    [Activity(Label = "MyApp.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly MyRepository _myRepository = new MyRepository();
        private TextView _myTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var button = FindViewById<Button>(Resource.Id.MyButton);
            _myTextView = FindViewById<TextView>(Resource.Id.MyTextView);

            button.Click += delegate
            {
                _myTextView.Text = _myRepository.GetMyModel().ToString();
            };
        }
    }
}

