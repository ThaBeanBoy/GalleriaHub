using System.Text;
using System.Security.Cryptography;
using System.Runtime.Intrinsics.Arm;

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

        public static bool Match(string arg1, string arg2) => Sha256.Equals(arg1, arg2);
    }
}