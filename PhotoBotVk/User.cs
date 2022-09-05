namespace PhotoBotVk
{
    public sealed class User
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public long? UserId { get; set; }
        public long? PeerId { get; set; }
        public long? TempPeerId { get; set; }
        public string EventId { get; set; }
        public int PhotoCount { get; set; }
        public int Location { get; set; }
        public int Price { get; set; }
    }
}