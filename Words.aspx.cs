using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace WordBank {
	public partial class Words : System.Web.UI.Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank"].ConnectionString);
		private const string ASCENDING = " ASC";
		private const string DESCENDING = " DESC";

		protected void Page_Load(object sender, EventArgs e) {
			if (Session["Username"] == null) {
				string OriginalUrl = HttpContext.Current.Request.RawUrl;
				string LoginPageUrl = "~/Account/Login.aspx";
				HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
			}

			if ((bool)Session["EditRedirect"]) {
				Label1.Attributes.Add("class", "alert alert-success");
				Label1.Text = "Entry Successfully Updated";
			}

			if ((bool)Session["LoginRedirect"]) {
				Label1.Attributes.Add("class", "alert alert-success");
				Label1.Text = "You are already logged in, here is your word list.";
			}

			if (!IsPostBack) {
				CheckWordTotal();
				GenerateTable();
			}
		}

		private void CheckWordTotal() {
			connection.Open();
			using (SqlCommand WordCheck = new SqlCommand("SELECT COUNT(*) FROM WordBank WHERE Username = @Username", connection)) {
				WordCheck.Parameters.AddWithValue("@Username", Session["Username"]);
				int WordAmount = (int)WordCheck.ExecuteScalar();
				if (WordAmount < 1) {
					Session["RedirectFromWordList"] = true;
					Response.Redirect("Input.aspx");
				}
			}
			connection.Close();
		}

		protected void GridView_RowDeleting(object sender, GridViewDeleteEventArgs e) {
			connection.Open();
			string Word = GridView.Rows[e.RowIndex].Cells[1].Text;
			using (SqlCommand DeleteWord = new SqlCommand("Delete FROM WordBank WHERE Word= @Word AND Username = @Username", connection)) {
				DeleteWord.Parameters.AddWithValue("@Word", Word);
				DeleteWord.Parameters.AddWithValue("@Username", Session["Username"]);
				DeleteWord.ExecuteNonQuery();
			}
			connection.Close();
			GenerateTable();
		}

		protected void GridView_RowEditing(object sender, GridViewEditEventArgs e) {
			GridView.EditIndex = e.NewEditIndex;
			Session["WordID"] = Convert.ToInt32(GridView.DataKeys[e.NewEditIndex].Value.ToString());
			Label1.Text = Session["WordID"].ToString();
			Response.Redirect("EditWord.aspx");
		}

		protected void GridView_RowUpdating(object sender, GridViewUpdateEventArgs e) {
			connection.Open();
			GridViewRow row = (GridViewRow)GridView.Rows[e.RowIndex];
			TextBox txtWord = (TextBox)row.Cells[1].Controls[0];
			TextBox txtDefinition = (TextBox)row.Cells[2].Controls[0];
			int WordID = Convert.ToInt32(GridView.DataKeys[e.RowIndex].Value.ToString());
			GridView.EditIndex = -1;
			using (SqlCommand Update = new SqlCommand("Update WordBank Set Word = @Word, Definition = @Definition WHERE Username = @Username AND ID = @ID", connection)) {
				Update.Parameters.AddWithValue("@Word", txtWord.Text);
				Update.Parameters.AddWithValue("@Definition", txtDefinition.Text);
				Update.Parameters.AddWithValue("@Username", Session["Username"]);
				Update.Parameters.AddWithValue("@ID", WordID);
				Update.ExecuteNonQuery();
				;
				GenerateTable();
			}
			connection.Close();
		}
		protected void GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
			GridView.EditIndex = -1;
			GenerateTable();
		}

		protected void GridView_Sorting(object sender, GridViewSortEventArgs e) {
			string sortExpression = e.SortExpression;

			if (GridViewSortDirection == SortDirection.Ascending) {
				GridViewSortDirection = SortDirection.Descending;
				SortGridView(sortExpression, DESCENDING);
			}
			else {
				GridViewSortDirection = SortDirection.Ascending;
				SortGridView(sortExpression, ASCENDING);
			}
		}

		private void SortGridView(string sortExpression, string direction) {
			connection.Open();
			using (SqlCommand Data = new SqlCommand("SELECT ID, Word, Definition, Sentence1, Sentence2, CorrectWord, IncorrectWord, CorrectDefinition, IncorrectDefinition, Informal, DateCreated FROM WordBank WHERE Username = @Username", connection)) {
				Data.Parameters.AddWithValue("@Username", Session["Username"]);

				var dataReader = Data.ExecuteReader();
				var dataTable = new DataTable();
				dataTable.Load(dataReader);

				DataView dv = new DataView(dataTable);
				dv.Sort = sortExpression + direction;
				GridView.DataSource = dv;
				GridView.DataBind();

				GridView.HeaderRow.TableSection = TableRowSection.TableHeader;
			}
			connection.Close();
		}

		public SortDirection GridViewSortDirection {
			get {
				if (ViewState["sortDirection"] == null)
					ViewState["sortDirection"] = SortDirection.Ascending;

				return (SortDirection)ViewState["sortDirection"];
			}
			set { ViewState["sortDirection"] = value; }
		}
		protected void GenerateTable() {
			connection.Open();
			using (SqlCommand Data = new SqlCommand("SELECT ID, Word, Definition, Sentence1, Sentence2, CorrectWord, IncorrectWord, CorrectDefinition, IncorrectDefinition, Informal, DateCreated FROM WordBank WHERE Username = @Username", connection)) {
				Data.Parameters.AddWithValue("@Username", Session["Username"]);
				SqlDataReader reader = Data.ExecuteReader();
				GridView.DataSource = reader;
				GridView.DataBind();
				GridView.HeaderRow.TableSection = TableRowSection.TableHeader;
			}
			connection.Close();
		}

		protected void Page_Unload(object sender, EventArgs e) {
			Session["EditRedirect"] = false;
			Session["LoginRedirect"] = false;
			connection.Close();
		}

		protected void ExportCSVBtn_Click(object sender, EventArgs e) {
				using (SqlCommand Export = new SqlCommand("SELECT Word,Informal, Definition, Sentence1 AS Contextual_Sentence, Sentence2 AS Personal_Sentence FROM WordBank WHERE Username = @Username", connection)) {
				Export.Parameters.AddWithValue("@Username", Session["Username"]);
				using (SqlDataAdapter sda = new SqlDataAdapter()) {
						sda.SelectCommand = Export;
						using (DataTable dt = new DataTable()) {
							sda.Fill(dt);

							//Build the CSV file data as a Comma separated string.
							string csv = string.Empty;

							foreach (DataColumn column in dt.Columns) {
								//Add the Header row for CSV file.
								csv += column.ColumnName + ',';
							}

							//Add new line.
							csv += "\r\n";

							foreach (DataRow row in dt.Rows) {
								foreach (DataColumn column in dt.Columns) {
									//Add the Data rows.
									csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
								}

								//Add new line.
								csv += "\r\n";
							}

							//Download the CSV file.
							Response.Clear();
							Response.Buffer = true;
							Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
							Response.Charset = "";
							Response.ContentType = "application/text";
							Response.Output.Write(csv);
							Response.Flush();
							Response.End();
						}
					}
				}
			}
	}
}