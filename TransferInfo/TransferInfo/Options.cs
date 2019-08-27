using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework;

namespace TransferInfo
{
    class Options
    {
        public static readonly string HarmonyModID = "vlk.TransferInfo.Harmony";
        public static readonly string GameStorageID = "vlk.TransferInfo.GameStorage";
        public static readonly string StorageVersion = "1.0.0.1";
        internal static bool Cleaning = false;
        internal static readonly string SettingsFileName = "TransferInfoSettings";
        internal static SavedBool debugEnabled = new SavedBool("debugEnabled", SettingsFileName);
    }
}
