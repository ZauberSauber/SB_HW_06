using System;

namespace Homework_07 {
    struct DailyPlannerItem {
        public DailyPlannerItem(string title, string description, DateTime date, string members, int importance) {
            _title = title;
            _description = description;
            _date = date;
            _members = members;
            _importance = importance;
            
            _constans = new Constans();
        }
        
        /// <summary>
        /// Возвращает запись в строковом формате
        /// </summary>
        /// <returns></returns>
        public string Print(int index) {
            string dateString = _date.ToString("dd MM yyyy");
            string importanceString = ImportanceToString(_importance);
            
            return $"{index, 5} {_title, 15} {_description, 15} {dateString, 15} {_members, 15} {importanceString, 15}";
        }
        
        /// <summary>
        /// Название мероприятия
        /// </summary>
        public string Title {
            get { return _title; }
            set {}
        }

        /// <summary>
        /// Описание мероприятия
        /// </summary>
        public string Description {
            get { return _description; }
            set {}
        }

        /// <summary>
        /// Дата проведения мероприятия
        /// </summary>
        public DateTime Date {
            get { return _date; }
            set {}
        }

        /// <summary>
        /// Участники мероприятия
        /// </summary>
        public string Members {
            get { return _members; }
            set {}
        }

        /// <summary>
        /// Важность мероприятия
        /// </summary>
        public int Importance {
            get { return _importance; }
            set {}
        }

        private string _title;

        private string _description;

        private DateTime _date;

        private string _members;

        private int _importance;

        
        private Constans _constans;
        
        /// <summary>
        /// Конвертирует числовое представление поля "важность" к строковому названию
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ImportanceToString(int value) {
            return _constans.ImportanceNames[value];
        }
    }
}