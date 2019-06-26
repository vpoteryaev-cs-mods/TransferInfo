using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransferReason = TransferManager.TransferReason;

namespace TransferInfo.Data
{
    //Not all possible combinations are used, but:
    // 0 - local receive
    // 1 - local sent
    // 2 - imported receive
    // 3 - imported sent
    // 4 - exported receive
    // 5 - exported sent
    [Flags]
    internal enum TransferConnectionType
    {
        Receive = 0x00,
        Sent = 0x01,
        Imported = 0x02,
        Exported = 0x04,
        NumConnectionTypes = 0x06
    }

    internal static class DataShared
    {
        //note: planned to use only selected by user types, so this set may help
        internal static HashSet<int> TrackedCargoTypes { get; }
        internal static TransfersStatistics Data { get; set; }

        static DataShared()
        {
            TrackedCargoTypes = new HashSet<int>
            {
                (int)TransferReason.Garbage,
                (int)TransferReason.GarbageMove,
                (int)TransferReason.Oil,
                (int)TransferReason.Petrol,
                (int)TransferReason.Petroleum,
                (int)TransferReason.Plastics,
                (int)TransferReason.Ore,
                (int)TransferReason.Coal,
                (int)TransferReason.Glass,
                (int)TransferReason.Metals,
                (int)TransferReason.Logs,
                (int)TransferReason.Lumber,
                (int)TransferReason.Paper,
                (int)TransferReason.PlanedTimber,
                (int)TransferReason.Grain,
                (int)TransferReason.Food,
                (int)TransferReason.AnimalProducts,
                (int)TransferReason.Flours,
                (int)TransferReason.Goods,
                (int)TransferReason.LuxuryProducts,
                (int)TransferReason.Snow,
                (int)TransferReason.SnowMove,
                (int)TransferReason.Mail,
                (int)TransferReason.UnsortedMail,
                (int)TransferReason.SortedMail,
                (int)TransferReason.OutgoingMail,
                (int)TransferReason.IncomingMail
            };
        }
    }
}
