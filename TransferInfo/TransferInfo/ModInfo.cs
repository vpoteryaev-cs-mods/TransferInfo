using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using ColossalFramework;
using UnityEngine;

namespace TransferInfo
{
    public class ModInfo: IUserMod
    {
        public string Name => "Transfer Info";
        public string Description => "Additional statistics of movement of goods";

        public ModInfo()
        {
            try
            {
                GameSettings.AddSettingsFile(new SettingsFile { fileName = Options.SettingsFileName });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        public void OnSettingsUI(UIHelperBase helper)
        {
            UI.Settings.OnSettingsUI(helper);
        }
    }
}
