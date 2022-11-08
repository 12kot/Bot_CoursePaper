using System;
using System.Threading;
using System.Threading.Tasks;
using S3_CoursePaper;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot_CoursePaper
{
    static class BotCoursePaper
    {
        static ITelegramBotClient bot = new TelegramBotClient("TOKEN");
        
        private static string _newAnimal = "",  //для создания животного
                            _search = "undefined"; //для поиска 

        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            Actions.AddAnimal("Членистоногое скорпион 2000 1000");
            Actions.AddAnimal("Рыба Окунь 800 100");
            Actions.AddAnimal("Млекопитающее Медведь 500 200");
            Actions.AddAnimal("Ракообразное Рак 1000 1");
            
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
        }
        
        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if (update.Type == UpdateType.Message && update.Message?.Text != null)
            {
                await HandlerMessage(botClient, update.Message);
                return;
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }

            await botClient.SendTextMessageAsync(
                update.Message.Chat.Id, "Я не воспринимаю стикеры/фото/эмодзи/видео");
        }

        private static async Task HandlerMessage(ITelegramBotClient botClient, Message message)
        {
            if (_newAnimal != "" && _newAnimal != "/start" && _newAnimal != "Добавить"
                && _newAnimal != "Отобразить" && _newAnimal != "Удалить"
                && _newAnimal != "Отсортировать" && _newAnimal != "Поиск")
            {
                _newAnimal += message.Text;
                
                await botClient.SendTextMessageAsync(
                    message.Chat.Id, Actions.AddAnimal(_newAnimal.Trim()));
                _newAnimal = "";
                return;
            }

            if (_search == "" && _search != "/start" && _search != "Добавить"
                && _search != "Отобразить" && _search != "Удалить"
                && _search != "Отсортировать" && _search != "Поиск")
            {
                await botClient.SendTextMessageAsync(
                    message.Chat.Id, Actions.Display(Actions.Search(message.Text)));
                _search = "undefined";
                return;
            }

            switch (message.Text)
            {
                case "/start":
                    await botClient.SendTextMessageAsync(message.Chat, $"Привет, {message.Chat.FirstName}");
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выбери действие", 
                        replyMarkup: GetButtons("Добавить", "Удалить", "Отобразить", "Отсортировать", "Поиск"));
                    return;
                
                case "Добавить":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выбери тип морского обитателя", 
                        replyMarkup: GetInlineButtons("Членистоногое", "Ракообразное", "Млекопитающее", "Рыба"));
                    return;
                
                case "Отобразить":
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id, Actions.Display(Actions.Animals));
                    return;
                
                case "Удалить":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Себя удали. Это мои рыбки!");
                    return;
                
                case "Отсортировать":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выбери по чём будем сортировать", 
                        replyMarkup: GetInlineButtons("Класс", "Наименование", "Возраст", "Популяция"));
                    return;
                
                case "Поиск":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введи имеющуюся информацию.");
                    _search = "";
                    return;
            }
            
            await botClient.SendTextMessageAsync(message.Chat.Id, "Не пиши в чат. Тыкай по кнопкам.", 
                replyMarkup: GetButtons("ПИШИ", "ЗАНОВО", "/start", "", ""));
        }

        private static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery? callbackQuery)
        {
            switch (callbackQuery.Data)
            {
                case "Класс":
                    Actions.SortByClass();
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Сортировка по классам выполена.");
                    return;
                
                case "Наименование":
                    Actions.SortByName();
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Сортировка по наименованию выполена.");
                    return;
                
                case "Возраст":
                    Actions.SortByAge();
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Сортировка по возрасту выполена.");
                    return;
                
                case "Популяция":
                    Actions.SortByPopulation();
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Сортировка по популяции выполена.");
                    return;
                
                case "Членистоногое":
                    _newAnimal += "Членистоногое ";
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Введи доп. информацию (ИМЯ ПОПУЛЯЦИЯ ВОЗРАСТ)");
                    return;
                
                case "Ракообразное": 
                    _newAnimal += "Ракообразное ";
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Введи доп. информацию (ИМЯ ПОПУЛЯЦИЯ ВОЗРАСТ) .");
                    return;
                
                case "Млекопитающее":
                    _newAnimal += "Млекопитающее ";
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Введи доп. информацию (ИМЯ ПОПУЛЯЦИЯ ВОЗРАСТ) .");
                    return;
                
                case "Рыба":
                    _newAnimal += "Рыба ";
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Введи доп. информацию (ИМЯ ПОПУЛЯЦИЯ ВОЗРАСТ) .");
                    
                    return;
            }
        }

        private static IReplyMarkup GetButtons(string btn1, string btn2, string btn3, string btn4, string btn5)
        {
            ReplyKeyboardMarkup keyboardMarkup = new(new[]
            {
                new KeyboardButton[] { btn1, btn2 },
                new KeyboardButton[] { btn3, btn4, btn5 }
            })
            {
                ResizeKeyboard = true
            };

            return keyboardMarkup;
        }

        private static IReplyMarkup GetInlineButtons(string btn1, string btn2, string btn3, string btn4)
        {
            InlineKeyboardMarkup keyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(btn1, btn1),
                    InlineKeyboardButton.WithCallbackData(btn2, btn2),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(btn3, btn3),
                    InlineKeyboardButton.WithCallbackData(btn4, btn4),
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