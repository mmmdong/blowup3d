namespace MondayOFF {
    internal abstract class AdTypeBase : System.IDisposable {
        private protected string _adUnitID = null;
        internal abstract bool IsReady();
        internal abstract bool Show();
        public abstract void Dispose();
    }

    internal abstract class FullscreenAdType : AdTypeBase {
        private protected int _retryAttempt = 0;
        internal static event System.Action OnBeforeShow = default;
        internal static event System.Action OnAfterShow = default;
        protected void CallOnBeforeShow() { OnBeforeShow?.Invoke(); }
        protected void CallOnAfterShow() { OnAfterShow?.Invoke(); }
    }
}