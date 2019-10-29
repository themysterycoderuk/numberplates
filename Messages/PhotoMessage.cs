using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class PhotoMessage : IMessage
    {
        public string Url { get; set; }
    }
}
