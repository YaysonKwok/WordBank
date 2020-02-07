using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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
            if (Session["Username"] == null)
            {
                string OriginalUrl = HttpContext.Current.Request.RawUrl;
                string LoginPageUrl = "~/Account/Login.aspx";
                HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
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
            else if (Session["SelectedAnswer"].ToString().Equals(Session["Answer"].ToString()))
            {
                Responselbl.Attributes.Add("class", "alert alert-success");
                Responselbl.Text = "Correct! Here's a new definition";
                using (SqlCommand CorrectAnswerUpdate = new SqlCommand("UPDATE WordBank SET CorrectDefinition = CorrectDefinition + 1, LastDefPractice = GETDATE() WHERE Word = @Word", connection))
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
                using (SqlCommand AttemptUpdate = new SqlCommand("UPDATE WordBank SET IncorrectDefinition = IncorrectDefinition + 1, LastDefPractice = GETDATE() WHERE Word = @Word", connection))
                {
                    AttemptUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
                    AttemptUpdate.ExecuteNonQuery();
                }
            }
        }

        protected void GenerateNewQuestion()
        {
			HintLbl.Visible = false;
			Random ran = new Random();
            var numbers = Enumerable.Range(1, 4).OrderBy(i => ran.Next()).ToList();
            List<ListItem> Answers = new List<ListItem>();

            using(SqlCommand PracticeDef = new SqlCommand("SELECT TOP 4 Word, Definition, Sentence1, (CorrectDefinition - IncorrectDefinition) AS Difference, LastDefPractice FROM WordBank WHERE Username = @Username ORDER BY LastDefPractice, Difference", connection))
            {
                PracticeDef.Parameters.AddWithValue("@Username", Session["Username"]);

                using (SqlDataReader DataReader = PracticeDef.ExecuteReader())
                {
                    DataReader.Read();
                    Answers.Add(new ListItem(DataReader.GetString(0)));
                    Definitionlbl.Text = DataReader.GetString(1);
					string input = DataReader.GetString(2);
					string[] sKeywords = DataReader.GetString(0).Split(' ');
					foreach (string sKeyword in sKeywords) {
						try {
							input = Regex.Replace(input, sKeyword, string.Format("______"), RegexOptions.IgnoreCase);
						}
						catch {
							//
						}
					}
					HintLbl.Text = input;
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
            using (SqlCommand WordCheck = new SqlCommand("SELECT COUNT(*) FROM WordBank WHERE Username = @Username", connection))
            {
                WordCheck.Parameters.AddWithValue("@Username", Session["Username"]);
                int WordAmount = (int)WordCheck.ExecuteScalar();
                if (WordAmount < 4)
                {
                    Session["RedirectFromPractice"] = true;
                    Response.Redirect("Input.aspx");
                }
            }
        }

		protected void HintBtn_Click(object sender, EventArgs e) {
			HintLbl.Visible = true;
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