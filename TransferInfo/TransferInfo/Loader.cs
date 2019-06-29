using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using ColossalFramework.UI;
using UnityEngine;
using TransferInfo.Data;

namespace TransferInfo
{
    public class Loader: LoadingExtensionBase
    {
        internal static bool IsActive { get; private set; }
        internal static UI.TransfersStatisticsPanel TransfersStatisticsPanel { get; private set; }
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);

            if (loading.currentMode != AppMode.Game) return;
            DataShared.Data = new TransfersStatistics(Options.StorageVersion);
            HarmonyPatches.Apply();
            IsActive = true;
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame && mode != LoadMode.NewGameFromScenario)
            {
                //note: I won't to perform checks about reloading the game. Be advised - quitting to Desktop must be performed before every new load.
                return;
            }
            TransfersStatisticsPanel = (UI.TransfersStatisticsPanel)UIView.GetAView().AddUIComponent(typeof(UI.TransfersStatisticsPanel));
            Hooking.Setup();
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();

            if (TransfersStatisticsPanel != null) GameObject.Destroy(TransfersStatisticsPanel);
            TransfersStatisticsPanel = null;
        }

        public override void OnReleased()
        {
            base.OnReleased();

            //note: Harmony does not provide 'un-patching'
            DataShared.Data = null;
            IsActive = false;
        }
    }
}
