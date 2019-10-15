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
	public partial class Words : System.Web.UI.Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
		private const string ASCENDING = " ASC";
		private const string DESCENDING = " DESC";
		

		protected void Page_Load(object sender, EventArgs e) {
			if (Session["UsernameID"] == null) {
				Response.Redirect("~/Account/Login.aspx");
			}

			if ((bool)Session["EditRedirect"]) {
				Label1.Attributes.Add("class", "alert alert-success");
				Label1.Text = "Successfully Updated";
			}

			if (!IsPostBack) {
				GenerateTable();
			}
		}

		protected void GridView_RowDeleting(object sender, GridViewDeleteEventArgs e) {
			connection.Open();
			string Word = GridView.Rows[e.RowIndex].Cells[1].Text;
			using (SqlCommand DeleteWord = new SqlCommand("Delete FROM WordBank WHERE Word= @Word AND UserID = @UsernameID", connection)) {
				DeleteWord.Parameters.AddWithValue("@Word", Word);
				DeleteWord.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
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
			using (SqlCommand Update = new SqlCommand("Update WordBank Set Word = @Word, Definition = @Definition WHERE UserID = @UsernameID AND ID = @ID", connection)) {
				Update.Parameters.AddWithValue("@Word", txtWord.Text);
				Update.Parameters.AddWithValue("@Definition", txtDefinition.Text);
				Update.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
				Update.Parameters.AddWithValue("@ID", WordID);
				Update.ExecuteNonQuery();
				connection.Close();
				GenerateTable();
			}
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
			using (SqlCommand Data = new SqlCommand("SELECT ID, Word, Definition, Sentence1, CorrectWord, WordAttempts, CorrectDefinition, DefinitionAttempts, Informal, DateCreated FROM WordBank WHERE UserID = @UsernameID", connection)) {
				Data.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);

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
			using (SqlCommand Data = new SqlCommand("SELECT ID, Word, Definition, Sentence1, CorrectWord, WordAttempts, CorrectDefinition, DefinitionAttempts, Informal, DateCreated FROM WordBank WHERE UserID = @UsernameID", connection)) {
				Data.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
				SqlDataReader reader = Data.ExecuteReader();
				GridView.DataSource = reader;
				GridView.DataBind();
				GridView.HeaderRow.TableSection = TableRowSection.TableHeader;
			}
			connection.Close();
		}

		protected void Page_Unload(object sender, EventArgs e) {
			Session["EditRedirect"] = false;
		}
	}
}