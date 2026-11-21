using SalonBooking.Api.Common;

namespace SalonBooking.UnitTests.Common;

public class ResultTests
{
    private static readonly Error SomeError = Error.NotFound("Test.NotFound","Not found");
    [Fact]
    public void Success_HasNoErrors()
    {
        var result = Result.Success(42);
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failure_HasError()
    {
        var result = Result.Failure<int>(SomeError);
        Assert.True(result.IsFailure);
        Assert.Equal(SomeError, result.Error);
    }

    [Fact]
    public void Value_OnFailure_Throws()
    {
        var result = Result.Failure<int>(SomeError);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}