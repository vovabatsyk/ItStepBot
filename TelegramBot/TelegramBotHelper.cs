using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace TelegramBot
{
    class TelegramBotHelper
    {

        private const string TEXT_EDU_FORMS = "Форми навчання";
        private const string TEXT_ACTIVITIES = "Заходи";
        private const string TEXT_ABOUT = "Про Академію";
        private const string TEXT_CONTACTS = "Контакти";
        private const string TEXT_BACK = "Назад";
        
        private const string TEXT_ADULT_EDU = "Освіта для дорослих";
        private const string TEXT_STUDENT_EDU = "Вища комп'ютерна освіта";
        private const string TEXT_CHILD_EDU = "Освіта для дітей";



        private string _token;
        private TelegramBotClient _botClient;

        public TelegramBotHelper(string token)
        {
            _token = token;
        }

        public void GetUpdates()
        {
            _botClient = new TelegramBotClient(_token);
            var me = _botClient.GetMeAsync().Result;
            if (me != null || !string.IsNullOrWhiteSpace(me.Username))
            {
                int offset = 0;

                while (true)
                {
                    try
                    {
                        var updates = _botClient.GetUpdatesAsync(offset).Result;
                        if (updates != null || updates.Length > 0)
                        {
                            foreach (var update in updates)
                            {
                                ProcessUpdate(update);
                                offset = update.Id + 1;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Thread.Sleep(1000);

                }
            }
        }

        private void ProcessUpdate(Update update)
        {

            switch (update.Type)
            {
                case UpdateType.Message:
                    var text = update.Message.Text;
                    string imgPath = null;

                    switch (text)
                    {
                        case "/start":
                            imgPath = Path.Combine(Environment.CurrentDirectory, "Images\\itstep-logo.jpg");
                            using (var stream = File.OpenRead(imgPath))
                            {
                                var r = _botClient.SendPhotoAsync(update.Message.Chat.Id,
                                    new InputOnlineFile(stream),
                                    ContentHelper.getInstance().Home, replyMarkup: GetButtons()).Result;
                            }
                            break;
                        
                        case TEXT_EDU_FORMS:
                            _botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: GetEduFormsButtons());
                            break;

                        case TEXT_ABOUT:
                            imgPath = Path.Combine(Environment.CurrentDirectory, "Images\\itstep-logo.jpg");
                            using (var stream = File.OpenRead(imgPath))
                            {
                                var r = _botClient.SendPhotoAsync(update.Message.Chat.Id,
                                    new InputOnlineFile(stream),
                                    ContentHelper.getInstance().About, replyMarkup: GetInlineButton("https://lviv.itstep.org")).Result;
                            }
                            break;

                        case TEXT_ACTIVITIES:
                            imgPath = Path.Combine(Environment.CurrentDirectory, "Images\\activities_1.png");
                            using (var stream = File.OpenRead(imgPath))
                            {
                                var r = _botClient.SendPhotoAsync(update.Message.Chat.Id,
                                    new InputOnlineFile(stream),
                                    ContentHelper.getInstance().Activities_1, replyMarkup: GetInlineButton("https://lviv.itstep.org/events/free-trial-it-lessons-for-children-7-14-years12345")).Result;
                            }

                            imgPath = Path.Combine(Environment.CurrentDirectory, "Images\\activities_2.jpg");
                            using (var stream = File.OpenRead(imgPath))
                            {
                                var r = _botClient.SendPhotoAsync(update.Message.Chat.Id,
                                    new InputOnlineFile(stream),
                                    ContentHelper.getInstance().Activities_2, replyMarkup: GetInlineButton("https://lviv.itstep.org/events/free-trial-it-lessons-for-children-7-14-years12345")).Result;
                            }

                            imgPath = Path.Combine(Environment.CurrentDirectory, "Images\\activities_3.png");
                            using (var stream = File.OpenRead(imgPath))
                            {
                                var r = _botClient.SendPhotoAsync(update.Message.Chat.Id,
                                    new InputOnlineFile(stream),
                                    ContentHelper.getInstance().Activities_3, replyMarkup: GetInlineButton("https://lviv.itstep.org/events/free-trial-it-lessons-for-children-7-14-years12345")).Result;
                            }

                            _botClient.SendTextMessageAsync(update.Message.Chat.Id, "_______________",
                                replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton()
                                    {Text = "Показати ще ", Url = "https://lviv.itstep.org/events"}));
                            break;

                        case TEXT_CONTACTS:
                            _botClient.SendTextMessageAsync(update.Message.Chat.Id, ContentHelper.getInstance().Contact);
                            break;

                        
                        case TEXT_BACK:
                            _botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: GetButtons());
                            break;
                        
                        case TEXT_ADULT_EDU:
                            imgPath = Path.Combine(Environment.CurrentDirectory, "Images\\adults.jpg");
                            using (var stream = File.OpenRead(imgPath))
                            {
                                var r = _botClient.SendPhotoAsync(update.Message.Chat.Id,
                                    new InputOnlineFile(stream),
                                    ContentHelper.getInstance().Adults, replyMarkup: GetInlineButton("https://lviv.itstep.org/adult_IT_courses")).Result;
                            }
                            break;

                        case TEXT_STUDENT_EDU:
                            imgPath = Path.Combine(Environment.CurrentDirectory, "Images\\student.jpg");
                            using (var stream = File.OpenRead(imgPath))
                            {
                                var r = _botClient.SendPhotoAsync(update.Message.Chat.Id,
                                    new InputOnlineFile(stream),
                                    ContentHelper.getInstance().Student, replyMarkup: GetInlineButton("https://lviv.itstep.org/higher-education")).Result;
                            }
                            break;
                        case TEXT_CHILD_EDU:
                            imgPath = Path.Combine(Environment.CurrentDirectory, "Images\\child.jpg");
                            using (var stream = File.OpenRead(imgPath))
                            {
                                var r = _botClient.SendPhotoAsync(update.Message.Chat.Id,
                                    new InputOnlineFile(stream),
                                    ContentHelper.getInstance().Child, replyMarkup: GetInlineButton("https://lviv.itstep.org/childrens_IT_courses")).Result;
                            }
                            break;
                        
                        default:
                            _botClient.SendTextMessageAsync(update.Message.Chat.Id, "$\"Receive text {text}\"");
                            break;
                    }
                    break;

                default:
                    Console.WriteLine(update.Type + "not implemented!");
                    break;
            }
        }

        private IReplyMarkup GetInlineButton(string url)
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton
            { Text = "Детальніше", Url = url });
        }

        private IReplyMarkup GetEduFormsButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>
                    {
                        new KeyboardButton{Text = TEXT_ADULT_EDU}, new KeyboardButton {Text = TEXT_STUDENT_EDU}
                    },
                    new List<KeyboardButton>
                    {
                        new KeyboardButton{Text = TEXT_CHILD_EDU}, new KeyboardButton {Text = TEXT_BACK                  }
                    }
                },
                ResizeKeyboard = true
            };
        }

        private IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                     new List<KeyboardButton>
                     {
                         new KeyboardButton{Text = TEXT_EDU_FORMS}, new KeyboardButton {Text = TEXT_ABOUT}
                     },
                     new List<KeyboardButton>
                     {
                         new KeyboardButton{Text = TEXT_ACTIVITIES}, new KeyboardButton {Text = TEXT_CONTACTS}
                     }
                }, ResizeKeyboard = true
            };
        }
    }
}
