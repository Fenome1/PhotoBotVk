using Data;
using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace PhotoBotVk
{
    internal static class Sender
    {
        private static Random random = new Random();
        public static void SendMessage(VkApi vkApi, string message, User user, bool keybordClear = false)
        {
            if (keybordClear)
            {
                var keybord = new KeyboardBuilder().Clear().Build();
                vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = random.Next(),
                    UserId = user.UserId,
                    PeerId = user.PeerId,
                    Message = message,
                    Keyboard = keybord,
                });
            }
            else
            {
                vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = random.Next(),
                    UserId = user.UserId,
                    PeerId = user.PeerId,
                    Message = message,
                });
            }
        }
        public static long? SendOrderKeyboard(VkApi vkApi, string message, User user, TypeKeyboard typeKeyboard = TypeKeyboard.OrderBoard, bool keybordClear = false)
        {
            if (keybordClear)
            {
                var keybord = new KeyboardBuilder().Clear().Build();
                return vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = random.Next(),
                    UserId = user.UserId,
                    PeerId = user.PeerId,
                    Message = message,
                    Keyboard = keybord,
                });
            }
            else
            {
                return vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = random.Next(),
                    UserId = user.UserId,
                    PeerId = user.PeerId,
                    Message = message,
                    Keyboard = typeKeyboard == TypeKeyboard.OrderBoard ? Keys.orderKeyboard : Keys.orderAcceptKeyboard,
                });
            }

        }
        public static void DeleteMessage(VkApi vkApi, long? randomId)
        {
            vkApi.Messages.Delete(new List<ulong> { (ulong) randomId }, deleteForAll:true);
        }
        public static void SendAnswer(VkApi vkApi, User user)
        {
            vkApi.Messages.SendMessageEventAnswer(user.EventId, (long)user.PeerId, (long)user.PeerId);
        }
    }
}
