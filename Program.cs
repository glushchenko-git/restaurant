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

    // Реализации методов системы бронирования (аналогично предыдущему решению)
    static void CreateTable() { /* реализация */ }
    static void CreateReservation() { /* реализация */ }
    static void EditTable() { /* реализация */ }
    static void ShowTableInfo() { /* реализация */ }
    static void ShowAvailableTables() { /* реализация */ }
    static void ShowAllReservations() { /* реализация */ }
    static void FindReservation() { /* реализация */ }

    // Реализации методов системы заказов
    static void CreateDish() { /* реализация */ }
    static void CreateOrder() { /* реализация */ }
    static void ShowOrderInfo() { /* реализация */ }
    static void CloseOrder() { /* реализация */ }
    static void PrintReceipt() { /* реализация */ }
}