using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordBank
{
    public partial class Input : System.Web.UI.Page
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsernameID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if ((bool)Session["InputRedirect"])
            {
                Redirectlbl.Text = "You have been redirected since you have less than 4 words";
                Redirectlbl.Visible = true;
            }
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            using (SqlCommand WordCheck = new SqlCommand("SELECT COUNT(*) FROM WordBank WHERE UserID = @UsernameID AND Word = @WordCheck", connection))
            {
                WordCheck.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
                WordCheck.Parameters.AddWithValue("@WordCheck", WordInput.Text);
                WordCheck.ExecuteScalar();

                if ((int)WordCheck.ExecuteScalar() != 1)
                {
                    InsertWord();
                }
                else
                {
                    SubmitResponse.Attributes.Add("class", "alert alert-danger");
                    SubmitResponse.Text = "You already have this word saved!";
                }
            }
        }

        private void Clear()
        {
            WordInput.Text = "";
            DefinitionInput.Text = "";
            Sentence1Input.Text = "";
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            Session["InputRedirect"] = false;
            Redirectlbl.Visible = false;
        }

        protected void InsertWord()
        {
            using (SqlCommand Insert = new SqlCommand("INSERT INTO WordBank(UserID, Word, Definition, Sentence1) VALUES(@UsernameID,@Word,@Definition,@Sentence1)", connection))
            {
                Insert.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
                Insert.Parameters.AddWithValue("@Word", WordInput.Text);
                Insert.Parameters.AddWithValue("@Definition", DefinitionInput.Text);
                Insert.Parameters.AddWithValue("@Sentence1", Sentence1Input.Text);
                try
                {
                    Insert.ExecuteNonQuery();
                    SubmitResponse.Attributes.Add("class", "alert alert-success");
                    SubmitResponse.Text = "Success! You can add another.";
                    Redirectlbl.Visible = false;
                    Clear();
                }
                catch (Exception ex)
                {
                    SubmitResponse.Text = ex.Message;
                }
            }
        }
    }
}