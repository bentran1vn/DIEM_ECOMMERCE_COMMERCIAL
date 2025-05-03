namespace DiemEcommerce.Web.Models.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; set; }
    public ErrorDetails? Error { get; set; }
}

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public ErrorDetails? Error { get; set; }
}

public class ErrorDetails
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}