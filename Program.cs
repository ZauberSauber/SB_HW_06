using System;

namespace Homework_07 {
    class Program {
        static void Main() {
            // Разработать ежедневник.
            // В ежедневнике реализовать возможность 
            // - создания
            // - удаления
            // - редактирования 
            // записей
            // 
            // В отдельной записи должно быть не менее пяти полей
            // 
            // Реализовать возможность 
            // - Загрузки данных из файла
            // - Выгрузки данных в файл
            // - Добавления данных в текущий ежедневник из выбранного файла
            // - Импорт записей по выбранному диапазону дат
            // - Упорядочивания записей ежедневника по выбранному полю

            ShowMainMenu();
        }
        
        /// <summary>
        /// Дефолтный путь к файлу
        /// </summary>
        static string path = @"data.csv";

        /// <summary>
        /// Инстанс класса пользовательских функций
        /// </summary>
        static UserAbilities UserAbilities = new UserAbilities();
        
        /// <summary>
        /// Инстанс класса ежедневника
        /// </summary>
        public static DailyPlanner DailyPlanner = new DailyPlanner(path);

        /// <summary>
        /// Выводит главное меню
        /// </summary>
        public static void ShowMainMenu() {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Что будем делать?");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Показать список мероприятий - 1");
            Console.WriteLine("Добавить мероприятие - 2");
            Console.WriteLine("Удалить мероприятие - 3");
            Console.WriteLine("Изменить мероприятие - 4");

            Console.WriteLine("Отсортировать по полю - 5");
            Console.WriteLine("Импорт записей из файла - 6");
            Console.WriteLine("Импорт по диапазону дат - 7");
            Console.WriteLine("Экспортировать в файл - 8");
            Console.WriteLine("Выход - 9");
            
            Console.ForegroundColor = ConsoleColor.White;

            string userInput = Console.ReadLine();
            
            switch (userInput) {
                case "1":
                    UserAbilities.PrintDBToConsole();
                    break;
                case "2":
                    UserAbilities.CreateEvent();
                    break;
                case "3":
                    UserAbilities.RemoveEvent();
                    break;
                case "4":
                    UserAbilities.ChangeEvent();
                    break;
                case "5":
                    UserAbilities.SortByField();
                    break;
                case "6":
                    UserAbilities.ImportFromFile();
                    break;
                case "7":
                    UserAbilities.ImportByDateRange();
                    break;
                case "8":
                    UserAbilities.ExportTo();
                    break;
                case "9":
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nОшибка ввода, попробуйте ещё раз\n");
                    
                    ShowMainMenu();
                    break;
            }
        }
    }
}
