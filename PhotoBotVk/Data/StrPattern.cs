﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBotVk
{
    internal sealed class StrPattern
    {
        public static string OrderButtonClick => "Вы пытаетесь сделать заказ!\n\n\n\nДля начала пришлите цифру того, сколько фотографий вы желаете обработать (от 1 - 10)";
        public static string OrderTz => "Для продолжения оформления заказа, пожалуйста, пришлите фото и комментарий мастеру с пожеланиями по обработке!\n\n\n\nПожалуйста, сделайте это в одном сообщении, иначе мастер не сможет получть ваш заказ корректно!";
        public static string OrderAgree => "Внимание! Заказ оформлен. &#128521;";
        public static string OrderDisAgree => "Очень жаль &#128546;\n\n\n\nНо вы снова можете попробовать восспользоваться нашими услугами.";
        public static string MoreButton => "Подробнее";
        public static string HelloBot => "Привет!";
    }
}