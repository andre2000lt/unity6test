using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAP_Manager : MonoBehaviour, IDetailedStoreListener
{
    public static string NoAdsPrice;
    public static string NoAdsCurrency;
    
    private const string DISABLE_ADS_ID = "disable_ads";

    private IStoreController _controller;
    //Этот объект отвечает за:
    //✔Запрос списка товаров
    //✔️ Покупку товара(storeController.InitiatePurchase(productID))
    //✔️ Проверку наличия покупок

    private IExtensionProvider _extensions;
    //Этот объект позволяет:
    //✔️ Восстанавливать покупки на iOS(storeExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions())
    //✔️ Делать запросы к Google Play или App Store
    //✔️ Работать с подписками



    private void Start()
    {
        InitIAP();
    }


    private void InitIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(DISABLE_ADS_ID, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
        // Initialize Unity IAP...
    }


    public void purchaseAdDisabling()
    {
        _controller.InitiatePurchase(DISABLE_ADS_ID);
    }


    //Called when Unity IAP is ready to make purchases.
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _controller = controller;
        _extensions = extensions;

        NoAdsPrice = controller.products.WithID(DISABLE_ADS_ID).metadata.localizedPriceString;
        NoAdsCurrency = controller.products.WithID(DISABLE_ADS_ID).metadata.isoCurrencyCode;
        Debug.Log(NoAdsPrice + NoAdsCurrency);
    }


    //Called when a purchase completes.
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {   
        //Retrieve the purchased product
        var product = purchaseEvent.purchasedProduct;

        if (product.definition.id == DISABLE_ADS_ID)
        {
            InAppData.DisableAds();
        }

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }



    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        
    }


}

