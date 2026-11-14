namespace Shop.Core;

public record Booking(
    string Room, DateTimeOffset Start, DateTimeOffset Created)
{
    public bool IsPaid { get; set; }
}

// Час отримується через TimeProvider, а не DateTime.Now.
public class BookingService(TimeProvider time)
{
    public static readonly TimeSpan PaymentWindow =
        TimeSpan.FromHours(24);
    public static readonly TimeSpan MinAdvance =
        TimeSpan.FromHours(2);

    public Booking Create(string room, DateTimeOffset start)
    {
        DateTimeOffset now = time.GetUtcNow();
        if (start - now < MinAdvance)
        {
            throw new InvalidOperationException(
                "бронювати можна щонайменше за 2 години");
        }
        return new Booking(room, start, now);
    }

    // Неоплачене бронювання скасовується через 24 години.
    public bool IsExpired(Booking booking) =>
        !booking.IsPaid
        && time.GetUtcNow() - booking.Created > PaymentWindow;
}
