﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;

namespace WordBank {
	public partial class Import : System.Web.UI.Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank"].ConnectionString);
		static string Username = "Username";
		DataColumn Word = new DataColumn("Word");
		DataColumn Definition = new DataColumn("Definition");
		DataColumn Sentence1 = new DataColumn("Sentence1");
		string Sentence2 = "Sentence2";
		DataColumn Informal = new DataColumn("Informal");

		protected void Page_Load(object sender, EventArgs e) {
			if (Session["Username"] == null) {
				string OriginalUrl = HttpContext.Current.Request.RawUrl;
				string LoginPageUrl = "~/Account/Login.aspx";
				HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
			}
		}

		protected void UploadBtn_Click(object sender, EventArgs e) {
			UploadMessage.Text = "";
			UploadFailed.Text = "";
			connection.Open();
			DataTable DataTable = new DataTable();
			if (IsPostBack && Upload.HasFile) {

				var parser = new TextFieldParser(Upload.FileContent);
				parser.SetDelimiters(new string[] { "," });
				DataTable.Columns.Add(Username);
				DataTable.Columns[Username].DefaultValue = Session["Username"];
				DataTable.Columns.Add(Word);
				DataTable.Columns.Add(Definition);
				DataTable.Columns.Add(Sentence1);
				DataTable.Columns.Add(Sentence2);
				DataTable.Columns[Sentence2].DefaultValue = "";
				DataTable.Columns.Add(Informal);

				if (TitleCheckBox.Checked) {
					parser.ReadLine();
				}
				try {
					while (!parser.EndOfData) {
						string[] fieldData = parser.ReadFields();
						DataRow newRow = DataTable.NewRow();

						newRow[Word] = fieldData[0];
						if (fieldData[1].ToLower() == "yes" || fieldData[1].ToLower() == "true" || fieldData[1].ToLower() == "y") {
							newRow[Informal] = true;
						}
						else {
							newRow[Informal] = false;
						}
						newRow[Definition] = fieldData[2];
						newRow[Sentence1] = fieldData[3];
						newRow[Sentence2] = fieldData[4];
						DataTable.Rows.Add(newRow);
					}

					GridView.DataSource = DataTable;
					GridView.DataBind();

					using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
						bulkCopy.DestinationTableName = "WordBank_Staging";
						try {
							SqlBulkCopyColumnMapping Username = new SqlBulkCopyColumnMapping("Username", "Username");
							bulkCopy.ColumnMappings.Add(Username);

							SqlBulkCopyColumnMapping Word = new SqlBulkCopyColumnMapping("Word", "Word");
							bulkCopy.ColumnMappings.Add(Word);

							SqlBulkCopyColumnMapping Informal = new SqlBulkCopyColumnMapping("Informal", "Informal");
							bulkCopy.ColumnMappings.Add(Informal);

							SqlBulkCopyColumnMapping Definition = new SqlBulkCopyColumnMapping("Definition", "Definition");
							bulkCopy.ColumnMappings.Add(Definition);

							SqlBulkCopyColumnMapping Sentence1 = new SqlBulkCopyColumnMapping("Sentence1", "Sentence1");
							bulkCopy.ColumnMappings.Add(Sentence1);

							SqlBulkCopyColumnMapping Sentence2 = new SqlBulkCopyColumnMapping("Sentence2", "Sentence2");
							bulkCopy.ColumnMappings.Add(Sentence2);

							bulkCopy.WriteToServer(DataTable);
							UploadMessage.Text = "Upload Completed!";
						}
						catch (Exception ex) {
							UploadFailed.Text = ex.Message;
						}
					}
				}
				catch (Exception ex) {
					UploadFailed.Text = ex.Message;
				}

				using (SqlCommand Merge = new SqlCommand("INSERT INTO WordBank(Username, Word, Definition, Sentence1, Sentence2, Informal) SELECT Username, Word, Definition, Sentence1, Sentence2, Informal FROM WordBank_Staging WHERE NOT EXISTS (SELECT WORD FROM WordBank WHERE WordBank.Word = WordBank_Staging.Word AND WordBank.Username = WordBank_Staging.Username);", connection)) {
					Merge.ExecuteNonQuery();
				}

				using (SqlCommand EmptyStagingTable = new SqlCommand("DELETE FROM WordBank_Staging", connection)) {
					EmptyStagingTable.ExecuteNonQuery();
				}
			}
			else {
				UploadFailed.Text = "You need to select a file!";
			}
			connection.Close();
		}
		protected void Page_Unload(object sender, EventArgs e) {
			connection.Close();
		}
	}
}