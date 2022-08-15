using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Package
{
    public class PackageDtto
    {
        public long create_time { get; set; }
        public string package_id { get; set; }
        public int package_status { get; set; }
        public long update_time { get; set; }
    }
}
