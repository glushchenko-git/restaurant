using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        RestaurantSystem.InitializeSampleData();

        while (true)
        {
            Console.WriteLine("\n=== РЕСТОРАН 'DELUXE' ===");
            Console.WriteLine("1. Система бронирования");
            Console.WriteLine("2. Система заказов");
            Console.WriteLine("3. Статистика");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите раздел: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": ShowReservationSystem(); break;
                case "2": ShowOrderSystem(); break;
                case "3": ShowStatistics(); break;
                case "0": return;
                default: Console.WriteLine("Неверный ввод."); break;
            }
        }
    }

    static void ShowReservationSystem()
    {
        while (true)
        {
            Console.WriteLine("\n=== СИСТЕМА БРОНИРОВАНИЯ ===");
            Console.WriteLine("1. Создать стол");
            Console.WriteLine("2. Создать бронирование");
            Console.WriteLine("3. Редактировать стол");
            Console.WriteLine("4. Информация о столе");
            Console.WriteLine("5. Доступные столы");
            Console.WriteLine("6. Все бронирования");
            Console.WriteLine("7. Поиск бронирования");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": CreateTable(); break;
                case "2": CreateReservation(); break;
                case "3": EditTable(); break;
                case "4": ShowTableInfo(); break;
                case "5": ShowAvailableTables(); break;
                case "6": ShowAllReservations(); break;
                case "7": FindReservation(); break;
                case "0": return;
                default: Console.WriteLine("Неверный ввод."); break;
            }
        }
    }

    static void CreateTable()
    {
        Console.Write("ID стола: ");
        var id = int.Parse(Console.ReadLine());
        Console.Write("Расположение: ");
        var location = Console.ReadLine();
        Console.Write("Количество мест: ");
        var seats = int.Parse(Console.ReadLine());
        RestaurantSystem.Tables.Add(new Table(id, location, seats));
        Console.WriteLine("Стол создан.");
    }

    static void CreateReservation()
    {
        Console.Write("Имя клиента: ");
        var name = Console.ReadLine();
        Console.Write("Телефон: ");
        var phone = Console.ReadLine();
        Console.Write("Время начала (гггг-мм-дд чч:мм): ");
        var start = DateTime.Parse(Console.ReadLine());
        Console.Write("Время окончания (гггг-мм-дд чч:мм): ");
        var end = DateTime.Parse(Console.ReadLine());
        Console.Write("Комментарий: ");
        var comment = Console.ReadLine();

        var availableTables = RestaurantSystem.Tables.Where(t =>
        {
            for (var time = start; time < end; time = time.AddHours(1))
            {
                if (t.Schedule.ContainsKey(time)) return false;
            }
            return true;
        }).ToList();

        if (!availableTables.Any())
        {
            Console.WriteLine("Нет доступных столов.");
            return;
        }

        Console.WriteLine("Доступные столы:");
        foreach (var t in availableTables)
            Console.WriteLine($"ID: {t.Id}, Мест: {t.Seats}, Расположение: {t.Location}");

        Console.Write("Выберите ID стола: ");
        var tableId = int.Parse(Console.ReadLine());
        var table = availableTables.First(t => t.Id == tableId);

        var reservation = new Reservation(RestaurantSystem.ReservationIdCounter++, name, phone, start, end, comment, table);
        RestaurantSystem.Reservations.Add(reservation);
        Console.WriteLine("Бронирование создано.");
    }

    static void EditTable()
    {
        Console.Write("ID стола для редактирования: ");
        var id = int.Parse(Console.ReadLine());
        var table = RestaurantSystem.Tables.FirstOrDefault(t => t.Id == id);
        if (table == null)
        {
            Console.WriteLine("Стол не найден.");
            return;
        }

        if (table.Schedule.Any())
        {
            Console.WriteLine("Редактирование невозможно: стол занят.");
            return;
        }

        Console.Write("Новое расположение: ");
        table.Location = Console.ReadLine();
        Console.Write("Новое количество мест: ");
        table.Seats = int.Parse(Console.ReadLine());
        Console.WriteLine("Информация обновлена.");
    }

    static void ShowTableInfo()
    {
        Console.Write("ID стола: ");
        var id = int.Parse(Console.ReadLine());
        var table = RestaurantSystem.Tables.FirstOrDefault(t => t.Id == id);
        if (table == null)
        {
            Console.WriteLine("Стол не найден.");
            return;
        }
        table.PrintInfo();
    }

    static void ShowAvailableTables()
    {
        Console.Write("Время начала (гггг-мм-дд чч:мм): ");
        var start = DateTime.Parse(Console.ReadLine());
        Console.Write("Время окончания (гггг-мм-дд чч:мм): ");
        var end = DateTime.Parse(Console.ReadLine());
        Console.Write("Минимальное количество мест: ");
        var minSeats = int.Parse(Console.ReadLine());

        var availableTables = RestaurantSystem.Tables.Where(t => t.Seats >= minSeats &&
            Enumerable.Range(0, (int)(end - start).TotalHours)
                .All(i => !t.Schedule.ContainsKey(start.AddHours(i))));

        Console.WriteLine("Доступные столы:");
        foreach (var t in availableTables)
            Console.WriteLine($"ID: {t.Id}, Мест: {t.Seats}, Расположение: {t.Location}");
    }

    static void ShowAllReservations()
    {
        foreach (var r in RestaurantSystem.Reservations)
        {
            Console.WriteLine($"ID: {r.Id}, Клиент: {r.ClientName}, Телефон: {r.PhoneNumber}, Стол: {r.AssignedTable.Id}, Время: {r.StartTime:dd.MM.yyyy HH:mm} - {r.EndTime:dd.MM.yyyy HH:mm}");
        }
    }

    static void FindReservation()
    {
        Console.Write("Последние 4 цифры телефона: ");
        var phoneEnd = Console.ReadLine();
        Console.Write("Имя клиента: ");
        var name = Console.ReadLine();

        var results = RestaurantSystem.Reservations.Where(r =>
            r.PhoneNumber.EndsWith(phoneEnd) &&
            r.ClientName.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (!results.Any())
        {
            Console.WriteLine("Бронирования не найдены.");
            return;
        }

        foreach (var r in results)
        {
            Console.WriteLine($"ID: {r.Id}, Клиент: {r.ClientName}, Телефон: {r.PhoneNumber}, Стол: {r.AssignedTable.Id}, Время: {r.StartTime:dd.MM.yyyy HH:mm} - {r.EndTime:dd.MM.yyyy HH:mm}");
        }
    }

    static void ShowOrderSystem()
    {
        while (true)
        {
            Console.WriteLine("\n=== СИСТЕМА ЗАКАЗОВ ===");
            Console.WriteLine("1. Создать блюдо");
            Console.WriteLine("2. Создать заказ");
            Console.WriteLine("3. Показать меню");
            Console.WriteLine("4. Информация о заказе");
            Console.WriteLine("5. Закрыть заказ");
            Console.WriteLine("6. Печать чека");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": CreateDish(); break;
                case "2": CreateOrder(); break;
                case "3": RestaurantSystem.PrintMenu(); break;
                case "4": ShowOrderInfo(); break;
                case "5": CloseOrder(); break;
                case "6": PrintReceipt(); break;
                case "0": return;
                default: Console.WriteLine("Неверный ввод."); break;
            }
        }
    }

    static void CreateDish()
    {
        Console.Write("ID блюда: ");
        var id = int.Parse(Console.ReadLine());
        Console.Write("Название: ");
        var name = Console.ReadLine();
        Console.Write("Состав: ");
        var composition = Console.ReadLine();
        Console.Write("Вес: ");
        var weight = Console.ReadLine();
        Console.Write("Цена: ");
        var price = decimal.Parse(Console.ReadLine());
        Console.Write("Категория (0-Напитки, 1-Салаты, 2-ХолодныеЗакуски, 3-ГорячиеЗакуски, 4-Супы, 5-ГорячиеБлюда, 6-Десерты): ");
        var category = (DishCategory)int.Parse(Console.ReadLine());
        Console.Write("Время готовки (мин): ");
        var cookingTime = int.Parse(Console.ReadLine());
        Console.Write("Теги (через запятую): ");
        var tags = Console.ReadLine().Split(',');

        var dish = new Dish(id, name, composition, weight, price, category, cookingTime, tags);
        RestaurantSystem.Dishes.Add(dish);
        Console.WriteLine("Блюдо создано.");
    }

    static void CreateOrder()
    {
        Console.Write("ID заказа: ");
        var id = int.Parse(Console.ReadLine());
        Console.Write("ID стола: ");
        var tableId = int.Parse(Console.ReadLine());
        Console.Write("Комментарий: ");
        var comment = Console.ReadLine();
        Console.Write("ID официанта: ");
        var waiterId = int.Parse(Console.ReadLine());

        Console.WriteLine("Доступные блюда:");
        foreach (var dish in RestaurantSystem.Dishes)
        {
            Console.WriteLine($"{dish.Id}. {dish.Name} - {dish.Price} руб.");
        }

        Console.Write("Введите ID блюд через запятую: ");
        var dishIds = Console.ReadLine().Split(',').Select(int.Parse).ToList();
        var selectedDishes = RestaurantSystem.Dishes.Where(d => dishIds.Contains(d.Id)).ToArray();

        var order = new Order(id, tableId, comment, waiterId, selectedDishes);
        RestaurantSystem.Orders.Add(order);
        Console.WriteLine("Заказ создан.");
    }

    static void ShowOrderInfo()
    {
        Console.Write("ID заказа: ");
        var id = int.Parse(Console.ReadLine());
        var order = RestaurantSystem.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            Console.WriteLine("Заказ не найден.");
            return;
        }
        order.PrintInfo();
    }

    static void CloseOrder()
    {
        Console.Write("ID заказа для закрытия: ");
        var id = int.Parse(Console.ReadLine());
        var order = RestaurantSystem.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            Console.WriteLine("Заказ не найден.");
            return;
        }
        order.CloseOrder();
        Console.WriteLine("Заказ закрыт.");
    }

    static void PrintReceipt()
    {
        Console.Write("ID заказа: ");
        var id = int.Parse(Console.ReadLine());
        var order = RestaurantSystem.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            Console.WriteLine("Заказ не найден.");
            return;
        }
        order.PrintReceipt(in RestaurantSystem.Dishes);
    }

    static void ShowStatistics()
    {
        Console.WriteLine("\n=== СТАТИСТИКА ===");
        
        // Статистика закрытых заказов
        decimal totalRevenue = RestaurantSystem.CalculateClosedOrdersTotal(out int totalOrders);
        Console.WriteLine($"Всего закрытых заказов: {totalOrders}");
        Console.WriteLine($"Общая выручка: {totalRevenue} руб.");

        // Статистика официанта
        Console.Write("\nВведите ID официанта для статистики: ");
        if (int.TryParse(Console.ReadLine(), out int waiterId))
        {
            RestaurantSystem.GetWaiterStatistics(in waiterId, out int waiterOrders, out decimal waiterRevenue);
            Console.WriteLine($"Официант {waiterId}: {waiterOrders} заказов, выручка: {waiterRevenue} руб.");
        }

        // Статистика по блюдам
        var dishStats = new Dictionary<string, int>();
        RestaurantSystem.GetDishStatistics(ref dishStats);
        Console.WriteLine("\nСтатистика по блюдам:");
        foreach (var stat in dishStats.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"  {stat.Key}: {stat.Value} заказов");
        }
    }
}