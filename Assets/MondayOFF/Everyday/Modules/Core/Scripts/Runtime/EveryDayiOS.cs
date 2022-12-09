#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine;

namespace MondayOFF {
    public static partial class EveryDay {
        private static void PrepareSettings(MaxSdkBase.SdkConfiguration sdkConfiguration) {
            var consentStatus = sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized;

            // MAX
            MaxSdk.SetHasUserConsent(consentStatus);
            MaxSdk.SetDoNotSell(!consentStatus);

            // FB
            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(consentStatus);
            if (consentStatus) {
                AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });
            } else {
                AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { "LDU" }, 0, 0);
            }

            // Privacy string
            char[] privacyCharacters = new char[4];
            privacyCharacters[0] = '1';
            if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies) {
                privacyCharacters[1] = 'Y';
                if (consentStatus) {
                    privacyCharacters[2] = 'N';
                } else {
                    privacyCharacters[2] = 'Y';
                }
            } else {
                privacyCharacters[1] = '-';
                privacyCharacters[2] = '-';
            }
            privacyCharacters[3] = '-';

            AdsManager.US_PRIVACY_STRING = new string(privacyCharacters);
            AdsManager.HAS_USER_CONSENT = consentStatus;
        }
    }
}
#endif
