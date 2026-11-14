namespace Shop.Core;

public static class PasswordValidator
{
    public const int MinLength = 8;

    // Повертає список порушених правил; порожній – пароль надійний.
    public static List<string> Validate(string? password)
    {
        List<string> errors = [];
        if (string.IsNullOrEmpty(password))
        {
            errors.Add("порожній пароль");
            return errors;
        }
        if (password.Length < MinLength)
            errors.Add($"менше {MinLength} символів");
        if (!password.Any(char.IsUpper))
            errors.Add("немає великої літери");
        if (!password.Any(char.IsDigit))
            errors.Add("немає цифри");
        if (password.All(char.IsLetterOrDigit))
            errors.Add("немає спеціального символу");
        if (password.Contains(' '))
            errors.Add("містить пробіл");
        return errors;
    }
}
