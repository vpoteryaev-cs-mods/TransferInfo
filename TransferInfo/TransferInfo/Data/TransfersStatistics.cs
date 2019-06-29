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

        //note: returning -1 is normal and only for tests with Original ModInfo statistics panel.
        internal int GetBuildingTransfersStorage(int period, ushort buildingID, TransferConnectionType transferConnectionType, TransferReason transferReason)
        {
            ConnectedTransfersStorage connectedTransfersStorage = _data[period].GetBuildingData(buildingID);
            if (connectedTransfersStorage == null)
                return -1;
            return connectedTransfersStorage.GetStorageByType(transferConnectionType).GetTransferedValue(transferReason);
        }
    }
}
