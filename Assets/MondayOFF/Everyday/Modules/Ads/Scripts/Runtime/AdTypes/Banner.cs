using UnityEngine;

namespace MondayOFF {
    internal sealed class Banner : AdTypeBase {
        public override void Dispose() {
            Debug.Log("[EVERYDAY] Disposing Banner Ad");
            Hide();
            MaxSdk.DestroyBanner(_adUnitID);
        }

        internal override bool IsReady() {
            // There is no preload for banner
            return true;
        }

        internal override bool Show() {
            Debug.Log("[EVERYDAY] Show Banner");
            //show banner
            MaxSdk.ShowBanner(_adUnitID);

            return true;
        }

        internal void Hide() {
            Debug.Log("[EVERYDAY] Hide Banner");
            MaxSdk.HideBanner(_adUnitID);
        }

        internal Banner(AdSettings setting, bool showBannerOnLoad) {
            Debug.Log("[EVERYDAY] Createing Banner Ad");

            _adUnitID = setting.bannerAdUnitId;

            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
            MaxSdk.CreateBanner(adUnitIdentifier: _adUnitID, setting.bannerPosition);

            if (showBannerOnLoad) {
                this.Show();
            }
        }
    }
}