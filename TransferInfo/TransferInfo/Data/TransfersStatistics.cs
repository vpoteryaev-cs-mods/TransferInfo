﻿using System;
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
        internal readonly string version;
        private readonly BuildingTransfersStorage[] _data;
        internal int updateInterval;

        internal TransfersStatistics(string version)
        {
            _data = new BuildingTransfersStorage[2];
            _data[0] = new BuildingTransfersStorage();
            _data[1] = new BuildingTransfersStorage();
            this.version = version;
        }

        internal void AddBatch(CargoBatch cargoBatch)
        {
                _data[0].AddTransfer(cargoBatch);
        }

        internal int GetBuildingTransfersStorage(int period, ushort buildingID, TransferConnectionType transferConnectionType, TransferReason transferReason)
        {
            ConnectedTransfersStorage connectedTransfersStorage = _data[period].GetBuildingData(buildingID);
            if (connectedTransfersStorage == null)
                return 0;
            return connectedTransfersStorage.GetStorageByType(transferConnectionType).GetTransferedValue(transferReason);
        }

        internal void UpdateStatistics()
        {
            _data[1] = _data[0];
            _data[0] = new BuildingTransfersStorage();
        }
    }
}
