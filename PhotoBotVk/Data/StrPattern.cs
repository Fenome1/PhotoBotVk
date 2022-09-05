namespace PhotoBotVk.Data
{
    internal sealed class StrPattern
    {
        public static string OrderButtonClick =>
            "Вы пытаетесь сделать заказ!\n\n\n\nДля начала пришлите количество того, сколько фотографий вы желаете обработать (от 1 - 10).\n\n\n\n";

        public static string OrderTz =>
            "Для продолжения оформления заказа, пожалуйста, пришлите фото и комментарий мастеру с пожеланиями по обработке!\n\n\n\n" +
            "Пожалуйста, сделайте это в одном сообщении, иначе мастер не сможет получть ваш заказ корректно!";

        public static string OrderAgree => "Внимание! Заказ оформлен. &#128521;";

        public static string OrderDisAgree =>
            "Очень жаль &#128546;\n\n\n\nНо вы снова можете попробовать восспользоваться нашими услугами.";

        public static string MoreButton => "Подробнее";
        public static string HelloBot => "Привет!";
        public static string[] PriseSynonyms => new string[]
        {"стоимость", "цена",}; //В будующем реализую алгоритм,
                                //если челы пишут по рофлу не юзая бота по прямому назначению. для вывода стоимости.
    }
}