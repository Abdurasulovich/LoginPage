using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
using Npgsql;
using LogInPage.Constans;
using System.Threading.Tasks;

namespace LogInPage
{
       
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
       
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Mouse_enter(object sender, MouseEventArgs e)
        {

        }
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private async void Loginbtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text != "")
            {
                if (txtPassword.Password != "")
                {
                    var connection = new NpgsqlConnection(DbConstants.DB_CONNECTION_STRING);
                    await connection.OpenAsync();
                    string query = $"select * from users where email='{txtEmail.Text}';";
                    var command = new NpgsqlCommand(query, connection);

                    NpgsqlDataReader dr = command.ExecuteReader();

                    var emailF = "";
                    var passHash = "";
                    if (dr.HasRows)
                    {
                        dr.Read();
                        emailF = dr["email"].ToString();
                        passHash = dr["password_hash"].ToString();
                    }
                    if (emailF == "")

                        MessageBox.Show("Bunday email mavjud emas!" +
                            "Emailingizni tekshirib qaytadan kiriting!");

                    if (passHash != "")
                    {
                        if (passHash == ComputeSha256Hash(txtPassword.Password))
                            MessageBox.Show("Qaytganingizdan mamnunmiz!");

                    }
                    else
                        MessageBox.Show("Email yoki parol xato kiritilgan!");
                    connection.Close();
                }
            }
        }

        private void Muselftbtn_Down(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Muselftbtn_Downs(object sender, MouseButtonEventArgs e)
        {
            
        }

    }
}
