using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using ICities;
using UnityEngine;

namespace TransferInfo.Data
{
    public class StorageManager: SerializableDataExtensionBase
    {
        internal static StorageManager Instance { get; private set; }

        internal TransfersStatistics Data { get; private set; }

        internal StorageManager()
        {
            Data = new TransfersStatistics(Options.StorageVersion);
            Instance = this;
        }

        public override void OnCreated(ISerializableData serializableData)
        {
            base.OnCreated(serializableData);
        }

        public override void OnLoadData()
        {
            var data = serializableDataManager.LoadData(Options.GameStorageID);
            if(data == null)
            {
#if DEBUG
                Debug.Log("TransferInfo: StorageManager.OnLoadData - data in saved game not found.");
#endif
                return;
            }
            MemoryStream stream = new MemoryStream(data);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var tempData = formatter.Deserialize(stream);
                if (tempData is TransfersStatistics tempStorageData)
                    if (tempStorageData.version == Options.StorageVersion)
                        Data = tempStorageData;
#if DEBUG
                    else
                        Debug.Log("TransferInfo: StorageManager.OnLoadData - wrong version.");
                else
                    Debug.Log("TransferInfo: StorageManager.OnLoadData - wrong data format.");
#endif
            }
            catch (SerializationException e)
            {
                Debug.LogError("TransferInfo: StorageManager.OnLoadData - Failed to deserialize. Reason: " + e.Message);
                //throw;
            }
            //finally
            //{
            //}

            base.OnLoadData();
        }

        public override void OnSaveData()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, Data);
            }
            catch (SerializationException e)
            {
                Debug.LogError("TransferInfo: StorageManager.OnSaveData - Failed to serialize. Reason: " + e.Message);
                //throw;
            }
            //finally
            //{
            //}

            serializableDataManager.SaveData(Options.GameStorageID, stream.ToArray());

            base.OnSaveData();
        }

        public override void OnReleased()
        {
            base.OnReleased();
        }
    }
}
