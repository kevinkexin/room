// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace com.atombooster.roomplus
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPageControl pcMainAdv { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scWeather { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView swMainAdv { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (pcMainAdv != null) {
                pcMainAdv.Dispose ();
                pcMainAdv = null;
            }

            if (scWeather != null) {
                scWeather.Dispose ();
                scWeather = null;
            }

            if (swMainAdv != null) {
                swMainAdv.Dispose ();
                swMainAdv = null;
            }
        }
    }
}