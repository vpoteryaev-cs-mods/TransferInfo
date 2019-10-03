using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransferInfo.Data;
using UnityEngine;

namespace TransferInfo.UI
{
    class CarriedCargoPanel : UIPanel
    {
        private const int Width = 120;
        private const int HandleHeight = 40;
        private readonly Vector2 ExitButtonSize = new Vector2(32, 32);
        private UILabel windowLabel;

        private readonly Vector2 ChartPanelSize = new Vector2(90, 90);
        private readonly Vector2 ChartSize = new Vector2(60, 60);
        private readonly RectOffset Padding = new RectOffset(2, 2, 2, 2);

        private CargoChart chart;

        public CarriedCargoPanel()
        {
        }

        public override void Awake()
        {
            base.Awake();

            backgroundSprite = "MenuPanel2";
            opacity = 0.9f;

            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;

            var handle = AddUIComponent<UIDragHandle>();
            handle.size = new Vector2(Width, HandleHeight);

            windowLabel = handle.AddUIComponent<UILabel>();
            windowLabel.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.CenterHorizontal;
            windowLabel.text = "Cargo Load";

            var closeButton = handle.AddUIComponent<UIButton>();
            closeButton.size = ExitButtonSize;
            closeButton.relativePosition = new Vector3(Width - ExitButtonSize.x, 0, 0);
            closeButton.anchor = UIAnchorStyle.Top | UIAnchorStyle.Right;
            closeButton.normalBgSprite = "buttonclose";
            closeButton.pressedBgSprite = "buttonclosepressed";
            closeButton.hoveredBgSprite = "buttonclosehover";
            closeButton.eventClicked += (sender, e) => Hide();

            var mainPanel = AddUIComponent<UIPanel>();
            mainPanel.size = new Vector2(Width, ChartPanelSize.y);
            mainPanel.autoLayout = true;
            mainPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            mainPanel.autoLayoutStart = LayoutStart.TopRight;
            mainPanel.autoLayoutPadding = Padding;

            chart = mainPanel.AddUIComponent<CargoChart>();
            chart.size = ChartSize;
            chart.SetValues(DataShared.TrackedCargoTypes.Select(t => 0f).ToArray());

            FitChildren(new Vector2(Padding.top, Padding.left));
        }

        public override void Start()
        {
            base.Start();

            canFocus = true;
            isInteractive = true;
            Hide();
        }

        public override void Update()
        {
            base.Update();

            if (!isVisible)
            {
                return;
            }
            UpdateChart();
        }

        private void UpdateChart()
        {
            var vehicleID = WorldInfoPanel.GetCurrentInstanceID().Vehicle;
            if (vehicleID != 0)
            {
                int guard = 0;

                Vehicle[] vehicles = VehicleManager.instance.m_vehicles.m_buffer;

                // Find leading vehicle that actually has all the cargo
                while (vehicles[vehicleID].m_leadingVehicle != 0)
                {
                    vehicleID = vehicles[vehicleID].m_leadingVehicle;

                    if (guard++ >= ushort.MaxValue)
                    {
                        if (Options.debugEnabled)
                        {
                            Debug.LogError("TransferInfo: CarriedCargoPanel.UpdateChart - Invalid list detected!");
                        }
                        Hide();
                        return;
                    }
                }

                var ai = vehicles[vehicleID].Info.m_vehicleAI;
                if (ai is CargoTrainAI || ai is CargoShipAI)
                {
                    var cargoID = vehicles[vehicleID].m_firstCargo;
                    
                    var cargoItems = new Dictionary<TransferManager.TransferReason, int>();

                    guard = 0;
                    while (cargoID != 0)
                    {
                        if (cargoItems.TryGetValue((TransferManager.TransferReason)vehicles[cargoID].m_transferType, out _))
                        {
                            cargoItems[(TransferManager.TransferReason)vehicles[cargoID].m_transferType] += vehicles[cargoID].m_transferSize;
                        }
                        else
                        {
                            cargoItems.Add((TransferManager.TransferReason)vehicles[cargoID].m_transferType, vehicles[cargoID].m_transferSize);
                        }
                        
                        cargoID = vehicles[cargoID].m_nextCargo;

                        if (guard++ >= ushort.MaxValue)
                        {
                            if (Options.debugEnabled)
                            {
                                Debug.LogError("TransferInfo: CarriedCargoPanel.UpdateChart - Invalid list detected!");
                            }
                            Hide();
                            return;
                        }
                    }
                    var total = cargoItems.Values.Sum();
                    chart.tooltip = string.Format("{0:N3} {1}", total / 1000d, "ton(s)");

                    if (total == 0)
                    {
                        chart.SetValues(DataShared.TrackedCargoTypes.Select(t => 0f).ToArray());
                        return;
                    }

                    int[] result = new int[DataShared.TrackedCargoTypes.Count];
                    for (int i = 0; i < DataShared.TrackedCargoTypes.Count; i++)
                    {
                        cargoItems.TryGetValue((TransferManager.TransferReason)DataShared.TrackedCargoTypes.ElementAt(i), out int val);
                        result[i] = val;
                    }
                    chart.SetValues(result.Select(v => v / (float)total).ToArray());
                    return;
                }
            }
            Hide();
        }
    }
}
