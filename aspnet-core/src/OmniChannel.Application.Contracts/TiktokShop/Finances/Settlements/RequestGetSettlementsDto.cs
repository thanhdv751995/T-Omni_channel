using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OmniChannel.Finances
{
    public class RequestGetSettlementsDto
    {
        public long? request_time_from { get; set; }
        public long? request_time_to { get; set; }
        [Required]
        public int page_size { get; set; }
        public string cursor { get; set; }
        public int? sort_type { get; set; }
    }
}
