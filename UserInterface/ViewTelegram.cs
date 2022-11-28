using Bot_CoursePaper.Logic;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
//при создании ToLower()
// в get увеличить Имя
//кнопка изменить
namespace Bot_CoursePaper.UserInterface
{
    static class ViewTelegram
    {
        static ITelegramBotClient bot = new TelegramBotClient("5603713455:AAGTlrFcSOBUrr0qZ2zVJZBiu0u5P-T5n1Y");

        static void Main(string[] args)
        {
            ChooseCommand.Deserialize();

            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            
            bot.StartReceiving(
                Update,
                Error,
                receiverOptions,
                cancellationToken
            );

            Console.ReadLine();
            ChooseCommand.Serialize();
        }
        
        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if (update.Type == UpdateType.CallbackQuery)
            {
               ChooseCommand.HandleCallbackQuery(update.CallbackQuery);
                return;
            }
            
            if (update.Message == null) return;
            
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
            {
                ChooseCommand.HandlerMessage(update.Message);
                return;
            }

            if (update.Message != null)
                await botClient.SendStickerAsync(
                    update.Message.Chat.Id,
                    @"https://i0.wp.com/www.printmag.com/wp-content/uploads/2021/02/4cbe8d_f1ed2800a49649848102c68fc5a66e53mv2.gif?fit=476%2C280&ssl=1");
        }

        public static async Task SendMessage(Message message, string text, IReplyMarkup replyMarkup)
        {
            await bot.SendTextMessageAsync(message.Chat.Id, text, 
                replyMarkup: replyMarkup, parseMode: ParseMode.Html);
        }
        
        public static IReplyMarkup GetButtons(string[] btn)
        {
            ReplyKeyboardMarkup keyboardMarkup = new(new[]
            {
                new KeyboardButton[] { btn[0], btn[1] },
                new KeyboardButton[] { btn[2], btn[3], btn[4] },
                new KeyboardButton[] { btn[5] }
            })
            {
                ResizeKeyboard = true
            };

            return keyboardMarkup;
        }

        public static IReplyMarkup GetInlineButtons(string[] btn)
        {
            InlineKeyboardMarkup keyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(btn[0], btn[0]),
                    InlineKeyboardButton.WithCallbackData(btn[1], btn[1]),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(btn[2], btn[2]),
                    InlineKeyboardButton.WithCallbackData(btn[3], btn[3]),
                },
            });

            return keyboard;
        }

        private static async Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}