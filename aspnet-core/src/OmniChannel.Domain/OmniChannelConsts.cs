namespace OmniChannel;

public static class OmniChannelConsts
{
    public const string DbTablePrefix = "App";

    public const string DbSchema = null;

    public const string TiktokShopShopDomain = "https://open-api.tiktokglobalshop.com";
    public const string TiktokShopAuthorizeDomain = "https://auth.tiktok-shops.com";

    public const string TiktokShopAPIGetListProduct = "/api/products/search";
    public const string TiktokShopAPIGetListOrder = "/api/orders/search";
    public const string TiktokShopAPIGetListCategory = "/api/products/categories";
    public const string TiktokShopAPIGetListAttribute = "/api/products/attributes";
    public const string TiktokShopAPIGetCategoryRule = "/api/products/categories/rules";
    public const string TiktokShopAPIGetListWareHouse = "/api/logistics/get_warehouse_list";
    public const string TiktokShopAPIGetProductDetail = "/api/products/details";
    public const string TiktokShopApiGetListBrand = "/api/products/brands";
    public const string TiktokShopApiActiveProduct = "/api/products/activate";
    public const string TiktokShopApiDeactiveProduct = "/api/products/inactivated_products";
    public const string TiktokShopApiRecoverDeletedProduct = "/api/products/recover";
    public const string TiktokShopApiUpdatePrice = "/api/products/prices";
    public const string TiktokShopApiUpdateStock = "/api/products/stocks";
    public const string TiktokShopApiGetAuthorizedShop = "/api/shop/get_authorized_shop";
    public const string TiktokShopApiGetOrderDetail = "/api/orders/detail/query";
    public const string TiktokShopApiGetVerifyOrderSplit = "/api/fulfillment/order_split/verify";
    public const string TiktokShopApiGetSearchPreCombinePkg = "/api/fulfillment/pre_combine_pkg/list";
    public const string TiktokShopUploadImage = "/api/products/upload_imgs";
    public const string TiktokShopCreateProduct = "/api/products";
    public const string TiktokShopRefreshAccessToken = "/api/token/refreshToken";
    public const string TiktokShopGetWarehouseList = "/api/logistics/get_warehouse_list";
    public const string TiktokShopGetShippingInfo = "/api/logistics/ship/get";
    public const string TiktokShopGetShippingProvider = "/api/logistics/shipping_providers";
    public const string TiktokShopGetShippingDocument = "/api/logistics/shipping_document";
    public const string TiktokShopGetSubscribedDeliveryOptions = "/api/logistics/get_subscribed_deliveryoptions";
    public const string TiktokShopUpdateShippingInfo = "/api/logistics/tracking";
    public const string TiktokShopRejectReverseRequest = "/api/reverse/reverse_request/reject";
    public const string TiktokShopGetReverseOrderList = "/api/reverse/reverse_order/list";
    public const string TiktokShopConfirmReverseRequest = "/api/reverse/reverse_request/confirm";
    public const string TiktokShopCancelOrder = "/api/reverse/order/cancel";
    public const string TiktokShopGetRejectReasonList = "/api/reverse/reverse_reason/list";
    public const string TiktokShopGetSettlements = "/api/finance/settlements/search";
    public const string TiktokShopGetTransactions = "/api/finance/transactions/search";
    public const string TiktokShopGetOrderSettlements = "/api/finance/order/settlements";
    public const string TiktokShopConfirmOrderSplit = "/api/fulfillment/order_split/confirm";
    public const string TiktokShopGetPackageShippingDocument = "/api/fulfillment/shipping_document";
    public const string TiktokShopUpdatePackageShippingInfo = "/api/fulfillment/shipping_info/update";
    public const string TiktokShopGetPackageShippingInfo = "/api/fulfillment/shipping_info";
    public const string TiktokShopSearchPackage = "/api/fulfillment/search";
    public const string TiktokShopShipPackage = "/api/fulfillment/rts";
    public const string TiktokShopGetPackagePickupConfig = "/api/fulfillment/package_pickup_config/list";
    public const string TiktokShopRemovePackageOrder = "/api/fulfillment/package/remove";
    public const string TiktokShopConfirmPrecombinePackage = "/api/fulfillment/pre_combine_pkg/confirm";
    public const string TiktokShopGetPackageDetail = "/api/fulfillment/detail";
    public const string TiktokShopUpdatePackageDeliveryStatus = "/api/fulfillment/delivery";
    public const string TiktokShopGetAccessToken = "/api/token/getAccessToken";
    public const string TiktokShopDefaultImageId = "tos-maliva-i-o3syd03w52-us/0ef9a89efdf545259c5e0e218d16d22b";
    public const string TiktokShopDefaultCustomValue = "Mặc định";
    public const string TiktokShopDefaultColorCustomValue = "Custom";
    public const string TiktokShopDefaultSizeCustomValue = "OneSize";
    public const string TiktokShopDefaultColor = "Màu sắc";
    public const string TiktokShopDefaultSize = "Kíchcỡ";
    public const string TiktokShopDefaultPackageWeight = "0.5";
    public const int TiktokShopDefaultAvailableStock = 99;
    public const string HttpResponseSuccessMessage = "Success";

    #region ORDER
    /// <summary>
    /// Trạng thái đơn hàng
    /// </summary>
    public enum ORDER_STATUS
    {
        UNPAID = 100,                       // Chưa thanh toán
        AWAITING_SHIPMENT = 111,            // Chờ vận chuyển
        AWAITING_COLLECTION = 112,          // Chờ lấy hàng
        IN_TRANSIT = 121,                   // Đang giao hàng     
        DELIVERED = 122,                    // Đã giao hàng
        COMPLETED = 130,                    // Đã hoàn thành
        CANCELLED = 140                     // Hủy đơn hàng
    }

    /// <summary>
    /// Trạng thái trả đơn hàng
    /// </summary>
    public enum REVERSE_TYPE
    {
        ORDER_REFUND = 2,
        ORDER_RETURN = 3,
        ORDER_REQUEST_CANCEL = 4
    }

    /// <summary>
    /// Trạng thái gửi thông báo
    /// </summary>
    public enum PUSH_NOTIFICATION_STATUS
    {
        ORDER_STATUS_UPDATE = 1,           // Order status update
        REVERSE_ORDER = 2,                 // Reverse order
        RECEIVER_ADDRESS_UPDATED = 3,       // Receiver address updated
        PACKAGE_UPDATED = 4,               // Package updated     
        PRODUCT_AUDIT_RESULT_UPDATE = 5    // Product audit result update
    }

    /// <summary>
    /// Trạng thái gửi thông báo của đơn hàng
    /// </summary>
    public static class PUSH_NOTIFICATION_ORDER_STATUS
    {
        public const string UNPAID = "UNPAID";                         // UNPAID
        public const string AWAITING_SHIPMENT = "AWAITING_SHIPMENT";   // AWAITING_SHIPMENT
        public const string CANCEL = "CANCEL";                         // CANCEL
    }

    /// <summary>
    /// Phương thức thanh toán
    /// </summary>
    public enum BANK_CARD
    {
        BANK_TRANSFER = 1,                  // Chuyển tiền qua tài khoản
        CASH = 2,                           // Tiền mặt 
        DANA_WALLET = 3,                    //
        BANK_CARD = 4,                      // Thanh toán bằng thẻ ngân hàng
        OVO = 5,                            // 
        CASH_ON_DELIVERY = 6,               // COD(Thu hộ)
        GO_PAY = 7,                         // Go pay
        PAYPAL = 8,                         // Paypal
        APPLEPAY = 9,                       // Apple pay
        SHOPEEPAY = 10,                     // Shopee pay  
        KLARNA = 11,                        //
        KLARNA_PAY_NOW = 12,                // 
        KLARNA_PAY_LATER = 13,              //
        KLARNA_PAY_OVER_TIME = 14,          //
        TRUE_MONEY = 15,                    //
        RABBIT_LINE_PAY = 16,               //
        IBANKING = 17,                      //
        TOUCH_GO = 18,                      //
        BOOST = 19,                         //
        ZALO_PAY = 20,                      // Zalo pay
        MOMO = 21,                          // Momo
        BLIK = 22                           // 
    }
    #endregion
}
