﻿using Harmony;
using System.Reflection;
using UnityEngine;
using TransferInfo.Data;

namespace TransferInfo
{
    static class HarmonyPatches
    {
        private static void ConditionalPatch(HarmonyInstance harmony, MethodBase method, HarmonyMethod prefix, HarmonyMethod postfix)
        {
            if (harmony.GetPatchInfo(method)?.Owners?.Contains(harmony.Id) == true)
            {
#if DEBUG
                Debug.LogWarningFormat("TransferInfo: HarmonyPatches.ConditionalPatch - Harmony patches already present for {0}", string.Format("{0}.{1}", method.ReflectedType?.Name ?? "(null)", method.Name));
#endif
                return;
            }
            harmony.Patch(method, prefix, postfix);
        }

        public static void Apply()
        {
            var harmony = HarmonyInstance.Create(Options.HarmonyModID);

            var truckSetSource = typeof(CargoTruckAI).GetMethod("SetSource");
            var truckSetSourcePostfix = typeof(HarmonyPatches).GetMethod("CargoTruckAI_PostSetSource");

            var truckChangeVehicleType = typeof(CargoTruckAI).GetMethod("ChangeVehicleType", BindingFlags.Instance | BindingFlags.NonPublic);
            var truckChangeVehicleTypePrefix = typeof(HarmonyPatches).GetMethod("CargoTruckAI_PreChangeVehicleType");
            var truckChangeVehicleTypePostfix = typeof(HarmonyPatches).GetMethod("CargoTruckAI_PostChangeVehicleType");

            ConditionalPatch(harmony, truckSetSource, null, new HarmonyMethod(truckSetSourcePostfix));
            ConditionalPatch(harmony, truckChangeVehicleType, new HarmonyMethod(truckChangeVehicleTypePrefix), new HarmonyMethod(truckChangeVehicleTypePostfix));
        }

        public static void CargoTruckAI_PostSetSource(ref Vehicle data, ushort sourceBuilding)
        {
            if (sourceBuilding != 0 && BuildingManager.instance.m_buildings.m_buffer[sourceBuilding].Info.m_buildingAI is CargoStationAI)
            {
                var batch = new CargoBatch(sourceBuilding, false, data.m_transferType, data.m_transferSize, data.m_flags);
                DataShared.Data.AddBatch(batch);
            }
        }
        
        public static void CargoTruckAI_PreChangeVehicleType(out CargoBatch __state, ref Vehicle vehicleData, PathUnit.Position pathPos, uint laneID)
        {
            Vector3 vector = NetManager.instance.m_lanes.m_buffer[laneID].CalculatePosition(0.5f);
            NetInfo info = NetManager.instance.m_segments.m_buffer[pathPos.m_segment].Info;
            ushort buildingID = BuildingManager.instance.FindBuilding(vector, 100f, info.m_class.m_service, ItemClass.SubService.None, Building.Flags.None, Building.Flags.None);

            if (buildingID != 0 && BuildingManager.instance.m_buildings.m_buffer[buildingID].Info.m_buildingAI is CargoStationAI)
                __state = new CargoBatch(buildingID, true, vehicleData.m_transferType, vehicleData.m_transferSize, vehicleData.m_flags);
            else
                __state = null;
        }

        public static void CargoTruckAI_PostChangeVehicleType(bool __result, ref CargoBatch __state)
        {
            if (__result && __state != null)
            {
                DataShared.Data.AddBatch(__state);
            }
        }
    }
}
