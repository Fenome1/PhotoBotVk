using System;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;
using Data;
using System.Threading;

namespace PhotoBotVk
{
    internal sealed class BotManager
    {
        private VkApi vkApi = new VkApi();
        private User user = new User();

        public BotManager()
        {
            vkApi.Authorize(new ApiAuthParams
            {
                AccessToken = Data.Token
            });
            Console.WriteLine($"Статус авторизации: {vkApi.IsAuthorized}");
        }
        public void StartMessageHandling()
        {

            var server = vkApi.Groups.GetLongPollServer(Data.GroupId);
            var ts = server.Ts;

            while (true)
            {
                try
                {
                    var poll = vkApi.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
                    {
                        Key = server.Key,
                        Server = server.Server,
                        Ts = ts,
                        Wait = 25
                    });

                    if (!poll.Updates.Any())
                        continue;

                    ts = poll.Ts;


                    foreach (var ivent in poll.Updates)
                    {
                        ThreadPool.QueueUserWorkItem(_ =>
                        {
                            if (ivent.Type == GroupUpdateType.MessageNew)
                            {
                                user.UserMessage = ivent.MessageNew.Message.Text.ToLower();
                                user.UserId = ivent.MessageNew.Message.UserId;
                                user.PeerId = ivent.MessageNew.Message.PeerId;

                                if (user.UserLocation == 3)
                                {
                                    Sender.SendOrderKeyboard(vkApi, "Подтвердить заказ?", user, TypeKeyboard.OrderAccept);
                                    user.UserLocation = 4;
                                }
                                if (user.UserLocation == 2)
                                {
                                    if (int.TryParse(user.UserMessage, out int result))
                                    {
                                        if (0 < Convert.ToInt32(user.UserMessage) && Convert.ToInt32(user.UserMessage) < 11)
                                        {
                                            user.PhotoCount = result;
                                            Sender.SendMessage(vkApi, StrPattern.OrderTz, user, true);
                                            user.UserLocation = 3;
                                        }
                                    }
                                    else
                                    {
                                        Sender.SendMessage(vkApi, "Пожалуйста, введите число 1 - 10", user, true);
                                    }
                                }
                                switch (ivent.MessageNew.Message.Payload)
                                {
                                    case "{\"command\":\"start\"}":
                                        Sender.SendOrderKeyboard(vkApi, StrPattern.HelloBot, user);
                                        user.UserLocation = 1;
                                        break;
                                }
                            }
                            if (ivent.Type == GroupUpdateType.MessageEvent)
                            {
                                user.PeerId = ivent.MessageEvent.PeerId!.Value;
                                user.UserId = ivent.MessageEvent.UserId!.Value;
                                user.EventId = ivent.MessageEvent.EventId;

                                switch (ivent.MessageEvent.Payload)
                                {
                                    case "1": //кнопка Заказа
                                        Sender.SendAnswer(vkApi, user);
                                        Sender.SendMessage(vkApi, StrPattern.OrderButtonClick, user, true);
                                        user.UserLocation = 2;
                                        break;
                                    case "2": //кнопка Подробнее
                                        Sender.SendAnswer(vkApi, user);
                                        Sender.SendMessage(vkApi, StrPattern.MoreButton, user);
                                        break;
                                    case "3": //кнопка Подтвердить заказ
                                        Sender.SendAnswer(vkApi, user);
                                        Sender.SendMessage(vkApi, StrPattern.OrderAgree, user, true);
                                        user.UserLocation = 5;
                                        break;
                                    case "4": //кнопка Отказаться от заказа
                                        Sender.SendAnswer(vkApi, user);
                                        Sender.SendOrderKeyboard(vkApi, StrPattern.OrderDisAgree, user);
                                        user.UserLocation = 1;
                                        break;
                                }
                            }
                        });
                    }

                }
                catch (LongPollKeyExpiredException)
                {
                    server = vkApi.Groups.GetLongPollServer(Data.GroupId);
                }
            }
        }
    }
}
