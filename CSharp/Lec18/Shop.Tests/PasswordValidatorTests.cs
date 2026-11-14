using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Core;

namespace Shop.Tests;

[TestClass]
public sealed class PasswordValidatorTests
{
    [TestMethod]
    [DataRow("Rome#2026")]
    [DataRow("Пароль_12")]                  // кирилиця теж літери
    [DataRow("Aa1!aaaa")]                   // рівно 8 символів
    public void Validate_StrongPassword_NoErrors(string password)
    {
        List<string> errors = PasswordValidator.Validate(password);
        Assert.IsEmpty(errors);
    }

    // Кожен рядок порушує рівно одне правило.
    [TestMethod]
    [DataRow("Aa1!aaa", "менше 8 символів")]
    [DataRow("rome#2026", "немає великої літери")]
    [DataRow("Rome#Oslo", "немає цифри")]
    [DataRow("Rome2026", "немає спеціального символу")]
    [DataRow("Rome 2026", "містить пробіл")]
    public void Validate_OneRuleBroken_OneError(
        string password, string expected)
    {
        List<string> errors = PasswordValidator.Validate(password);
        CollectionAssert.AreEqual(new[] { expected }, errors);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void Validate_NullOrEmpty_SingleError(string? password)
    {
        List<string> errors = PasswordValidator.Validate(password);
        Assert.HasCount(1, errors);
        Assert.AreEqual("порожній пароль", errors[0]);
    }

    [TestMethod]
    public void Validate_WeakPassword_ReportsAllRules()
    {
        List<string> errors = PasswordValidator.Validate("abc");
        Assert.HasCount(4, errors);
        CollectionAssert.Contains(errors, "немає цифри");
    }
}
