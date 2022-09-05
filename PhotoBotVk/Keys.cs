using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace PhotoBotVk
{
    internal static class Keys
    {
        public static readonly MessageKeyboard OrderKeyboard = new KeyboardBuilder().AddButton(new MessageKeyboardButtonAction
        {
            Type = KeyboardButtonActionType.Callback,
            Label = "Сделать заказ",
            Payload = "1" //кнопка сделать заказ Payload = 1 
        }, KeyboardButtonColor.Primary)
            .AddLine()
            .AddButton(new MessageKeyboardButtonAction
            {
                Type = KeyboardButtonActionType.Callback,
                Label = "Подробнее",
                Payload = "2" //кнопка подробнее Payload = 2 
            }, KeyboardButtonColor.Default)
            .Build();

        public static readonly MessageKeyboard OrderAcceptKeyboard = new KeyboardBuilder().AddButton(
                new MessageKeyboardButtonAction
                {
                    Type = KeyboardButtonActionType.Callback,
                    Label = "Оформить",
                    Payload = "3" //кнопка поддтвердить заказ Payload = 3 
                }, KeyboardButtonColor.Primary)
            .SetInline()
            .AddButton(new MessageKeyboardButtonAction
            {
                Type = KeyboardButtonActionType.Callback,
                Label = "Не оформлять",
                Payload = "4" //кнопка отказаться от заказа Payload = 4 
            }, KeyboardButtonColor.Negative)
            .SetInline()
            .Build();
    }
}