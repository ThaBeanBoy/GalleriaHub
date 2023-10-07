// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace server.Utills
// {
//     public class RequestBody : Dictionary<string, Stream>
//     {
//         public Dictionary<string, Stream> Body { get; }


//         public RequestBody(Stream Body)
//         {
//             (using StreamReader = new StreamReader)
//             this.Body = Body.Dictionary();
//         }

//         public TValue Get(string Key)
//         {

//         } 
        
//     }

//     public static class RequestBodyParser
//     {
//         public static Dictionary<string, Stream> Dictionary(this Stream Body)
//         {
//             return new Dictionary<string, Stream>();
//         }
//     }
// }