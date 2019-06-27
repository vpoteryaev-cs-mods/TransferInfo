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
        private const int cargoWidth = 300;
        private const int itemWidth = 75;
        private const int windowWidth = itemWidth * 6 + cargoWidth;
        private const int headingHeight = 40;
        private const int labelHeight = 20;
        private readonly Vector2 closeButtonSize = new Vector2(32, 32);

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
            handle.name = "handle";
            handle.size = new Vector2(windowWidth, headingHeight);

            var windowLabel = handle.AddUIComponent<UILabel>();
            windowLabel.name = "windowLabel";
            windowLabel.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.CenterHorizontal;
            windowLabel.text = "Transfers Statistics";

            var closeButton = handle.AddUIComponent<UIButton>();
            closeButton.name = "closeButton";
            closeButton.size = closeButtonSize;
            closeButton.relativePosition = new Vector3(windowWidth - closeButtonSize.x, 0, 0);
            closeButton.anchor = UIAnchorStyle.Top | UIAnchorStyle.Right;
            closeButton.normalBgSprite = "buttonclose";
            closeButton.pressedBgSprite = "buttonclosepressed";
            closeButton.hoveredBgSprite = "buttonclosehover";
            closeButton.eventClicked += (sender, e) => Hide();

            var captionsPanel = AddUIComponent<UIPanel>();
            captionsPanel.name = "captionsPanel";
            captionsPanel.size = new Vector2(windowWidth, labelHeight * 2);
            captionsPanel.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left;
            captionsPanel.autoLayout = true;
            captionsPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            captionsPanel.autoLayoutStart = LayoutStart.TopLeft;
            {
                var captionsCargoPanel = captionsPanel.AddUIComponent<UIPanel>();
                captionsCargoPanel.name = "captionsCargoPanel";
                captionsCargoPanel.size = new Vector2(cargoWidth, labelHeight * 2);
                {
                    var cargoCaptionLabel = captionsCargoPanel.AddUIComponent<UILabel>();
                    cargoCaptionLabel.name = "cargoCaptionLabel";
                    cargoCaptionLabel.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.CenterHorizontal;
                    cargoCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                    cargoCaptionLabel.text = "Cargo";
                }
                var captionsLocalPanel = captionsPanel.AddUIComponent<UIPanel>();
                captionsLocalPanel.name = "captionsLocalPanel";
                captionsLocalPanel.size = new Vector2(itemWidth * 2, labelHeight * 2);
                captionsLocalPanel.autoLayout = true;
                captionsLocalPanel.autoLayoutDirection = LayoutDirection.Vertical;
                captionsLocalPanel.autoLayoutStart = LayoutStart.TopLeft;
                {
                    var localCaptionPanel = captionsLocalPanel.AddUIComponent<UIPanel>();
                    localCaptionPanel.name = "localCaptionPanel";
                    localCaptionPanel.size = new Vector2(itemWidth * 2, labelHeight);
                    {
                        var localCaptionLabel = localCaptionPanel.AddUIComponent<UILabel>();
                        localCaptionLabel.name = "localCaptionLabel";
                        localCaptionLabel.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.CenterHorizontal;
                        localCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        localCaptionLabel.text = "Local";
                    }
                    var localInOutCaptionPanel = captionsLocalPanel.AddUIComponent<UIPanel>();
                    localInOutCaptionPanel.name = "localInOutCaptionPanel";
                    localInOutCaptionPanel.size = new Vector2(itemWidth * 2, labelHeight);
                    localInOutCaptionPanel.autoLayout = true;
                    localInOutCaptionPanel.autoLayoutDirection = LayoutDirection.Horizontal;
                    localInOutCaptionPanel.autoLayoutStart = LayoutStart.TopLeft;
                    {
                        var localInCaptionLabel = localInOutCaptionPanel.AddUIComponent<UILabel>();
                        localInCaptionLabel.name = "localInCaptionLabel";
                        localInCaptionLabel.autoSize = false;
                        localInCaptionLabel.size = new Vector2(itemWidth, labelHeight);
                        localInCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        localInCaptionLabel.text = "In";

                        var localOutCaptionLabel = localInOutCaptionPanel.AddUIComponent<UILabel>();
                        localOutCaptionLabel.name = "localOutCaptionLabel";
                        localOutCaptionLabel.autoSize = false;
                        localOutCaptionLabel.size = new Vector2(itemWidth, labelHeight);
                        localOutCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        localOutCaptionLabel.text = "Out";
                    }
                }
                var captionsImportPanel = captionsPanel.AddUIComponent<UIPanel>();
                captionsImportPanel.name = "captionsImportPanel";
                captionsImportPanel.size = new Vector2(itemWidth * 2, labelHeight * 2);
                captionsImportPanel.autoLayout = true;
                captionsImportPanel.autoLayoutDirection = LayoutDirection.Vertical;
                captionsImportPanel.autoLayoutStart = LayoutStart.TopLeft;
                {
                    var importCaptionPanel = captionsImportPanel.AddUIComponent<UIPanel>();
                    importCaptionPanel.name = "importCaptionPanel";
                    importCaptionPanel.size = new Vector2(itemWidth * 2, labelHeight);
                    {
                        var importCaptionLabel = importCaptionPanel.AddUIComponent<UILabel>();
                        importCaptionLabel.name = "importCaptionLabel";
                        importCaptionLabel.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.CenterHorizontal;
                        importCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        importCaptionLabel.text = "Import";
                    }
                    var importInOutCaptionPanel = captionsImportPanel.AddUIComponent<UIPanel>();
                    importInOutCaptionPanel.name = "importInOutCaptionPanel";
                    importInOutCaptionPanel.size = new Vector2(itemWidth * 2, labelHeight);
                    importInOutCaptionPanel.autoLayout = true;
                    importInOutCaptionPanel.autoLayoutDirection = LayoutDirection.Horizontal;
                    importInOutCaptionPanel.autoLayoutStart = LayoutStart.TopLeft;
                    {
                        var importInCaptionLabel = importInOutCaptionPanel.AddUIComponent<UILabel>();
                        importInCaptionLabel.name = "importInCaptionLabel";
                        importInCaptionLabel.autoSize = false;
                        importInCaptionLabel.size = new Vector2(itemWidth, labelHeight);
                        importInCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        importInCaptionLabel.text = "In";

                        var importOutCaptionLabel = importInOutCaptionPanel.AddUIComponent<UILabel>();
                        importOutCaptionLabel.name = "importOutCaptionLabel";
                        importOutCaptionLabel.autoSize = false;
                        importOutCaptionLabel.size = new Vector2(itemWidth, labelHeight);
                        importOutCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        importOutCaptionLabel.text = "Out";
                    }
                }
                var captionsExportPanel = captionsPanel.AddUIComponent<UIPanel>();
                captionsExportPanel.name = "captionsExportPanel";
                captionsExportPanel.size = new Vector2(itemWidth * 2, labelHeight * 2);
                captionsExportPanel.autoLayout = true;
                captionsExportPanel.autoLayoutDirection = LayoutDirection.Vertical;
                captionsExportPanel.autoLayoutStart = LayoutStart.TopLeft;
                {
                    var exportCaptionPanel = captionsExportPanel.AddUIComponent<UIPanel>();
                    exportCaptionPanel.name = "exportCaptionPanel";
                    exportCaptionPanel.size = new Vector2(itemWidth * 2, labelHeight);
                    {
                        var exportCaptionLabel = exportCaptionPanel.AddUIComponent<UILabel>();
                        exportCaptionLabel.name = "exportCaptionLabel";
                        exportCaptionLabel.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.CenterHorizontal;
                        exportCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        exportCaptionLabel.text = "Export";
                    }
                    var exportInOutCaptionPanel = captionsExportPanel.AddUIComponent<UIPanel>();
                    exportInOutCaptionPanel.name = "exportInOutCaptionPanel";
                    exportInOutCaptionPanel.size = new Vector2(itemWidth * 2, labelHeight);
                    exportInOutCaptionPanel.autoLayout = true;
                    exportInOutCaptionPanel.autoLayoutDirection = LayoutDirection.Horizontal;
                    exportInOutCaptionPanel.autoLayoutStart = LayoutStart.TopLeft;
                    {
                        var exportInCaptionLabel = exportInOutCaptionPanel.AddUIComponent<UILabel>();
                        exportInCaptionLabel.name = "exportInCaptionLabel";
                        exportInCaptionLabel.autoSize = false;
                        exportInCaptionLabel.size = new Vector2(itemWidth, labelHeight);
                        exportInCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        exportInCaptionLabel.text = "In";

                        var exportOutCaptionLabel = exportInOutCaptionPanel.AddUIComponent<UILabel>();
                        exportOutCaptionLabel.name = "exportOutCaptionLabel";
                        exportOutCaptionLabel.autoSize = false;
                        exportOutCaptionLabel.size = new Vector2(itemWidth, labelHeight);
                        exportOutCaptionLabel.textAlignment = UIHorizontalAlignment.Center;
                        exportOutCaptionLabel.text = "Out";
                    }
                }
            }

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
