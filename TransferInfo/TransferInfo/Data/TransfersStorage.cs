using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TransferReason = TransferManager.TransferReason;

namespace TransferInfo.Data
{
    /// <summary>
    /// Used as base storage in Dictionary by pairs: TransferReason - Quantity
    /// </summary>
    internal class ReasonTransfersStorage
    {
        //Code in the JoiningByType region is related to Feature https://dev.azure.com/vpoteryaev-cs-mods/TransferInfo/_workitems/edit/10/
        #region JoiningByType
        
        internal static readonly HashSet<TransferReason> GarbageTypes = new HashSet<TransferReason>()
        {
            TransferReason.Garbage,
            TransferReason.GarbageMove
        };

        internal static readonly HashSet<TransferReason> OilTypes = new HashSet<TransferReason>()
        {
            TransferReason.Oil,
            TransferReason.Petrol,
            TransferReason.Petroleum,
            TransferReason.Plastics
        };

        internal static readonly HashSet<TransferReason> OreTypes = new HashSet<TransferReason>()
        {
            TransferReason.Ore,
            TransferReason.Coal,
            TransferReason.Glass,
            TransferReason.Metals
        };

        internal static readonly HashSet<TransferReason> ForestryTypes = new HashSet<TransferReason>()
        {
            TransferReason.Lumber,
            TransferReason.Logs,
            TransferReason.Paper,
            TransferReason.PlanedTimber
        };

        internal static readonly HashSet<TransferReason> AgricultureTypes = new HashSet<TransferReason>()
        {
            TransferReason.Grain,
            TransferReason.Food,
            TransferReason.AnimalProducts,
            TransferReason.Flours
        };

        internal static readonly HashSet<TransferReason> GoodsTypes = new HashSet<TransferReason>()
        {
            TransferReason.Goods,
            TransferReason.LuxuryProducts
        };

        internal static readonly HashSet<TransferReason> SnowServiceTypes = new HashSet<TransferReason>()
        {
            TransferReason.Snow,
            TransferReason.SnowMove
        };

        internal static readonly HashSet<TransferReason> MailTypes = new HashSet<TransferReason>()
        {
            TransferReason.Mail,
            TransferReason.UnsortedMail,
            TransferReason.SortedMail,
            TransferReason.OutgoingMail,
            TransferReason.IncomingMail
        };

        internal int GetTransferedSum(HashSet<TransferReason> reasons)
        {
            var query =
                from Transfer in _data
                join types in reasons on Transfer.Key equals types
                select Transfer.Value;
            return query.Sum();
        }
        
        #endregion

        private readonly Dictionary<TransferReason, int> _data;

        internal ReasonTransfersStorage()
        {
            _data = new Dictionary<TransferReason, int>();
        }

        internal ReasonTransfersStorage(TransferReason transferReason, int quantity)
        {
            _data = new Dictionary<TransferReason, int>();
            AddTransfer(transferReason, quantity);
        }

        internal void AddTransfer(TransferReason transferReason, int value)
        {
            if (_data.TryGetValue(transferReason, out _))
                _data[transferReason] += value;
            else
                _data.Add(transferReason, value);
        }

        internal int GetTransferedValue(TransferReason transferReason)
        {
            _data.TryGetValue(transferReason, out int val);
            return val;
        }
    }

    /// <summary>
    /// All transfers in conjunction with ConnectionType:
    ///   Every ConnectionType (by index) refers to ReasonTransfersStorage
    /// </summary>
    internal class ConnectedTransfersStorage
    {
        private readonly ReasonTransfersStorage[] _data;
        //internal ReasonTransfersStorage[] Transfers { get { return _data; } }

        internal ConnectedTransfersStorage()
        {
            _data = new ReasonTransfersStorage[CargoBatch.NumConnectionTypes];
            Init();
        }

        internal ConnectedTransfersStorage(TransferConnectionType transferConnectionType, TransferReason transferReason, int quantity)
        {
            _data = new ReasonTransfersStorage[CargoBatch.NumConnectionTypes];
            Init();
            AddTransfer(transferConnectionType, transferReason, quantity);
        }

        private void Init()
        {
            for (int i = 0; i < CargoBatch.NumConnectionTypes; i++)
                _data[i] = new ReasonTransfersStorage();
        }
        
        internal void AddTransfer(TransferConnectionType transferConnection, TransferReason transferReason, int quantity)
        {
            _data[(int)transferConnection].AddTransfer(transferReason, quantity);
        }
        
        /*
        internal TransfersStorage GetStorageByType(TransferConnectionType type)
        {
            return Transfers[(int)type];
        }
        */
    }

    /// <summary>
    /// Aggregate all transfers by Building
    /// </summary>
    internal class BuildingTransfersStorage
    {
        private readonly Dictionary<ushort, ConnectedTransfersStorage> _data;
        //internal Dictionary<ushort, ConnectedTransfersStorage> BuildingData { get { return _data; } }

        internal BuildingTransfersStorage()
        {
             _data = new Dictionary<ushort, ConnectedTransfersStorage>();
        }

        internal void AddTransfer(CargoBatch cargoBatch)
        {
            if (_data.TryGetValue(cargoBatch.buildingID, out ConnectedTransfersStorage connectedTransfers))
            {
                connectedTransfers.AddTransfer(cargoBatch.transferConnectionType, cargoBatch.transferReason, cargoBatch.transferSize);
            }
            else
            {
                connectedTransfers = new ConnectedTransfersStorage(cargoBatch.transferConnectionType, cargoBatch.transferReason, cargoBatch.transferSize);
                _data.Add(cargoBatch.buildingID, connectedTransfers);
            }
        }
    }
}
