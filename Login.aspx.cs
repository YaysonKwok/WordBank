using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace WordBank
{
    public partial class Login : System.Web.UI.Page
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            connection.Open();
        }

        protected void SignInBtn_Click(object sender, EventArgs e)
        {
            SqlCommand ExistingUsername = new SqlCommand("SELECT Id FROM Username WHERE Username = @Username", connection);
            ExistingUsername.Parameters.AddWithValue("@Username", UsernameInput.Text);

            if (ExistingUsername.ExecuteScalar() == null)
            {
                SqlCommand NewUsername = new SqlCommand("INSERT INTO Username(Username) VALUES(@Username)", connection);
                NewUsername.Parameters.AddWithValue("@Username", UsernameInput.Text);
                NewUsername.ExecuteNonQuery();
                SqlDataReader dr = ExistingUsername.ExecuteReader();
                dr.Read();
                Session["Username"] = UsernameInput.Text;
                Session["UsernameID"] = dr.GetValue(0);
                Loginlbl.Text = "Created new user as " + UsernameInput.Text + " with id:" + dr.GetValue(0);
            }
            else
            {
                SqlDataReader dr = ExistingUsername.ExecuteReader();
                dr.Read();
                Session["UsernameID"] = dr.GetValue(0);
                Session["Username"] = UsernameInput.Text;
                Loginlbl.Text = "Logged in as " + UsernameInput.Text + " with id:" + dr.GetValue(0) + " You will now be redirected in 5 seconds";

                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "5;url=WordPractice.aspx";
                this.Page.Controls.Add(meta);
            }

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            connection.Close();
        }
    }
}