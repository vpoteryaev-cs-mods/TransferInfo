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

        private static MouseEventHandler displayCragoChartHandler;
        private const string displayCargoChartMsg = "Transfer Info -> Display Cargo Chart";

        private static UIPanel vehiclePanel;

        internal static void Setup()
        {
            var servicePanel = (UIPanel)UIView.library.Get("CityServiceWorldInfoPanel");
            statsHookedPanel = servicePanel?.Find<UIPanel>("Right");
            if (servicePanel == null || statsHookedPanel == null)
            {
                if(Options.debugEnabled)
                    Debug.LogError("TransferInfo: Hooking.Setup - Hooking targets have to be checked");
                return;
            }
#if DEBUG
            Debug.Log("TransferInfo: Hooking.Setup - CityServiceWorldInfoPanel.Right panel found");
#endif
            displayStatisticsHandler = (sender, e) =>
            {
                //todo: update PanelHook with additional check functionality
                ushort buildingID = WorldInfoPanel.GetCurrentInstanceID().Building;
                if (buildingID != 0 && BuildingManager.instance.m_buildings.m_buffer[buildingID].Info.m_buildingAI is CargoStationAI)
                {
                    //note: temporary disable own panel
                    //Loader.TransfersStatisticsPanel.Show();
                    Loader.OrigCargoInfoPanel.Show();
                }
            };

            if (!HookManager.IsHooked(displayStatisticsHandler, statsHookedPanel))
                HookManager.AddHook(displayStatisticsHandler, statsHookedPanel, displayStatisticsMsg, BuildingChecker);

            var vehicleInfoPanel = (UIPanel)UIView.library.Get("CityServiceVehicleWorldInfoPanel");
            vehiclePanel = vehicleInfoPanel?.Find<UIPanel>("Panel");
            if (vehiclePanel == null)
            {
                if (Options.debugEnabled)
                    Debug.LogError("TransferInfo: Hooking.Setup - Hooking targets have to be checked");
                return;
            }
#if DEBUG
            Debug.Log("TransferInfo: Hooking.Setup - CityServiceVehicleWorldInfoPanel.Panel panel found");
#endif
            displayCragoChartHandler = (sender, e) =>
            {
                Loader.CarriedCargoPanel.Show();
            };

            if (!HookManager.IsHooked(displayCragoChartHandler, vehiclePanel))
            {
                HookManager.AddHook(displayCragoChartHandler, vehiclePanel, displayCargoChartMsg, VehicleChecker);
            }
        }

        internal static void Cleanup()
        {
            if (HookManager.IsHooked(displayStatisticsHandler, statsHookedPanel))
                 HookManager.RemoveHook(displayStatisticsHandler, statsHookedPanel);
            if (HookManager.IsHooked(displayCragoChartHandler, vehiclePanel))
                HookManager.RemoveHook(displayCragoChartHandler, vehiclePanel);
        }

        internal static bool BuildingChecker()
        {
            ushort buildingID = WorldInfoPanel.GetCurrentInstanceID().Building;
            if (buildingID != 0 && BuildingManager.instance.m_buildings.m_buffer[buildingID].Info.m_buildingAI is CargoStationAI)
                return true;
            return false;
        }

        internal static bool VehicleChecker()
        {
            var vehicleID = WorldInfoPanel.GetCurrentInstanceID().Vehicle;
            if (vehicleID != 0)
            {
                var ai = VehicleManager.instance.m_vehicles.m_buffer[vehicleID].Info.m_vehicleAI;
                return (ai is CargoTrainAI || ai is CargoShipAI) ? true : false;
            }
            return false;
        }
    }
}
