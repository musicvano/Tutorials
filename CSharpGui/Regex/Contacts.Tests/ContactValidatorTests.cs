using Contacts.Core;

namespace Contacts.Tests;

public class ContactValidatorTests
{
    [Theory]
    [InlineData("+380671234567")]
    [InlineData("0671234567")]
    [InlineData("067 123 45 67")]
    [InlineData("+38050-123-45-67")]
    public void IsValidPhone_CorrectNumber_ReturnsTrue(string phone)
    {
        Assert.True(ContactValidator.IsValidPhone(phone));
    }

    [Theory]
    [InlineData("")]
    [InlineData("067123456")]
    [InlineData("+390671234567")]
    [InlineData("0671234567\n")]
    public void IsValidPhone_WrongNumber_ReturnsFalse(string phone)
    {
        Assert.False(ContactValidator.IsValidPhone(phone));
    }
}
