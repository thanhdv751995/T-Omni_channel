using OmniChannel.Channels;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OmniChannel.General.Orders
{
    /// <summary>
    /// Orders DTO
    /// </summary>
    public class GOrdersDto
    {
        /// <summary>
        /// Mã cửa hàng
        /// </summary>
        [Required]
        public string Shop_id { get; set; }

        /// <summary>
        /// Kênh bán hàng
        /// </summary>
        [Required]
        public EChannel E_channel { get; set; }

        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        [DefaultValue("")]
        public string Order_id { get; set; }

        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public int Order_status { get; set; }

        /// <summary>
        /// Thời gian ...
        /// </summary>
        public long Update_time { get; set; }

        /// <summary>
        /// Mã người mua hàng
        /// </summary>
        [DefaultValue("")]
        public string Buyer_uid { get; set; }

        /// <summary>
        /// Ngày bán
        /// </summary>
        public long Sale_date { get; set; }

        /// <summary>
        /// Thông tin thanh toán
        /// </summary>
        public PyamentInfo Payment { get; set; }

        /// <summary>
        /// Tổng tiền của đơn hàng(Chưa chiết khấu)
        /// </summary>
        public decimal Total_amount { get; set; }

        /// <summary>
        /// Chiết khấu
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Tổng tiền của đơn hàng(Sau chiết khấu)
        /// </summary>
        public decimal Sub_total { get; set; }

        /// <summary>
        /// Thông tin chi tiết đơn hàng
        /// </summary>
        public GOrderDetailDto Order_detail { get; set; }
    }

    public class PyamentInfo
    {
        /// <summary>
        /// Tên trên thẻ ngân hàng
        /// </summary>
        public string Card_name { get; set; }

        /// <summary>
        /// Ngày thanh toán
        /// </summary>
        public DateTime Payment_date { get; set; }

        /// <summary>
        /// Phương thức thanh toán
        /// </summary>
        public int Payment_method { get; set; }

        /// <summary>
        /// Tổng chi phí thanh toán
        /// </summary>
        public decimal Total_amount { get; set; }
    }

    public class SearchOrderDto : SearchOrderByShopIdDto
    {
        public int Order_status { get; set; }
    }

    public class SearchOrderByShopIdDto
    {
        [Required]
        [DefaultValue("0")]
        public EChannel EChannel { get; set; }

        [Required]
        [DefaultValue("7494636233052817886")]
        public string Shop_id { get; set; }
    }

    public class SearchOrderByIdDto : SearchOrderByShopIdDto
    {
        [Required]
        public string Order_id { get; set; }
    }

    public class SearchOrderByDateDto : SearchOrderDto
    {
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
    }
}
