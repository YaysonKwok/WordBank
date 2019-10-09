using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;

namespace WordBank
{
    public partial class Import : System.Web.UI.Page
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.Open();
            if (Session["UsernameID"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (IsPostBack && Upload.HasFile)
            {
                var parser = new TextFieldParser(Upload.FileContent);
                parser.SetDelimiters(new string[] { "," });
                dt.Columns.Add("UserID", typeof(int));
                dt.Columns.Add("Word", typeof(String));
                dt.Columns.Add("Definition", typeof(String));
                dt.Columns.Add("Sentence1", typeof(String));

                foreach (DataRow dr in dt.Rows)
                {
                    dr["UserID"] = Session["UsernameID"];
                }

                while (!parser.EndOfData)
                {
                    string[] fieldData = parser.ReadFields();
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == "")
                        {
                            dt.Rows.Add(fieldData);
                        }
                    }
                    
                }
                var table = "WordBank";
                using (connection)
                {
                    var bulkCopy = new SqlBulkCopy(connection);
                    bulkCopy.DestinationTableName = table;
                    bulkCopy.WriteToServer(dt);
                    Label1.Text = "success";
                }
            }
            else
            {
                Label1.Text = "YOU NEED TO SELECT A FILE";
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            connection.Close();
        }
    }
}