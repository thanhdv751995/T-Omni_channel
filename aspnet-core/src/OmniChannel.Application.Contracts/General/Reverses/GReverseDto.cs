using OmniChannel.Channels;

namespace OmniChannel.General.Reverses
{
    public class GReverseDto
    {
        /// <summary>
        /// Kênh bán hàng
        /// </summary>
        public EChannel E_channel { get; set; }
        /// <summary>
        /// Mã cửa hàng
        /// </summary>
        public string Shop_id { get; set; }
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string Order_id { get; set; }
        /// <summary>
        /// Tổng tiền hoàn lại
        /// </summary>
        public decimal Refund_total { get; set; }
        /// <summary>
        /// Lý do hoàn tiền
        /// </summary>
        public string Return_reason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Return_tracking_id { get; set; }
        /// <summary>
        /// Mã đơn hàng trả
        /// </summary>
        public string Reverse_order_id { get; set; }
        /// <summary>
        /// Thời gian gừi yêu cầu trả hàng
        /// </summary>
        public long Reverse_request_time { get; set; }
        /// <summary>
        /// Trạng thái trả hàng
        /// </summary>
        public int Reverse_status_value { get; set; }
        /// <summary>
        /// Loại trả hàng
        /// </summary>
        public int Reverse_type { get; set; }
        /// <summary>
        /// Cập nhật thời gian trả hàng
        /// </summary>
        public long Reverse_update_time { get; set; }
    }
}
