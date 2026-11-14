using System.Collections;
using System.ComponentModel;

namespace Registration;

public class RegistrationViewModel : ObservableObject,
    INotifyDataErrorInfo
{
    private readonly Dictionary<string, string> errors = [];

    // Не з лекції: перевірка Email «влаштована так само», як Age.
    public string Email
    {
        get;
        set
        {
            SetProperty(ref field, value);
            SetError(nameof(Email), value.Contains('@')
                ? null : "Email must contain @");
        }
    } = "";

    public int Age
    {
        get;
        set
        {
            SetProperty(ref field, value);
            SetError(nameof(Age), value is >= 16 and <= 100
                ? null : "Age must be from 16 to 100");
        }
    } = 18;

    public bool HasErrors => errors.Count > 0;

    public event EventHandler<DataErrorsChangedEventArgs>?
        ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName) =>
        propertyName is not null
        && errors.TryGetValue(propertyName, out string? error)
            ? new[] { error } : Array.Empty<string>();

    private void SetError(string property, string? error)
    {
        if (error is null)
        {
            errors.Remove(property);
        }
        else
        {
            errors[property] = error;
        }
        ErrorsChanged?.Invoke(this,
            new DataErrorsChangedEventArgs(property));
        OnPropertyChanged(nameof(HasErrors));
    }
}
