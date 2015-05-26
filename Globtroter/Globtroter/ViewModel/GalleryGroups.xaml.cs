using Globtroter.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace Globtroter.ViewModel
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    // A simple business object
   

    public sealed partial class GalleryGroups : Globtroter.Common.LayoutAwarePage
    {
        //public ObservableCollection<Globtroter.Data.SampleDataGroup> MyMusic = new ObservableCollection<Recording>();
        //System.Collections.Generic.IEnumerable<Globtroter.Data.SampleDataGroup> SampleDataGroups = new System.Collections.Generic.IEnumerable<SampleDataGroup>();
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
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]

            System.Collections.Generic.IEnumerable<Globtroter.Data.SampleDataGroup> sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            var group9 = new SampleDataGroup("Group-1",
                   "Group Title: 1",
                   "Group Subtitle: 1",
                   "Assets/DarkGray.png",
                   "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            
            //sampleDataGroups.Add(group9);
            //this.DefaultViewModel["Groups"] = sampleDataGroups;
            
            
           // var sampleDataGroups = App.Instance.DataSource.ItemGroups;
            DefaultViewModel["Groups"] = sampleDataGroups;
             
            /**/
            
 
            /**/
                   
            printFolder();
        }

        public async void printFolder()
        {
            IReadOnlyList<StorageFolder> folderList = await takeFolder();
            foreach (StorageFolder f in folderList)
            {
                Debug.WriteLine("    " + f.Name);
            }
        }

        public async Task<IReadOnlyList<StorageFolder>> takeFolder()
        {

            StorageFolder libraryFolder = KnownFolders.PicturesLibrary;
            StorageFolder globtroterFolder = await libraryFolder.GetFolderAsync("Globtroter");
            StorageFolderQueryResult queryResult = globtroterFolder.CreateFolderQuery();
            IReadOnlyList<StorageFolder> folderList = await queryResult.GetFoldersAsync();
            Debug.WriteLine("lista podfolderow:");
            foreach (StorageFolder f in folderList)
            {
                Debug.WriteLine("    "+f.Name);
            }
            return folderList;
        }
    }
}
