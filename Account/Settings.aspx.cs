using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordBank.Account {
	public partial class Settings : System.Web.UI.Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);

		protected void Page_Load(object sender, EventArgs e) {

			if (Session["Username"] == null) {
				Response.Redirect("~/Account/Login.aspx");
			}

			connection.Open();
			using (SqlCommand RedirectQuery = new SqlCommand("SELECT Redirect FROM Login WHERE Username = @Username", connection)) {
				RedirectQuery.Parameters.AddWithValue("@Username", Session["Username"]);
				SqlDataReader dr = RedirectQuery.ExecuteReader();

				if (dr.HasRows) {
					dr.Read();
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
			if (RedirectList.SelectedItem.Value != "None" && NewPassword.Text == String.Empty && ConfirmPassword.Text == String.Empty) {
				using (SqlCommand InsertRedirect = new SqlCommand("UPDATE Login SET Redirect = @Redirect WHERE Username = @Username", connection)) {
					InsertRedirect.Parameters.AddWithValue("@Username", Session["Username"]);
					InsertRedirect.Parameters.AddWithValue("@Redirect", RedirectList.SelectedItem.Value);
					try {
						InsertRedirect.ExecuteNonQuery();
						SubmitResponse.Attributes.Add("class", "alert alert-success");
						SubmitResponse.Text = "Redirect Updated";
					}
					catch (Exception ex) {
						SubmitResponse.Text = ex.ToString();
					}
				}
			}
			//Changing Password
			else if (RedirectList.SelectedItem.Value == "None" && NewPassword.Text != String.Empty && ConfirmPassword.Text != String.Empty && NewPassword.Text == ConfirmPassword.Text) {
				using (SqlCommand InsertPassword = new SqlCommand("UPDATE Login SET Password = @Password WHERE Username = @Username", connection)) {
					InsertPassword.Parameters.AddWithValue("@Username", Session["Username"]);
					InsertPassword.Parameters.AddWithValue("@Password", PasswordHash);
					try {
						InsertPassword.ExecuteNonQuery();
						SubmitResponse.Attributes.Add("class", "alert alert-success");
						SubmitResponse.Text = "Password Updated";
					}
					catch (Exception ex) {
						SubmitResponse.Text = ex.ToString();
					}
				}
			}//Changing Password & Redirect
			else if (RedirectList.SelectedItem.Value != "None" && NewPassword.Text != String.Empty && ConfirmPassword.Text != String.Empty && NewPassword.Text == ConfirmPassword.Text) {
				using (SqlCommand InsertBoth = new SqlCommand("UPDATE Login SET Password = @Password, Redirect = @Redirect  WHERE Username = @Username", connection)) {
					InsertBoth.Parameters.AddWithValue("@Username", Session["Username"]);
					InsertBoth.Parameters.AddWithValue("@Password", PasswordHash);
					InsertBoth.Parameters.AddWithValue("@Redirect", RedirectList.SelectedItem.Value);
					try {
						InsertBoth.ExecuteNonQuery();
						SubmitResponse.Attributes.Add("class", "alert alert-success");
						SubmitResponse.Text = "Profile Updated";
					}
					catch (Exception ex) {
						SubmitResponse.Text = ex.ToString();
					}
				}
			}
			//Error
			else {
				SubmitResponse.Attributes.Add("class", "alert alert-danger");
				SubmitResponse.Text = "New and old passwords don't match!";
			}



			connection.Close();
		}
	}
}