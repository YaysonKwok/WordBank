using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordBank
{
    public partial class DefinitionPractice : Page
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            connection.Open();
            if (Session["UsernameID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            CheckWordTotal();

            if (!IsPostBack)
            {
                Clear();
                GenerateNewQuestion();
            }

        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            Session["SelectedAnswer"] = AnswerList.SelectedValue;

            if (AnswerList.SelectedIndex == -1)
            {
                Responselbl.Attributes.Add("class", "alert alert-danger");
                Responselbl.Text = "You must choose an answer!";
            }

            if (Session["SelectedAnswer"].ToString().Equals(Session["Answer"].ToString()))
            {
                Responselbl.Attributes.Add("class", "alert alert-success");
                Responselbl.Text = "Correct! Here's a new definition";
                using (SqlCommand CorrectAnswerUpdate = new SqlCommand("UPDATE WordBank SET CorrectDefinition = CorrectDefinition + 1 WHERE Word = @Word", connection))
                {
                    CorrectAnswerUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
                    CorrectAnswerUpdate.ExecuteNonQuery();
                }

                Clear();
                GenerateNewQuestion();
            }
            else
            {
                Responselbl.Attributes.Add("class", "alert alert-danger");
                Responselbl.Text = "Incorrect, Try Again";
                AnswerList.Items[AnswerList.SelectedIndex].Enabled = false;
                using (SqlCommand AttemptUpdate = new SqlCommand("UPDATE WordBank SET DefinitionAttempts = DefinitionAttempts + 1 WHERE Word = @Word", connection))
                {
                    AttemptUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
                    AttemptUpdate.ExecuteNonQuery();
                }
            }
        }

        protected void GenerateNewQuestion()
        {
            Random ran = new Random();
            var numbers = Enumerable.Range(1, 4).OrderBy(i => ran.Next()).ToList();
            List<ListItem> Answers = new List<ListItem>();

            using(SqlCommand PracticeDef = new SqlCommand("SELECT TOP 4 Word, Definition, Sentence1, (CorrectDefinition - DefinitionAttempts) AS Difference  FROM WordBank WHERE UserID = @UsernameID ORDER BY Difference", connection))
            {
                PracticeDef.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);

                using (SqlDataReader DataReader = PracticeDef.ExecuteReader())
                {
                    DataReader.Read();
                    Answers.Add(new ListItem(DataReader.GetString(0)));
                    Definitionlbl.Text = DataReader.GetString(1);
                    Session["Word"] = DataReader.GetString(0);
                    Session["Answer"] = DataReader.GetString(0);
                    DataReader.Read();
                    Answers.Add(new ListItem(DataReader.GetString(0)));
                    DataReader.Read();
                    Answers.Add(new ListItem(DataReader.GetString(0)));
                    DataReader.Read();
                    Answers.Add(new ListItem(DataReader.GetString(0)));
                }
            }
           


            foreach (int num in numbers)
            {
                AnswerList.Items.Add(Answers[num - 1]);
            }
        }

        protected void CheckWordTotal()
        {
            using (SqlCommand WordCheck = new SqlCommand("SELECT COUNT(*) FROM WordBank WHERE UserID = @UsernameID", connection))
            {
                WordCheck.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
                int WordAmount = (int)WordCheck.ExecuteScalar();
                if (WordAmount < 4)
                {
                    Session["InputRedirect"] = true;
                    Response.Redirect("Input.aspx");
                }
            }
        }
        protected void Clear()
        {
            AnswerList.Items.Clear();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            connection.Close();
        }
    }
}