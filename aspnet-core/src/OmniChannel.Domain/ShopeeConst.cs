using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel
{
    public static class ShopeeConst
    {
        #region COMMON LINK

        public const string DOMAIN_URL = "https://partner.shopeemobile.com";
        //public const string DOMAIN_URL = "https://partner.test-stable.shopeemobile.com";
        public const string CALLBACK_REDIRECT_URL = "http://localhost:4200/callback-url";

        #endregion

        #region VARIABLE DEFINE

        public const string LANGUAGE = "vi";
        public const string IMAGE_URL_REGION = "VN";
        public const string LOGISTICS_CHANNEL_NAME = "Nhanh";
        public const string ACTIVE_ITEM_STATUS = "NORMAL";
        //public const string shop_id = "59444";

        #endregion

        #region API LINK

        public const string API_AUTH_PARTNER = "/api/v2/shop/auth_partner";
        public const string API_GET_CATEGORY = "/api/v2/product/get_category";
        public const string API_GET_TOKEN = "/api/v2/auth/token/get";
        public const string API_GET_SHOP_INFO = "/api/v2/shop/get_shop_info";
        public const string API_GET_SHOP_PROFILE = "/api/v2/shop/get_profile";
        public const string API_GET_WAREOUSE_DETAIL = "/api/v2/shop/get_warehouse_detail";
        public const string API_GET_ATTRIBUTES = "/api/v2/product/get_attributes";
        public const string API_PRODUCT_LIST = "/api/v2/product/get_item_list";
        public const string API_REFRESH_ACCESS_TOKEN = "/api/v2/auth/access_token/get";
        public const string API_GET_PRODUCT_LIST = "/api/v2/product/get_item_list";
        public const string API_GET_PRODUCT_BASE_INFO = "/api/v2/product/get_item_base_info";
        public const string API_UPLOAD_IMAGE = "/api/v2/media_space/upload_image";
        public const string API_GET_BRAND_LIST = "/api/v2/product/get_brand_list";
        public const string API_ADD_ITEM = "/api/v2/product/add_item";
        public const string API_GET_CHANNEL_LIST = "/api/v2/logistics/get_channel_list";
        public const string API_ADD_MODEL = "/api/v2/product/add_model";
        public const string API_UPDATE_MODEL = "/api/v2/product/update_model";
        public const string API_DELETE_MODEL = "/api/v2/product/delete_model";
        public const string API_GET_MODEL_LIST = "/api/v2/product/get_model_list";
        public const string API_INIT_TIER_VARIATION = "/api/v2/product/init_tier_variation";
        public const string API_UPDATE_TIER_VARIATION = "/api/v2/product/update_tier_variation";
        public const string API_DELETE_ITEM = "/api/v2/product/delete_item";
        public const string API_GET_COMMENT = "/api/v2/product/get_comment";
        public const string API_UPDATE_ITEM = "/api/v2/product/update_item";
        public const string API_UPDATE_PRICE = "/api/v2/product/update_price";
        public const string API_UNLIST_ITEM = "/api/v2/product/unlist_item";

        #endregion
    }
}
