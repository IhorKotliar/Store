using System;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
namespace Store
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlCommand cmd;
        public Form1()
        {
            InitializeComponent();
            //textBoxLogin.();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Store\Store\Store\DatabaseStore.mdf;Integrated Security=True";

            connection = new SqlConnection(conStr);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
            MessageBox.Show("Connection Successfull");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            cmd.Connection = connection;
            Random rnd = new Random();
            cmd.CommandText = "INSERT INTO JOR_SALE(Man_Id,Prod_Id,TimeSale,Cnt) VALUES ";
            for (int i = 0; i < 100; i++)
            {
                /*
                 Подзапрос - извлечь случайный Id из таблицы:
                 SELECT TOP 1 Id FROM DIC_MANAGERS ORDER BY NEWID()
                 Сформировать случайную дату
                 '2017-01-01' + DATEADD(day, (ABS(CHECKSUM(NEWID())) % 365), 0)+ DATEADD(hour, (ABS(CHECKSUM(NEWID())) % 24), 0)+ DATEADD(minute, (ABS(CHECKSUM(NEWID())) % 60), 0)
                 На основе него заполняем журнал случайными записями
                  */
                cmd.CommandText += @"((SELECT TOP 1 Id FROM DIC_MANAGERS ORDER BY NEWID()),
                                        (SELECT TOP 1 Id FROM DIC_PRODUCTS ORDER BY NEWID()),
                                        ('2017-01-01' + DATEADD(day, (ABS(CHECKSUM(NEWID())) % 365), 0)+ DATEADD(hour, (ABS(CHECKSUM(NEWID())) % 24), 0)+ DATEADD(minute, (ABS(CHECKSUM(NEWID())) % 60), 0))," +
                                        rnd.Next(100) + "),";
            }
            cmd.CommandText += @"((SELECT TOP 1 Id FROM DIC_MANAGERS ORDER BY NEWID()),
                                        (SELECT TOP 1 Id FROM DIC_PRODUCTS ORDER BY NEWID()),
                                        ('2017-01-01' + DATEADD(day, (ABS(CHECKSUM(NEWID())) % 365), 0)+ DATEADD(hour, (ABS(CHECKSUM(NEWID())) % 24), 0)+ DATEADD(minute, (ABS(CHECKSUM(NEWID())) % 60), 0))," +
                                        rnd.Next(100) + ")";//без запятой в конце
            try { cmd.ExecuteNonQuery(); }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            MD5 md = MD5.Create();
            string hash = GetMd5Hash(md, textBoxPassword.Text);
           // textBoxLogin.Text = hash;
            cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "Select name from DIC_MANAGERS where login=N'" + textBoxLogin.Text + "' and password =N'" + hash + "'";

            //MessageBox.Show(hash);
            SqlDataReader ans = null;
            try
            {
                ans = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
            if (ans.HasRows)
            {
                ans.Read();
                MessageBox.Show("hello " + ans.GetString(0));
                ans.Close();
                Form2 f = new Form2(connection,cmd,ans);
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("?");
                ans.Close();
            }
           
        }
       
    }

}
