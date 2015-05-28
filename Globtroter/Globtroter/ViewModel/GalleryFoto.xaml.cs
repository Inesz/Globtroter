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
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]

            //odbieranie wartości przekazanych do funkcji
            string name = navigationParameter as string;
            if (!string.IsNullOrWhiteSpace(name))
            {
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

             foreach (StorageFile file in storageFiles)
             {
                     // Open a stream for the selected file.
                     Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                     // Set the image source to the selected bitmap.
                     Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                     bitmapImage.SetSource(fileStream);
                     myImage.Source = bitmapImage;
                     this.DataContext = file;             
             }
        }
        /*
                StorageFolder appFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync("Globtroter");
                    IReadOnlyList<StorageFolder> storageFolders = await appFolder.GetFoldersAsync();

                    Subgroups b = new Subgroups();
                    b.Name = "hh";
                    b.Group = "Sopot";
                    myApp.Subgroups.Add(b);

                    foreach (StorageFolder storageFolder in storageFolders)
                    {
                        Subgroups c = new Subgroups();
                        c.Name = storageFolder.Name;
                        c.Group = "Ania";
                        //myApp.Subgroups.Add(c);
                        Subgroups.Add(c);
                        Debug.WriteLine("   mam:"+storageFolder.Name);
                    }
         * 
         * if (file != null)
                        {
                            // Open a stream for the selected file.
                            Windows.Storage.Streams.IRandomAccessStream fileStream =
                                await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                            // Set the image source to the selected bitmap.
                            Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage =
                                new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                            bitmapImage.SetSource(fileStream);
                            myImage.Source = bitmapImage;
                            this.DataContext = file;

                        }
        */
        /*
        async Task<BitmapSource> LoadBitmapAsync(StorageFile storageFile)
        {
            BitmapSource bitmapSource = null;
            // Otwarcie StorageFile do odczytu
            using (IRandomAccessStreamWithContentType stream = await storageFile.OpenReadAsync())
            {
                bitmapSource = await LoadBitmapAsync(stream);
            }
            return bitmapSource;
        }

        async Task<BitmapSource> LoadBitmapAsync(StorageItemThumbnail thumbnail)
        {
            return await LoadBitmapAsync(thumbnail as IRandomAccessStream);
        }

        async Task<BitmapSource> LoadBitmapAsync(IRandomAccessStream stream)
        {
            WriteableBitmap bitmap = null;

            // Tworzenie BitmapDecoder ze strumienia
            BitmapDecoder decoder = null;

            try
            {
                decoder = await BitmapDecoder.CreateAsync(stream);
            }
            catch
            {
                // Po porstu pomiń nieprawidłowe
                return null;
            }

            // Pobranie pierwszej ramki
            BitmapFrame bitmapFrame = await decoder.GetFrameAsync(0);

            // Pobranie pikseli
            PixelDataProvider dataProvider =
                    await bitmapFrame.GetPixelDataAsync(BitmapPixelFormat.Bgra8,
                                                        BitmapAlphaMode.Premultiplied,
                                                        new BitmapTransform(),
                                                        ExifOrientationMode.RespectExifOrientation,
                                                        ColorManagementMode.ColorManageToSRgb);

            byte[] pixels = dataProvider.DetachPixelData();

            // Tworzenie WriteableBitmap i ustawienie pikseli
            bitmap = new WriteableBitmap((int)bitmapFrame.PixelWidth,
                                            (int)bitmapFrame.PixelHeight);

            using (Stream pixelStream = bitmap.PixelBuffer.AsStream())
            {
                pixelStream.Write(pixels, 0, pixels.Length);
            }

            bitmap.Invalidate();
            return bitmap;
        }
        */
    }
}
