using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Package
{
    public class SearchPackageDto
    {
        public long create_time_from { get; set; }
        public long create_time_to { get; set; }
        public long update_time_from { get; set; }
        public long update_time_to { get; set; }
        public int package_status { get; set; }
        public string cursor { get; set; }
        public int sort_by { get; set; }
        public int sort_type { get; set; }
        public int page_size { get; set; }
    }
}
