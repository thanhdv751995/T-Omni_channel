namespace OmniChannel.Reverses
{
    /// <summary>
    /// Từ chối trà hàng
    /// </summary>
    public class RequestRejectReverseRequestDto
    {
        /// <summary>
        /// M34 đơn hàng trả
        /// </summary>
        public string reverse_order_id { get; set; }
        /// <summary>
        /// Nội dung ghi chú
        /// </summary>
        public string reverse_reject_comments { get; set; }
        /// <summary>
        /// Lý do trả hàng
        /// </summary>
        public string reverse_reject_reason_key { get; set; }
    }
}
