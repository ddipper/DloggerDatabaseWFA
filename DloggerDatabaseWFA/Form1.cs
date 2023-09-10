using System.IO;

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select action or club", "System");
            }
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
    }
}