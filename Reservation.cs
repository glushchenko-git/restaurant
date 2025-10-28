public class Reservation
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Comment { get; set; }
    public Table AssignedTable { get; set; }

    public Reservation(int id, string clientName, string phoneNumber, DateTime startTime, DateTime endTime, string comment, Table table)
    {
        Id = id;
        ClientName = clientName;
        PhoneNumber = phoneNumber;
        StartTime = startTime;
        EndTime = endTime;
        Comment = comment;
        AssignedTable = table;

        // Заполняем расписание стола
        for (var time = startTime; time < endTime; time = time.AddHours(1))
        {
            table.Schedule[time] = this;
        }
    }

    public void UpdateReservation(DateTime newStart, DateTime newEnd, Table newTable)
    {
        // Освобождаем старые слоты
        for (var time = StartTime; time < EndTime; time = time.AddHours(1))
        {
            AssignedTable.Schedule.Remove(time);
        }

        StartTime = newStart;
        EndTime = newEnd;
        AssignedTable = newTable;

        // Занимаем новые слоты
        for (var time = newStart; time < newEnd; time = time.AddHours(1))
        {
            newTable.Schedule[time] = this;
        }
    }

    public void CancelReservation()
    {
        for (var time = StartTime; time < EndTime; time = time.AddHours(1))
        {
            AssignedTable.Schedule.Remove(time);
        }
    }
}