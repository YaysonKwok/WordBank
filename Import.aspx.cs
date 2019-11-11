using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;

namespace WordBank {
	public partial class Import : System.Web.UI.Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
		static string UserID = "UserID";
		DataColumn Word = new DataColumn("Word");
		DataColumn Definition = new DataColumn("Definition");
		DataColumn Sentence1 = new DataColumn("Sentence1");
		DataColumn Informal = new DataColumn("Informal");

		protected void Page_Load(object sender, EventArgs e) {
			if (Session["UsernameID"] == null) {
				Response.Redirect("~/Account/Login.aspx");
			}
		}

		protected void UploadBtn_Click(object sender, EventArgs e) {
			connection.Open();
			DataTable DataTable = new DataTable();
			if (IsPostBack && Upload.HasFile) {

				var parser = new TextFieldParser(Upload.FileContent);
				parser.SetDelimiters(new string[] { "," });
				DataTable.Columns.Add(UserID);
				DataTable.Columns[UserID].DefaultValue = Session["UsernameID"];
				DataTable.Columns.Add(Word);
				DataTable.Columns.Add(Definition);
				DataTable.Columns.Add(Sentence1);
				Informal.DataType = System.Type.GetType("System.Boolean");
				DataTable.Columns.Add(Informal);

				while (!parser.EndOfData) {
					string[] fieldData = parser.ReadFields();
					DataRow newRow = DataTable.NewRow();

					newRow[Word] = fieldData[0];
					newRow[Definition] = fieldData[1];
					newRow[Sentence1] = fieldData[2];
					newRow[Informal] = fieldData[3];
					DataTable.Rows.Add(newRow);
				}

				GridView.DataSource = DataTable;
				GridView.DataBind();

				using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
					bulkCopy.DestinationTableName = "WordBank";
					try {
						SqlBulkCopyColumnMapping UserID = new SqlBulkCopyColumnMapping("UserID", "UserID");
						bulkCopy.ColumnMappings.Add(UserID);

						SqlBulkCopyColumnMapping Word = new SqlBulkCopyColumnMapping("Word", "Word");
						bulkCopy.ColumnMappings.Add(Word);

						SqlBulkCopyColumnMapping Definition = new SqlBulkCopyColumnMapping("Definition", "Definition");
						bulkCopy.ColumnMappings.Add(Definition);

						SqlBulkCopyColumnMapping Sentence1 = new SqlBulkCopyColumnMapping("Sentence1", "Sentence1");
						bulkCopy.ColumnMappings.Add(Sentence1);

						SqlBulkCopyColumnMapping Informal = new SqlBulkCopyColumnMapping("Informal", "Informal");
						bulkCopy.ColumnMappings.Add(Informal);
						bulkCopy.WriteToServer(DataTable);
						UploadMessage.Text = "Upload Completed!";
					}
					catch (Exception ex) {
						UploadFailed.Text = ex.Message;
					}

				}
			}
			else {
				UploadFailed.Text = "You need to select a file!";
			}
		}
		protected void Page_Unload(object sender, EventArgs e) {
			connection.Close();
		}
	}
}