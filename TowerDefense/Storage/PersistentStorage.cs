using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerDefense.Storage
{
    public class PersistentStorage
    {

        //
        // Safeguard against multiple save/load happening at the same time
        private bool saving = false;
        private bool loading = false;
        /// <summary>
        /// Demonstrates how serialize an object to storage
        /// </summary>
        public void Save<T>(string key,T toSave)
        {
            lock (this)
            {
                if (!this.saving)
                {
                    this.saving = true;
                    //
                    FinalizeSave(key,toSave);
                }
            }
        }
        private void FinalizeSave<T>(string key,T state)
        {
           
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(key, FileMode.OpenOrCreate))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(T));
                                mySerializer.Serialize(fs, state);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                this.saving = false;
            
        }
        /// <summary>
        /// Demonstrates how to deserialize an object from storage device
        /// </summary>
        public T Load<T>(string key)
        {
            lock (this)
            {
                if (!this.loading)
                {
                    this.loading = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    return FinalizeLoad<T>(key);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
                return default;
            }
        }
        private T FinalizeLoad<T>(string key)
        {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists(key))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile(key, FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(T));
                                    return (T)mySerializer.Deserialize(fs);
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                        return default;
                    }
                    finally
                    {
                        this.loading = false;
                    }
                }

                return default;
          
        }
    }
}
