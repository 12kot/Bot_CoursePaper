using System.Xml;
using System.Xml.Serialization;
using Bot_CoursePaper.UserInterface;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot_CoursePaper.Logic;

public class ChooseCommand
{
    private Actions _actions = new ();

    private string _newAnimal = "", //для создания животного
        _search = "undefined", //для поиска 
        _deleteName = "undefined", //для удаления
        _edit = "undefined", _eName = "undefined", _eAge = "undefined", _ePopulation = "undefined";

    private IReplyMarkup _buttons = null!;

    public void Deserialize()
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
    public void Serialize() 
    {
         _actions.SerialiseToXml();
    }
    
    public async void HandlerMessage(Message message)
    {
        if (message.Text == null) return;
        
        string messText = message.Text.ToLower().Trim();
        
        if(await CheckActions(message, messText)) return;
        
        switch (messText)
        {
            case "/start":
                StartMessage(message);
                return;

            case "добавить":
                AddMessage(message);
                return;
                
            case "отобразить":
               OutputMessage(message);
                return;
            
            case "удалить":
                DeleteMessage(message);
                return;
                
            case "отсортировать":
                SortMessage(message);
                return;
                
            case "поиск":
                SearchMessage(message);
                return;
            
            case "изменить":
                EditMessage(message);
                return;
        }

        _buttons = ViewTelegram.GetButtons(new[] { "/start", "", "", "", "", "" });
        await ViewTelegram.SendMessage(message, "Такой команды нет", _buttons);
    }
    private   async Task<bool> CheckActions(Message message, string messText)
    {
         if (_newAnimal != "")
         {
             _newAnimal += messText;
                
             await ViewTelegram.SendMessage(message, 
                 _actions.AddAnimal(_newAnimal), null!);
                
             _newAnimal = "";
             return true;
         }

         if (_search == "")
         {
             await ViewTelegram.SendMessage(message,
                 _actions.Display(_actions.Search(messText)), null!);
                
             _search = messText;
             return true;
         }
        
         if (_deleteName == "")
         {
             await ViewTelegram.SendMessage(message,
                 _actions.RemoveAnimal(messText), null!);
                
             _deleteName = messText;
             return true;
         }
        
         if (_edit == "")
         {
             if (!_actions.CheckName(messText))
             {
                 await ViewTelegram.SendMessage(message,
                     "Морские обитатели с данным именем не найдены", null!);
                 _edit = "undefined";
                 return true;
             }

             _edit = messText;
             _buttons = ViewTelegram.GetInlineButtons(
                 new[] { "Изм. имя", "Изм. возраст", "Изм. популяцию", " " });
                            
             await ViewTelegram.SendMessage(
                 message, "Что вы хотите изменить", _buttons);
             return true;
         }

         if (_eName == "")
         {
             if(_actions.CheckName(messText)) {
                 await ViewTelegram.SendMessage(message,
                     "Морские обитатели с данным именем уже существуют", null!);
                 _eName = "undefined";
                 return true;
             }

             _eName = messText;
             _actions.EditName(_eName, _edit);
            
             MessageDone(message);
             return true;
         }
        
         if (_eAge == "")
         {
             _eAge = messText;
             _actions.EditAge(_eAge, _edit);
            
             MessageDone(message);
             return true;
         }
        
         if (_ePopulation == "")
         {
             _ePopulation = messText;
             _actions.EditPopulation(_ePopulation, _edit);
            
             MessageDone(message);
             return true;
         }

         return false;
    }
    
    private async void StartMessage(Message message)
    {
        _buttons = ViewTelegram.GetButtons(
            new[] { "Добавить", "Удалить", "Отобразить", "Отсортировать", "Поиск", "Изменить" });
        await ViewTelegram.SendMessage(message, $"Привет, {message.Chat.FirstName}\n" +
                                                "Выбери действие", _buttons);
    }
    
    private async void AddMessage(Message message) {//if(await MessageNonAdmin(message)) return;
        _buttons = ViewTelegram.GetInlineButtons(
            new[] { "Членистоногое", "Ракообразное", "Млекопитающее", "Рыба" });
        await ViewTelegram.SendMessage(
            message, "Выберите тип морского обитателя", _buttons);
    }
    
    private async void OutputMessage(Message message)
    {
        await ViewTelegram.SendMessage(
            message, _actions.Display(), null!);
    }
    
    private async void DeleteMessage(Message message)
    {  //if(await MessageNonAdmin(message)) return;
        await ViewTelegram.SendMessage(
            message, "Введи имя морского обитателя", null!);
        _deleteName = "";
    }
    
    private async void SortMessage(Message message)
    {
        _buttons =  ViewTelegram.GetInlineButtons(
            new[] { "Класс", "Имя", "Возраст", "Популяция" });
        await ViewTelegram.SendMessage(
            message, "Выбери критерий для сортировки", _buttons);
    }
    
    private async void SearchMessage(Message message)
    {
        await ViewTelegram.SendMessage(
            message, "Введи имеющуюся информацию", null!);
        _search = "";
    }
    
    private async void EditMessage(Message message)
    { //  if(await MessageNonAdmin(message)) return;
        await ViewTelegram.SendMessage(
            message, "Введи имя морского обитателя", null!);
        _edit = "";
    }
    
    
    public void HandleCallbackQuery(CallbackQuery? callbackQuery)
    {
        if (callbackQuery?.Data == null || callbackQuery.Message == null) return;
        
        string data = callbackQuery.Data.ToLower().Trim();
        
        switch (data)
        { 
            case "класс": 
                SortClass(callbackQuery.Message);
                return;
                
            case "имя": 
                SortName(callbackQuery.Message);
                return;
                
            case "возраст": 
                SortAge(callbackQuery.Message);
                return;
                
            case "популяция": 
                SortPopulation(callbackQuery.Message);
                return;
                
            case "членистоногое": 
            case "ракообразное": 
            case "млекопитающее": 
            case "рыба":
                AddAnimal(callbackQuery.Message, data);
                return;
            
            case "изм. имя":
                EditName(callbackQuery.Message);
                return;
            
            case "изм. возраст":
                EditAge(callbackQuery.Message);
                return;
            
            case "изм. популяцию":
                EditPopulation(callbackQuery.Message);
                return;
        }
    }

    private async void SortClass(Message message)
    {
        _actions.SortByClass();
        await ViewTelegram.SendMessage(
            message, "Сортировка по классам выполнена.", null!);
    }
    
    private async void SortName(Message message)
    {
        _actions.SortByName();
        await ViewTelegram.SendMessage(
            message,"Сортировка по имени выполнена.", null!);
    }
    
    private async void SortAge(Message message)
    {
        _actions.SortByAge();
        await ViewTelegram.SendMessage(
            message, "Сортировка по возрасту выполнена.", null!);
    }
    
    private async void SortPopulation(Message message)
    {
        _actions.SortByPopulation();
        await ViewTelegram.SendMessage(
            message, "Сортировка по популяции выполнена.", null!);
    }
    
    private async void AddAnimal(Message message, string data)
    {
        _newAnimal = data + " ";
        await ViewTelegram.SendMessage(
            message, "Введи доп. информацию (ИМЯ ПОПУЛЯЦИЯ ВОЗРАСТ)", null!);
    }
    
    private async void EditName(Message message)
    {
        await ViewTelegram.SendMessage(message,
            "Введите новое имя для морского обитателя", null!);
        _eName = "";
    }
    
    private async void EditAge(Message message)
    {
        await ViewTelegram.SendMessage(message,
            "Введите новый возраст для морского обитателя", null!);
        _eAge = "";
    }
    
    private async void EditPopulation(Message message)
    {
        await ViewTelegram.SendMessage(message,
            "Введите новую популяцию для морского обитателя", null!);
        _ePopulation = "";
    }

    
    private async Task<bool> MessageNonAdmin(Message message)
    {
        if (message.Chat.Id == 801384711) return false;
        await ViewTelegram.SendMessage(
            message, "Вы не являетесь администратором.", null!);
        return true;

    }
    private async void MessageDone(Message message)
    {
         await ViewTelegram.SendMessage(
            message, "Готово", null!);
    }
}