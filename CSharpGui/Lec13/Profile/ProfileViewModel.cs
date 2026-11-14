namespace Profile;

public class ProfileViewModel : ObservableObject
{
    public string FirstName
    {
        get;
        set
        {
            if (SetProperty(ref field, value))
            {
                OnPropertyChanged(nameof(Greeting));
            }
        }
    } = "Olena";

    public DateTime? BirthDate
    {
        get;
        set => SetProperty(ref field, value);
    } = new DateTime(2006, 3, 14);

    public string Greeting => $"Hello, {FirstName}!";
}
