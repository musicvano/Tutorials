using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Time.Testing;
using Shop.Core;

namespace Shop.Tests;

[TestClass]
public sealed class BookingServiceTests
{
    private static readonly DateTimeOffset Monday9 =
        new(2026, 9, 14, 9, 0, 0, TimeSpan.Zero);

    private FakeTimeProvider clock = null!;
    private BookingService service = null!;

    [TestInitialize]                         // перед кожним тестом
    public void SetUp()
    {
        clock = new FakeTimeProvider(Monday9);
        service = new BookingService(clock);
    }

    [TestMethod]
    public void Create_TooLate_Throws()
    {
        DateTimeOffset start = Monday9.AddMinutes(90);
        Assert.ThrowsExactly<InvalidOperationException>(
            () => service.Create("A-101", start));
    }

    [TestMethod]
    public void Create_ExactlyTwoHoursAhead_Succeeds()
    {
        Booking booking =
            service.Create("A-101", Monday9.AddHours(2));
        Assert.AreEqual(Monday9, booking.Created);
    }

    [TestMethod]
    public void IsExpired_Unpaid_AfterWindow_True()
    {
        Booking booking = service.Create("A-101", Monday9.AddDays(3));

        clock.Advance(TimeSpan.FromHours(24));     // рівно 24 год
        Assert.IsFalse(service.IsExpired(booking));

        clock.Advance(TimeSpan.FromMinutes(1));    // 24 год 1 хв
        Assert.IsTrue(service.IsExpired(booking));
    }

    [TestMethod]
    public void IsExpired_Paid_NeverExpires()
    {
        Booking booking = service.Create("A-101", Monday9.AddDays(3));
        booking.IsPaid = true;

        clock.Advance(TimeSpan.FromDays(10));
        Assert.IsFalse(service.IsExpired(booking));
    }
}
