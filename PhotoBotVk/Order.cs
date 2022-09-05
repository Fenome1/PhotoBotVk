using System.Collections.ObjectModel;
using System.Net.Mail;
using VkNet.Model.Attachments;

namespace PhotoBotVk
{
    internal sealed class Order
    {
        public int OrderId { get; set; }
        public string OrderMessage { get; set; }
        public ReadOnlyCollection<MediaAttachment> OrderProductPhoto { get; set; }
        public string UserLink { get; set; }
        public long? UserId { get; set; }
        public long? PeerId { get; set; }
        public int Price { get; set; }

        public void Clear()
        {
            OrderId = 0;
            OrderMessage = string.Empty;
            OrderProductPhoto = null;
            UserId = 0;
            PeerId = 0;
            UserLink = string.Empty;
        }
    }
}