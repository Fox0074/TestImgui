using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ClickableTransparentOverlay
{
    public enum Language { None, Ru, En }
    public class T
    {
        public static Language CurrentLang = Language.Ru;
        private static List<Phrase> _phrases;
        private static List<Phrase> Phrases
        {
            get
            {
                if (_phrases == null || _phrases?.Count == 0)
                    _phrases = LoadData();

                return _phrases;
            }
            set { _phrases = value; }
        }

        private class Phrase
        {
            public string Ru;
            public string En;
        }

        public static string GetString(string key, string input)
        {
            //var inputB = Encoding.Unicode.GetBytes(input);
            //var inputK = Encoding.Default.GetBytes(key);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < inputB.Length; i++)
            //    sb.Append((char)(inputB[i] ^ inputK[(i % inputK.Length)]));
            //String result = sb.ToString();

            return GetString(input);
        }


        public static string GetString(string key, byte[] input)
        {
            var inputK = Encoding.Default.GetBytes(key);
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < input.Length; i++)
            {
                var t1 = input[i];
                var t2 = inputK[(i % inputK.Length)];
                bytes.Add((byte)(t1 ^ t2));
            }

            var ruString = Encoding.UTF8.GetString(bytes.ToArray());
            return GetString(ruString);
        }



        public static string GetString(string ru)
        {
            var phrase = Phrases.FirstOrDefault(x => x.Ru == ru);

            if (phrase == null)
                return ru;

            switch (CurrentLang)
            {
                case Language.Ru:
                    return ru;

                case Language.En:
                    return phrase.En;

                default:
                    return phrase.Ru;
            }
        }

        private static List<Phrase> LoadData()
        {
            using (FileStream fs = new FileStream("LangConf", FileMode.OpenOrCreate))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                var loadedData = FromJson<List<Phrase>>(buffer);
                if (loadedData == null)
                    return new List<Phrase>();
                return loadedData;
            }
        }

        private static T FromJson<T>(byte[] data)
        {
            var jsonData = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

#if DEBUG
        public static void SaveData()
        {
            using (FileStream fs = new FileStream("LangConf", FileMode.Create))
            {
                var jsonMessage = JsonConvert.SerializeObject(Phrases2);
                var byteData = Encoding.UTF8.GetBytes(jsonMessage);
                fs.Write(byteData, 0, byteData.Length);
            }
        }

        private static List<Phrase> Phrases2 = new List<Phrase>()
        {
            //Tundra
            new Phrase() { Ru = "Авторизация", En = "Authorization"},
            new Phrase() { Ru = "Ключ", En = "Key"},
            new Phrase() { Ru = "Старт", En = "Start"},
            new Phrase() { Ru = "Функции", En = "Functions"},
            new Phrase() { Ru = "Рисовать коробки", En = "Draw Boxes"},
            new Phrase() { Ru = "Рисовать орудие", En = "Draw Weapon"},
            new Phrase() { Ru = "Точка упреждения", En = "Lead point"},
            new Phrase() { Ru = "Ники", En = "Show Nicks"},
            new Phrase() { Ru = "Счетчик врагов", En = "Show enemy counter"},
            new Phrase() { Ru = "Изменить кратность прицела", En = "Change multiply sight"},
            new Phrase() { Ru = "Ракетный прицел", En = "Rocket crossHair"},
            new Phrase() { Ru = "Аркадный прицел", En = "Arcade crossHair"},
            new Phrase() { Ru = "Захват самолетов", En = "Ground to Air Hook"},
            new Phrase() { Ru = "Вид от 3-го лица", En = "ThirstPP (only simulator)"},
            new Phrase() { Ru = "Стрелки на врагов", En = "Arrows"},
            new Phrase() { Ru = "Индикатор самолетов", En = "Plane indicator"},
            new Phrase() { Ru = "Дистарция в прицеле", En = "Distance in the scope"},
            new Phrase() { Ru = "Подсветка контура", En = "Outline Glow"},
            new Phrase() { Ru = "Бомбовый прицел", En = "Bobm Ballistic"},
            new Phrase() { Ru = "Мин. зум", En = "Min. sight zoom"},
            new Phrase() { Ru = "Макс. зум", En = "Max. sight zoom"},
            new Phrase() { Ru = "Затенение в прицеле", En = "Sight shadow"},
            new Phrase() { Ru = "Радар", En = "Draw Radar"},
            new Phrase() { Ru = "Масштаб радара", En = "Radar scale"},
            new Phrase() { Ru = "Цвета", En = "Colors"},
            new Phrase() { Ru = "Танки", En = "Tanks"},
            new Phrase() { Ru = "САУ", En = "TankDestroyer"},
            new Phrase() { Ru = "Тяжи", En = "Heavy T."},
            new Phrase() { Ru = "Зенитки", En = "SPAA"},
            new Phrase() { Ru = "Самолеты", En = "Plane"},
            new Phrase() { Ru = "Бомбардировщики", En = "Bombers"},
            new Phrase() { Ru = "Штурмовики", En = "Strikers"},
            new Phrase() { Ru = "Вертолеты", En = "Helicopters"},
            new Phrase() { Ru = "Остальное", En = "Other"},
            new Phrase() { Ru = "Точка упреждения", En = "Lead point color"},
            new Phrase() { Ru = "Настройки", En = "Settings"},
            new Phrase() { Ru = "Позиция радара X", En = "Radar position X"},
            new Phrase() { Ru = "Позиция радара Y", En = "Radar position Y"},
            new Phrase() { Ru = "Позиция счетчика врагов", En = "Enemy counters position"},
            new Phrase() { Ru = "Показывать отладку", En = "Show debug"},
            new Phrase() { Ru = "Размер шрифта", En = "Font size"},
            new Phrase() { Ru = "Инфо", En = "Info"},
            new Phrase() { Ru = "Время действия ключа: ", En = "Key validity period: "},
            new Phrase() { Ru = "Версия программы: ", En = "Program version: "},
            new Phrase() { Ru = "Позиция: ", En = "Position: "},
            new Phrase() { Ru = "Скорость: ", En = "Speed: "},
            new Phrase() { Ru = "Настройки", En = "Settings"},


            //Tarkov
            new Phrase() {Ru = "Лут", En = "Loot"},
            new Phrase() {Ru = "Дикий", En = "Scav"},
            new Phrase() {Ru = "Редкий лут", En = "Rare loot"},
            new Phrase() {Ru = "Трупы", En = "Corpses"},
            new Phrase() {Ru = "Бот", En = "Bot"},
            new Phrase() {Ru = "Босы", En = "Bosses"},
            new Phrase() {Ru = "Свита", En = "Entourage"},
            new Phrase() {Ru = "Другое", En = "Other"},
            new Phrase() {Ru = "Радиус захвата (Aim FOV)", En = "Aim FOV"},
            new Phrase() {Ru = "Кнопка для аима", En = "Aim button"},
            new Phrase() {Ru = "Свое смещение прицела", En = "Custom sight offset"},
            new Phrase() {Ru = "Свой", En = "Own"},
            new Phrase() {Ru = "Голова", En = "Head"},
            new Phrase() {Ru = "П.Рука", En = "R.Hand"},
            new Phrase() {Ru = "Л.Рука", En = "L.Hand"},
            new Phrase() {Ru = "Л.Нога", En = "L.Leg"},
            new Phrase() {Ru = "П.Нога", En = "R.Leg"},
            new Phrase() {Ru = "Грудь", En = "Chest"},
            new Phrase() {Ru = "Живот", En = "Belly"},
            new Phrase() {Ru = "Аимбот", En = "Aimbot"},
            new Phrase() {Ru = "Аим", En = "Aim"},
            new Phrase() {Ru = "Точка по центру экрана", En = "Displayed crosshair"},
            new Phrase() {Ru = "Бесконечная выносливость", En = "Infinity stamina"},
            new Phrase() {Ru = "Антиотдача", En = "Antirecoil"},
            new Phrase() {Ru = "Разное", En = "Misc"},
            new Phrase() {Ru = "Дата регистрации", En = "Reg date"},
            new Phrase() {Ru = "Уровень", En = "Level"},
            new Phrase() {Ru = "ID Команды", En = "Command ID"},
            new Phrase() {Ru = "Жизни (HP)", En = "HP"},
            new Phrase() {Ru = "Масштаб радара", En = "Radar scale"},
            new Phrase() {Ru = "Радар", En = "Radar"},
            new Phrase() {Ru = "Ник", En = "Nicks"},
            new Phrase() {Ru = "Рамка", En = "Frame"},
            new Phrase() {Ru = "Скелет", En = "Skilet"},
            new Phrase() {Ru = "Дистанция отрисовки ", En = "Darw distance"},
            new Phrase() {Ru = "ЕСП", En = "ESP"},
            new Phrase() {Ru = "Показывать лут", En = "Draw Loot"},
            new Phrase() {Ru = "Минимальная цена (тыс.)", En = "Min loot price"},
            new Phrase() {Ru = "Отображать предметы без цены", En = "Display items without a price"},
            new Phrase() {Ru = "Отображать контейнеры", En = "Display containers"},
            new Phrase() {Ru = "Фильтр лута", En = "Loot filter"},

            //Dayz
            new Phrase() {Ru = "Электрозаводск", En = "Electro"},
            new Phrase() {Ru = "Черногорск", En = "Chernogorsk"},
            new Phrase() {Ru = "Новоселки", En = "Novoselky"},
            new Phrase() {Ru = "Дубово", En = "Dubovo"},
            new Phrase() {Ru = "Высотово", En = "Vyssotovo"},
            new Phrase() {Ru = "Зеленогорск", En = "Zelenogorsk"},
            new Phrase() {Ru = "Солнечный", En = "Solnechny"},
            new Phrase() {Ru = "Поляна", En = "Polyana"},
            new Phrase() {Ru = "Горка", En = "Gorka"},
            new Phrase() {Ru = "Старый собор", En = "Stariy sobor"},
            new Phrase() {Ru = "Выбор", En = "Vibor"},
            new Phrase() {Ru = "Новая петровка", En = "Novaya Petrovka"},
            new Phrase() {Ru = "Североград", En = "Severograd"},
            new Phrase() {Ru = "Красностав", En = "Krasnostav"},
            new Phrase() {Ru = "Светлоярск", En = "Svetloyarsk"},
            new Phrase() {Ru = "Черная поляна", En = "Chernaya polyana"},
            new Phrase() {Ru = "Новодмитровск", En = "Novodmmitrovsk"},
            new Phrase() {Ru = "Тисы", En = "Tisy"},
            new Phrase() {Ru = "Троицкое", En = "Troickoe"},
            new Phrase() {Ru = "Аэропорт", En = "Air port"},
            new Phrase() {Ru = "Отображать NULL", En = "Show NULL"},

            new Phrase() {Ru = "Города", En = "Cities"},
            new Phrase() {Ru = "Предмет в руках", En = "Item in hand"},
            new Phrase() {Ru = "Кемпинг", En = "Camping"},
            new Phrase() {Ru = "Еда", En = "Food"},
            new Phrase() {Ru = "Контейнеры", En = "Containers"},
            new Phrase() {Ru = "Инструменты", En = "Tools"},
            new Phrase() {Ru = "Оружие", En = "Weapon"},
            new Phrase() {Ru = "Броня", En = "Armor"},
            new Phrase() {Ru = "Одежда", En = "Clothes"},
            new Phrase() {Ru = "Расходники", En = "Consumables"},
            new Phrase() {Ru = "Медицина", En = "Medical"},
            new Phrase() {Ru = "Машины", En = "Car"},
            new Phrase() {Ru = "Авт. запчасти", En = "Vehicles"},
            new Phrase() {Ru = "Игроки", En = "Players"},
            new Phrase() {Ru = "Зомби", En = "Zombie"},
            new Phrase() {Ru = "Животные", En = "Animals"},
            new Phrase() {Ru = "Все", En = "All"},
            new Phrase() {Ru = "Дистанция отрисовки сущностей", En = "Entities range"},
            new Phrase() {Ru = "Дистанция отрисовки лута", En = "Loot range"},
            new Phrase() {Ru = "Изменять время", En = "Change time"},
            new Phrase() {Ru = "Установить время", En = "Set time"},
            new Phrase() {Ru = "Отключить траву", En = "Disable grass"},
            new Phrase() {Ru = "Ручной фильтр лута", En = "Custom loot filter"},
            new Phrase() {Ru = "Сохранение позиции", En = "Save position"},
            new Phrase() {Ru = "Сохраненные позиции", En = "Saved positions"},
            new Phrase() {Ru = "Друзья", En = "Friends"},
            new Phrase() {Ru = "Координаты", En = "Coordinate"},
            new Phrase() {Ru = "Название", En = "Name"},
            new Phrase() {Ru = "Сохранить", En = "Save"},
            new Phrase() {Ru = "Удалить", En = "Delete"},
            new Phrase() {Ru = "Сохранить из текущей позиции", En = "Save from my point"},
            new Phrase() {Ru = "Включить", En = "Enable"},
            new Phrase() {Ru = "ПКМ", En = "RMB"},
            new Phrase() {Ru = "ЛКМ", En = "LMB"},
            new Phrase() {Ru = "Радиус", En = "Radius"},
            new Phrase() {Ru = "Тень под текстом", En = "Text shadow"},
            new Phrase() {Ru = "Список друзей", En = "Friend list"},
            new Phrase() {Ru = "Горячие клавиши:", En = "Hotkeys"},
            new Phrase() {Ru = "Ctrl + F: добавить в друзья", En = "Ctrl + F: add to ftiends"},
            new Phrase() {Ru = "Shift + СКМ: показать города", En = "Shift + MMB: show cities"}

        };
#endif

    }
}
