using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace USTeco
{
    class MyGlobal
    {
        private static int guest;
        public static int IsGuest
        {
            get
            {
                // Reads are usually simple
                return guest;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                guest = value;
            }
        }
        public static string MyServer;
        public static int MyPort;
        public static string MySid;
        public static string MyUser;
        public static string MyPass;
        public static bool showLogs;
        public static bool autoStart;
        public static bool stopFailed;
        public static bool showDetails;
        public static bool dataCheck;
        public static int ServerType;
        public static string sqlServer;
        public static string sqlUser;
        public static int MyPeriod;
        public static bool IsConnected;
        public static int TryNo;
        public static int Place_id = 0;
        public static bool IsConnected2;
        public static System.Drawing.Color color1, color2;
        public static bool RepFromLog;
        public static string apiLink;
        public static string mysqlServer;
        public static string mysqlDatabase;
        public static string mysqlUid;
        public static string mysqlPassword;

    }
}
