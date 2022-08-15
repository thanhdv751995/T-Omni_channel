using System.Collections.Generic;
using System.ComponentModel;

namespace OmniChannel.General.Orders
{
    /// <summary>
    /// Order detail DTO
    /// </summary>
    public class GOrderDetailDto
    {
        /// <summary>
        /// Chú thích của người mua hàng
        /// </summary>
        [DefaultValue("")]
        public string Buyer_message { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public long cancel_order_sla { get; set; }
        /// <summary>
        /// Lý do hủy đơn hàng
        /// </summary>
        [DefaultValue("")]
        public string Cancel_reason { get; set; }
        /// <summary>
        /// Người hủy đơn
        /// </summary>
        [DefaultValue("")]
        public string Cancel_user { get; set; }
        /// <summary>
        /// Thời gian tạo đơn hàng
        /// </summary>
        [DefaultValue("")]
        public long Create_time { get; set; }
        /// <summary>
        /// Thông tin vận chuyển
        /// </summary>
        //[DefaultValue("")]
        //public string delivery_option { get; set; }
        /// <summary>
        /// 
        /// </summary>
       // public int ext_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public int fulfillment_type { get; set; }
        /// <summary>
        /// Danh sách sản phẩm của đơn hàng
        /// </summary>
        public List<ItemOrderDetailDto> Item_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OrderLineOrderDetailDto> Order_line_list { get; set; }
        ///// <summary>
        ///// Trạng thái đơn hàng
        ///// Available value: 
        ///// UNPAID = 100;
        ///// AWAITING_SHIPMENT = 111; 
        ///// AWAITING_COLLECTION = 112;
        ///// IN_TRANSIT = 121; 
        ///// DELIVERED = 122;
        ///// COMPLETED = 130;
        ///// CANCELLED = 140;
        ///// </summary>
        //public int order_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PakageOrderDetailDto> Package_list { get; set; }
        /// <summary>
        /// Ngày thanh toán
        /// </summary>
        public long Paid_time { get; set; }
        /// <summary>
        /// Thông tin thanh toán
        /// </summary>
        public PaymentInfoOrderDetailDto Payment_info { get; set; }
        /// <summary>
        /// Phương thức thanh toán
        /// </summary>
        [DefaultValue("")]
        public string Payment_method { get; set; }
        /// <summary>
        /// Người nhận cập nhật địa chỉ nhận
        /// </summary>
        public int receiver_address_updated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RecipientAddressOrderDetailDto Recipient_address { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public long rts_sla { get; set; }
        /// <summary>
        /// Tên nhà cung cấp dịch vụ vận chuyển
        /// </summary>
        [DefaultValue("")]
        public string Shipping_provider { get; set; }
        /// <summary>
        /// Mã nhà cung cấp dịch vụ vận chuyển
        /// </summary>
        [DefaultValue("")]
        public string Shipping_provider_id { get; set; }
        /// <summary>
        /// Mã vận đơn
        /// </summary>
        [DefaultValue("")]
        public string Tracking_number { get; set; }
    }

    /// <summary>
    /// Danh sách sản phẩm của đơn hàng
    /// </summary>
    public class ItemOrderDetailDto
    {
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        [DefaultValue("")]
        public string Product_id { get; set; }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        [DefaultValue("")]
        public string Product_name { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //[DefaultValue("")]
        //public string seller_sku { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //[DefaultValue("")]
        //public string sku_cancel_reason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //[DefaultValue("")]
        //public string sku_cancel_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public int sku_display_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public int sku_ext_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public string Sku_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //[DefaultValue("")]
        //public string sku_image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //[DefaultValue("")]
        //public string sku_name { get; set; }
        /// <summary>
        /// Dơn giá gốc
        /// </summary>
        public int Sku_original_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public int sku_platform_discount { get; set; }
        /// <summary>
        /// Đơn giảm giá
        /// </summary>
        public int Sku_sale_price { get; set; }
        /// <summary>
        /// Giảm giá từ nhà bán hàng
        /// </summary>
        public int Sku_seller_discount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public int sku_type { get; set; }
        public double weight { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OrderLineOrderDetailDto
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public string Order_line_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public string Sku_id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PakageOrderDetailDto
    {
        /// <summary>
        /// Mã gói hàng
        /// </summary>
        [DefaultValue("")]
        public string package_id { get; set; }
    }

    /// <summary>
    /// Thông tin thanh toán đơn hàng
    /// </summary>
    public class PaymentInfoOrderDetailDto
    {
        /// <summary>
        ///
        /// </summary>
        //[DefaultValue("")]
        //public string currency { get; set; }
        /// <summary>
        /// Phí giao hàng gốc
        /// </summary>
        public int Original_shipping_fee { get; set; }
        /// <summary>
        /// Tổng giá gốc sản phẩm
        /// </summary>
        public int Original_total_product_price { get; set; }
        /// <summary>
        /// Giảm giá từ nền tảng
        /// </summary>
       // public int platform_discount { get; set; }
        /// <summary>
        /// Giảm giá từ nhà bán hàng
        /// </summary>
        //public int seller_discount { get; set; }
        /// <summary>
        /// Phí vận chuyển
        /// </summary>
        public int Shipping_fee { get; set; }
        /// <summary>
        /// Giảm giá phí vận chuyển từ nền tảng
        /// </summary>
        public int Shipping_fee_platform_discount { get; set; }
        /// <summary>
        /// Giảm giá phí vận chuyển từ nhà bán hàng
        /// </summary>
        public int Shipping_fee_seller_discount { get; set; }
        /// <summary>
        /// Tổng tiền
        /// </summary>
        public int Sub_total { get; set; }
        /// <summary>
        /// Thuế
        /// </summary>
        public int Taxes { get; set; }
        /// <summary>
        /// Tổng chi phí
        /// </summary>
        public int Total_amount { get; set; }
    }

    /// <summary>
    /// Thông tin hóa đơn
    /// </summary>
    public class RecipientAddressOrderDetailDto
    {
        /// <summary>
        /// Địa chỉ chi tiết
        /// </summary>
        //[DefaultValue("")]
        //public string address_detail { get; set; }
        /// <summary>
        /// Danh sách địa chỉ
        /// </summary>
        //public List<string> address_line_list { get; set; }
        /// <summary>
        /// Thành phố
        /// </summary>
        //[DefaultValue("")]
       // public string city { get; set; }
        /// <summary>
        /// Quận
        /// </summary>
        //[DefaultValue("")]
        //public string district { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        [DefaultValue("")]
        public string Full_address { get; set; }
        /// <summary>
        /// Tên người nhận
        /// </summary>
        [DefaultValue("")]
        public string Name { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        [DefaultValue("")]
        public string Phone { get; set; }
        /// <summary>
        /// Tên vùng(Quốc gia)
        /// </summary>
        //[DefaultValue("")]
        //public string region { get; set; }
        /// <summary>
        /// Mã vùng
        /// </summary>
        //[DefaultValue("")]
        //public string region_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //[DefaultValue("")]
        //public string state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //[DefaultValue("")]
        //public string town { get; set; }
        /// <summary>
        /// ZipCode
        /// </summary>
        //[DefaultValue("")]
        //public string zipcode { get; set; }
    }
}
