using System.Text;
using System.Security.Cryptography;

namespace Utility{

    public class Security{
        public static string hashauth(string input){
        string hashauth =string.Empty;

        using(SHA256 sha = SHA256.CreateBuilder()){

            byte[] value =sha.ComputeHash(Enconding.UTF8.GetBytes[input]);

            foreach(byte b in value){
                hashauth +=b;
            }
            return hashauth;

        }
    }
    }

    
}