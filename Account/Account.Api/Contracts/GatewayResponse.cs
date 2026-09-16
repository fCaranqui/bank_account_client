namespace Account.Api.Contracts;

public class GatewayResponse<T>
{
    public bool Success { get; set; }

    public T? Data { get; set; }

    public List<GatewayError>? Errors { get; set; }

    public static GatewayResponse<T> Ok(T data) => new()
    {
        Success = true,
        Data = data,
    };

    public static GatewayResponse<T> Fail(params IEnumerable<GatewayError> errors) => new()
    {
        Success = false,
        Errors = errors.ToList(),
    };
}

public class GatewayError
{
    public string Code { get; set; } = default!;

    public string Message { get; set; } = default!;
}
