using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;

namespace TransferInfo.UI
{
    class Settings
    {
        private const string _info =
            "Before deleting this mod push \"Clean data\" button,\n" +
            "save the game, exit and unsubscribe from \"Transfer Info\"";

        internal static void OnSettingsUI(UIHelperBase helper)
        {
            var group = helper.AddGroup(_info);
            group.AddButton("Clean data", () => { Serialization.CleanData(); Hooking.Cleanup(); });
            var debugGroup = helper.AddGroup("Debug");
            debugGroup.AddCheckbox("Enable debug logging", Options.debugEnabled, state => Options.debugEnabled.value = state);
        }
    }
}
