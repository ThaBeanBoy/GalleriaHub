using System.Text;
using System.Security.Cryptography;

namespace Utility{

    public class Security{
        public static string hashauth(string input){
        string hashauth =string.Empty;

        using(SHA256 sha = SHA256.Create()){

            byte[] value =sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            foreach(byte b in value){
                hashauth +=b;
            }
            return hashauth;

        }
    }
    }

    
}