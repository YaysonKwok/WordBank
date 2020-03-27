using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordBank {
	public partial class Input : System.Web.UI.Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank"].ConnectionString);
		protected void Page_Load(object sender, EventArgs e) {
			if (Session["Username"] == null) {
				string OriginalUrl = HttpContext.Current.Request.RawUrl;
				string LoginPageUrl = "~/Account/Login.aspx";
				HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
			}

			if ((bool)Session["RedirectFromPractice"]) {
				Redirectlbl.Text = "Please add at least 5 words before practicing";
				Redirectlbl.Attributes.Add("class", "alert alert-danger");
				Redirectlbl.Visible = true;
			}

			if ((bool)Session["RedirectFromWordList"]) {
				Redirectlbl.Text = "Your word list is empty, add at least 1 word here!";
				Redirectlbl.Attributes.Add("class", "alert alert-danger");
				Redirectlbl.Visible = true;
			}
		}

		protected void SubmitBtn_Click(object sender, EventArgs e) {
			if (WordInput.Text == String.Empty) {
				SubmitResponse.Attributes.Add("class", "alert alert-danger");
				SubmitResponse.Text = "You cannot submit an empty word";
			}
			else {
				connection.Open();
				using (SqlCommand WordCheck = new SqlCommand("SELECT COUNT(*) FROM WordBank WHERE Username = @Username AND Word = @WordCheck", connection)) {
					WordCheck.Parameters.AddWithValue("@Username", Session["Username"]);
					WordCheck.Parameters.AddWithValue("@WordCheck", WordInput.Text);
					WordCheck.ExecuteScalar();

					if ((int)WordCheck.ExecuteScalar() != 1) {
						InsertWord();
					}
					else {
						SubmitResponse.Attributes.Add("class", "alert alert-danger");
						SubmitResponse.Text = "You already have this word saved!";
					}
				}
				connection.Close();
			}
		}

		private void Clear() {
			WordInput.Text = "";
			DefinitionInput.Text = "";
			Sentence1Input.Text = "";
		}

		protected void Page_Unload(object sender, EventArgs e) {
			Session["RedirectFromPractice"] = false;
			Session["RedirectFromWordList"] = false;
			Redirectlbl.Visible = false;
		}

		protected void InsertWord() {
			connection.Open();
			using (SqlCommand Insert = new SqlCommand("INSERT INTO WordBank(Username, Word, Definition, Sentence1, Sentence2, Informal) VALUES(@Username,@Word,@Definition,@Sentence1, @Sentence2, @Informal)", connection)) {
				Insert.Parameters.AddWithValue("@Username", Session["Username"]);
				Insert.Parameters.AddWithValue("@Word", WordInput.Text);
				Insert.Parameters.AddWithValue("@Definition", DefinitionInput.Text);
				Insert.Parameters.AddWithValue("@Sentence1", Sentence1Input.Text);
				Insert.Parameters.AddWithValue("@Sentence2", Sentence2Input.Text);
				Insert.Parameters.AddWithValue("@Informal", InformalCheckBox.Checked);
				try {
					Insert.ExecuteNonQuery();
					SubmitResponse.Attributes.Add("class", "alert alert-success");
					SubmitResponse.Text = "Word Added! You can add another.";
					Redirectlbl.Visible = false;
					Clear();
				}
				catch (Exception ex) {
					SubmitResponse.Text = ex.Message;
				}
			}
			connection.Close();
		}
	}
}