using System;
using System.Collections.Generic;
using PhotoBotVk.Data;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model.Attachments;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace PhotoBotVk
{
    internal static class Sender
    {
        private static readonly Random random = new();

        public static void SendMessage(VkApi vkApi, string message, User user, bool keyboardClear = false)
        {
            if (keyboardClear)
            {
                var keyboard = new KeyboardBuilder().Clear().Build();
                vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = random.Next(),
                    UserId = user.UserId,
                    PeerId = user.PeerId,
                    Message = message,
                    Keyboard = keyboard
                });
            }
            else
            {
                vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = random.Next(),
                    UserId = user.UserId,
                    PeerId = user.PeerId,
                    Message = message
                });
            }
        }

        public static long? SendOrderKeyboard(VkApi vkApi, string message, User user,
            TypeKeyboard typeKeyboard = TypeKeyboard.OrderBoard, bool keyboardClear = false)
        {
            if (keyboardClear)
            {
                var keyboard = new KeyboardBuilder().Clear().Build();
                return vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = random.Next(),
                    UserId = user.UserId,
                    PeerId = user.PeerId,
                    Message = message,
                    Keyboard = keyboard
                });
            }

            return vkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = random.Next(),
                UserId = user.UserId,
                PeerId = user.PeerId,
                Message = message,
                Keyboard = typeKeyboard == TypeKeyboard.OrderBoard ? Keys.OrderKeyboard : Keys.OrderAcceptKeyboard
            });
        }

        public static void DeleteMessage(IVkApi vkApi,
            IEnumerable<long> messageIds,
            long peerId,
            bool deleteForAll = true,
            bool spam = false)
        {
            var parameters = new Dictionary<string, string>
            {
                { "message_ids", string.Join(',', messageIds) },
                { "peer_id", peerId.ToString() },
                { "spam", Convert.ToInt32(spam).ToString() },
                { "delete_for_all", Convert.ToInt32(deleteForAll).ToString() }
            };
            vkApi.Call("messages.delete", new VkParameters(parameters));
        }

        public static void SendAnswer(VkApi vkApi, User user)
        {
            vkApi.Messages.SendMessageEventAnswer(user.EventId, (long)user.PeerId, (long)user.PeerId);
        }
        //public static void SendOrder(VkApi vkApi, string message, long userId, Order order)
        //{
        //    _ = vkApi.Messages.Send(new MessagesSendParams
        //    {
        //        UserId = userId,
        //        PeerId = userId,
        //        Message = $"{message}: Номер заказа - {order.OrderId}\nЗаказчик: {order.UserId}" +
        //        $"\nОписание заказа:{order.OrderMessage}\nНавар))):{order.Price}",
        //        Attachments = new List<MediaAttachment> { order.OrderProductPhoto }
        //    });
        //}
    }
}
