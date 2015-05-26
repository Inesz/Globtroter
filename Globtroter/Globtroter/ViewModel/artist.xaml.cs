using Globtroter.Common;
using Globtroter.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


//using System.IO.IsolatedStorage;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Globtroter.ViewModel
{
   
    

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class artist : Page
    {
      
        public List<Groups> Groups = new List<Groups>();
        //private XmlSerializer serializer = new XmlSerializer(typeof(List<Groups>));
        private DataContractSerializer serializer = new DataContractSerializer(typeof(List<Groups>));
        
        public artist()
        {
            this.InitializeComponent();
   
            Groups.Add(new Groups("glowna", "to jest grupa glowna", DateTime.Now, "asd") { });
            Groups.Add(new Groups("nieglowna", "to jest grupa nie glowna", DateTime.Now, "asdf") { });
           /**/
            string key = "nowy";
            var IS = new IsolatedStorage<Groups>();
            String s = IS.ToXml(Groups);
          IS.SaveInfo(key, s);
            String pom = IS.GetInfo(key);
            List<Groups> d = IS.FromXml(pom);
            /**/

            ComboBox1.DataContext = d;                  
        }

       /*serializacja działa poprawnie*/


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {                        
        }


    }
}
