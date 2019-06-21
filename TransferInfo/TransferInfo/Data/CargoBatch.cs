using System;
using UnityEngine;

using TransferType = TransferManager.TransferReason;

namespace TransferInfo.Data
{
    //all possible combinations are used
    // 0 - local receive
    // 1 - local sent
    // 2 - imported receive
    // 3 - imported sent
    // ...
    [Flags]
    internal enum TransferConnectionType
    {
        None = 0x00,
        Sent = 0x01,
        Imported = 0x02,
        Exported = 0x04
    }

    internal readonly struct CargoBatch
    {
        internal readonly ushort buildingID;
        internal readonly ushort transferSize;
        internal readonly TransferConnectionType transferConnectionType;
        internal readonly TransferType transferType;
        internal CargoBatch(ushort buildingID, bool incoming, byte transferType, ushort transferSize, Vehicle.Flags flags)
        {
            this.transferSize = transferSize;
            this.buildingID = buildingID;
            transferConnectionType = incoming ? TransferConnectionType.None : TransferConnectionType.Sent;

            if ((flags & Vehicle.Flags.Exporting) != 0)
                transferConnectionType |= TransferConnectionType.Exported;
            else if ((flags & Vehicle.Flags.Importing) != 0)
                transferConnectionType |= TransferConnectionType.Imported;

            this.transferType = (TransferType)transferType;
            switch ((TransferType)transferType)
            {
                case TransferType.Garbage:
                case TransferType.GarbageMove:
                case TransferType.Oil:
                case TransferType.Petrol:
                case TransferType.Petroleum:
                case TransferType.Plastics:
                case TransferType.Ore:
                case TransferType.Coal:
                case TransferType.Glass:
                case TransferType.Metals:
                case TransferType.Lumber:
                case TransferType.Paper:
                case TransferType.PlanedTimber:
                case TransferType.Grain:
                case TransferType.AnimalProducts:
                case TransferType.Flours:
                case TransferType.Goods:
                case TransferType.Food:
                case TransferType.LuxuryProducts:
                case TransferType.Snow:
                case TransferType.SnowMove:
                case TransferType.Mail:
                case TransferType.UnsortedMail:
                case TransferType.SortedMail:
                case TransferType.OutgoingMail:
                case TransferType.IncomingMail:
                    this.transferType = (TransferType)transferType;
                    break;
                default:
#if DEBUG
                    Debug.LogErrorFormat("Unexpected transfer type: {0}", Enum.GetName(typeof(TransferType), transferType));
#endif
                    break;
            }
        }
    }
}
