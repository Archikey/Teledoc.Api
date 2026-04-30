namespace Teledoc.ApiServices.DTO.Responses
{
    public sealed class MessageResponse<T>
    {
        public int StatusCode { get; set; } = 200;
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
