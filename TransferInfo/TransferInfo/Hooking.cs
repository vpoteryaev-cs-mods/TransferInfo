using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using ColossalFramework.UI;
using UnityEngine;
using PanelHook;

namespace TransferInfo
{
    internal static class Hooking
    {
        private static MouseEventHandler displayStatisticsHandler;
        private const string displayStatisticsMsg = "Transfer Info -> Display Statistics";

        private static UIPanel statsHookedPanel;

        internal static void Setup()
        {
            var servicePanel = (UIPanel)UIView.library.Get("CityServiceWorldInfoPanel");
            statsHookedPanel = servicePanel?.Find<UIPanel>("Right");
            if (servicePanel == null || statsHookedPanel == null)
            {
                Debug.LogError("TransferInfo: Hooking.Setup - Hooking targets have to be checked");
                return;
            }
#if DEBUG
            Debug.Log("TransferInfo: Hooking.Setup - CityServiceWorldInfoPanel.Right panel found");
#endif
            displayStatisticsHandler = (sender, e) =>
            {
#if DEBUG
                Debug.Log("TransferInfo: Hooking.displayStatisticsHandler - I'm Called");
#endif
                //todo: show Stats window
            };

            if (!HookManager.IsHooked(displayStatisticsHandler, statsHookedPanel))
                HookManager.AddHook(displayStatisticsHandler, statsHookedPanel, displayStatisticsMsg);
        }

        internal static void Cleanup()
        {
            if (HookManager.IsHooked(displayStatisticsHandler, statsHookedPanel))
                 HookManager.RemoveHook(displayStatisticsHandler, statsHookedPanel);
        }
    }
}
