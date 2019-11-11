using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordBank {
	public partial class _Default : Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
		protected void Page_Load(object sender, EventArgs e) {
            Session["EditRedirect"] = false;
            Session["InputRedirect"] = false;

			if (!IsPostBack) {
				Clear();
				GenerateNewQuestion();
			}
		}

		protected void SubmitBtn_Click(object sender, EventArgs e) {
			Session["SelectedAnswer"] = AnswerList.SelectedValue;

			if (AnswerList.SelectedIndex == -1) {
				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "You must choose an answer!";
			}

			if (Session["SelectedAnswer"].ToString().Equals(Session["Answer"].ToString())) {
				Responselbl.Attributes.Add("class", "alert alert-success");
				Responselbl.Text = "Correct! Here's a new word";

				Clear();
				GenerateNewQuestion();
			}
			else {

				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "Incorrect, Try Again";
				AnswerList.Items[AnswerList.SelectedIndex].Enabled = false;
			}
		}
		protected void GenerateNewQuestion() {
			connection.Open();
			HintLbl.Visible = false;

			Random ran = new Random();
			var numbers = Enumerable.Range(1, 4).OrderBy(i => ran.Next()).ToList();
			List<ListItem> Answers = new List<ListItem>();

				using (SqlCommand PracticeWord = new SqlCommand("SELECT TOP 4 Word, Definition, Sentence1 FROM WordBank ORDER BY newid()", connection)) {

					using (SqlDataReader DataReader = PracticeWord.ExecuteReader()) {
						if (DataReader != null) {
							DataReader.Read();
							Answers.Add(new ListItem(DataReader.GetString(1)));
							Wordlbl.Text = DataReader.GetString(0);

							string input = DataReader.GetString(2);
							string[] sKeywords = DataReader.GetString(0).Split(' ');
							foreach (string sKeyword in sKeywords) {
								try {
									input = Regex.Replace(input, sKeyword, string.Format("<span style=\"background-color:yellow\">{0}</span>", "$0"), RegexOptions.IgnoreCase);
								}
								catch {
									//
								}
							}
							HintLbl.Text = input;

							Session["Word"] = DataReader.GetString(0);
							Session["Answer"] = DataReader.GetString(1);
							DataReader.Read();
							Answers.Add(new ListItem(DataReader.GetString(1)));
							DataReader.Read();
							Answers.Add(new ListItem(DataReader.GetString(1)));
							DataReader.Read();
							Answers.Add(new ListItem(DataReader.GetString(1)));
						}
					}
				}


			foreach (int num in numbers) {
				AnswerList.Items.Add(Answers[num - 1]);
			}
			connection.Close();
		}
		protected void Clear() {
			AnswerList.Items.Clear();
		}

		protected void HintBtn_Click(object sender, EventArgs e) {
			HintLbl.Visible = true;
		}

		protected void Guest_Click(object sender, EventArgs e)
        {
            Session["UsernameID"] = 0;
            Session["Username"] = "Guest";
            Response.Redirect("~/Input.aspx");
        }
    }
}