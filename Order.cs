public class Order
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public List<Dish> Dishes { get; set; }
    public string Comment { get; set; }
    public DateTime OrderTime { get; set; }
    public int WaiterId { get; set; }
    public DateTime? CloseTime { get; set; }
    public decimal TotalPrice { get; set; }

    public Order(int id, int tableId, string comment, int waiterId, params Dish[] dishes)
    {
        Id = id;
        TableId = tableId;
        Comment = comment;
        WaiterId = waiterId;
        OrderTime = DateTime.Now;
        Dishes = new List<Dish>(dishes);
        CalculateTotalPrice();
    }

    public void UpdateOrder(int tableId, string comment, int waiterId, params Dish[] dishes)
    {
        TableId = tableId;
        Comment = comment;
        WaiterId = waiterId;
        Dishes = new List<Dish>(dishes);
        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = Dishes.Sum(d => d.Price);
    }

    public void PrintInfo()
    {
        Console.WriteLine($"ID заказа: {Id}");
        Console.WriteLine($"Столик: {TableId}");
        Console.WriteLine($"Официант: {WaiterId}");
        Console.WriteLine($"Время заказа: {OrderTime:dd.MM.yyyy HH:mm}");
        Console.WriteLine($"Комментарий: {Comment}");
        Console.WriteLine("Блюда:");
        foreach (var dish in Dishes)
        {
            Console.WriteLine($"  - {dish.Name} ({dish.Price} руб.)");
        }
        Console.WriteLine($"Итого: {TotalPrice} руб.");
        Console.WriteLine($"Статус: {(CloseTime.HasValue ? "Закрыт" : "Открыт")}");
    }

    public void CloseOrder()
    {
        CloseTime = DateTime.Now;
    }

    public void PrintReceipt(in List<Dish> allDishes)
    {
        if (!CloseTime.HasValue)
        {
            Console.WriteLine("Чек можно распечатать только для закрытых заказов!");
            return;
        }

        Console.WriteLine("***********************");
        Console.WriteLine($"Столик: {TableId}");
        Console.WriteLine($"Официант: {WaiterId}");
        Console.WriteLine($"Период обслуживания: с {OrderTime:dd.MM.yyyy HH:mm} по {CloseTime:dd.MM.yyyy HH:mm}");
        Console.WriteLine();

        // Группируем блюда по категориям
        var dishesByCategory = Dishes
            .GroupBy(d => d.Category)
            .OrderBy(g => g.Key);

        decimal total = 0;

        foreach (var categoryGroup in dishesByCategory)
        {
            Console.WriteLine($"{categoryGroup.Key}:");
            
            var dishCounts = categoryGroup
                .GroupBy(d => d.Id)
                .Select(g => new { Dish = g.First(), Count = g.Count() });

            decimal categorySubtotal = 0;

            foreach (var item in dishCounts)
            {
                decimal dishTotal = item.Dish.Price * item.Count;
                Console.WriteLine($"  {item.Dish.Name}    {item.Count}*{item.Dish.Price}={dishTotal} руб.");
                categorySubtotal += dishTotal;
            }

            Console.WriteLine($"Под-итог категории: {categorySubtotal} руб.");
            Console.WriteLine();
            total += categorySubtotal;
        }

        Console.WriteLine($"Итог счета: {total} руб.");
        Console.WriteLine("***********************");
    }
}