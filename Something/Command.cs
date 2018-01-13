using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpPhone.Something
{
    class Command
    {
        public const string FIND_CLIENTS = "find";
        public const string CLIENT_LADING = "lading";
        public const string CLIENT_ONLINE = "online";
        public const string CALL_CLIENT = "call";
        public const string ANSWER_CLIENT = "answer";

        public const string NOT_CONNECTED = "未连接";
        public const string CONNECTED = "已连通";
        public const string BE_CALLED = "被呼叫";
        public const string CALLING = "呼叫中";
    }
}
