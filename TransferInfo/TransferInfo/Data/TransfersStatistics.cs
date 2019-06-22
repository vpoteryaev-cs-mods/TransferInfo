using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransferInfo.Data
{
    //All transfers in conjunction with ConnectionType
    internal class TransfersStatistics
    {
        internal TransfersStorage[] Transfers { get; }

        internal TransfersStatistics()
        {
            int numConnectionTypes = (int)Enum.GetValues(typeof(TransferConnectionType)).Cast<TransferConnectionType>().Aggregate((v, agg) => agg | v) + 1;
            Transfers = new TransfersStorage[numConnectionTypes];
            for (int i = 0; i < numConnectionTypes; i++)
                Transfers[i] = new TransfersStorage();
        }

        /*
        internal TransfersStorage GetStorageByType(TransferConnectionType type)
        {
            return Transfers[(int)type];
        }
        */
    }
}
