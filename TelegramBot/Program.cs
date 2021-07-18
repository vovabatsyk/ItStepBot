using System;

namespace TelegramBot
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("ItStepLvivBot");
            try
            {
                TelegramBotHelper botHelper = new TelegramBotHelper("1730341472:AAGk_HtHlFfmVZD2G4tPEVRAKBP81tc8us0");
                botHelper.GetUpdates();

            }
            catch (Exception e) { Console.WriteLine(e.Message); }


        }

    }


}
