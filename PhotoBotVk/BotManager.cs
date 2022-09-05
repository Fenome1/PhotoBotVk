using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PhotoBotVk.Data;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace PhotoBotVk
{
    internal sealed class BotManager
    {
        private readonly User _user = new();
        private readonly VkApi _vkApi = new();
        private readonly Order _order = new();

        public BotManager()
        {
            _vkApi.Authorize(new ApiAuthParams
            {
                AccessToken = Data.Data.Token
            });
            Console.WriteLine($"Статус авторизации: {_vkApi.IsAuthorized}");
        }

        public void StartMessageHandling()
        {
            var server = _vkApi.Groups.GetLongPollServer(Data.Data.GroupId);
            var ts = server.Ts;

            while (true)
                try
                {
                    var poll = _vkApi.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
                    {
                        Key = server.Key,
                        Server = server.Server,
                        Ts = ts,
                        Wait = 25
                    });

                    if (!poll.Updates.Any())
                        continue;

                    ts = poll.Ts;


                    foreach (var update in poll.Updates)
                        ThreadPool.QueueUserWorkItem(_ =>
                        {
                            if (update.Type == GroupUpdateType.MessageNew)
                            {
                                _user.Message = update.MessageNew.Message.Text.ToLower();
                                _user.UserId = update.MessageNew.Message.UserId;
                                _user.PeerId = update.MessageNew.Message.PeerId;

                                if (_user.Location == 3)
                                {
                                    var photoInfo = update.MessageNew.Message.Attachments;

                                    if (photoInfo.Count == _user.PhotoCount &&
                                        !string.IsNullOrEmpty(_user.Message))
                                    {
                                        //_order.OrderId = Data.Data.OrderDataId++;
                                        //_order.UserId = _user.UserId;
                                        //_order.PeerId = _user.PeerId;
                                        //_order.Price = _user.Price;
                                        //_order.OrderMessage = _user.Message;
                                        //_order.OrderProductPhoto = attachments;

                                        Sender.SendMessage(_vkApi, $"Внимание! Цена за {_user.PhotoCount} шт. " +
                                            $"обработки фото будет стоить: {_user.Price} руб.", _user, true);

                                        _user.TempPeerId = Sender.SendOrderKeyboard(_vkApi, "Подтвердить заказ?", _user,
                                            TypeKeyboard.OrderAccept);
                                        _user.Location = 4;
                                    }
                                    else
                                    {
                                        Sender.SendMessage(_vkApi, $"Повторите попытку!\nЗагружены не все фотографии, либо не добавленно описание!", _user, true);
                                    }
                                }

                                if (_user.Location == 2)
                                {
                                    if (int.TryParse(_user.Message, out var result))
                                    {
                                        if (0 < Convert.ToInt32(_user.Message) &&
                                            Convert.ToInt32(_user.Message) < 11)
                                        {
                                            _user.PhotoCount = result;
                                            _user.Price = result * Data.Data.Prise;
                                            Sender.SendMessage(_vkApi, StrPattern.OrderTz, _user, true);
                                            _user.Location = 3;
                                        }
                                    }
                                    else
                                    {
                                        Sender.SendMessage(_vkApi, $"Пожалуйста, введите число 1 - 10", _user, true);
                                    }
                                }

                                switch (update.MessageNew.Message.Payload)
                                {
                                    case "{\"command\":\"start\"}":
                                        Sender.SendOrderKeyboard(_vkApi, StrPattern.HelloBot, _user);
                                        _user.Location = 1;
                                        break;
                                }
                            }

                            if (update.Type == GroupUpdateType.MessageEvent)
                            {
                                _user.PeerId = update.MessageEvent.PeerId!.Value;
                                _user.UserId = update.MessageEvent.UserId!.Value;
                                _user.EventId = update.MessageEvent.EventId;

                                switch (update.MessageEvent.Payload)
                                {
                                    case "1": //кнопка Заказа
                                        Sender.SendAnswer(_vkApi, _user);
                                        Sender.SendMessage(_vkApi, $"{StrPattern.OrderButtonClick}" +
                                            $"Стоимость обработки 1 шт. состовляет: {Data.Data.Prise} руб.", _user, true);
                                        _user.Location = 2;
                                        break;
                                    case "2": //кнопка Подробнее
                                        Sender.SendAnswer(_vkApi, _user);
                                        Sender.SendMessage(_vkApi, StrPattern.MoreButton, _user);
                                        break;
                                    case "3": //кнопка Подтвердить заказ
                                        Sender.SendAnswer(_vkApi, _user);
                                        Sender.SendMessage(_vkApi, StrPattern.OrderAgree, _user, true);
                                        Sender.DeleteMessage(_vkApi, new List<long> { (long)_user.TempPeerId }, (long)_user.PeerId, true);
                                        //Sender.SendOrder(_vkApi, "Поступил заказ!", Data.Data.AdminId.First(), _order);
                                        _user.Location = 5;
                                        break;
                                    case "4": //кнопка Отказаться от заказа
                                        Sender.SendAnswer(_vkApi, _user);
                                        Sender.SendOrderKeyboard(_vkApi, StrPattern.OrderDisAgree, _user);
                                        Sender.DeleteMessage(_vkApi, new List<long> { (long)_user.TempPeerId }, (long)_user.PeerId, true);
                                        _order.Clear();
                                        _user.Location = 1;
                                        break;
                                }
                            }
                        });
                }
                catch (LongPollKeyExpiredException)
                {
                    server = _vkApi.Groups.GetLongPollServer(Data.Data.GroupId);
                }
        }
    }
}