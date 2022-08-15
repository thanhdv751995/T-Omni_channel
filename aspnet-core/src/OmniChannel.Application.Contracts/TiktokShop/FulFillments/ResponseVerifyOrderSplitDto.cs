using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments
{
    public class ResponseVerifyOrderSplitDto
    {
        public List<FailListDto> fail_list { get; set; }
        public List<ResultListDto> result_list { get; set; }
    }
}
