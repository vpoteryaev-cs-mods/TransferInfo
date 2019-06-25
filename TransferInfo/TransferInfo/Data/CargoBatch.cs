using System;
using UnityEngine;

using TransferReason = TransferManager.TransferReason;

namespace TransferInfo.Data
{
    internal readonly struct CargoBatch
    {
        internal static readonly int NumConnectionTypes = 6;

        internal readonly ushort buildingID;
        internal readonly ushort transferSize;
        internal readonly TransferConnectionType transferConnectionType;
        internal readonly TransferReason transferReason;
        internal CargoBatch(ushort buildingID, bool incoming, byte transferReason, ushort transferSize, Vehicle.Flags flags)
        {
            this.transferSize = transferSize;
            this.buildingID = buildingID;
            transferConnectionType = incoming ? TransferConnectionType.Receive : TransferConnectionType.Sent;
            if ((flags & Vehicle.Flags.Exporting) != 0)
                transferConnectionType |= TransferConnectionType.Exported;
            else if ((flags & Vehicle.Flags.Importing) != 0)
                transferConnectionType |= TransferConnectionType.Imported;

            switch ((TransferReason)transferReason)
            {
                case TransferReason.Garbage:
                case TransferReason.GarbageMove:
                case TransferReason.Oil:
                case TransferReason.Petrol:
                case TransferReason.Petroleum:
                case TransferReason.Plastics:
                case TransferReason.Ore:
                case TransferReason.Coal:
                case TransferReason.Glass:
                case TransferReason.Metals:
                case TransferReason.Logs:
                case TransferReason.Lumber:
                case TransferReason.Paper:
                case TransferReason.PlanedTimber:
                case TransferReason.Grain:
                case TransferReason.Food:
                case TransferReason.AnimalProducts:
                case TransferReason.Flours:
                case TransferReason.Goods:
                case TransferReason.LuxuryProducts:
                case TransferReason.Snow:
                case TransferReason.SnowMove:
                case TransferReason.Mail:
                case TransferReason.UnsortedMail:
                case TransferReason.SortedMail:
                case TransferReason.OutgoingMail:
                case TransferReason.IncomingMail:
                    this.transferReason = (TransferReason)transferReason;
                    break;
                default:
#if DEBUG
                    Debug.LogErrorFormat("Unexpected transfer type: {0}", Enum.GetName(typeof(TransferReason), transferReason));
#endif
                    this.transferReason = TransferReason.None;
                    break;
            }
        }
    }
}
