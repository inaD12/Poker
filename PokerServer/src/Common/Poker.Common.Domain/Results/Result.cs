namespace Poker.Common.Domain.Results;

public class Result<T>
{
    private Result(bool isSuccess, Response response, T? value)
    {
        IsSuccess = isSuccess;
        Response = response;
        Value = value;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Response Response { get; }
    public T? Value { get; }

    public static Result<T> Success(T value, Response response)
    {
        return new Result<T>(true, response, value);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, Response.Ok, value);
    }

    public static Result<T> Failure(Response response)
    {
        return new Result<T>(false, response, default);
    }
}

public class Result
{
    private Result(bool isSuccess, Response? response)
    {
        IsSuccess = isSuccess;
        Response = response;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Response? Response { get; }


    public static Result Success(Response response)
    {
        return new Result(true, response);
    }

    public static Result Success()
    {
        return new Result(true, Response.Ok);
    }

    public static Result Failure(Response response)
    {
        return new Result(false, response);
    }
}