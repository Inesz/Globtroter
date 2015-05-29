using Globtroter.Common;
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
            Subgroup.DataContext = p; 
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

            String name = myApp._currentFoto.Name;
            String subgroup = myApp._currentFoto.Subgroup;

            StorageFolder appFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync("Globtroter");
            StorageFolder currentFolder = await appFolder.GetFolderAsync(subgroup);
            StorageFile File = await currentFolder.GetFileAsync(name);

            if (Name.Text != myApp._currentFoto.Name)
            {
                    myApp._currentFoto.Name = Name.Text;
                    try
                    {
                        await File.RenameAsync(Name.Text.ToString(), NameCollisionOption.FailIfExists);
                    }
                    catch
                    {
                        msg = "Nie udało się zmienić nazwy. Plik o podanej nazwie już istnieje";
                    }
            }

            if (Subgroup.SelectedItem != myApp._currentFoto.Subgroup)
                {
                    myApp._currentFoto.Subgroup = Subgroup.SelectedItem.ToString();
                    StorageFolder updateFolder = await appFolder.GetFolderAsync(Subgroup.SelectedItem.ToString());
                    try
                    {
                        await File.MoveAsync(updateFolder, myApp._currentFoto.Name, NameCollisionOption.FailIfExists);
                    }
                    catch
                    {
                        msg = "Nie udało się przenieść pliku. Plik o podanej nazwie już istnieje";
                    }
                }               
            
  
            myApp._currentFoto.Description = Description.Text;
            myApp._currentFoto.Localization = Localization.Text;

            //aktualizacja wpisu 
            Debug.WriteLine(myApp._currentFoto.Description + ", " + myApp._currentFoto.Localization + ", " + myApp._currentFoto.Name + ", " + myApp._currentFoto.Subgroup);

            Fotos f = new Fotos();
            /*
            if (!String.IsNullOrEmpty(myApp._currentFoto.Description)) f.Description = myApp._currentFoto.Description;
            if (!String.IsNullOrEmpty(myApp._currentFoto.Localization)) f.Lozalization = myApp._currentFoto.Localization;
            if (!String.IsNullOrEmpty(myApp._currentFoto.Name)) f.Name = myApp._currentFoto.Name;
            if (!String.IsNullOrEmpty(myApp._currentFoto.Subgroup)) f.Subgroup = myApp._currentFoto.Subgroup;
            */
            f.Description = myApp._currentFoto.Description;
            f.Lozalization = myApp._currentFoto.Localization;
            f.Name = myApp._currentFoto.Name;
            f.Subgroup = myApp._currentFoto.Subgroup;

            if (myApp.Fotos.Exists(x => x.Name == name && x.Subgroup == subgroup))
            {
                // myApp.Fotos.Find(x => x.Name == name && x.Subgroup == subgroup))
                for (int i = 0; i < myApp.Fotos.Count; i++)
                {
                    if (myApp.Fotos[i].Name == name && myApp.Fotos[i].Subgroup == subgroup)
                    {
                        myApp.Fotos[i] = f;
                    }
                }
            }
            else
            {
                myApp.Fotos.Add(f);
            }
            //serializacja danych
            string key = "Fotos";
            var IS = new IsolatedStorage<Fotos>();
            String s = IS.ToXml(myApp.Fotos);
            IS.SaveInfo(key, s);

            Debug.WriteLine(msg);
        }

        private void OnButtonClick_CancelChanges(object sender, RoutedEventArgs e)
        {
            App myApp = Application.Current as App;

            Name.Text = myApp._currentFoto.Name;

           //błąd - pole nie może być puste????
            //Description.Text = myApp._currentFoto.Description;
            //Localization.Text = myApp._currentFoto.Localization;
            Group.Text = "";

            //IEnumerable<string> p = myApp.Subgroups.Select(Subgroups => Subgroups.Name);
            //Subgroup.DataContext = p;
            Subgroup.SelectedItem = myApp._currentFoto.Subgroup;

            
        }

        private void ComboBoxSubgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group.DataContext = "Zmień grupę";
        }

        private void OnButtonClick_Home(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }  
        }
    }
}
