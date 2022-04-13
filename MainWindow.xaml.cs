using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace client_Ari
{
    /// <summary>
    /// S-Developer : Asaf Louk
    /// Interaction logic for MainWindow.xaml
    /// Long numbers are represented on a scientific method from Mentisa : 1.4035376053483076E+37
    /// </summary>
    public partial class MainWindow : Window
    {
        // Server Controller
        ServerAPI local_server;
        public MainWindow()
        {
            InitializeComponent();
            init();
        }
        /// <summary>
        /// Creates a connection to the server and also receives the list of usable operators
        /// </summary>
        private void init()
        {
            this.local_server = new ServerAPI();
            if(local_server != null && local_server.IsReady())
            {
                string tosplit = local_server.GetOperatorsList();
                string [] splits_strs = tosplit.Split(",");
                if(splits_strs.Length > 0)
                    foreach (string str in splits_strs)
                        operator_combobox.Items.Add(str);
                else
                    operator_combobox.Items.Add("no operators loads");
                return;
            }
        }
        /// <summary>
        /// Performs a client-side application with data on the server-side REST_API.
        /// Calculation receives two data and an operator mark All the data is passed as strings
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="mathematica_operator"></param>
        /// <returns></returns>
        private string Eval(string val1, string val2, string mathematica_operator)
        {
            if (local_server != null && local_server.IsReady())
            {
                return local_server.Eval(val1, val2, mathematica_operator);
            }
            return GlobalsSettings.DISCONNECTED;
        }
        //Events ==>
        private void submit_Click(object sender, RoutedEventArgs e)
        {
            string val1 = val1_txtbox.Text;
            string val2= val2_txtbox.Text;
            string mathematica_operator = operator_combobox.SelectedItem.ToString();
            result_txtbox.Text =  Eval(val1, val2, mathematica_operator);
        }

        private void validate_decimal(object sender, TextChangedEventArgs e)
        {

        }
    }
}
