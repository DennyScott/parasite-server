using System;
namespace Hive.Contracts
{
    public class Commands
    {
        private Commands(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
        public static Commands Connect => new Commands("Connect");
        public static Commands Refuse => new Commands("Refust");
        public static Commands Disconnect => new Commands("Disconnect");
    }
}
