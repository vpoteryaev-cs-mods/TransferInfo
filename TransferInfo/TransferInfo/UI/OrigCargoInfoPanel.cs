using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ColossalFramework.UI;
using UnityEngine;
using TransferInfo.Data;


namespace TransferInfo.UI
{
    class UICargoChart : UIRadialChart
    {
        public UICargoChart()
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

    class OrigCargoInfoPanel: UIPanel
    {
        private const int Width = 384;
        private const int HandleHeight = 40;
        private const int LabelHeight = 20;
        private const int LabelWidth = 90;
        private const int StatPanelHeight = 30;
        private readonly Vector2 ExitButtonSize = new Vector2(32, 32);
        private readonly Vector2 ChartSize = new Vector2(90, 90);
        private readonly RectOffset Padding = new RectOffset(2, 2, 2, 2);
        private readonly Color32 CargoUnitColor = new Color32(206, 248, 0, 255);
        private readonly Vector2 ModeButtonSize = new Vector2(32, 10);
        private bool bDisplayPrevPeriod;
        private ushort lastSelectedBuilding;

        public OrigCargoInfoPanel()
        {
        }

        private readonly List<UICargoChart> charts = new List<UICargoChart>();
        private readonly List<UILabel> labels = new List<UILabel>();
        private UILabel windowLabel, localLabel, importLabel, exportLabel, rcvdLabel, sentLabel;
        private UIButton switchPeriodButton;

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

            var closeButton = handle.AddUIComponent<UIButton>();
            closeButton.size = ExitButtonSize;
            closeButton.relativePosition = new Vector3(Width - ExitButtonSize.x, 0, 0);
            closeButton.anchor = UIAnchorStyle.Top | UIAnchorStyle.Right;
            closeButton.normalBgSprite = "buttonclose";
            closeButton.pressedBgSprite = "buttonclosepressed";
            closeButton.hoveredBgSprite = "buttonclosehover";
            closeButton.eventClicked += (sender, e) => Hide();

            var labelPanel = AddUIComponent<UIPanel>();
            labelPanel.size = new Vector2(Width, LabelHeight);
            labelPanel.autoLayout = true;
            labelPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            labelPanel.autoLayoutStart = LayoutStart.TopRight;
            labelPanel.autoLayoutPadding = Padding;

            localLabel = labelPanel.AddUIComponent<UILabel>();
            localLabel.autoSize = false;
            localLabel.size = new Vector2(ChartSize.x, LabelHeight);
            localLabel.textAlignment = UIHorizontalAlignment.Center;

            importLabel = labelPanel.AddUIComponent<UILabel>();
            importLabel.autoSize = false;
            importLabel.size = new Vector2(ChartSize.x, LabelHeight);
            importLabel.textAlignment = UIHorizontalAlignment.Center;

            exportLabel = labelPanel.AddUIComponent<UILabel>();
            exportLabel.autoSize = false;
            exportLabel.size = new Vector2(ChartSize.x, LabelHeight);
            exportLabel.textAlignment = UIHorizontalAlignment.Center;

            var rcvdPanel = AddUIComponent<UIPanel>();
            rcvdPanel.size = new Vector2(Width, ChartSize.y);
            rcvdPanel.autoLayout = true;
            rcvdPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            rcvdPanel.autoLayoutStart = LayoutStart.TopRight;
            rcvdPanel.autoLayoutPadding = Padding;

            rcvdLabel = rcvdPanel.AddUIComponent<UILabel>();
            rcvdLabel.textAlignment = UIHorizontalAlignment.Right;
            rcvdLabel.verticalAlignment = UIVerticalAlignment.Middle;
            rcvdLabel.autoSize = false;
            rcvdLabel.size = new Vector2(LabelWidth, ChartSize.y);

            var rcvdStatPanel = AddUIComponent<UIPanel>();
            rcvdStatPanel.size = new Vector2(Width, StatPanelHeight);
            rcvdStatPanel.autoLayout = true;
            rcvdStatPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            rcvdStatPanel.autoLayoutStart = LayoutStart.TopRight;
            rcvdStatPanel.autoLayoutPadding = Padding;

            var sentPanel = AddUIComponent<UIPanel>();
            sentPanel.size = new Vector2(Width, ChartSize.y);
            sentPanel.autoLayout = true;
            sentPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            sentPanel.autoLayoutStart = LayoutStart.TopRight;
            sentPanel.autoLayoutPadding = Padding;

            sentLabel = sentPanel.AddUIComponent<UILabel>();
            sentLabel.textAlignment = UIHorizontalAlignment.Right;
            sentLabel.verticalAlignment = UIVerticalAlignment.Middle;
            sentLabel.autoSize = false;
            sentLabel.size = new Vector2(LabelWidth, ChartSize.y);

            var sentStatPanel = AddUIComponent<UIPanel>();
            sentStatPanel.size = new Vector2(Width, StatPanelHeight);
            sentStatPanel.autoLayout = true;
            sentStatPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            sentStatPanel.autoLayoutStart = LayoutStart.TopRight;
            sentStatPanel.autoLayoutPadding = Padding;

            switchPeriodButton = sentStatPanel.AddUIComponent<UIButton>();
            switchPeriodButton.text = "Cur";
            switchPeriodButton.normalBgSprite = "ButtonMenu";
            switchPeriodButton.pressedBgSprite = "ButtonMenuPressed";
            switchPeriodButton.hoveredBgSprite = "ButtonMenuHovered";
            switchPeriodButton.textScale = 0.6f;
            switchPeriodButton.autoSize = false;
            switchPeriodButton.size = ModeButtonSize;

            switchPeriodButton.eventClicked += (sender, e) =>
            {
                bDisplayPrevPeriod = !bDisplayPrevPeriod;
                switchPeriodButton.text = bDisplayPrevPeriod ? "Prev" : "Cur";
                switchPeriodButton.tooltip = bDisplayPrevPeriod
                    ? "Switch between displayed periods (now displaying values for the previous period)"
                    : "Switch between displayed periods (now displaying values for the current period)";
                switchPeriodButton.RefreshTooltip();
            };

            for (int n = 0; n < (int)TransferConnectionType.NumConnectionTypes; n++)
            {
                var chart = (n % 2 == 1 ? sentPanel : rcvdPanel).AddUIComponent<UICargoChart>();
                chart.size = ChartSize;
                charts.Add(chart);

                var label = (n % 2 == 1 ? sentStatPanel : rcvdStatPanel).AddUIComponent<UILabel>();
                label.autoSize = false;
                label.size = new Vector2(ChartSize.x, StatPanelHeight);
                label.textScale = 0.8f;
                label.textColor = CargoUnitColor;
                label.textAlignment = UIHorizontalAlignment.Center;
                labels.Add(label);
            }

            windowLabel.text = "Cargo Statistics";
            localLabel.text = "LOCAL";
            importLabel.text = "IMPORT";
            exportLabel.text = "EXPORT";
            rcvdLabel.text = "RECEIVED";
            sentLabel.text = "SENT";

            FitChildren(new Vector2(Padding.top, Padding.left));

            //UpdateCounterValues();
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

            if (!isVisible) return;

            if (WorldInfoPanel.GetCurrentInstanceID().Building != 0)
                lastSelectedBuilding = WorldInfoPanel.GetCurrentInstanceID().Building;

            UpdateCounterValues();
        }

        private int[] GetBuildingTransferedValues(int period, ushort buildingID, TransferConnectionType transferConnectionType)
        {
            int[] result = new int[DataShared.TrackedCargoTypes.Count];
            for (int i = 0; i < DataShared.TrackedCargoTypes.Count; i++)
            {
                result[i] = DataShared.Data.GetBuildingTransfersStorage(period, buildingID, transferConnectionType, (TransferManager.TransferReason)DataShared.TrackedCargoTypes.ElementAt(i));
            }
            return result;
        }

        public void UpdateCounterValues()
        {
            int period = bDisplayPrevPeriod ? 1 : 0;

            for (var i = 0; i < (int)TransferConnectionType.NumConnectionTypes; i++)
            {
                int[] buildingData = GetBuildingTransferedValues(period, lastSelectedBuilding, (TransferConnectionType)i);
                int categoryTotal = buildingData.Sum();

                labels[i].text = string.Format("{0:0}{1}", categoryTotal / 1000, "K Units");

                if (categoryTotal == 0)
                {
                    charts[i].SetValues(DataShared.TrackedCargoTypes.Select(t => 0f).ToArray());
                    continue;
                }

                charts[i].SetValues(buildingData.Select(t => t / (float)categoryTotal).ToArray());
            }
        }
    }
}
