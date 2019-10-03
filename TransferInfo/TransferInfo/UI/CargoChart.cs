using ColossalFramework.UI;
using System.Linq;
using TransferInfo.Data;
using UnityEngine;

namespace TransferInfo.UI
{
    class CargoChart : UIRadialChart
    {
        public CargoChart()
        {
            spriteName = "PieChartBg";
            size = new Vector2(90, 90);
            for (int i = 0; i < DataShared.TrackedCargoTypes.Count; i++)
            {
                var resourceColor = TransferManager.instance.m_properties.m_resourceColors[DataShared.TrackedCargoTypes.ElementAt(i)];
                AddSlice();
                GetSlice(i).innerColor = GetSlice(i).outterColor = resourceColor;
            }
            SetValues(DataShared.TrackedCargoTypes.Select(t => 0f).ToArray());
        }
    }
}
