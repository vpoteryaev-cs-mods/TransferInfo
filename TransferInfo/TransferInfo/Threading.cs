using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using System.Reflection;
using UnityEngine;

namespace TransferInfo
{
    public class Threading: ThreadingExtensionBase
    {
        public override void OnCreated(IThreading threading)
        {
            base.OnCreated(threading);

            HarmonyPatches.Apply();
        }

        public override void OnReleased()
        {
            base.OnReleased();
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            base.OnUpdate(realTimeDelta, simulationTimeDelta);
        }
    }
}
