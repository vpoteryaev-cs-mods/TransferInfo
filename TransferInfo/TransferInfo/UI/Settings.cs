using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;

namespace TransferInfo.UI
{
    class Settings
    {
        internal static void OnSettingsUI(UIHelperBase helper)
        {
            var dataUpdateGroup = helper.AddGroup("Update statistics interval\n\n" +
                "Note: by default mounthly updates in the \"Cargo Statistics\" window are used.\n" +
                "Seems too long for the \"Real Time\" mod users.");
            dataUpdateGroup.AddCheckbox("Use hourly update", Options.useHourlyUpdates, state => Options.useHourlyUpdates.value = state);

            var cleanGroup = helper.AddGroup("Before deleting this mod push \"Clean data\" button,\n" +
                "save the game, exit and unsubscribe from \"Transfer Info\"");
            cleanGroup.AddButton("Clean data", () => { Serialization.CleanData(); Hooking.Cleanup(); });

            var debugGroup = helper.AddGroup("Debug");
            debugGroup.AddCheckbox("Enable debug logging", Options.debugEnabled, state => Options.debugEnabled.value = state);
        }
    }
}
