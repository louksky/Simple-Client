using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client_Ari
{
    internal class ServerAPI
    {
        /// <summary>
        /// S-Developer : Asaf Louk
        /// A class that contains an object that produces a connection to a server and actually manages the connection type of controller
        /// </summary>
        API_connection server;
        public ServerAPI()
        {
            this.server = new API_connection();
            try
            {
                this.server.Login();
            }catch { }
        }
        public string Eval(string val1, string val2, string mathematica_operator)
        {
            ///A function that transmits the data to the open API server using three parameters 
            ///If no answer is received the function will return an answer that there is no connection to the server
            string res = GlobalsSettings.DISCONNECTED;
            try
            {
                if (this.server.Is_connect)
                    res = server.Eval(val1, val2, mathematica_operator);
            }catch { }
            return res;
        }
        public string GetOperatorsList()
        {
            ///A function that returns the list of usable operators
            string res = GlobalsSettings.DISCONNECTED;
            try
            {
                if (this.server.Is_connect)
                    res = server.GetOperatorsList();
            }catch { }
            return res;
        }
        public bool IsReady()
        {
            //A function that returns whether the server is OK
            return this.server.Is_connect;
        }
    }
}
