public enum DishCategory
{
    Напитки,
    Салаты,
    ХолодныеЗакуски,
    ГорячиеЗакуски,
    Супы,
    ГорячиеБлюда,
    Десерты
}

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Composition { get; set; }
    public string Weight { get; set; }
    public decimal Price { get; set; }
    public DishCategory Category { get; set; }
    public int CookingTime { get; set; }
    public string[] Tags { get; set; }

    public Dish(int id, string name, string composition, string weight, decimal price, 
                DishCategory category, int cookingTime, params string[] tags)
    {
        Id = id;
        Name = name;
        Composition = composition;
        Weight = weight;
        Price = price;
        Category = category;
        CookingTime = cookingTime;
        Tags = tags;
    }

    public void UpdateDish(string name, string composition, string weight, decimal price,
                          DishCategory category, int cookingTime, params string[] tags)
    {
        Name = name;
        Composition = composition;
        Weight = weight;
        Price = price;
        Category = category;
        CookingTime = cookingTime;
        Tags = tags;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Название: {Name}");
        Console.WriteLine($"Состав: {Composition}");
        Console.WriteLine($"Вес: {Weight}");
        Console.WriteLine($"Цена: {Price} руб.");
        Console.WriteLine($"Категория: {Category}");
        Console.WriteLine($"Время готовки: {CookingTime} мин.");
        Console.WriteLine($"Теги: {string.Join(", ", Tags)}");
    }

    public void DeleteDish(ref List<Dish> dishes)
    {
        dishes.Remove(this);
    }
}