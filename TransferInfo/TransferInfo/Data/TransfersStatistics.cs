using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransferInfo.Data
{
    /// <summary>
    /// Final data aggregation and management
    ///   index 0: data for the current period
    ///   index 1: data for the previous period
    /// </summary>
    internal class TransfersStatistics
    {
        internal static TransfersStatistics Instance { get; private set; }

        private readonly BuildingTransfersStorage[] _data;

        internal TransfersStatistics()
        {
            Instance = this;
            _data = new BuildingTransfersStorage[2];
            _data[0] = new BuildingTransfersStorage();
            _data[1] = new BuildingTransfersStorage();
        }

        internal void AddBatch(CargoBatch cargoBatch)
        {
            _data[0].AddTransfer(cargoBatch);
        }
    }
}
