namespace EmployeeDepartmentAPI.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public T? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, string message = "", int statusCode = 200, bool status = true)
        {
            Data = data;
            Message = message;
            Status = status;
            StatusCode = statusCode;
        }
    }
}
