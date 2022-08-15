namespace OmniChannel.Orders
{
    /// <summary>
    /// Get Order List
    /// </summary>
    public class RequestSearchListOrderDto
    {
        /// <summary>
        /// Use this field to specify the maximum number of orders to obtain in a single page. Must be 1-50
        /// </summary>
        public int Page_size { get; set; } = 20;

        /// <summary>
        /// deprecated plz use create_time_from
        /// </summary>
        public int? Start_time { get; set; }

        /// <summary>
        /// deprecated plz use create_time_to
        /// </summary>
        public int? End_time { get; set; }

        /// <summary>
        /// Use this field to obtain orders in a specific status
        /// - UNPAID = 100;
        /// - AWAITING_SHIPMENT = 111; 
        /// - AWAITING_COLLECTION = 112;
        /// - PARTIALLY_SHIPPING = 114;
        /// - IN_TRANSIT = 121; 
        /// - DELIVERED = 122;
        /// - COMPLETED = 130;
        /// - CANCELLED = 140;
        /// </summary>
        public int? Order_status { get; set; }

        /// <summary>
        /// CREATE_TIME
        /// </summary>
        public string Sort_by { get; set; }

        /// <summary>
        /// This field value would be returned in response data and you can use this to 
        /// search the data on the next page. You do not need it at first search
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// Unix timestamp. Order creation time
        /// </summary>
        public int? Create_time_from { get; set; }

        /// <summary>
        ///	Unix timestamp.Order creation time
        /// </summary>
        public int? Create_time_to { get; set; }

        /// <summary>
        /// Unix timestamp. Order creation time
        /// </summary>
        public int? Update_time_from { get; set; }

        /// <summary>
        /// Unix timestamp. Order creation time
        /// </summary>
        public int? Update_time_to { get; set; }

        /// <summary>
        /// Available value: ASCE = 1;DESC = 2; (default)
        /// </summary>
        public int? Sort_type { get; set; }
    }
}
