using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace WordBank.Account {
	public partial class Settings : System.Web.UI.Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
		protected int resortValue { get; set; }
		protected void Page_Load(object sender, EventArgs e) {

			if (Session["Username"] == null) {
				Response.Redirect("~/Account/Login.aspx");
			}

			connection.Open();
			using (SqlCommand RedirectQuery = new SqlCommand("SELECT Redirect, Resort FROM Login WHERE Username = @Username", connection)) {
				RedirectQuery.Parameters.AddWithValue("@Username", Session["Username"]);
				SqlDataReader DataReader = RedirectQuery.ExecuteReader();

				if (DataReader.HasRows) {
					DataReader.Read();
					this.resortValue = (int) DataReader.GetValue(1);
				}
				connection.Close();
			}
		}
		protected void SubmitBtn_Click(object sender, EventArgs e) {
			connection.Open();
			var sha256 = new SHA256Managed();
			var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(NewPassword.Text);
			var hash = sha256.ComputeHash(bytes);
			string PasswordHash = Convert.ToBase64String(hash);
			//Changing redirect
			if (RedirectList.SelectedItem.Value != "None") {
				using (SqlCommand InsertRedirect = new SqlCommand("UPDATE Login SET Redirect = @Redirect WHERE Username = @Username", connection)) {
					InsertRedirect.Parameters.AddWithValue("@Username", Session["Username"]);
					InsertRedirect.Parameters.AddWithValue("@Redirect", RedirectList.SelectedItem.Value);
					try {
						InsertRedirect.ExecuteNonQuery();
						RedirectResponse.Attributes.Add("class", "alert alert-success");
						RedirectResponse.Text = "Redirect Updated";
					}
					catch (Exception ex) {
						RedirectResponse.Text = ex.ToString();
					}
				}
			}
			//Changing Password
			if (NewPassword.Text != String.Empty && ConfirmPassword.Text != String.Empty && NewPassword.Text == ConfirmPassword.Text) {
				using (SqlCommand InsertPassword = new SqlCommand("UPDATE Login SET Password = @Password WHERE Username = @Username", connection)) {
					InsertPassword.Parameters.AddWithValue("@Username", Session["Username"]);
					InsertPassword.Parameters.AddWithValue("@Password", PasswordHash);
					try {
						InsertPassword.ExecuteNonQuery();
						PasswordResponse.Attributes.Add("class", "alert alert-success");
						PasswordResponse.Text = "Password Updated";
					}
					catch (Exception ex) {
						PasswordResponse.Text = ex.ToString();
					}
				}
			}

			if (ResortAmount.Text != String.Empty) {
				using (SqlCommand InsertResort = new SqlCommand("Update Login SET Resort = @Resort WHERE Username = @Username", connection)) {
					InsertResort.Parameters.AddWithValue("@Username", Session["Username"]);
					InsertResort.Parameters.AddWithValue("@Resort", ResortAmount.Text);
					try {
						InsertResort.ExecuteNonQuery();
						ResortResponse.Attributes.Add("class", "alert alert-success");
						ResortResponse.Text = "Password Updated";
					}
					catch (Exception ex) {
						ResortResponse.Text = ex.ToString();
					}
				}
			}

			connection.Close();
		}
	}
}