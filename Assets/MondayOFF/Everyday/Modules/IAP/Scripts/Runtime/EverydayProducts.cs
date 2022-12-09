using UnityEngine;

#if UNITY_PURCHASING
namespace MondayOFF {
    // [CreateAssetMenu(menuName = "products", fileName = "Products", order = 10)]
    internal class EverydayProducts : ScriptableObject {
        [Header("Please enter Product IDs here")] public ProductData[] products = default;
    }

    [System.Serializable]
    internal class ProductData {
        public string productID;
        public UnityEngine.Purchasing.ProductType productType;
        public System.Action onPurchase { private get; set; }
        public int purchaseCount { get; set; }
        public bool isRegistered => onPurchase != null && onPurchase.GetInvocationList().Length > 0;

        public void CompletePurchase() {
            if (isRegistered) {
                onPurchase.Invoke();
            } else {
                if (productType == UnityEngine.Purchasing.ProductType.Consumable) {
                    ++purchaseCount;
                } else {
                    purchaseCount = 1;
                }
            }
        }
    }
}

#else
namespace MondayOFF {
    internal class EverydayProducts : ScriptableObject { }
}
#endif