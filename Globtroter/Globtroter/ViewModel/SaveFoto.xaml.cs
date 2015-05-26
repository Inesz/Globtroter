using Globtroter.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Globtroter.ViewModel
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SaveFoto : Globtroter.Common.LayoutAwarePage
    {
        public SaveFoto()
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
            //System.Collections.Generic.IEnumerable<Globtroter.Data.SampleDataGroup> sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            App myApp = Application.Current as App;
            //DefaultViewModel["CurrentFoto"] = myApp._currentFoto;

            Name.DataContext = myApp._currentFoto.Name;
            Data.DataContext = myApp._currentFoto.AddDate;
            Localization.DataContext = myApp._currentFoto.Localization;
            Description.DataContext = myApp._currentFoto.Description;
            _currentFoto.DataContext = myApp._currentFoto._currentFoto;
            Group.DataContext = myApp._currentFoto.Group;

            IEnumerable<string> p = myApp.Subgroups.Select(Subgroups => Subgroups.Name);
            Subgroup.DataContext = p; //myApp.Subgroups.SelectMany(p => p.Name); //SelectMany(myApp.Subgroups => Name);

            //foreach (Subgroups s in myApp._currentFoto.Subgroup)
            {

            }
            Subgroup.SelectedItem = myApp._currentFoto.Subgroup;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            //Debug.WriteLine(Name.DataContext);
        }

        private async void OnButtonClick_SaveChanges(object sender, RoutedEventArgs e)
        {
            App myApp = Application.Current as App;
            String msg = "zapisano zmiany";


            Debug.WriteLine("   Name.Text= " + Name.Text + " app=" + myApp._currentFoto.Name);
            if (Name.Text != myApp._currentFoto.Name)
            Debug.WriteLine("       rozne");
            else
            Debug.WriteLine("       takie same");

            Debug.WriteLine("   Subgroup.SelectedItem=" + Subgroup.SelectedItem + " app=" + myApp._currentFoto.Subgroup);

            if (Subgroup.SelectedItem != myApp._currentFoto.Subgroup)
                Debug.WriteLine("       rozne");
            else
                Debug.WriteLine("       takie same");


            if (Name.Text != myApp._currentFoto.Name || Subgroup.SelectedItem != myApp._currentFoto.Subgroup)
            {    
            StorageFolder appFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync("Globtroter");
            StorageFolder currentFolder = await appFolder.GetFolderAsync(myApp._currentFoto.Subgroup);
            StorageFile File = await currentFolder.GetFileAsync(myApp._currentFoto.Name);

            if (Name.Text != myApp._currentFoto.Name)
            {
                    myApp._currentFoto.Name = Name.Text;
                    try
                    {
                        await File.RenameAsync(Name.Text.ToString(), NameCollisionOption.FailIfExists);
                    }
                    catch
                    {
                        msg = "Nie udało się zapisać zmian. Plik o podanej nazwie już istnieje";
                    }
                }

            if (Subgroup.SelectedItem != myApp._currentFoto.Subgroup)
                {
                    myApp._currentFoto.Subgroup = Subgroup.SelectedItem.ToString();
                    StorageFolder updateFolders = await appFolder.GetFolderAsync(Subgroup.SelectedItem.ToString());
                    try
                    {
                        await File.MoveAsync(updateFolders, myApp._currentFoto.Name, NameCollisionOption.FailIfExists);
                    }
                    catch
                    {
                        msg = "Nie udało się zapisać zmian. Plik o podanej nazwie już istnieje";
                    }
                }               
            }
  
            myApp._currentFoto.Description = Description.Text;
            myApp._currentFoto.Localization = Localization.Text;

            Debug.WriteLine(msg);
        }

        private void OnButtonClick_CancelChanges(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxSubgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group.DataContext = "myApp._currentFoto.Group";
        }
    }
}
