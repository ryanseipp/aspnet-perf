using AspnetPerf.MinimalApi.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AspnetPerf.MinimalApi.Endpoints.Todos;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapRoutes(IEndpointRouteBuilder builder)
    {
        var todos = builder.MapGroup("/todo").WithTags("todo");

        todos
            .MapGet(
                "/",
                async (AppDbContext dbContext, CancellationToken ct) =>
                    TypedResults.Ok(
                        await dbContext.Todos
                            .Where(t => t.Status != TodoStatus.Deleted)
                            .Select(
                                t =>
                                    new TodoViewModel
                                    {
                                        Id = t.Id,
                                        Text = t.Text,
                                        Status = t.Status.ToString(),
                                        CreatedAtTimestampUtc = t.CreatedAtTimestampUtc,
                                        UpdatedAtTimestampUtc = t.UpdatedAtTimestampUtc,
                                    }
                            )
                            .ToListAsync(ct)
                    )
            )
            .WithName("GetTodos");

        todos
            .MapPost(
                "/",
                async (
                    AppDbContext dbContext,
                    CreateTodoRequestModel model,
                    CancellationToken ct
                ) =>
                {
                    var todo = new Todo(1, model.Text);
                    dbContext.Todos.Add(todo);
                    await dbContext.SaveChangesAsync(ct);
                    return TypedResults.CreatedAtRoute("GetTodoById", new { Id = todo.Id });
                }
            )
            .WithName("CreateTodo");

        todos
            .MapGet(
                "/{id:int}",
                async Task<Results<Ok<TodoViewModel>, NotFound>> (
                    AppDbContext dbContext,
                    int id,
                    CancellationToken ct
                ) =>
                {
                    var todo = await dbContext.Todos
                        .Where(t => t.Id == id)
                        .Where(t => t.Status != TodoStatus.Deleted)
                        .Select(
                            t =>
                                new TodoViewModel
                                {
                                    Id = t.Id,
                                    Text = t.Text,
                                    Status = t.Status.ToString(),
                                    CreatedAtTimestampUtc = t.CreatedAtTimestampUtc,
                                    UpdatedAtTimestampUtc = t.UpdatedAtTimestampUtc,
                                }
                        )
                        .FirstOrDefaultAsync(ct);

                    if (todo is null)
                    {
                        return TypedResults.NotFound();
                    }

                    return TypedResults.Ok(todo);
                }
            )
            .WithName("GetTodoById");

        todos
            .MapPut(
                "/{id:int}",
                async Task<Results<NoContent, NotFound>> (
                    AppDbContext dbContext,
                    int id,
                    UpdateTodoRequestModel model,
                    CancellationToken ct
                ) =>
                {
                    var todo = await dbContext.Todos
                        .Where(t => t.Id == id)
                        .Where(t => t.Status != TodoStatus.Deleted)
                        .FirstOrDefaultAsync(ct);

                    if (todo is null)
                    {
                        return TypedResults.NotFound();
                    }

                    todo.UpdateText(model.Text);
                    await dbContext.SaveChangesAsync(ct);

                    return TypedResults.NoContent();
                }
            )
            .WithName("UpdateTodo");

        todos
            .MapDelete(
                "/{id:int}",
                async Task<Results<NoContent, NotFound>> (
                    AppDbContext dbContext,
                    int id,
                    CancellationToken ct
                ) =>
                {
                    var todo = await dbContext.Todos
                        .Where(t => t.Id == id)
                        .Where(t => t.Status != TodoStatus.Deleted)
                        .FirstOrDefaultAsync(ct);

                    if (todo is null)
                    {
                        return TypedResults.NotFound();
                    }

                    todo.Delete();
                    await dbContext.SaveChangesAsync(ct);

                    return TypedResults.NoContent();
                }
            )
            .WithName("DeleteTodo");

        todos
            .MapPut(
                "/{id:int}/status",
                async Task<Results<NoContent, NotFound, ValidationProblem>> (
                    AppDbContext dbContext,
                    int id,
                    UpdateTodoStatusRequestModel model,
                    CancellationToken ct
                ) =>
                {
                    var todo = await dbContext.Todos
                        .Where(t => t.Id == id)
                        .Where(t => t.Status != TodoStatus.Deleted)
                        .FirstOrDefaultAsync(ct);

                    if (todo is null)
                    {
                        return TypedResults.NotFound();
                    }

                    switch (model.Status)
                    {
                        case UpdateTodoStatus.InProgress:
                            todo.Restart();
                            break;
                        case UpdateTodoStatus.Complete:
                            todo.Complete();
                            break;
                        default:
                            return TypedResults.ValidationProblem(
                                new Dictionary<string, string[]>()
                                {
                                    {
                                        nameof(model.Status),
                                        new[]
                                        {
                                            $"Status must be one of {nameof(UpdateTodoStatus.InProgress)} or {nameof(UpdateTodoStatus.Complete)}"
                                        }
                                    }
                                }
                            );
                    }

                    await dbContext.SaveChangesAsync(ct);
                    return TypedResults.NoContent();
                }
            )
            .WithName("UpdateTodoStatus");

        return builder;
    }
}

public class CreateTodoRequestModel
{
    public string Text { get; set; } = string.Empty;
}

public class TodoViewModel
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtTimestampUtc { get; set; }
    public DateTimeOffset? UpdatedAtTimestampUtc { get; set; }
}

public class UpdateTodoRequestModel
{
    public string Text { get; set; } = string.Empty;
}

public class UpdateTodoStatusRequestModel
{
    public UpdateTodoStatus Status { get; set; }
}

public enum UpdateTodoStatus
{
    InProgress,
    Complete,
}
