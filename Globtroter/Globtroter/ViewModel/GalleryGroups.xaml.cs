﻿using Globtroter.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Globtroter.ViewModel
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class GalleryGroups : Globtroter.Common.LayoutAwarePage
    {
        public GalleryGroups()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
            App myApp = Application.Current as App;
            DefaultViewModel["Items"] = myApp.Subgroups;
        }


        public void OnButtonClick_Subgroup(object sender, RoutedEventArgs e)
        {
            //pobierz wartośc tag z aktualnego guziczka
            FrameworkElement eSource = e.OriginalSource as FrameworkElement;
            string subgroup = eSource.Tag.ToString();

            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(GalleryFoto), subgroup);
            }
        }
    }
}
