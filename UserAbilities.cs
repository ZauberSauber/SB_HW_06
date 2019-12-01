using System;
using System.Globalization;
using System.IO;

namespace Homework_07 {
    class UserAbilities {
        /// <summary>
        /// Создание события в ежедневнике
        /// </summary>
        public void CreateEvent() {
            DailyPlannerItem newEvent = CreateEventData();
            
            Program.DailyPlanner.AddEvent(newEvent);
            Program.DailyPlanner.Save();
            Program.ShowMainMenu();
        }

        /// <summary>
        /// Удаляет событие в ежедневнике 
        /// </summary>
        public void RemoveEvent() {
            int removeIndex;
            
            Console.WriteLine("Укажите номер события, которое требуется удалить");
            removeIndex = isItemInArray();
            
            Program.DailyPlanner.RemoveEvent(removeIndex);
            
            Console.WriteLine($"Событие {removeIndex} успешно удалено");

            Program.DailyPlanner.Save();
            Program.ShowMainMenu();
        }

        /// <summary>
        /// Изменяет событие в ежедневнике
        /// </summary>
        public void ChangeEvent() {
            int changeIndex;
            DailyPlannerItem newEvent;
            
            Console.WriteLine("Укажите номер события, которое требуется изменить");
            changeIndex = isItemInArray();
            
            newEvent = CreateEventData();

            Program.DailyPlanner.EditEvent(changeIndex, newEvent);

            Console.WriteLine($"Событие {changeIndex} успешно изменено");
            
            Program.DailyPlanner.Save();
            Program.ShowMainMenu();
        }

        /// <summary>
        /// Сортирует события по указанному полю
        /// </summary>
        public void SortByField() {
            // список заголовков
            string[] titles = Program.DailyPlanner.GetTitles();
            
            Console.WriteLine("\nПо какому полю нужно отсортировать?\n");

            for (int i = 0; i < titles.Length; i++) {
                Console.WriteLine($"{i+1} - {titles[i]}");
            }

            string userInput = Console.ReadLine();

            switch (userInput) {
                case "1":
                    Program.DailyPlanner.SortByName();
                    break;
                case "2" :
                    Program.DailyPlanner.SortByDescription();
                    break;
                case "3":
                    Program.DailyPlanner.SortByDate();
                    break;
                case "4":
                    Program.DailyPlanner.SortByMembers();
                    break;
                case "5":
                    Program.DailyPlanner.SortByImportance();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Некорректный ввод");
                    Console.ForegroundColor = ConsoleColor.White;
                    SortByField();
                    break;
            }
            
            Program.DailyPlanner.Save();
            Program.ShowMainMenu();
        }

        /// <summary>
        /// Выводит список в консоль в виде таблицы
        /// </summary>
        public void PrintDBToConsole() {
            Program.DailyPlanner.PrintDbToConsole();
            Program.DailyPlanner.Save();
            Program.ShowMainMenu();
        }

        /// <summary>
        /// Запрашивает имя файла и импортирует из него данные, если файл существует
        /// </summary>
        public void ImportFromFile() {
            Console.WriteLine("Укажите название файла для импорта данных без расширения");

            string fileName = Console.ReadLine();
            bool isFileExist = File.Exists($@"{fileName}.csv");

            if (isFileExist) {
                Program.DailyPlanner.ImportFrom(fileName);
            } else {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nФайл не найден\n");
                Console.ForegroundColor = ConsoleColor.White;
                
                ImportFromFile();
            }
            
            Console.WriteLine("\nИмпорт успешно завершён\n");
            
            Program.DailyPlanner.Save();
            Program.ShowMainMenu();
        }

        /// <summary>
        /// Импортирует записи по указанному диапазону дат из указанного файла
        /// </summary>
        public void ImportByDateRange() {
            Console.WriteLine("Укажите название файла для импорта данных без расширения");
            
            string fileName = Console.ReadLine();
            bool isFileExist = File.Exists($@"{fileName}.csv");
            
            Console.WriteLine("Укажите с какой даты начинать");
            DateTime dateFrom = CreateUserDate();
            
            Console.WriteLine("Укажите какой датой закончить");
            DateTime dateTo = CreateUserDate();

            // поменять местами, если дата начала меньше даты конца
            if (dateFrom > dateTo) {
                DateTime temp = dateFrom;
                dateFrom = dateTo;
                dateTo = temp;
            }
            
            if (isFileExist) {
                Program.DailyPlanner.ImportByDateRange($@"{fileName}.csv", dateFrom, dateTo);
            } else {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nФайл не найден\n");
                Console.ForegroundColor = ConsoleColor.White;
                
                ImportByDateRange();
            }
            
            Console.WriteLine("\nИмпорт записей успешно завершён\n");
            
            Program.DailyPlanner.Save();
            Program.ShowMainMenu();
        }

        /// <summary>
        /// Пользовательский метод сохранения в указанный файл
        /// </summary>
        public void ExportTo() {
            Console.WriteLine("Укажите название файла");

            string userInput = Console.ReadLine();
            
            Program.DailyPlanner.Save(userInput);
            Program.ShowMainMenu();
        }
        
        
        /// <summary>
        /// Проверяет ввод пользователя на преобразование к числу
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="isNegativAllowed"></param>
        /// <returns></returns>
        public object ParseUserInput(string userInput = "1", bool isNegativAllowed = false) {
            bool isSuccess = Int32.TryParse(userInput, out int result);

            if (!isSuccess) {
                return null;
            }

            if (!isNegativAllowed) {
                result = Math.Abs(result);
            }
            
            return result;
        }
        
        /// <summary>
        /// Проверяет ввод пользователя для создания даты
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private int CheckValueForUserDate(string valueName, int minValue, int maxValue) {
            object value;
            int convertedValue;
            
            Console.WriteLine($"Какой {valueName}?");
            value = ParseUserInput(Console.ReadLine());

            if (value == null) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Неверный формат, укажите {valueName} заново\n");
                Console.ForegroundColor = ConsoleColor.White;

                CheckValueForUserDate(valueName, minValue, maxValue);
            } else {
               convertedValue = Convert.ToInt32(value.ToString());
               
               if (convertedValue < minValue || convertedValue > maxValue) {
                   Console.ForegroundColor = ConsoleColor.DarkRed;
                   Console.WriteLine($"Указанное значение слишком мало или велико, укажите {valueName} заново\n");
                   Console.ForegroundColor = ConsoleColor.White;
                                              
                   CheckValueForUserDate(valueName, minValue, maxValue);
               }      
               return convertedValue;           
            }
            return minValue;
        }
        
        /// <summary>
        /// Опрашивает пользователя и создаёт дату
        /// </summary>
        /// <returns></returns>
        private DateTime CreateUserDate() {
            string dateString;
            int daysInMonth;
            string dateDay;
            string dateMonth;
            int dateYear;
            string format;

            dateYear = CheckValueForUserDate("год", 1, 9999);
            
            dateMonth = CheckValueForUserDate("месяц", 1, 12).ToString();
            daysInMonth = DateTime.DaysInMonth(dateYear, Convert.ToInt32(dateMonth));
            dateMonth = ConvertToTwoDigit(dateMonth);

            dateDay = CheckValueForUserDate("день", 1, daysInMonth).ToString();
            dateDay = ConvertToTwoDigit(dateDay);
            
            dateString = $"{dateDay} {dateMonth} {dateYear}";
            format = "dd MM yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;

            return DateTime.ParseExact(dateString, format, provider);
        }


        /// <summary>
        /// Опрашивает пользователя и создаёт данные для события
        /// </summary>
        /// <returns></returns>
        private DailyPlannerItem CreateEventData() {
            string title;
            string description;
            DateTime date;
            string members;
            int importance;
            
            Console.WriteLine("Укажите название мероприятия");
            title = Console.ReadLine();
            
            Console.WriteLine("Укажите описание мероприятия");
            description = Console.ReadLine();
            
            date = CreateUserDate();

            Console.WriteLine("Укажите участников");
            members = Console.ReadLine();
            
            Console.WriteLine("Оцените важность мероприятия от 0 до 2");
            importance = NormalizeImportance(Console.ReadLine());

            return new DailyPlannerItem(title, description, date, members, importance);
        }


        /// <summary>
        /// Проверяет пользовательский ввод при создании поля "важность"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int NormalizeImportance(string value) {
            int importance;
            object test = ParseUserInput(value);

            if (test == null) {
                importance = 0;
            }
            else {
                importance = Convert.ToInt32(test);
            }

            if (importance < 0 || importance > 2) {
                importance = 0;
            }
            
            return importance;
        }
        

        /// <summary>
        /// Проверяет ввод пользователя на число и вхождение в список
        /// </summary>
        /// <returns></returns>
        private int isItemInArray() {
            int index;
            int eventsCount = Program.DailyPlanner.GetEventsCount();
            object test = ParseUserInput(Console.ReadLine());

            if (test == null) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Неверный формат, возврат в главное меню\n");
                Console.ForegroundColor = ConsoleColor.White;
                
                Program.ShowMainMenu();
            }
            
            index = Convert.ToInt32(test);

            if (index > eventsCount) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Указанный номер события не найден, возврат в главное меню\n");
                Console.ForegroundColor = ConsoleColor.White;

                Program.ShowMainMenu();                
            }

            return index;
        }

        /// <summary>
        /// Преобразует к двузначному виду
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ConvertToTwoDigit(string value) {
            if (value.Length < 2) {
                value = "0"+ value;
            }

            return value;
        }
    }
}