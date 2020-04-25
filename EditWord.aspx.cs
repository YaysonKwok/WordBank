using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace WordBank {
	public partial class EditWord : System.Web.UI.Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank"].ConnectionString);
		protected void Page_Load(object sender, EventArgs e) {
			if (Session["Username"] == null) {
				string OriginalUrl = HttpContext.Current.Request.RawUrl;
				string LoginPageUrl = "~/Account/Login.aspx";
				HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
			}

			if (!IsPostBack) {
				LoadData();
			}
		}

		private void LoadData() {
			connection.Open();
			using (SqlCommand LoadData = new SqlCommand("SELECT Word, Definition, Sentence1, Sentence2, Informal FROM WordBank WHERE ID = @WordID", connection)) {
				LoadData.Parameters.AddWithValue("@WordID", Session["WordID"]);
				using (var reader = LoadData.ExecuteReader()) {
					while (reader.Read()) {
						WordInput.Text = reader.GetString(reader.GetOrdinal("Word"));
						WordTitle.Text = reader.GetString(reader.GetOrdinal("Word"));
						DefinitionInput.Text = reader.GetString(reader.GetOrdinal("Definition"));
						Sentence1Input.Text = reader.GetString(reader.GetOrdinal("Sentence1"));
						Sentence2Input.Text= reader.GetString(reader.GetOrdinal("Sentence2"));
						if (reader.GetBoolean(reader.GetOrdinal("Informal"))){
							InformalCheckBox.Checked = true;
						}

					}
				}
			}
			connection.Close();
		}

		protected void EditWord_Click(object sender, EventArgs e) {
			connection.Open();
			using (SqlCommand Edit = new SqlCommand("Update WordBank Set Word = @Word, Definition = @Definition, Sentence1 = @Sentence1, Sentence2 = @Sentence2, Informal = @Informal WHERE ID = @WordID", connection)) {
				Edit.Parameters.AddWithValue("@Word", WordInput.Text);
				Edit.Parameters.AddWithValue("@Definition", DefinitionInput.Text);
				Edit.Parameters.AddWithValue("@Sentence1", Sentence1Input.Text);
				Edit.Parameters.AddWithValue("@Sentence2", Sentence2Input.Text);
				Edit.Parameters.AddWithValue("@Informal", InformalCheckBox.Checked);
				Edit.Parameters.AddWithValue("@WordID", Session["WordID"]);
				// connection.Open(); Canny: v0.1.3, Opening twice caused "The connection was not closed" exception.
				try {
					Edit.ExecuteNonQuery();
					SubmitResponse.Attributes.Add("class", "alert alert-success");
					SubmitResponse.Text = "Successfully Updated";
					Session["EditRedirect"] = true;
					Response.Redirect("Words.aspx");
				}
				catch (Exception ex) {
					SubmitResponse.Text = ex.Message;
				}
				
			}
			connection.Close();
		}

		protected void Cancel_Click(object sender, EventArgs e) {
			Response.Redirect("Words.aspx");
		}

		protected void Page_Unload(object sender, EventArgs e) {
			connection.Close();
		}
	}
}