using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordBank
{
    public partial class Words : System.Web.UI.Page
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.Open();
            if (Session["UsernameID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            SqlCommand Data = new SqlCommand("SELECT Word, Definition, Sentence1, CorrectWord, WordAttempts, CorrectDefinition, DefinitionAttempts FROM WordBank WHERE UserID = @UsernameID", connection);
            Data.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);

            SqlDataReader reader = Data.ExecuteReader();
            GridView.DataSource = reader;
            GridView.DataBind();
            connection.Close();
        }
    }
}