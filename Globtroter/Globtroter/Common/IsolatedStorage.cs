using Globtroter.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Globtroter.Common
{
    class IsolatedStorage<T> {
        //private static XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
        //Deserialize from xml
        public List<T> FromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            List<T> value;
            using (StringReader stringReader = new StringReader(xml))
            {
                object deserialized = serializer.Deserialize(stringReader);
                value = (List<T>)deserialized;
            }
            return value;
        }

        //Serialize to xml
        public string ToXml(List<T> value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true,
            };


            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                serializer.Serialize(xmlWriter, value);
            }
            return stringBuilder.ToString();
        }

        public void SaveInfo(string key, string value)
        {

            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values[key].ToString() != null)
                {
                    // do update             
                    Debug.WriteLine("   zapisuje !!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] = value;
                }
            }
            else
            {
                // do create key and save value, first time only.

                Windows.Storage.ApplicationData.Current.LocalSettings.CreateContainer(key, ApplicationDataCreateDisposition.Always);

                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] == null)
                {

                    Debug.WriteLine("   zapisuje po raz pierwszy!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] = value;
                }
            }
        }

        public String GetInfo(string key)
        {
            object value = new object();
            String p = "";
            StringReader writer = new StringReader(p);

            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values[key].ToString() != null)
                {
                    // do update

                    Debug.WriteLine("   odczytuje !!!!!!!!!!!!!!!!!!!!!!!!!!");
                    value = Windows.Storage.ApplicationData.Current.LocalSettings.Values[key];
                    // Debug.WriteLine("   " + value.ToString());
                    p = value.ToString();
                }
            }
            return p;
        }


    /*
    class Setting<T>
    {
        private XmlSerializer serializer;
        public Setting()
        {
            serializer = new XmlSerializer(typeof(T));
        }
        private string FileName(T Obj, string Handle)
        {
            var str = String.Concat(Handle, String.Format("{0}", Obj.GetType().ToString()));
            return str;
        }

        public async Task<string> SaveAsync(string Key, T Obj)
        {
            string fileName = FileName(Activator.CreateInstance<T>(), Key);
            try
            {
                if (Obj != null)
                {
                    Debug.WriteLine("   9");
                    StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    using (Stream outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result)
                    {
                        Debug.WriteLine("   8");
                        serializer.Serialize(outStream, Obj);
                        await outStream.FlushAsync();
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("   7");
                throw;
            }

            return "koniec zapisu";
        }

       


        /// <summary>
        /// Delete a stored instance of T from Windows.Storage.ApplicationData
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            string fileName = FileName(Activator.CreateInstance<T>(), String.Empty);
            await DeleteAsync(fileName);
        }

        /// <summary>
        /// Delete a stored instance of T with a specified handle from Windows.Storage.ApplicationData.
        /// Specification of a handle supports storage and deletion of different instances of T.
        /// </summary>
        /// <param name="Handle">User-defined handle for the stored object</param>
        public async Task DeleteAsync(string Key)
        {
            if (Key == null)
                throw new ArgumentNullException("Handle");
            string fileName = FileName(Activator.CreateInstance<T>(), Key);
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                if (file != null)
                {
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve a stored instance of T with a specified handle from Windows.Storage.ApplicationData.
        /// Specification of a handle supports storage and deletion of different instances of T.
        /// </summary>
        /// <param name="Handle">User-defined handle for the stored object</param>
        public async Task<T> LoadAsync(string Key)
        {
            string fileName = FileName(Activator.CreateInstance<T>(), Key);

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);
                using (Stream inStream = Task.Run(() => readStream.AsStreamForRead()).Result)
                {
                    Debug.WriteLine("   1");
                    return (T)serializer.Deserialize(inStream);
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("   2");
                //file not existing is perfectly valid so simply return the default 
                return default(T);
                //Interesting thread here: How to detect if a file exists (http://social.msdn.microsoft.com/Forums/en-US/winappswithcsharp/thread/1eb71a80-c59c-4146-aeb6-fefd69f4b4bb)
                //throw;
            }
            catch (Exception)
            {
                Debug.WriteLine("   3");
                //Unable to load contents of file
                throw;
            }
            //return 0;
        }
     * */
    
}

}
