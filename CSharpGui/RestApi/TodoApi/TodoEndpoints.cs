using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TodoApi;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/todos").WithTags("Todos");
        group.MapGet("/", GetAll);
        group.MapGet("/{id:int}", GetById).WithName("GetTodo");
        group.MapPost("/", Create);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);
        return group;
    }

    // GET /api/todos?done=false&page=1&pageSize=10
    static async Task<Ok<PagedResult<TodoItem>>> GetAll(
        ITodoRepository repo, bool? done = null,
        [Range(1, 1000)] int page = 1,
        [Range(1, 50)] int pageSize = 10) =>
        TypedResults.Ok(
            await repo.GetPageAsync(done, page, pageSize));

    static async Task<Results<Ok<TodoItem>, NotFound>> GetById(
        int id, ITodoRepository repo) =>
        await repo.FindAsync(id) is { } item
            ? TypedResults.Ok(item)
            : TypedResults.NotFound();

    static async Task<Created<TodoItem>> Create(
        TodoCreateDto dto, ITodoRepository repo)
    {
        var item = await repo.AddAsync(new TodoItem
        {
            Title = dto.Title.Trim(),
            Priority = dto.Priority,
            DueDate = dto.DueDate
        });
        return TypedResults.Created($"/api/todos/{item.Id}", item);
    }

    static async Task<Results<NoContent, NotFound>> Update(
        int id, TodoUpdateDto dto, ITodoRepository repo)
    {
        var item = new TodoItem
        {
            Id = id, Title = dto.Title.Trim(),
            Priority = dto.Priority, DueDate = dto.DueDate,
            IsDone = dto.IsDone
        };
        return await repo.UpdateAsync(item)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }

    static async Task<Results<NoContent, NotFound>> Delete(
        int id, ITodoRepository repo) =>
        await repo.DeleteAsync(id)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
}
