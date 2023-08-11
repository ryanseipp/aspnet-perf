namespace AspnetPerf.MinimalApi.Domain.Entities;

public class User
{
    public User(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        CreatedAtTimestampUtc = DateTimeOffset.UtcNow;
    }

    private User() { }

    public int Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAtTimestampUtc { get; private set; }
    public DateTimeOffset? UpdatedAtTimestampUtc { get; private set; }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAtTimestampUtc = DateTimeOffset.UtcNow;
    }
}
