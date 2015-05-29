using Globtroter.Common;
using Globtroter.DataModel;
using Globtroter.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Devices.Geolocation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Web.Syndication;

namespace Globtroter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WriteableBitmap _writeableBitmap;
        public MainPage()
        {
        
            this.InitializeComponent();
        }

        private ShareOperation _shareOperation;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.RegisterBackgroundTask();

            initialize();

            var args = e.Parameter as ShareTargetActivatedEventArgs;
            if (args != null)
            {
                _shareOperation = args.ShareOperation;

                if (_shareOperation.Data.Contains(
                    StandardDataFormats.Bitmap))
                {
                    _bitmap = await _shareOperation.Data.GetBitmapAsync();
                    await ProcessBitmap();
                }
                else if (_shareOperation.Data.Contains(
                    StandardDataFormats.StorageItems))
                {
                    _items = await _shareOperation.Data.GetStorageItemsAsync();
                    await ProcessStorageItems();
                }
                else _shareOperation.ReportError(
                    "Aplikacja nie znalazła prawidłowej mapy bitowej.");
            }
        }

        private async Task LoadBitmap(IRandomAccessStream stream)
        {
            _writeableBitmap = new WriteableBitmap(1, 1);
            _writeableBitmap.SetSource(stream);
            _writeableBitmap.Invalidate();
            await Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => image.Source = _writeableBitmap); ;
        }

        RandomAccessStreamReference _bitmap;

        private async Task ProcessBitmap()
        {
            if (_bitmap != null)
            {
                await LoadBitmap(await _bitmap.OpenReadAsync());
            }
        }

        IReadOnlyList<IStorageItem> _items;
        private IRandomAccessStream output;
    
        private async Task ProcessStorageItems()
        {
            foreach (var item in _items)
            {
                if (item.IsOfType(StorageItemTypes.File))
                {
                    var file = item as StorageFile;
                    if (file.ContentType.StartsWith(
                        "zdjecie",
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        await LoadBitmap(await file.OpenReadAsync());
                        break;
                    }
                }
            }
        }

        private void CheckAndClearShareOperation()
        {
            if (_shareOperation != null)
            {
                _shareOperation.ReportCompleted();
                _shareOperation = null;
            }
        }

        
        public async void OnButtonClick_Create(object sender,
            RoutedEventArgs e)
        {
            CheckAndClearShareOperation();
            var camera = new CameraCaptureUI();
            var result = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (result != null)
            {
                await LoadBitmap(await result.OpenAsync(
                    FileAccessMode.Read));
            }
        }
        
        public async void OnButtonClick_Save(object sender, RoutedEventArgs e)
        {
            if (_writeableBitmap != null)
            {
                cerateFolder("Globtroter");

                var picker = new FileSavePicker();
                //kiedy brak, otwiera sie ostatnio uzywana lokalizacja
                //nie wiem jak ustawic inna scierzke niz domysla

                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                //lista do FileTypeChoices
                IReadOnlyList<BitmapCodecInformation> codecInfos = BitmapDecoder.GetDecoderInformationEnumerator();
                foreach (BitmapCodecInformation codecInfo in codecInfos)
                    foreach (string extension in codecInfo.FileExtensions)
                       // picker.FileTypeFilter.Add(extension);
                        picker.FileTypeChoices.Add(extension, new List<string>() { extension });
        
                picker.DefaultFileExtension = ".png";
                picker.SuggestedFileName = DateTime.Now.ToString();

                var savedFile = await picker.PickSaveFileAsync();

                //zapis do globalnego pola 
              

                try
                {
                    if (savedFile != null)
                    {
                        using (output = await savedFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, output);

                            byte[] pixels;

                            using (var stream = _writeableBitmap.PixelBuffer.AsStream())
                            {
                                pixels = new byte[stream.Length];
                                await stream.ReadAsync(pixels, 0, pixels.Length);
                            }

                            encoder.SetPixelData(BitmapPixelFormat.Rgba8,
                                                    BitmapAlphaMode.Straight,
                                                    (uint)_writeableBitmap.PixelWidth,
                                                    (uint)_writeableBitmap.PixelHeight,
                                                    96.0, 96.0,
                                                    pixels);

                            await encoder.FlushAsync();
                            await output.FlushAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var s = ex.ToString();
                }
                finally
                {
                    CheckAndClearShareOperation();
                    UpdateCurrentFoto(savedFile, _writeableBitmap);
                    if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(SaveFoto));
                    }
                }
            }
        }

        public async void UpdateCurrentFoto(StorageFile savedFile, WriteableBitmap writableBitmap)
        {
            App myApp = Application.Current as App;

            //myApp._currentFoto._currentFoto = writableBitmap;
            myApp._currentFoto.AddDate = savedFile.DateCreated.DateTime;
            myApp._currentFoto.Name = savedFile.Name;
            myApp._currentFoto.Description = "";
            myApp._currentFoto.Localization = "";

            myApp._currentFoto.Id = savedFile.Name;

            string path = savedFile.Path;
            string[] split = path.Split(new Char[] { '\\', '/' });

            myApp._currentFoto.Subgroup = split[split.Length - 2];  

            
            foreach(var s in myApp.Subgroups){
                if (s.Name == myApp._currentFoto.Subgroup)
                {
                    myApp._currentFoto.Group = s.Group;
                }
            }
            
            myApp._currentFoto._currentFoto = await GetImageBitmap(myApp._currentFoto.Subgroup, myApp._currentFoto.Id);
        }

        public async Task<BitmapImage> GetImageBitmap(string path,string name)
        {

            string FolderPath = @"Globtroter\" + path;

            StorageFolder CurrentFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync(FolderPath);
            StorageFile file = await CurrentFolder.GetFileAsync(name);

            Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            bitmapImage.SetSource(fileStream);

            return bitmapImage;
        }

        public void OnButtonClick_Gallery(object sender, RoutedEventArgs e)
        {  
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(GalleryGroups), "AllGroups");
            }
        }

        public void OnButtonClick_Artist(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(artist));
            }

        }

        public async void cerateFolder(string folderName)
        {
            StorageFolder libraryFolder = Windows.Storage.KnownFolders.PicturesLibrary;

            try
            {
                await libraryFolder.CreateFolderAsync(folderName, CreationCollisionOption.FailIfExists);
            }
            catch { }
        }

        public async void initialize()
        {
            App myApp = Application.Current as App;

            StorageFolder appFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync("Globtroter");
            IReadOnlyList<StorageFolder> storageFolders = await appFolder.GetFoldersAsync();

            foreach (StorageFolder storageFolder in storageFolders)
            {
                Subgroups c = new Subgroups();
                c.Name = storageFolder.Name;
                c.AddDate = storageFolder.DateCreated.DateTime;
                myApp.Subgroups.Add(c);
            }

            string key = "nowy";
            var IS = new IsolatedStorage<Fotos>();
            myApp.Fotos = IS.FromXml(IS.GetInfo(key));
        }

        /*
            string mainPath = @"c:\Top-Level Folder";

            var picker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            string pathString = System.IO.Path.Combine(picker.SuggestedStartLocation, "SubFolder");
            System.IO.Directory.CreateDirectory(pathString);

            if (!System.IO.File.Exists(pathString))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileName);
                return;
            }

        }
         * */

        private void OnButtonClick_Groups(object sender, RoutedEventArgs e)
        {

        }

        /*live lites*/
        private async void RegisterBackgroundTask()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if( backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity )
            {
                foreach( var task in BackgroundTaskRegistration.AllTasks )
                {
                    if( task.Value.Name == taskName )
                    {
                        task.Value.Unregister( true );
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = taskEntryPoint;
                taskBuilder.SetTrigger( new TimeTrigger( 15, false ) );
                var registration = taskBuilder.Register();
            }
        }

        private const string taskName = "BlogFeedBackgroundTask";
        private const string taskEntryPoint = "BackgroundTasks.BlogFeedBackgroundTask";
        /**/
    }
}
