using System.Collections.Generic;

namespace OmniChannel.General.PushNotifications
{
    public class BaseModel
    {
        /// <summary>
        /// Loại thông báo
        /// </summary>
        public int Push_type { get; set; }

        /// <summary>
        /// Mã cửa hàng
        /// </summary>
        public string Shop_id { get; set; }

        /// <summary>
        /// Thời gian
        /// </summary>
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TikTokNotificatio1nModel : BaseModel
    {
        /// <summary>
        /// Data
        /// </summary>
        public DataType1 Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TikTokNotificatio2nModel : BaseModel
    {
        /// <summary>
        /// Data
        /// </summary>
        public DataType2 Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TikTokNotificatio3nModel : BaseModel
    {
        /// <summary>
        /// Data
        /// </summary>
        public DataType3 Data { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class TikTokNotificatio4nModel : BaseModel
    {
        /// <summary>
        /// Data
        /// </summary>
        public DataType4 Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TikTokNotificatio5nModel : BaseModel
    {
        /// <summary>
        /// Data
        /// </summary>
        public DataType5 Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataType1
    {
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string Order_id { get; set; }

        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public string Order_status { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public long Update_time { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataType2
    {
        /// <summary>
        /// mã đơn hàng
        /// </summary>
        public string Order_id { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public long Update_time { get; set; }

        public string? Reverse_event_type { get; set; }

        public string? Reverse_order_id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataType3
    {
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string Order_id { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public long Update_time { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataType4
    {
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Sc_type { get; set; }

        /// <summary>
        /// Quyền hạn
        /// </summary>
        public string Role_type { get; set; }

        /// <summary>
        /// Thông tin của gói hàng
        /// </summary>
        public PackageInfo Package_list { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public long Update_time { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataType5
    {
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string Product_id { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Lý do từ chối
        /// </summary>
        public string Suspended_reason { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public long Update_time { get; set; }
    }

    public class PackageInfo
    {
        /// <summary>
        /// Mã gói hàng
        /// </summary>
        public string Package_id { get; set; }

        /// <summary>
        /// Danh sách mã đơn hàng
        /// </summary>
        public List<string> Order_id_list { get; set; }
    }
}
