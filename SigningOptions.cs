using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lundk
{
    public class SigningOptions
    {
        public static string Key = "my_secret_key_12345"; //this should be same which is used while creating token      
        public static string Issuer = "http://mysite.com";  //this should be same which is used while creating token  
    }
}
