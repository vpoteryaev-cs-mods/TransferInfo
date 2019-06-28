using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using TransferReason = TransferManager.TransferReason;

namespace TransferInfo.Data
{
    /// <summary>
    /// Final data aggregation and management
    ///   index 0: data for the current period
    ///   index 1: data for the previous period
    /// </summary>
    [Serializable]
    internal class TransfersStatistics
    {
        private readonly BuildingTransfersStorage[] _data;
        internal readonly string version;

        internal TransfersStatistics(string version)
        {
            _data = new BuildingTransfersStorage[2];
            _data[0] = new BuildingTransfersStorage();
            _data[1] = new BuildingTransfersStorage();
            this.version = version;
        }

        internal void AddBatch(CargoBatch cargoBatch)
        {
            //note: move check to HarmonyPatches
            if (cargoBatch.buildingID != 0 && BuildingManager.instance.m_buildings.m_buffer[cargoBatch.buildingID].Info.m_buildingAI is CargoStationAI)
                _data[0].AddTransfer(cargoBatch);
        }

        //note: returning values as Critical Error codes: -1 and -2 - initialization was failed.
        //  This checks only for early stages of development and have to be deleted after first tests
        //returning -3 is normal and only for tests with Original ModInfo statistics panel.
        internal int GetBuildingTransfersStorage(int period, ushort buildingID, TransferConnectionType transferConnectionType, TransferReason transferReason)
        {
            int result = 0;
            BuildingTransfersStorage buildingTransfersStorage = _data[period];
#if DEBUG
            if (buildingTransfersStorage == null)
            {
                Debug.LogError("TransferInfo: TransferStatistics.GetBuildingTransfersStorage - buildingTransfersStorage is null");
                return -1;
            }
#endif
            ConnectedTransfersStorage connectedTransfersStorage = buildingTransfersStorage.GetBuildingData(buildingID);
            if (connectedTransfersStorage == null)
            {
#if DEBUG
                Debug.LogFormat("TransferInfo: TransferStatistics.GetBuildingTransfersStorage - for building {0} there is no data", buildingID);
#endif
                //return result;
                return -3;
            }
            ReasonTransfersStorage reasonTransfersStorage = connectedTransfersStorage.GetStorageByType(transferConnectionType);
#if DEBUG
            if (reasonTransfersStorage == null)
            {
                Debug.LogErrorFormat("TransferInfo: TransferStatistics.GetBuildingTransfersStorage - reasonTransfersStorage is null for building {0}", buildingID);
                return -2;
            }
#endif
            result = reasonTransfersStorage.GetTransferedValue(transferReason);

            return result;
        }
    }
}
