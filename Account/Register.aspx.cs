using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.UI;

namespace WordBank.Account
{
    public partial class Register : Page
    {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
		protected void CreateUser_Click(object sender, EventArgs e)
        {
			connection.Open();

			if (Username.Text != null && Password.Text != null) {
				var sha256 = new SHA256Managed();
				var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(Password.Text);
				var hash = sha256.ComputeHash(bytes);
				string PasswordHash = Convert.ToBase64String(hash);
				Label1.Text = PasswordHash;
				using (SqlCommand UsernameCheck = new SqlCommand("IF NOT EXISTS(SELECT 1 FROM Login WHERE Username = @Username) INSERT INTO Login (Username,Password) VALUES (@Username, @Password)", connection)) {
					UsernameCheck.Parameters.AddWithValue("@Username", Username.Text);
					UsernameCheck.Parameters.AddWithValue("@Password", PasswordHash);
					UsernameCheck.ExecuteNonQuery();
					
				}

				using (SqlCommand ExistingUsername = new SqlCommand("SELECT Id FROM Login WHERE Username = @Username", connection)) {
					ExistingUsername.Parameters.AddWithValue("@Username", Username.Text);
					SqlDataReader dr = ExistingUsername.ExecuteReader();
					dr.Read();
					Session["UsernameID"] = dr.GetValue(0);
					Session["Username"] = Username.Text;
					Session["InputRedirect"] = false;
					connection.Close();

				}

			}

		}
	}
}