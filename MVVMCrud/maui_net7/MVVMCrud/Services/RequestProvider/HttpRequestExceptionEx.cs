using System;
namespace HRCoffee.Services.RequestProvider
{
    public class HttpRequestUrlEmptyExceptionEx : Exception
    {
        public HttpRequestUrlEmptyExceptionEx()
        {
        }

        public override string Message => "Url is empty";
    }


}
