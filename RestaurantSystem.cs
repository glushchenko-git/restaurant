public static class RestaurantSystem
{
    // Система бронирования
    public static List<Table> Tables = new List<Table>();
    public static List<Reservation> Reservations = new List<Reservation>();
    public static int ReservationIdCounter = 1;

    // Система заказов
    public static List<Dish> Dishes = new List<Dish>();
    public static List<Order> Orders = new List<Order>();
    public static int OrderIdCounter = 1;

    // Методы с использованием in, out, ref, params
    public static void InitializeSampleData()
    {
        // Столы
        Tables.Add(new Table(1, "у окна", 4));
        Tables.Add(new Table(2, "у прохода", 2));
        Tables.Add(new Table(3, "в глубине", 6));

        // Блюда
        Dishes.Add(new Dish(1, "Цезарь", "Салат, курица, сыр", "250/50/30", 450, DishCategory.Салаты, 10, "классика"));
        Dishes.Add(new Dish(2, "Борщ", "Свекла, мясо, сметана", "300/50", 350, DishCategory.Супы, 25, "традиционный"));
        Dishes.Add(new Dish(3, "Стейк", "Говядина, специи", "200/50", 890, DishCategory.ГорячиеБлюда, 20, "мясо", "премиум"));
        Dishes.Add(new Dish(4, "Кофе", "Арабика", "200", 180, DishCategory.Напитки, 5, "горячий"));
    }

    public static void PrintMenu()
    {
        var dishesByCategory = Dishes.GroupBy(d => d.Category).OrderBy(g => g.Key);
        
        foreach (var category in dishesByCategory)
        {
            Console.WriteLine($"\n--- {category.Key} ---");
            foreach (var dish in category)
            {
                Console.WriteLine($"{dish.Name} - {dish.Price} руб. ({dish.Weight})");
                Console.WriteLine($"  Состав: {dish.Composition}");
                if (dish.Tags.Length > 0)
                    Console.WriteLine($"  Теги: {string.Join(", ", dish.Tags)}");
            }
        }
    }

    public static decimal CalculateClosedOrdersTotal(out int totalCount)
    {
        var closedOrders = Orders.Where(o => o.CloseTime.HasValue).ToList();
        totalCount = closedOrders.Count;
        return closedOrders.Sum(o => o.TotalPrice);
    }

    public static void GetWaiterStatistics(in int waiterId, out int orderCount, out decimal totalRevenue)
    {
        var waiterOrders = Orders.Where(o => o.WaiterId == waiterId && o.CloseTime.HasValue).ToList();
        orderCount = waiterOrders.Count;
        totalRevenue = waiterOrders.Sum(o => o.TotalPrice);
    }

    public static void GetDishStatistics(ref Dictionary<string, int> dishStatistics)
    {
        dishStatistics.Clear();
        foreach (var order in Orders)
        {
            foreach (var dish in order.Dishes)
            {
                if (dishStatistics.ContainsKey(dish.Name))
                    dishStatistics[dish.Name]++;
                else
                    dishStatistics[dish.Name] = 1;
            }
        }
    }

    public static void AddDishesToOrder(ref Order order, params Dish[] dishes)
    {
        order.Dishes.AddRange(dishes);
        order.CalculateTotalPrice();
    }
}