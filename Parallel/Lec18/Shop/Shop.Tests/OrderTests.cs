using System.Net.Http.Json;
using System.Text.Json;
using Npgsql;

namespace Shop.Tests;

public class OrderTests(ShopFixture shop) : IClassFixture<ShopFixture>
{
    [Fact]
    public async Task ConfirmedOrderCreatesShipment()
    {
        HttpClient gateway = shop.App.CreateHttpClient("gateway");
        var resp = await gateway.PostAsJsonAsync("/api/orders", new
        {
            customer = "test",
            lines = new[] { new { productId = 2, quantity = 1 } },
        });
        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        JsonElement order = await resp.Content
            .ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Confirmed",
            order.GetProperty("status").GetString());

        // Подія йде через Outbox і RabbitMQ: чекаємо до 15 с.
        string? cs =
            await shop.App.GetConnectionStringAsync("shippingdb");
        await using var db = new NpgsqlConnection(cs);
        await db.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            """
            select count(*) from "Shipments" where "OrderId" = @id
            """,
            db);
        cmd.Parameters.AddWithValue("id",
            order.GetProperty("id").GetGuid());
        long count = 0;
        for (int i = 0; i < 30 && count == 0; i++)
        {
            await Task.Delay(500);
            count = (long)(await cmd.ExecuteScalarAsync())!;
        }
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task DeclinedPaymentCancelsOrder()
    {
        HttpClient gateway = shop.App.CreateHttpClient("gateway");
        var resp = await gateway.PostAsJsonAsync("/api/orders", new
        {
            customer = "test",       // 2 × 32 000 грн > ліміту 50 000
            lines = new[] { new { productId = 1, quantity = 2 } },
        });
        JsonElement order = await resp.Content
            .ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Cancelled",
            order.GetProperty("status").GetString());
        Assert.Equal("оплату відхилено",
            order.GetProperty("reason").GetString());
    }
}
