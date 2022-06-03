using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Threading;
using System.Text;

namespace PROG___POE
{
    public partial class LoginPage : System.Web.UI.Page
    {
        static string Encrypt(string value)
        {
            //used to encrypt password
            using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = MD5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            Thread th = Thread.CurrentThread;

            SqlConnection con = new SqlConnection(@"Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");
            string encryptedPassword = Encrypt(txtPassword.Text);

            con.Open();
            //SQL Query to login
            String query = "SELECT COUNT(1) FROM Login WHERE Student_ID=@Student_ID AND Student_Password=@Student_Password";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Student_ID", txtUsername.Text);
            cmd.Parameters.AddWithValue("@Student_Password", encryptedPassword);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            //Loops used to perform certain actions like grant access or give an error message
            if (count == 1)
            {
                //used to open Main window
                Response.Redirect("MainWindow.aspx");
            }
            else
            {
                Response.Write("<script>alert('Incorrect Username and Password Combination');</script>");
            }

            try
            {

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void BtnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                string encryptedPassword = Encrypt(txtPassword.Text);
                SqlConnection con = new SqlConnection("Data Source=ravjee;Initial Catalog=Time_Management_App;Integrated Security=True");

                con.Open();
                //SQL Query to create a new user
                SqlCommand cmd2 = new SqlCommand(@"INSERT INTO [dbo].[Login]
                                                         ([Student_ID]
                                                         ,[Student_Password])
                                                    VALUES
                                                         ('" + txtUsername.Text + "', '" + encryptedPassword + "');", con);
                SqlDataReader dr = cmd2.ExecuteReader();
                con.Close();

                //Message displayed to show user their username or password
                Response.Write("Username: " + txtUsername.Text + "\nPassword: " + txtPassword.Text);
                Response.Write("<script>alert('Account Successfully Created');</script>");

            }
            catch
            {
                Response.Write("<script>alert('Username is Taken');</script>");
            }
        }
    }
}