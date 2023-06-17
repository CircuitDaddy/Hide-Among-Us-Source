using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;
using System.Collections;
public class IAPPlugin : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static IAPPlugin Instance;
    public bool InappsClick = false;
    //public List<Keys> keys = new List<Keys>();

    public MyEvents OnSuccessfulPurchase;

    //public MyEvents OnRestosePurchase;


    private void Awake()
    {
        if (Instance == null)
        {

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {

        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
        else
        {
            //Debug.Log("***** not Initializing******");
        }

    }
    // private const string kNoProduct = "<None>";

    // private List<string> m_ValidIDs = new List<string>();
    public void InitializePurchasing()
    {
        //Debug.Log("*****Initializing******");
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


        var catalog = ProductCatalog.LoadDefaultCatalog();

        // m_ValidIDs.Clear();
        // m_ValidIDs.Add(kNoProduct);

        foreach (var product in catalog.allProducts)
        {
          //  keys.Add(new Keys(key,))
            // m_ValidIDs.Add(product.id);
            builder.AddProduct(product.id, product.type);
        }



        //for (int i = 0; i < keys.Count; i++)
        //{
         
        //    builder.AddProduct(keys[i].key, keys[i].Type);
        //}

        UnityPurchasing.Initialize(this, builder);
    }

   
    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string ProductKey)
    {
        if (InappsClick == false)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                string ProductId = PlayerPrefs.GetString(ProductKey);
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Debug.Log(string.Format("Product ID :", ProductId));
                Product product = m_StoreController.products.WithID(ProductId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }

            inappsClick_Checkfunction();

        }

    }
    /// <summary>
    /// Don't use Restores the purchases.
    /// </summary>
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            //var samsung = m_StoreExtensionProvider.GetExtension<ISamsungAppsExtensions>();


            //samsung.RestoreTransactions(result =>
            //{
            //    if (result)
            //    {
            //        Debug.Log("Iap restored");
            //    }
            //    else
            //    {
            //        Debug.Log("restore failed");
            //    }
            //});
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    #region OtherFunctions

    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        //Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));


        OnSuccessfulPurchase.Invoke(args);

        return PurchaseProcessingResult.Complete;

    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    #endregion


    public void inappsClick_Checkfunction()
    {
        InappsClick = true;
        StartCoroutine(InappBool(1.5f));
    }


    public IEnumerator InappBool(float a)
    {
        yield return new WaitForSecondsRealtime(a);
        InappsClick = false;
    }

    
}

[System.Serializable]
public class MyEvents:UnityEvent<PurchaseEventArgs>
{
    public UnityEvent MyEvent;
}