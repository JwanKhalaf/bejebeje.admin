using bejebeje.admin.Application.Common.Exceptions;
using FluentValidation.Results;
using NUnit.Framework;
using Shouldly;

namespace bejebeje.admin.Application.UnitTests.Common.Exceptions;

public class ValidationExceptionTests
{
    [Test]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        actual.Keys.ShouldBeEmpty();
    }

    [Test]
    public void SingleValidationFailureCreatesASingleElementErrorDictionary()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Age", "must be over 18"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.ShouldBe(new[] { "Age" }, ignoreOrder: true);
        actual["Age"].ShouldBe(new[] { "must be over 18" }, ignoreOrder: true);
    }

    [Test]
    public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Age", "must be 18 or older"),
                new ValidationFailure("Age", "must be 25 or younger"),
                new ValidationFailure("Password", "must contain at least 8 characters"),
                new ValidationFailure("Password", "must contain a digit"),
                new ValidationFailure("Password", "must contain upper case letter"),
                new ValidationFailure("Password", "must contain lower case letter"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.ShouldBe(new[] { "Password", "Age" }, ignoreOrder: true);

        actual["Age"].ShouldBe(new[]
        {
                "must be 25 or younger",
                "must be 18 or older",
        }, ignoreOrder: true);

        actual["Password"].ShouldBe(new[]
        {
                "must contain lower case letter",
                "must contain upper case letter",
                "must contain at least 8 characters",
                "must contain a digit",
        }, ignoreOrder: true);
    }
}
