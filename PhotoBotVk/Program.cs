using System;

namespace PhotoBotVk
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var photoBot = new BotManager();
                photoBot.StartMessageHandling();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}