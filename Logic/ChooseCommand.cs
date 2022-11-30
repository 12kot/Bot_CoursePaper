using System.Xml;
using System.Xml.Serialization;
using Bot_CoursePaper.UserInterface;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot_CoursePaper.Logic;

public static class ChooseCommand
{
    private static Actions _actions = new ();

    private static string _newAnimal = "", //для создания животного
        _search = "undefined", //для поиска 
        _deleteName = "undefined", //для удаления
        _editName = "undefined", _eName = "undefined", _eAge = "undefined", _ePopulation = "undefined";

    private static IReplyMarkup _buttons = null!;

    public static void Deserialize()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Actions));
        
        XmlReader textReader = XmlReader.Create(@"C:\Users\User\RiderProjects\Bot_CoursePaper\Bot_CoursePaper\ocean.xml");
        try
        {
            _actions = (Actions)xmlSerializer.Deserialize(textReader);
        }
        catch
        {
            // ignored
        }

        textReader.Close();
    }
    public static void Serialize() 
    {
        _actions.SerialiseToXml();
    }
    
    public static void HandlerMessage(Message message)
    {
        if (message.Text == null) return;
        
        string messText = message.Text.ToLower().Trim();
        
        if (_newAnimal != "")
        {
            _newAnimal += messText;
                
            ViewTelegram.SendMessage(message, 
                _actions.AddAnimal(_newAnimal), null);
                
            _newAnimal = "";
            return;
        }

        if (_search == "")
        {
            ViewTelegram.SendMessage(message,
                _actions.Display(_actions.Search(messText)), null);
                
            _search = messText;
            return;
        }
        
        if (_deleteName == "")
        {
            ViewTelegram.SendMessage(message,
                _actions.RemoveAnimal(messText), null);
                
            _deleteName = messText;
            return;
        }
        
        if (_editName == "")
        {
            if (!_actions.CheckName(messText))
            {
                ViewTelegram.SendMessage(message,
                    "Морские обитатели с данным именем не найдены", null);
                _editName = "undefined";
                return;
            }

            _editName = messText;
            _buttons = ViewTelegram.GetInlineButtons(
                new[] { "Изм. имя", "Изм. возраст", "Изм. популяцию", " " });
                            
            ViewTelegram.SendMessage(
                message, "Что вы хотите изменить", _buttons);
            return;
        }

        if (_eName == "")
        {
            if(_actions.CheckName(messText)) {
                ViewTelegram.SendMessage(message,
                    "Морские обитатели с данным именем уже существуют", null);
                _eName = "undefined";
                return;
            }

            _eName = messText;
            _actions.EditName(_eName, _editName);
            
            MessageDone(message);
            return;
        }
        
        if (_eAge == "")
        {
            _eAge = messText;
            _actions.EditAge(_eAge, _editName);
            
            MessageDone(message);
            return;
        }
        
        if (_ePopulation == "")
        {
            _ePopulation = messText;
            _actions.EditPopulation(_ePopulation, _editName);
            
            MessageDone(message);
            return;
        }
        
        switch (messText)
        {
            case "/start":
                _buttons = ViewTelegram.GetButtons(
                    new[] { "Добавить", "Удалить", "Отобразить", "Отсортировать", "Поиск", "Изменить" });
                
                ViewTelegram.SendMessage(message, $"Привет, {message.Chat.FirstName}\n" +
                                                  "Выбери действие", _buttons);
                return;

            case "добавить":
                if(MessageNonAdmin(message)) return;
                
                _buttons = ViewTelegram.GetInlineButtons(
                    new[] { "Членистоногое", "Ракообразное", "Млекопитающее", "Рыба" });
                    
                ViewTelegram.SendMessage(
                    message, "Выберите тип морского обитателя", _buttons);
                return;
                
            case "отобразить":
                ViewTelegram.SendMessage(
                    message, _actions.Display(), null!);
                return;
            
            case "удалить":
                if(MessageNonAdmin(message)) return;
                
                ViewTelegram.SendMessage(
                    message, "Введи имя морского обитателя", null);
                _deleteName = "";
                return;
                
            case "отсортировать":
                _buttons = ViewTelegram.GetInlineButtons(
                    new[] { "Класс", "Имя", "Возраста", "Популяция" });
                    
                ViewTelegram.SendMessage(
                    message, "Выбери критерий для сортировки", _buttons);
                return;
                
            case "поиск":
                ViewTelegram.SendMessage(
                    message, "Введи имеющуюся информацию", null);
                _search = "";
                return;
            
            case "изменить":
                if(MessageNonAdmin(message)) return;
                
                ViewTelegram.SendMessage(
                    message, "Введи имя морского обитателя", null);
                _editName = "";
                return;
        }

        _buttons = ViewTelegram.GetButtons(new[] { "/start", "", "", "", "", "" });
        ViewTelegram.SendMessage(message, "Такой команды нет", _buttons);
    }
    public static void HandleCallbackQuery(CallbackQuery? callbackQuery)
    {
        if (callbackQuery?.Data == null || callbackQuery.Message == null) return;
        
        string data = callbackQuery.Data.ToLower().Trim();
        
        switch (data)
        { 
            case "класс": 
                _actions.SortByClass();
                
                ViewTelegram.SendMessage(
                        callbackQuery.Message, "Сортировка по классам выполнена.", null);
                return;
                
            case "имя": 
                _actions.SortByName();
                ViewTelegram.SendMessage(
                        callbackQuery.Message,"Сортировка по имени выполнена.", null);
                return;
                
            case "возраст": 
                _actions.SortByAge();
                ViewTelegram.SendMessage(
                        callbackQuery.Message, "Сортировка по возрасту выполнена.", null);
                return;
                
            case "популяция": 
                _actions.SortByPopulation();
                ViewTelegram.SendMessage(
                        callbackQuery.Message, "Сортировка по популяции выполнена.", null);
                return;
                
            case "членистоногое": 
            case "ракообразное": 
            case "млекопитающее": 
            case "рыба":
                _newAnimal = data + " ";
                ViewTelegram.SendMessage(
                        callbackQuery.Message, "Введи доп. информацию (ИМЯ ПОПУЛЯЦИЯ ВОЗРАСТ)", null);
                return;
            
            case "изм. имя":
                ViewTelegram.SendMessage(callbackQuery.Message,
                    "Введите новое имя для морского обитателя", null);
                _eName = "";
                return;
            
            case "изм. возраст":
                ViewTelegram.SendMessage(callbackQuery.Message,
                    "Введите новый возраст для морского обитателя", null);
                _eAge = "";
                return;
            
            case "изм. популяцию":
                ViewTelegram.SendMessage(callbackQuery.Message,
                    "Введите новую популяцию для морского обитателя", null);
                _ePopulation = "";
                return;
        }
    }
    private static bool MessageNonAdmin(Message message)
    {
        if (message.Chat.Id != 801384711)
        {
            ViewTelegram.SendMessage(
                message, "Вы не являетесь администратором.", null);
            return true;
        }

        return false;
    }
    private static void MessageDone(Message message)
    {
        ViewTelegram.SendMessage(
            message, "Готово", null);
    }
}