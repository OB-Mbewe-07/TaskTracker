using System.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;

namespace TaskTracker.Utilities;

public static class ChangeTracker
{
    public static List<FieldChange> GetChanges(object before, object after)
    {
        var change = new List<FieldChange>();
        var properties = before.GetType().GetProperties();

        foreach (var property in properties)
        {
            var oldValue = property.GetValue(before);
            var newValue = property.GetValue(after);

            if (!Equals(oldValue, newValue))
            {
                change.Add(new FieldChange(property.Name, oldValue, newValue));
            }
        }

        return change;
    }
}

public record FieldChange(string PropertyName, object? OldValue, object? NewValue);