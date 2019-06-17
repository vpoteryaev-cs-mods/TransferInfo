using Harmony;
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
                Debug.LogWarningFormat("Harmony patches already present for {0}", string.Format("{0}.{1}", method.ReflectedType?.Name ?? "(null)", method.Name));
#endif
                return;
            }
            harmony.Patch(method, prefix, postfix);
        }

        public static void Apply()
        {
            //todo: store basic shared values in some common place
            string sModID = "vlk.TransferInfo";
            var harmony = HarmonyInstance.Create(sModID);

            var truckSetSource = typeof(CargoTruckAI).GetMethod("SetSource");
            var truckSetSourcePostfix = typeof(HarmonyPatches).GetMethod("CargoTruckAI_PostSetSource");

            var truckChangeVehicleType = typeof(CargoTruckAI).GetMethod("ChangeVehicleType", BindingFlags.Instance | BindingFlags.NonPublic);
            var truckChangeVehicleTypePrefix = typeof(HarmonyPatches).GetMethod("CargoTruckAI_PreChangeVehicleType");
            var truckChangeVehicleTypePostfix = typeof(HarmonyPatches).GetMethod("CargoTruckAI_PostChangeVehicleType");

            ConditionalPatch(harmony, truckSetSource, null, new HarmonyMethod(truckSetSourcePostfix));
            ConditionalPatch(harmony, truckChangeVehicleType, new HarmonyMethod(truckChangeVehicleTypePrefix), new HarmonyMethod(truckChangeVehicleTypePostfix));
#if DEBUG
            Debug.Log("Harmony patches applied");
#endif
        }

        //note: seems in the Harmony docs stated that unused parameters could be omitted
        //public static void CargoTruckAI_PostSetSource(ushort vehicleID, ref Vehicle data, ushort sourceBuilding)
        public static void CargoTruckAI_PostSetSource(ref Vehicle data, ushort sourceBuilding)
        {
            var batch = new CargoBatch(sourceBuilding, false, data.m_transferType, data.m_transferSize, data.m_flags);
            //todo: add new batch to common database
            //TransferStorage.Instance.Count(batch);
        }
        
        //public static void CargoTruckAI_PreChangeVehicleType(out CargoBatch __state, ushort vehicleID, ref Vehicle vehicleData, PathUnit.Position pathPos, uint laneID)
        public static void CargoTruckAI_PreChangeVehicleType(out CargoBatch __state, ref Vehicle vehicleData, PathUnit.Position pathPos, uint laneID)
        {
            Vector3 vector = NetManager.instance.m_lanes.m_buffer[laneID].CalculatePosition(0.5f);
            NetInfo info = NetManager.instance.m_segments.m_buffer[pathPos.m_segment].Info;
            ushort buildingID = BuildingManager.instance.FindBuilding(vector, 100f, info.m_class.m_service, ItemClass.SubService.None, Building.Flags.None, Building.Flags.None);

            __state = new CargoBatch(buildingID, true, vehicleData.m_transferType, vehicleData.m_transferSize, vehicleData.m_flags);
        }

        //public static void CargoTruckAI_PostChangeVehicleType(bool __result, ref CargoBatch __state, ushort vehicleID, ref Vehicle vehicleData, PathUnit.Position pathPos, uint laneID)
        public static void CargoTruckAI_PostChangeVehicleType(bool __result, ref CargoBatch __state)
        {
            if (__result)
            {
                //todo: add new batch to common database
                //TransferStorage.Instance.Count(__state);
            }
        }
    }
}
