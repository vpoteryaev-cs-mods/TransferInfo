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
            _data[0].AddTransfer(cargoBatch);
        }
    }
}
