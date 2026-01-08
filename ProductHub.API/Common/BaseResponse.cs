namespace ProductHub.API.Common;

public class BaseResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }

    public static BaseResponse Ok()
        => new() { Success = true };

    public static BaseResponse Fail(string message)
        => new() { Success = false, Message = message };
}

public sealed class BaseResponse<T> : BaseResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }

    public static BaseResponse<T> Ok(T? data = default)
        => new() { Success = true, Data = data };

    public static BaseResponse<T> Fail(string message)
        => new() { Success = false, Message = message };
}

