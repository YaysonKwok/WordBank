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
    public partial class WordPractice : Page
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.Open();
            if(Session["UsernameID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            SqlCommand WordCheck = new SqlCommand("SELECT COUNT(*) FROM WordBank WHERE UserID = @UsernameID", connection);
            WordCheck.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
            int WordAmount = (int) WordCheck.ExecuteScalar();
            if (WordAmount < 4)
            {
                Session["InputRedirect"] = true;
                Response.Redirect("Input.aspx");
            }

            if (!IsPostBack)
            {
                Clear();
                GenerateNewQuestion();
            }
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            Session["SelectedAnswer"] = AnswerList.SelectedValue;
            if (Session["SelectedAnswer"].ToString().Equals(Session["Answer"].ToString()))
            {
                Responselbl.Attributes.Add("class", "alert alert-success");
                Responselbl.Text = "Correct! Here's a new word";
                SqlCommand CorrectAnswerUpdate = new SqlCommand("UPDATE WordBank SET CorrectWord = CorrectWord + 1 WHERE Word = @Word", connection);
                CorrectAnswerUpdate.CommandType = CommandType.Text;
                CorrectAnswerUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
                CorrectAnswerUpdate.ExecuteNonQuery();
                Clear();
                GenerateNewQuestion();
            }
            else
            {
                if (AnswerList.SelectedIndex == -1)
                {
                    Responselbl.Attributes.Add("class", "alert alert-danger");
                    Responselbl.Text = "You must choose an answer!";
                }
                else
                {
                    SqlCommand AttemptUpdate = new SqlCommand("UPDATE WordBank SET WordAttempts = WordAttempts - 1 WHERE Word = @Word", connection);
                    AttemptUpdate.CommandType = CommandType.Text;
                    AttemptUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
                    AttemptUpdate.ExecuteNonQuery();

                    Responselbl.Attributes.Add("class", "alert alert-danger");
                    Responselbl.Text = "Incorrect, Try Again";
                    AnswerList.Items[AnswerList.SelectedIndex].Enabled = false;
                }
            }
        }

        protected void GenerateNewQuestion()
        {
            HintLbl.Visible = false;

            Random ran = new Random();
            var numbers = Enumerable.Range(1, 4).OrderBy(i => ran.Next()).ToList();
            List<ListItem> Answers = new List<ListItem>();

            using (SqlCommand PracticeWord = new SqlCommand("SELECT TOP 1 Word, Definition, Sentence1, (CorrectWord - WordAttempts) AS Difference  FROM WordBank WHERE UserID = @UsernameID ORDER BY Difference", connection))
            {
                PracticeWord.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);

                using (SqlDataReader DataReader = PracticeWord.ExecuteReader())
                {
                    if (DataReader != null)
                    {
                        DataReader.Read();
                        Answers.Add(new ListItem(DataReader.GetString(1)));
                        Wordlbl.Text = DataReader.GetString(0);
                        HintLbl.Text = DataReader.GetString(2);
                        Session["Word"] = DataReader.GetString(0);
                        Session["Answer"] = DataReader.GetString(1);
                    }
                }
            }

            using (SqlCommand AnswerChoices = new SqlCommand("SELECT TOP 3 Word, Definition, Sentence1 FROM WordBank WHERE UserID = @UsernameID ORDER BY NEWID() ", connection))
            {
                AnswerChoices.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
                using (SqlDataReader DataReader = AnswerChoices.ExecuteReader())
                {
                    if (DataReader != null)
                    {
                        DataReader.Read();
                        Answers.Add(new ListItem(DataReader.GetString(1)));
                        DataReader.Read();
                        Answers.Add(new ListItem(DataReader.GetString(1)));
                        DataReader.Read();
                        Answers.Add(new ListItem(DataReader.GetString(1)));
                    }
                }

            }

            foreach (int num in numbers)
            {
                AnswerList.Items.Add(Answers[num - 1]);
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

        protected void HintBtn_Click(object sender, EventArgs e)
        {
            HintLbl.Visible = true;
        }
    }
}