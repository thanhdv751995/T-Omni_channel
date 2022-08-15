using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Package
{
    public class ResponseSearchPackageDto
    {
        public bool more { get; set; }
        public string next_cursor { get; set; }
        public List<PackageDtto> package_list { get; set; }
        public int total { get; set; }
    }
}
