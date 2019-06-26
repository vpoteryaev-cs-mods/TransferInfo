using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using TransferInfo.Data;

namespace TransferInfo
{
    public class Loader: LoadingExtensionBase
    {
        internal static bool IsActive { get; private set; }
        internal static TransfersStatistics Data { get; set; }

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);

            if (loading.currentMode != AppMode.Game) return;
            Data = new TransfersStatistics(Options.StorageVersion);
            HarmonyPatches.Apply();
            IsActive = true;
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            if (mode != LoadMode.LoadGame || mode != LoadMode.NewGame || mode != LoadMode.NewGameFromScenario)
            {
                //note: I won't to perform checks about reloading the game. Be advised - quitting to Desktop must be performed before every new load.
                return;
            }
            //todo: ui, panels hooking etc.
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();

            //todo: free resources from custom ui
        }

        public override void OnReleased()
        {
            base.OnReleased();

            //note: Harmony does not provide 'un-patching'
            Data = null;
            IsActive = false;
        }
    }
}
