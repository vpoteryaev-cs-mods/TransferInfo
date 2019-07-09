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
        private DateTime lastUpdate = DateTime.MinValue;

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
            if ((Data.DataShared.Data.updateInterval == 3 && lastUpdate.Hour < tempDateTime.Hour && tempDateTime.Minute == 0) ||
                (Data.DataShared.Data.updateInterval == 2 && lastUpdate.Day < tempDateTime.Day && tempDateTime.Hour == 0) ||
                (Data.DataShared.Data.updateInterval == 1 && lastUpdate.Day < tempDateTime.Day && tempDateTime.DayOfWeek == DayOfWeek.Monday) ||
                (Data.DataShared.Data.updateInterval == 0 && lastUpdate.Month < tempDateTime.Month && tempDateTime.Day == 1))
            {
                lastUpdate = tempDateTime;
                Data.DataShared.Data.UpdateStatistics();
            }
        }
    }
}
