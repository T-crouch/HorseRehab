namespace HorseRehab.Core.Horses;

/// <summary>
/// Represents a diagnosed injury or medical condition recorded for a horse.
/// </summary>
public sealed class HorseCondition
{
    /// <summary>
    /// Initializes a horse condition.
    /// </summary>
    /// <param name="id">The stable identifier for the condition record.</param>
    /// <param name="name">The clinical or commonly understood condition name.</param>
    /// <param name="category">The broad category of the condition.</param>
    /// <param name="status">The current condition status.</param>
    public HorseCondition(
        Guid id,
        string name,
        ConditionCategory category,
        ConditionStatus status = ConditionStatus.Active)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "A condition identifier cannot be empty.",
                nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Id = id;
        Name = name.Trim();
        Category = category;
        Status = status;
    }

    /// <summary>
    /// Gets the stable identifier for the condition record.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the condition name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the broad category of the condition.
    /// </summary>
    public ConditionCategory Category { get; }

    /// <summary>
    /// Gets the current status of the condition.
    /// </summary>
    public ConditionStatus Status { get; }
}
