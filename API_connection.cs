using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace client_Ari
{
    /// <summary>
    /// S-Developer : Asaf Louk
    /// A class that represents an object to connect to a Django REST API server,
    /// Default data for a local connection, includes two objects, one that contains default data,
    /// The class API contains functionality and from it will inherit the ServerApi class that will handle errors.
    /// </summary>
    
    internal class API_connection
    {
        /// <summary>
        /// Class transmits the data "as it is" to a server which handle all end cases and communication is done through POST requests.
        /// Connection and token is made The passwords and 
        /// Tokens are not encrypted and stored in the exercise and the requests are made to HTTP
        /// </summary>
        string ip { get; set; } = Defaults_Vars.DEF_IP;
        string port { get; set; } = Defaults_Vars.DEF_PORT;
        string user_name { get; set; } = Defaults_Vars.DEF_USER;
        string pass { get; set; } = Defaults_Vars.DEF_PASS;
        int TIME_OUT = 70;
        public bool Is_connect { get; set; } = false;
        string AuthToken = String.Empty;

        public API_connection(string ip, string port, string user_name, string pass)
        {
            bool valid_data = !String.IsNullOrEmpty(ip) && !String.IsNullOrEmpty(port) && !String.IsNullOrEmpty(user_name) && !String.IsNullOrEmpty(pass);
            bool validators_data = Validators.IsValidEmailAddress(user_name) && Validators.IsValidIPAddress(ip) && Validators.IsValidPortAddress(port);
            if (valid_data && validators_data)
            {
                this.ip = ip;
                this.port = port;
                this.user_name = user_name;
                this.pass = pass;
            }
        }
        public API_connection()
        {
            this.ip = Defaults_Vars.DEF_IP;
            this.port = Defaults_Vars.DEF_PORT;
            this.user_name = Defaults_Vars.DEF_USER;
            this.pass = Defaults_Vars.DEF_PASS;

            bool validators_data = Validators.IsValidEmailAddress(this.user_name) && Validators.IsValidIPAddress(this.ip) && Validators.IsValidPortAddress(this.port);
            if (validators_data)
                Console.WriteLine();
        }
        private string Build_Connection_Url()
        {

            return String.Join(":", new String[] { this.ip, this.port });
        }
        private dynamic PostRequest(string url, FormUrlEncodedContent data)
        {
            /// A function that performs POST calls that contain a token that allows access to the secure server
            try
            {
                var client = new HttpClient();
                if(this.Is_connect)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(GlobalsSettings.TOKEN, this.AuthToken);
                HttpResponseMessage response = client.PostAsync(GlobalsSettings.HTTPWWW + url, data).GetAwaiter().GetResult();
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    HttpContent str = response.Content;
                    string myContent = str.ReadAsStringAsync().Result;
                    dynamic json = JsonConvert.DeserializeObject(myContent);
                    return json;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogTarget.File, ex.ToString());
            }
            return false; 

            return GlobalsSettings.JSONRESULT;
        }
        /// <summary>
        /// Connects with user information and password.
        /// Stores the token in the object in order to generate verified requests
        /// </summary> 
        ///<returns>True/False if login successfully done</returns>
        public bool Login()
       
        {
            bool ans =this.Authentication();
            this.Is_connect = ans;

            return this.Is_connect;
        }
        /// <summary>
        /// Performs a POST request with data to receive a token returns true if it was able to get a 200 and token confirmation
        /// </summary>
        /// <returns>True/False if authentication successfully done</returns>
        private bool Authentication()
        {
            try
            {
                string url = this.Build_Connection_Url();
                url = String.Join("/", new String[] { url, "auth", "token", "login" });
                var data = new[]
                {
                new KeyValuePair<string, string>("password", this.pass),
                new KeyValuePair<string, string>("email", this.user_name),
                };
                var EncodeData = new FormUrlEncodedContent(data);
                dynamic json = PostRequest(url, EncodeData);
                string token = json["auth_token"];
                this.AuthToken = String.Join("", token);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(LogTarget.File, ex.ToString());
            }
            return false; 
            
        }
        /// <summary>
        /// Performs a calculation of the data receives 2 parameters for operation
        /// as a data and a parameter which represents an operator
        /// </summary> 
        /// <returns>API string result</returns>
        public string Eval(string val1, string val2, string mathematica_operator)
        {
            string ans = GlobalsSettings.DISCONNECTED;
            if (this.Is_connect)
                ans = PostEval(val1, val2, mathematica_operator);

            return ans;
        }
        private string PostEval(string val1, string val2, string mathematica_operator)
        {
            try
            {
                string url = this.Build_Connection_Url();
                url = String.Join("/", new String[] { url, "auth", "api_eng", "eval_any/" });
                var data = new[]
                {
                    new KeyValuePair<string, string>("val1", val1),
                    new KeyValuePair<string, string>("val2", val2),
                    new KeyValuePair<string, string>("mathematica_operator", mathematica_operator),
                };
                var EncodeData = new FormUrlEncodedContent(data);
                dynamic json = PostRequest(url, EncodeData);
                string result = json["result"];

                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(LogTarget.File, ex.ToString());
            }
            return GlobalsSettings.ERROR_CONNECTION;
        }
        ///<summary>
        ///Get the list of available operators The implementation of new operators is done.
        ///Server side by adding a single class with 2 functions
        ///</summary>
        /// <returns>API string list of operators</returns>
        public string GetOperatorsList()
        {
            string ans = GlobalsSettings.DISCONNECTED;
            if (this.Is_connect)
                ans = PostOperators();

            return ans;
        }
        private string PostOperators()
        {
            try
            {
                string url = this.Build_Connection_Url();
                url = String.Join("/", new String[] { url, "auth", "api_eng", "get_list/" });
                var data = new[]
                {
                    new KeyValuePair<string, string>("option", ""),
                };
                var EncodeData = new FormUrlEncodedContent(data);
                dynamic json = PostRequest(url, EncodeData);
                string result = json["result"];
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(LogTarget.File, ex.ToString());
            }
            return GlobalsSettings.ERROR_LIST;
        }
       
    }
}
