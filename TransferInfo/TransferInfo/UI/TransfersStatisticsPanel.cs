using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using UnityEngine;
using TransferInfo.Data;

namespace TransferInfo.UI
{
    internal class TransfersStatisticsPanel : UIPanel
    {
        private const int windowWidth = 400;
        private const int headingHeight = 40;
        private const int labelPanelHeight = 20;
        private readonly Vector2 closeButtonSize = new Vector2(32, 32);
        new private readonly RectOffset padding = new RectOffset(2, 2, 2, 2);

        public TransfersStatisticsPanel()
        {
            //
        }

        public override void Awake()
        {
            base.Awake();

            backgroundSprite = "MenuPanel2";
            opacity = 0.9f;

            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;

            var handle = AddUIComponent<UIDragHandle>();
            handle.size = new Vector2(windowWidth, headingHeight);

            var windowLabel = handle.AddUIComponent<UILabel>();
            windowLabel.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.CenterHorizontal;
            windowLabel.text = "Transfers Statistics";

            var closeButton = handle.AddUIComponent<UIButton>();
            closeButton.size = closeButtonSize;
            closeButton.relativePosition = new Vector3(windowWidth - closeButtonSize.x, 0, 0);
            closeButton.anchor = UIAnchorStyle.Top | UIAnchorStyle.Right;
            closeButton.normalBgSprite = "buttonclose";
            closeButton.pressedBgSprite = "buttonclosepressed";
            closeButton.hoveredBgSprite = "buttonclosehover";
            closeButton.eventClicked += (sender, e) => Hide();

            var labelPanel = AddUIComponent<UIPanel>();
            labelPanel.size = new Vector2(windowWidth, labelPanelHeight);
            labelPanel.autoLayout = true;
            labelPanel.autoLayoutDirection = LayoutDirection.Vertical;
            labelPanel.autoLayoutStart = LayoutStart.TopRight;
            labelPanel.autoLayoutPadding = padding;

            #region labelPanel_components
            var label1 = labelPanel.AddUIComponent<UILabel>();
            label1.autoSize = false;
            label1.size = new Vector2(windowWidth, labelPanelHeight);
            label1.textAlignment = UIHorizontalAlignment.Center;
            label1.text = "Label 1";
            #endregion

            FitChildren(new Vector2(padding.top, padding.left));
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
            if (!isVisible) return;
            if (DataShared.Data == null) return;
            //todo: update actions
        }
    }
}
