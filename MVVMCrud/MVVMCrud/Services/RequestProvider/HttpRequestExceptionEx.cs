using System;
namespace MVVMCrud.Services.RequestProvider
{
    public class HttpRequestUrlEmptyExceptionEx : Exception
    {
        public HttpRequestUrlEmptyExceptionEx()
        {
        }

        public override string Message => "Url is empty";
    }


}
