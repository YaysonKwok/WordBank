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
			if (Session["UsernameID"] != null) {
				Response.Redirect("~/Words.aspx");
			}
			connection.Open();
			Session["EditRedirect"] = false;
			Session["ReSort"] = true;
			Session["ReSortCounter"] = 0;
			RegisterHyperLink.NavigateUrl = "Register";
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

			//LoginMessage.Text = PasswordHash;
			if (Username.Text != null ) {
				using (SqlCommand ExistingUsername = new SqlCommand("SELECT Id FROM Login WHERE Username = @Username AND Password = @Password", connection)) {
					ExistingUsername.Parameters.AddWithValue("@Username", Username.Text);
					ExistingUsername.Parameters.AddWithValue("@Password", PasswordHash);
					SqlDataReader dr = ExistingUsername.ExecuteReader();

					if (dr.HasRows) {
						dr.Read();
						Session["UsernameID"] = dr.GetValue(0);
						Session["Username"] = Username.Text;
						Session["InputRedirect"] = false;

						LoginMessage.Text = "You have logged in!";
						connection.Close();
					}
					else {
						ErrorMessage.Text = "Incorrect Username or Password";
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