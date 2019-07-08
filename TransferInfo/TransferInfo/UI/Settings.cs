using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using ColossalFramework.UI;

namespace TransferInfo.UI
{
    class Settings
    {
        private static UIDropDown updateIntervalDropDown;
        private static UIButton cleanupButton;

        private static readonly string[] updateIntervalLabels =
        {
            "Monthly",
            "Weekly",
            "Dayly",
            "Hourly"
        };

        internal static void OnLoaded()
        {
            updateIntervalDropDown.selectedIndex = TransferInfo.Data.DataShared.Data.updateInterval;
        }

        internal static void OnSettingsUI(UIHelperBase helper)
        {
            int updateInterval = 0;

            if (Loader.IsActive)
                updateInterval = TransferInfo.Data.DataShared.Data.updateInterval;
            updateIntervalDropDown = (UIDropDown)helper.AddDropdown("Update interval", updateIntervalLabels, updateInterval, state =>
            {
                TransferInfo.Data.DataShared.Data.updateInterval = state;
            });
            var cleanGroup = helper.AddGroup("Before deleting this mod push \"Clean data\" button,\n" +
                "save the game, exit and unsubscribe from \"Transfer Info\"");
            cleanupButton = (UIButton)cleanGroup.AddButton("Clean data", () => { Serialization.CleanData(); Hooking.Cleanup(); });

            updateIntervalDropDown.isEnabled = Loader.IsActive;
            cleanupButton.isEnabled = Loader.IsActive;

            helper.AddSpace(5);
            var globalGroup = helper.AddGroup("Global options");
            globalGroup.AddCheckbox("Enable debug logging", Options.debugEnabled, state => Options.debugEnabled.value = state);
        }
    }
}
