using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Store
{
    public partial class Form2 : Form
    {
        SqlConnection connection;
        SqlCommand cmd;
        SqlDataReader ans;
        public Form2(SqlConnection con,SqlCommand cm, SqlDataReader an)
        {
            InitializeComponent();
            connection = con;
            cmd = cm;
            ans = an;
        }

        private void managers_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            cmd.CommandText = "Select name from DIC_MANAGERS order by Name";
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
                while (ans.Read())
                {
                    comboBox1.Items.Add(ans.GetString(0));
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("?");
            }
            ans.Close();
           
        }

        private void products_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            cmd.CommandText = "Select Title,Price from DIC_PRODUCTS order by Title";
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
                while (ans.Read())
                {
                    comboBox1.Items.Add(string.Format("{0}\t{1}", ans.GetString(0),ans.GetValue(1)));
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("?");
            }
            ans.Close();

        }

        private void sales_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            cmd.CommandText = "Select m.Name,p.Title,p.Price,s.Cnt,s.TimeSale,a.Title,(p.Price*s.Cnt)as sum from JOR_SALE as s " +
                " join DIC_MANAGERS as m on s.Man_Id=m.Id " +
                "join DIC_PRODUCTS as p on s.Prod_Id=p.Id " +
                "join DIC_ACTIONS as a on s.Action_Id= a.Id order by s.Cnt desc,s.TimeSale";
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
                while (ans.Read())
                {
                    comboBox1.Items.Add(string.Format("Менеджер {0} продал {1}  {2} грн   {3} шт   {4}  ,акция {5}......{6} грн", ans.GetString(0),
                        ans.GetString(1),ans.GetValue(2),ans.GetInt32(3),ans.GetDateTime(4),ans.GetString(5),ans.GetValue(6)));
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("?");
            }
            ans.Close();

        }

        private void action_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            cmd.CommandText = "Select Discount,Title from DIC_ACTIONS order by Discount";
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
                while (ans.Read())
                {
                    comboBox1.Items.Add(string.Format("{0} %\t{1}", ans.GetValue(0), ans.GetValue(1)));
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("?");
            }
            ans.Close();
        }
    }
}
