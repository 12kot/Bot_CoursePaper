using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
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
        private static Actions actions = new Actions();
        
        static ITelegramBotClient bot = new TelegramBotClient("TOKEN");
        
        private static string _newAnimal = "",  //для создания животного
                            _search = "undefined", //для поиска 
                            _deleteName = "undefined"; //для удаления

        static void Main(string[] args)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Actions));
            XmlReader textReader = XmlReader.Create(@"C:\Users\User\RiderProjects\Bot_CoursePaper\Bot_CoursePaper\ocean.xml");
            try
            {
                actions = (Actions)xmlSerializer.Deserialize(textReader);
            }
            catch
            {
                // ignored
            }

            textReader.Close();

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
            actions.SerialiseToXml();
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

            await botClient.SendStickerAsync(
                update.Message.Chat.Id, @"https://i0.wp.com/www.printmag.com/wp-content/uploads/2021/02/4cbe8d_f1ed2800a49649848102c68fc5a66e53mv2.gif?fit=476%2C280&ssl=1");
        }
        
        private static async Task HandlerMessage(ITelegramBotClient botClient, Message message)
        {
            if (_newAnimal != "")
            {
                _newAnimal += message.Text;
                
                await botClient.SendTextMessageAsync(
                    message.Chat.Id, actions.AddAnimal(_newAnimal.Trim()));
                _newAnimal = "";
                return;
            }

            if (_search == "")
            {
                await botClient.SendTextMessageAsync(
                    message.Chat.Id, actions.Display(actions.Search(message.Text.Trim().ToLower())));
                _search = message.Text;
                return;
            }

            if (_deleteName == "")
            {
                await botClient.SendTextMessageAsync(
                    message.Chat.Id, actions.RemoveAnimal(message.Text.Trim().ToLower()));
                _deleteName = message.Text;
                return;
            }

            switch (message.Text)
            {
                case "/start":
                    await botClient.SendTextMessageAsync(message.Chat, $"Привет, {message.Chat.FirstName}");
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выбери действие", 
                        replyMarkup: GetButtons("Добавить", "Удалить", "Отобразить", "Отсортировать", "Поиск", ""));
                    return;
                
                case "Добавить":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выбери тип морского обитателя", 
                        replyMarkup: GetInlineButtons("Членистоногое", "Ракообразное", "Млекопитающее", "Рыба"));
                    return;
                
                case "Отобразить":
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id, actions.Display(actions.Animals));
                    return;
                
                case "Удалить":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введи наименование морского обитателя");
                    _deleteName = "";
                    return;
                
                case "Отсортировать":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выбери по чём будем сортировать", 
                        replyMarkup: GetInlineButtons("Класс", "Наименование", "Возраст", "Популяция"));
                    return;
                
                case "Поиск":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введи имеющуюся информацию");
                    _search = "";
                    return;
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, "Не стоит писать в чат, когда не просят", 
                replyMarkup: GetButtons("Лучше", "тыкай ", "по кнопкам", "/start", "", ""));
        }

        private static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery? callbackQuery)
        {
            switch (callbackQuery.Data)
            {
                case "Класс":
                    actions.SortByClass();
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Сортировка по классам выполена.");
                    return;
                
                case "Наименование":
                    actions.SortByName();
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Сортировка по наименованию выполена.");
                    return;
                
                case "Возраст":
                    actions.SortByAge();
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id, "Сортировка по возрасту выполена.");
                    return;
                
                case "Популяция":
                    actions.SortByPopulation();
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

        private static IReplyMarkup GetButtons(string btn1, string btn2, string btn3, string btn4, string btn5, string btn6)
        {
            ReplyKeyboardMarkup keyboardMarkup = new(new[]
            {
                new KeyboardButton[] { btn1, btn2 },
                new KeyboardButton[] { btn3, btn4, btn5 },
                new KeyboardButton[] { btn6 }
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