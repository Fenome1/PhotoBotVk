﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBotVk
{
    internal sealed class Order
    {
        public int OrderId { get; set; }
        public string OrderMessage { get; set; }
        public string OrderProductPhoto { get; set; }
        public string UserLick { get; set; }
        public long? UserId { get; set; }
        public long? PeerId { get; set; }
    }
}