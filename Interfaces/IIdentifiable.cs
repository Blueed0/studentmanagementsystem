namespace StudentManagementSystem.Interfaces
{
    // Shared contract: every identifiable entity exposes a read-only Id.
    public interface IIdentifiable
    {
        string Id { get; }
    }
}
