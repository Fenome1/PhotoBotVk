﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBotVk
{
    public sealed class User
    {
        public string UserName { get; set; }
        public string UserMessage { get; set; }
        public string UserLick { get; set; }
        public long? UserId { get; set; }
        public long? PeerId { get; set; }
        public string EventId { get; set; }
        public int PhotoCount { get; set; }
        public int UserLocation { get; set; } = 0;

    }
}