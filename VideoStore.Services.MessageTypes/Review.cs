// Added file
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStore.Services.MessageTypes
{
    public class Review : MessageType
    {
        public int MediaId { get; set; }
        public DateTime ReviewDate { get; set; }
        public String Reviewer { get; set; }
        public String ReviewLocation { get; set; }
        public String ReviewTitle { get; set; }
        public int Rating { get; set; }
        public String ReviewContent { get; set; }

    }
}
