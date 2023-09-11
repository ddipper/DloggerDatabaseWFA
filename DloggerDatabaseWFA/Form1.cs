using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.CodeDom;
using Telegram.Bot;
using System;
using Microsoft.Extensions.Configuration;

namespace DloggerDatabaseWFA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            lbTime.Text = DateTime.Now.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var id = tbId.Text;
            var name = tbName.Text;
            var time = DateTime.Now.ToString();

            try // try for "Action"
            {
                var action = cmbAction.SelectedItem.ToString();
                var club = cmbClub.SelectedItem.ToString();
                MessageBox.Show($@"Save on D:\Prog\Log.txt", "ddipper");
                WriteLog(id, name, action, club);
                TelegramBotAsync(id, name, action, club);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select action or club", "System");
            }

            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=Lenovo_s340\\SQLEXPRESS01;Initial Catalog=Dlogger_control_db;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "insert into dbo.Users (Id, Name, Time, Action, Club) values (@id, @name, @time, @action, @club)";
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@id", tbId.Text);
            cmd.Parameters.AddWithValue("@name", tbName.Text);
            cmd.Parameters.AddWithValue("@time", DateTime.Now);
            cmd.Parameters.AddWithValue("@action", cmbAction.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@club", cmbClub.SelectedItem.ToString());
            if (cmd.ExecuteNonQuery() > 0)
                MessageBox.Show("Row Update");
            con.Close();



        }

        private void WriteLog(string id, string name, string action, string club)
        {
            if (id == String.Empty)
                id = "none";
            if (name == String.Empty)
                name = "none";

            string log = $"Id: {id}\n" +
                         $"Name: {name}\n" +
                         $"Time: {DateTime.Now}\n" +
                         $"Action: {action}\n" +
                         $"Club: {club}";

            string path = @"D:\Prog\Logs\log.txt";

            File.WriteAllText(path, log);
        }

        private async Task TelegramBotAsync(string id, string name, string action, string club)
        {
            var idMe = 892871556;
            var idNik = 525820323;

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("config.yml")
                .Build();


            var client = new TelegramBotClient(config["token"]);
            using CancellationTokenSource cts = new();

            var me = await client.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");

            string log = $"Id: {id}\n" +
             $"Name: {name}\n" +
             $"Time: {DateTime.Now}\n" +
             $"Action: {action}\n" +
             $"Club: {club}";

            client.SendTextMessageAsync(chatId: 892871556, text: $"{log}\nby ddipper with love <3");
            client.SendTextMessageAsync(chatId: 525820323, text: $"{log}\nby ddipper with love <3");

        }
    }
}