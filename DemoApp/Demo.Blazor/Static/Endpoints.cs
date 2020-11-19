using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Blazor.Static
{
    public static class Endpoints
    {
        public static string BaseUrl = "https://localhost:44393";
        public static string AuthorsEndPoint = $"{BaseUrl}/api/authors/";
        public static string BooksEndPoint = $"{BaseUrl}/api/books/";
        public static string RegisterEndPoint = $"{BaseUrl}/api/users/register/";
        public static string LoginEndPoint = $"{BaseUrl}/api/users/login/";
    }
}
