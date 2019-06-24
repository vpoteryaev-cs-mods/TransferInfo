using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransferInfo.Data
{
    //All transfers in conjunction with ConnectionType
    internal class TransfersStatistics
    {
        private readonly TransfersStorage[] _transfers = new TransfersStorage[CargoBatch.NumConnectionTypes];
        internal TransfersStorage[] Transfers { get { return _transfers; } }

        internal TransfersStatistics()
        {
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < CargoBatch.NumConnectionTypes; i++)
                _transfers[i] = new TransfersStorage();
        }
        /*
        internal TransfersStorage GetStorageByType(TransferConnectionType type)
        {
            return Transfers[(int)type];
        }
        */
    }
}
