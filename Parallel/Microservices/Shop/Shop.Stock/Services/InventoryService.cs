using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Shop.Stock;

public class InventoryService(StockDb db,
    ILogger<InventoryService> log) : Inventory.InventoryBase
{
    public override async Task<ReserveReply> Reserve(
        ReserveRequest request, ServerCallContext context)
    {
        Guid order = Guid.Parse(request.OrderId);
        // Ідемпотентність: повторний виклик для того самого
        // замовлення не резервує товар удруге.
        if (await db.Reservations.AnyAsync(r => r.OrderId == order))
            return new ReserveReply { Ok = true };
        // Aspire вмикає повтори EF Core після збоїв з’єднання, тому
        // власна транзакція виконується через стратегію виконання.
        return await db.Database.CreateExecutionStrategy()
            .ExecuteAsync(() => ReserveAsync(order, request.Lines));
    }

    async Task<ReserveReply> ReserveAsync(Guid order,
        IEnumerable<Line> lines)
    {
        db.ChangeTracker.Clear();
        await using var tx =
            await db.Database.BeginTransactionAsync();
        foreach (Line line in lines)
        {
            // Атомарно: UPDATE … SET available = available - q
            // WHERE available >= q.
            int updated = await db.Items
                .Where(i => i.ProductId == line.ProductId
                    && i.Available >= line.Quantity)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    i => i.Available,
                    i => i.Available - line.Quantity));
            if (updated == 0)
            {
                await tx.RollbackAsync();
                log.LogInformation(
                    "Замовлення {Order}: товару {Id} недостатньо",
                    order, line.ProductId);
                return new ReserveReply { Ok = false,
                    Reason = $"товару {line.ProductId} недостатньо" };
            }
            db.Reservations.Add(new Reservation { OrderId = order,
                ProductId = line.ProductId,
                Quantity = line.Quantity });
        }
        await db.SaveChangesAsync();
        await tx.CommitAsync();
        log.LogInformation("Замовлення {Order}: товар зарезервовано",
            order);
        return new ReserveReply { Ok = true };
    }

    // Компенсація: повернути зарезервований товар (ідемпотентно).
    public override Task<ReleaseReply> Release(
        ReleaseRequest request, ServerCallContext context) =>
        db.Database.CreateExecutionStrategy().ExecuteAsync(
            () => ReleaseAsync(Guid.Parse(request.OrderId)));

    async Task<ReleaseReply> ReleaseAsync(Guid order)
    {
        db.ChangeTracker.Clear();
        await using var tx =
            await db.Database.BeginTransactionAsync();
        List<Reservation> list = await db.Reservations
            .Where(r => r.OrderId == order && !r.Released)
            .ToListAsync();
        foreach (Reservation r in list)
        {
            await db.Items.Where(i => i.ProductId == r.ProductId)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    i => i.Available, i => i.Available + r.Quantity));
            r.Released = true;
        }
        await db.SaveChangesAsync();
        await tx.CommitAsync();
        log.LogInformation(
            "Замовлення {Order}: резерв знято ({Count})", order,
            list.Count);
        return new ReleaseReply { Released = list.Count };
    }
}
