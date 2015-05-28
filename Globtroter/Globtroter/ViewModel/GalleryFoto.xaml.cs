using Globtroter.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Globtroter.ViewModel
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class GalleryFoto : Globtroter.Common.LayoutAwarePage
    {
        public GalleryFoto()
        {
            this.InitializeComponent();
            //Loaded += OnMainPageLoaded;
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
        private string Subgroup = "";

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]

            //odbieranie wartości przekazanych do funkcji
            string name = navigationParameter as string;
            if (!string.IsNullOrWhiteSpace(name))
            {
                Subgroup = name;
                Debug.WriteLine("   Podgrupa:" + name);
            }

            OnMainPageLoaded(name);
        }

        /******************************/

        //async void OnMainPageLoaded(object sender, RoutedEventArgs args)
        async void OnMainPageLoaded(string name)
        {       
  
            string FolderPath = @"Globtroter\" + name;
            
            StorageFolder CurrentFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync(FolderPath);    
            IReadOnlyList<StorageFile> storageFiles = await CurrentFolder.GetFilesAsync();

            Subgroup = name;

            List<AllFotos> AllFotos = new List<AllFotos>();
                      
             foreach (StorageFile file in storageFiles)
             {
                     // Open a stream for the selected file.
                     Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                     // Set the image source to the selected bitmap.
                     Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                     bitmapImage.SetSource(fileStream);

                     AllFotos currentFoto = new AllFotos();
                     currentFoto.Name = file.Name;
                     currentFoto.CurrentFoto = bitmapImage;
                     AllFotos.Add(currentFoto);
             }

             DefaultViewModel["Items"] = AllFotos;
        }

        private async void OnButtonClick_SaveFoto(object sender, RoutedEventArgs e)
        {
            FrameworkElement eSource = e.OriginalSource as FrameworkElement;
            string Name = eSource.Tag.ToString();

            App myApp = Application.Current as App;

            //Aktualizuj current foto
            foreach (Fotos foto in myApp.Fotos)
            {
                if (foto.Name == Name)
                {
                    myApp._currentFoto.AddDate = foto.AddDate;
                    myApp._currentFoto.Description = foto.Description;
                    myApp._currentFoto.Group = "";
                    myApp._currentFoto.Id = foto.Id;
                    myApp._currentFoto.Localization = foto.Lozalization;             
                }
            }           
            myApp._currentFoto._currentFoto = await PobierzBitmapImage(Subgroup, Name);
            myApp._currentFoto.Name = Subgroup;
            myApp._currentFoto.Name = Name;
            
            
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(SaveFoto));
            }          
        }

        async Task<BitmapImage> PobierzBitmapImage(string path, string name)
        {
            string FolderPath = @"Globtroter\" + path;

            StorageFolder CurrentFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync(FolderPath);
            StorageFile file = await CurrentFolder.GetFileAsync(name);

            Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            bitmapImage.SetSource(fileStream);

            return bitmapImage;
        }


  ////////////////////
        /*
                    //ustawienie zawartości pola xaml widoku z poziomu kontrolera
                    myImage.Source = bitmapImage;
                    this.DataContext = file; 
         
         //dodanie elementu xaml z poziomu kontrolera 
         Image image = new Image
           {
               Source = bitmapImage,
               Stretch = Stretch.None,
               Name = file.Name
           };

         Foto.Children.Add(image);
        */
        ///////////////////////
    }
}
