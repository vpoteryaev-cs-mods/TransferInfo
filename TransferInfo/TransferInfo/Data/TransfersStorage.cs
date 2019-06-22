using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TransferReason = TransferManager.TransferReason;

namespace TransferInfo.Data
{
    //used as storage by pairs: TransferReason - Quantity
    internal class TransfersStorage
    {
        //Code in the JoiningByType region is related to Note https://github.com/vpoteryaev/CS-TransferInfo/projects/1#card-23084985
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
            TransferReason.Paper,
            TransferReason.PlanedTimber
        };

        internal static readonly HashSet<TransferReason> AgricultureTypes = new HashSet<TransferReason>()
        {
            TransferReason.Grain,
            TransferReason.AnimalProducts,
            TransferReason.Flours
        };

        internal static readonly HashSet<TransferReason> GoodsTypes = new HashSet<TransferReason>()
        {
            TransferReason.Goods,
            TransferReason.Food,
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
                from Transfer in Transfers
                join types in reasons on Transfer.Key equals types
                select Transfer.Value;
            return query.Sum();
        }
        #endregion

        private readonly Dictionary<TransferReason, int> Transfers;


        internal TransfersStorage()
        {
            Transfers = new Dictionary<TransferReason, int>();
        }

        internal void AddToTransfers(TransferReason transferReason, int value)
        {
            if (Transfers.TryGetValue(transferReason, out _))
                Transfers[transferReason] += value;
            else
                Transfers.Add(transferReason, value);
        }

        internal int GetTransferedValue(TransferReason transferReason)
        {
            Transfers.TryGetValue(transferReason, out int val);
            return val;
        }
    }
}
