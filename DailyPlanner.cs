using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;


namespace Homework_07 {
    struct DailyPlanner {
        /// <summary>
        /// Константы проекта
        /// </summary>
        private Constans _constans;
        
        /// <summary>
        /// Список дел
        /// </summary>
        private List<DailyPlannerItem> _dailyPlans;
        
        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string _path;

        /// <summary>
        /// Массив заголовков к столбцам списка дел
        /// </summary>
        private string[] _titles;

        private string[] _importanceNames;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path"></param>
        public DailyPlanner(string path) {
            _constans = new Constans();
            _path = path;
            _titles = _constans.Titles;
            _importanceNames = _constans.ImportanceNames;
            _dailyPlans = new List<DailyPlannerItem>();
            
            Load();
        }

        /// <summary>
        /// Создаёт новый файл, если указанный отсутствует
        /// </summary>
        private void CreateFile() {
            string temp = String.Format("{0},{1},{2},{3},{4}",
                _titles[0],
                _titles[1],
                _titles[2],
                _titles[3],
                _titles[4]);
            
            File.AppendAllText(_path, $"{temp}\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nСоздан новый файл\n");
            Console.ForegroundColor = ConsoleColor.White;
            
            Load();
        }
        
        /// <summary>
        /// Загружает данные из файла
        /// </summary>
        private void Load() {
            string line;
            string format;
            string dateString;
            DateTime date;
            
            try {
                using (StreamReader streamReader = new StreamReader(_path)) {
                    line = streamReader.ReadLine();

                    // получение заголовков
                    if (line != null) {
                        _titles = line.Split(',');
                    } else {
                        streamReader.Close();
                        CreateFile();
                        return;
                    }

                    while (!streamReader.EndOfStream) {
                        string[] args = streamReader.ReadLine().Split(',');

                        dateString = String.Join(" ", args[2].Split(' '));
                        
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        format = "dd MM yyyy";
                        
                        try {
                            date = DateTime.ParseExact(dateString, format, provider);
                        }
                        catch (Exception e) {
                            DateTime today = DateTime.Today;
                            dateString = $"{today.Day} {today.Month} {today.Year}";
                            date = DateTime.ParseExact(dateString, format, provider);
                        }

                        int test;
                        if (!int.TryParse(args[4], out test)) {
                            test = 0;
                        }

                        AddEvent(new DailyPlannerItem(args[0], args[1], date, args[3], test));
                    }
                }
            }
            catch (Exception e) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Стандартный файл с данными не обнаружен, будет создан новый.\n");
                Console.ForegroundColor = ConsoleColor.White;
                
                CreateFile();
            }
        }

        /// <summary>
        /// Загружает данные из указанного файла
        /// </summary>
        /// <param name="filePath"></param>
        private void Load(string filePath) {
            string format;
            string dateString;
            DateTime date;
            
            using (StreamReader streamReader = new StreamReader(filePath)) {
                streamReader.ReadLine();

                while (!streamReader.EndOfStream) {
                    string[] args = streamReader.ReadLine().Split(',');

                    dateString = String.Join(" ", args[2].Split(' '));
                        
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    format = "dd MM yyyy";
                        
                    try {
                        date = DateTime.ParseExact(dateString, format, provider);
                    }
                    catch (Exception e) {
                        DateTime today = DateTime.Today;
                        dateString = $"{today.Day} {today.Month} {today.Year}";
                        date = DateTime.ParseExact(dateString, format, provider);
                    }
                    
                    int test;
                    if (!int.TryParse(args[4], out test)) {
                        test = 0;
                    }

                    AddEvent(new DailyPlannerItem(args[0], args[1], date, args[3], test));
                }
            }
        }

        /// <summary>
        /// Загружает данные из указанного файла по указанному диапазону дат
        /// </summary>
        /// 
        /// <param name="filePath"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        private void Load(string filePath, DateTime dateFrom, DateTime dateTo) {
            List<DailyPlannerItem> tempList = new List<DailyPlannerItem>();
            string format;
            string dateString;
            DateTime date;

            using (StreamReader streamReader = new StreamReader(filePath)) {
                streamReader.ReadLine();

                while (!streamReader.EndOfStream) {
                    string[] args = streamReader.ReadLine().Split(',');

                    dateString = String.Join(" ", args[2].Split(' '));
                        
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    format = "dd MM yyyy";
                        
                    try {
                        date = DateTime.ParseExact(dateString, format, provider);
                    }
                    catch (Exception e) {
                        DateTime today = DateTime.Today;
                        dateString = $"{today.Day} {today.Month} {today.Year}";
                        date = DateTime.ParseExact(dateString, format, provider);
                    }
                    
                    int test;
                    if (!int.TryParse(args[4], out test)) {
                        test = 0;
                    }

                    tempList.Add(new DailyPlannerItem(args[0], args[1], date, args[3], test));
                }
            }

            // берём из списка только входящие в диапзон
            tempList = tempList.FindAll(IsInDateRange(dateFrom, dateTo));

            _dailyPlans.AddRange(tempList);
        }

        /// <summary>
        /// Фильтрует список по датам
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private Predicate<DailyPlannerItem> IsInDateRange(DateTime fromDate, DateTime toDate) {
            return delegate(DailyPlannerItem item) {
                if (item.Date >= fromDate && item.Date <= toDate) {
                    return true;
                }

                return false;
            };
        }

        /// <summary>
        /// Записывает данные в файл
        /// </summary>
        /// <param name="path"></param>
        private void WriteToFile(string path) {
            File.Delete(path);
            
            // запись заголовков
            string temp = String.Format("{0},{1},{2},{3},{4}",
                _titles[0],
                _titles[1],
                _titles[2],
                _titles[3],
                _titles[4]);
            
            File.AppendAllText(path, $"{temp}\n");

            for (int i = 0; i < _dailyPlans.Count; i++) {
                temp = String.Format("{0},{1},{2:dd MM yyyy},{3},{4}",
                    StringToCSVCell(_dailyPlans[i].Title),
                    StringToCSVCell(_dailyPlans[i].Description),
                    _dailyPlans[i].Date,
                    StringToCSVCell(_dailyPlans[i].Members),
                    _dailyPlans[i].Importance);
                
                File.AppendAllText(path, $"{temp}\n");
            }
        }
        
        
        /// <summary>
        /// Записывает список в файл
        /// </summary>
        public void Save() {
            WriteToFile(_path);
        }
        
        /// <summary>
        /// Записывает список в указанный файл
        /// </summary>
        public void Save(string name) {
            string fileName = $@"{name}.csv";
            
            WriteToFile(fileName);
        }

        public void PrintDbToConsole() {
            Console.WriteLine($"{"№", 5} {_titles[0], 15} {_titles[1], 15} {_titles[2], 15} {_titles[3], 15} {_titles[4], 15}");

            for (int i = 0; i < _dailyPlans.Count; i++) {
                Console.WriteLine(_dailyPlans[i].Print(i));
            }
        }
        
        public void AddEvent(DailyPlannerItem item) {
            _dailyPlans.Add(item);
        }

        public void RemoveEvent(int removeIndex) {
            _dailyPlans.RemoveAt(removeIndex);
        }

        public void EditEvent(int index, DailyPlannerItem editedEvent) {
            _dailyPlans[index] = editedEvent;
        }

        #region Методы сортировки
        /// <summary>
        /// Сортирует список по имени
        /// </summary>
        public void SortByName() {
            _dailyPlans.Sort((x, y) => String.Compare(x.Title, y.Title, StringComparison.Ordinal));
        }

        /// <summary>
        /// сортирует список по описанию
        /// </summary>
        public void SortByDescription() {
            _dailyPlans.Sort((x, y) => String.Compare(x.Description, y.Description, StringComparison.Ordinal));
        }

        /// <summary>
        /// Сортирует список по дате
        /// </summary>
        public void SortByDate() {
            _dailyPlans.Sort((x, y) => x.Date.CompareTo(y.Date));
        }
        
        /// <summary>
        /// Сортирует список по участникам
        /// </summary>
        public void SortByMembers() {
            _dailyPlans.Sort((x, y) => String.Compare(x.Members, y.Members, StringComparison.Ordinal));
        }

        /// <summary>
        /// Сортирует список по важности
        /// </summary>
        public void SortByImportance() {
            _dailyPlans.Sort((x, y) => x.Importance.CompareTo(y.Importance));
        }
        #endregion
        

        /// <summary>
        /// Импортирует все данные из файла
        /// </summary>
        /// <param name="filePath"></param>
        public void ImportFrom(string filePath) {
            Load(filePath);
        }

        /// <summary>
        /// Импортирует данные из указанного файла по указанному диапазону
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public void ImportByDateRange(string filePath, DateTime dateFrom, DateTime dateTo) {
            Load(filePath, dateFrom, dateTo);
        }
        
        

        /// <summary>
        /// Возращает кол-во записей в ежедневинке
        /// </summary>
        /// <returns></returns>
        public int GetEventsCount() {
            return _dailyPlans.Count;
        }

        /// <summary>
        /// Возвращает массив заголовком
        /// </summary>
        /// <returns></returns>
        public string[] GetTitles() {
            return _titles;
        }
        
        /// <summary>
        /// Экранирует символы строки для сохранения в 1 ячейке csv файла
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string StringToCSVCell(string str) {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            
            if (mustQuote) {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                
                foreach (char nextChar in str) {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                
                return sb.ToString();
            }

            return str;
        }
    }
}