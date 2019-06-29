using System;
using UnityEngine;

using TransferReason = TransferManager.TransferReason;

namespace TransferInfo.Data
{
    internal readonly struct CargoBatch
    {
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

            if (DataShared.TrackedCargoTypes.Contains(transferReason))
                this.transferReason = (TransferReason)transferReason;
            else
            {
                if(Options.debugEnabled)
                    Debug.LogErrorFormat("TransferInfo: CargoBatch.CargoBatch - Unexpected transfer type: {0}", Enum.GetName(typeof(TransferReason), transferReason));
                this.transferReason = TransferReason.None;

            }
        }
    }
}
