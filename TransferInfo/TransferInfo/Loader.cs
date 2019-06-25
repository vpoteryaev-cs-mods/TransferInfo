using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;

namespace TransferInfo
{
    public class Loader: LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
        }

        public override void OnReleased()
        {
            base.OnReleased();
        }
    }
}
