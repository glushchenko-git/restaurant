public class Table
{
    public int Id { get; set; }
    public string Location { get; set; }
    public int Seats { get; set; }
    public Dictionary<DateTime, Reservation> Schedule { get; set; }

    public Table(int id, string location, int seats)
    {
        Id = id;
        Location = location;
        Seats = seats;
        Schedule = new Dictionary<DateTime, Reservation>();
    }

    public void UpdateInfo(string location, int seats)
    {
        Location = location;
        Seats = seats;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Расположение: {Location}");
        Console.WriteLine($"Количество мест: {Seats}");
        Console.WriteLine("Расписание:");
        for (int hour = 9; hour < 18; hour++)
        {
            var time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, 0, 0);
            var endTime = time.AddHours(1);
            if (Schedule.ContainsKey(time))
            {
                var res = Schedule[time];
                Console.WriteLine($"{time:HH:mm}-{endTime:HH:mm} --- ID {res.Id}, {res.ClientName}, {res.PhoneNumber}");
            }
            else
            {
                Console.WriteLine($"{time:HH:mm}-{endTime:HH:mm} ---");
            }
        }
    }
}