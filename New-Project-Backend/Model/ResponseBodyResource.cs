using Microsoft.VisualBasic;

namespace New_Project_Backend.Model
{
    public class ResponseBodyResource<TResult>
    {

        public string Software { get; set; } = CustomMessages.TITLE;
        public string Version { get; set; } = CustomMessages.VERSION;
        public bool Success { get; set; }
        public string ResCode { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public TResult Result { get; set; }

        public ResponseBodyResource(bool isSuccess, int code, string message)
        {
            Code = code;
            Success = isSuccess;
            Message = message;
        }

        public ResponseBodyResource(bool isSuccess, int code, ErrorCodes errorCodes)
        {
            Code = code;
            Success = isSuccess;
            ResCode = errorCodes.ToString();
            Message = errorCodes.ToString();
        }

        public ResponseBodyResource() : this(true, StatusCodes.Status200OK, string.Empty)
        {

        }

        public ResponseBodyResource(string statusMessage) : this(false, StatusCodes.Status200OK, statusMessage)
        {

        }

        public ResponseBodyResource(ErrorCodes errorCode) : this(false, StatusCodes.Status200OK, errorCode)
        {

        }

    }
}
