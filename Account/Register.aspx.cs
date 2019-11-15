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

		protected void Page_Load(object sender, EventArgs e) {

		}

		private void Clear() {
			ErrorMessage.Text = "";
			SuccessMessage.Text = "";
		}

		protected void CreateUser_Click(object sender, EventArgs e)
        {
			connection.Open();
			Clear();
			if (Username.Text != null && Password.Text != null) {
				var sha256 = new SHA256Managed();
				var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(Password.Text);
				var hash = sha256.ComputeHash(bytes);
				string PasswordHash = Convert.ToBase64String(hash);
				//Label1.Text = PasswordHash;
				using (SqlCommand UsernameCheck = new SqlCommand("SELECT COUNT(*) FROM Login WHERE Username = @Username", connection)) {
					UsernameCheck.Parameters.AddWithValue("@Username", Username.Text);
					if((int)UsernameCheck.ExecuteScalar() < 1) {
						using (SqlCommand RegisterNewUsername = new SqlCommand("INSERT INTO Login(Username, Password) VALUES(@Username, @Password)", connection)) {
							RegisterNewUsername.Parameters.AddWithValue("@Username", Username.Text);
							RegisterNewUsername.Parameters.AddWithValue("@Password", PasswordHash);
							RegisterNewUsername.ExecuteNonQuery();
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
						SuccessMessage.Text = "Successfully Registered";
					}
					else {
						ErrorMessage.Text = "Username taken, please pick another.";
						connection.Close();
					}

				}
			}

		}
	}
}