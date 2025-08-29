namespace TaskManager.Domain.ValueObjects;

public class TaskTitle
{
    public string Value { get; }

    // Rendre le constructeur internal pour qu'il soit accessible dans le même assembly
    internal TaskTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty", nameof(value));
        
        if (value.Length > 100)
            throw new ArgumentException("Title cannot exceed 100 characters", nameof(value));

        Value = value;
    }

    // Pour EF Core - constructeur sans paramètres (doit être privé)
    private TaskTitle()
    {
        Value = string.Empty;
    }

    // Méthode factory pour créer des instances (optionnelle)
    public static TaskTitle Create(string value)
    {
        return new TaskTitle(value);
    }

    // Opérateurs de conversion
    public static implicit operator string(TaskTitle title) => title.Value;
    
    public override bool Equals(object? obj)
    {
        return obj is TaskTitle other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
}