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
			if (Session["Username"] != null) {
				Session["LoginRedirect"] = true;
				Response.Redirect("~/Words.aspx");
			}
			connection.Open();
			Session["EditRedirect"] = false;
			Session["LoginRedirect"] = false;
			Session["RedirectFromPractice"] = false;
			Session["RedirectFromWordList"] = false;
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
				LoginMessage.Text = "";
				ErrorMessage.Text = "";
				using (SqlCommand ExistingUsername = new SqlCommand("SELECT Redirect FROM Login WHERE Username = @Username AND Password = @Password", connection)) {
					ExistingUsername.Parameters.AddWithValue("@Username", Username.Text);
					ExistingUsername.Parameters.AddWithValue("@Password", PasswordHash);
					SqlDataReader dr = ExistingUsername.ExecuteReader();


					if (dr.HasRows) {
						Session["Username"] = Username.Text;
						Session["InputRedirect"] = false;
						if (this.Request.QueryString["ReturnUrl"] != null) {
							this.Response.Redirect(Request.QueryString["ReturnUrl"].ToString());
						}
						else {
							dr.Read();
							Response.Redirect("~/" + dr.GetValue(0) + ".aspx");
						}
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