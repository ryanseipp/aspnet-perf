namespace AspnetPerf.MinimalApi.Domain.Entities;

public class Todo
{
    public Todo(int userId, string text)
    {
        UserId = userId;
        Text = text;
        Status = TodoStatus.InProgress;
        CreatedAtTimestampUtc = DateTimeOffset.UtcNow;
    }

    private Todo() { }

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public User? User { get; private set; }

    public string Text { get; private set; } = string.Empty;
    public TodoStatus Status { get; private set; }

    public DateTimeOffset CreatedAtTimestampUtc { get; private set; }
    public DateTimeOffset? UpdatedAtTimestampUtc { get; private set; }

    public void UpdateText(string text)
    {
        Text = text;
        UpdatedAtTimestampUtc = DateTimeOffset.UtcNow;
    }

    public void Complete()
    {
        Status = TodoStatus.Completed;
        UpdatedAtTimestampUtc = DateTimeOffset.UtcNow;
    }

    public void Delete()
    {
        Status = TodoStatus.Deleted;
        UpdatedAtTimestampUtc = DateTimeOffset.UtcNow;
    }
}
