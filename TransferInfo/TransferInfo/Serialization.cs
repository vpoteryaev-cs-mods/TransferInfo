using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using ICities;
using UnityEngine;
using TransferInfo.Data;

namespace TransferInfo
{
    public class Serialization: SerializableDataExtensionBase
    {
        //public override void OnCreated(ISerializableData serializableData)
        //{
            //called not only in game mode
        //    base.OnCreated(serializableData);
        //}

        public override void OnLoadData()
        {
            base.OnLoadData();

            //note: seems in docs stated that fired only in simulation thread.
            //if (!Loader.IsActive) return;
            var data = serializableDataManager.LoadData(Options.GameStorageID);
            if (data == null)
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
                        DataShared.Data = tempStorageData;
                    else
                    {
                        CleanData(false);
#if DEBUG
                        Debug.Log("TransferInfo: StorageManager.OnLoadData - wrong version.");
#endif
                    }
                else
                {
                    CleanData(false);
#if DEBUG
                    Debug.Log("TransferInfo: StorageManager.OnLoadData - wrong data format.");
#endif
                }
            }
            catch (SerializationException e)
            {
                Debug.LogError("TransferInfo: StorageManager.OnLoadData - Failed to deserialize. Reason: " + e.Message);
                //throw;
            }
            //finally
            //{
            //}
        }

        public override void OnSaveData()
        {
            base.OnSaveData();

            //note: seems in docs stated that fired only in simulation thread.
            //if (!Loader.IsActive) return;

            if (Options.Cleaning) return;

            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, DataShared.Data);
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
        }

        //public override void OnReleased()
        //{
        //    //called not only in game mode
        //    base.OnReleased();
        //}

        internal static void CleanData(bool preventSave = true)
        {
            Options.Cleaning = preventSave;

            if (SimulationManager.instance.m_serializableDataStorage.ContainsKey(Options.GameStorageID))
            {
                SimulationManager.instance.m_SerializableDataWrapper.EraseData(Options.GameStorageID);
#if DEBUG
                Debug.LogFormat("TransferInfo: StorageManager.CleanData - '{0}' data removed from savegame file", Options.GameStorageID);
#endif
            }
#if DEBUG
            else
                Debug.LogFormat("TransferInfo: StorageManager.CleanData - '{0}' data not present in savegame file", Options.GameStorageID);
#endif
        }
    }
}
