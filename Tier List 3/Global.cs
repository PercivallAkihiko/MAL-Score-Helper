using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tier_List_3
{
    public static class Global
    {
        public static MAL Mal;
        public static int num = 0;

        public static string CLIENT_ID = "c4c2872f810d3d0bc17502beec3ceb68";
        public static string VALID_CHAR = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
        public static string UriScheme = "mal";
        public static string FriendlyName = "MAL Tier Score";        
        public static string applicationLocation = System.IO.Directory.GetCurrentDirectory() + "\\MAL Score Helper.exe";
    }
}
