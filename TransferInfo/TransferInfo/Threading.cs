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
        private DateTime lastUpdate;
        //public override void OnCreated(IThreading threading)
        //{
        //    base.OnCreated(threading);
        //}

        //public override void OnReleased()
        //{
        //    base.OnReleased();
        //}

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            base.OnUpdate(realTimeDelta, simulationTimeDelta);

            if (!Loader.IsActive) return;

            DateTime tempDateTime = SimulationManager.instance.m_currentGameTime;
            if ((Options.useHourlyUpdates && lastUpdate.Hour < tempDateTime.Hour) ||
                (!Options.useHourlyUpdates && lastUpdate < tempDateTime && tempDateTime.Day == 1))
            {
                lastUpdate = tempDateTime;
                Data.DataShared.Data.UpdateStatistics();
            }
        }
    }
}
