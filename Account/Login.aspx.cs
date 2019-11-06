using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;

namespace WordBank.Account {
	public partial class Login : Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);

		protected void Page_Load(object sender, EventArgs e) {
			connection.Open();
			Session["EditRedirect"] = false;
			Session["ReSort"] = true;
			Session["ReSortCounter"] = 0;
			RegisterHyperLink.NavigateUrl = "Register";
			OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
			var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
			if (!String.IsNullOrEmpty(returnUrl)) {
				RegisterHyperLink.NavigateUrl = "?ReturnUrl=" + returnUrl;
			}
		}

		protected void LogIn(object sender, EventArgs e) {
			var sha256 = new SHA256Managed();
			var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(Password.Text);
			var hash = sha256.ComputeHash(bytes);
			string PasswordHash = Convert.ToBase64String(hash);

			Label1.Text = PasswordHash;
			if (Email.Text != null ) {
				using (SqlCommand ExistingUsername = new SqlCommand("SELECT Id FROM Login WHERE Username = @Username AND Password = @Password", connection)) {
					ExistingUsername.Parameters.AddWithValue("@Username", Email.Text);
					ExistingUsername.Parameters.AddWithValue("@Password", PasswordHash);
					SqlDataReader dr = ExistingUsername.ExecuteReader();

					if (dr.HasRows) {
						dr.Read();
						Session["UsernameID"] = dr.GetValue(0);
						Session["Username"] = Email.Text;
						Session["InputRedirect"] = false;
						
						Label1.Text = "You have logged in!";
						connection.Close();
					}
					else {
						Label1.Text = "Incorrect Username or Password";
						connection.Close();
					}


				}
			}
			else {

			}
		}

		protected void Page_Unload(object sender, EventArgs e) {
			connection.Close();
		}
	}
}