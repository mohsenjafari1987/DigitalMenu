using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.AppService.Response
{    
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public BaseResponse()
        {
            Success = true;
            Message = "Success";
        }

        public void SetError(Exception ex)
        {
            Success = false;
            Message = ex.Message;            
        }

        public void SetError(string message)
        {
            Success = false;
            Message = message;            
        }
    }

    public class BaseResponse<TData> : Response
    {
        public TData Data { get; set; }
    }
}
