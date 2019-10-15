using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordBank {
	public partial class WordPractice : Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);
		protected void Page_Load(object sender, EventArgs e) {
			connection.Open();
			if (Session["UsernameID"] == null) {
				Response.Redirect("~/Account/Login.aspx");
			}

			if (!IsPostBack) {
				CheckWordTotal();
				Clear();
				GenerateNewQuestion();
			}
		}

		private void CheckWordTotal() {
			using (SqlCommand WordCheck = new SqlCommand("SELECT COUNT(*) FROM WordBank WHERE UserID = @UsernameID", connection)) {
				WordCheck.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);
				int WordAmount = (int)WordCheck.ExecuteScalar();
				if (WordAmount < 4) {
					Session["InputRedirect"] = true;
					Response.Redirect("Input.aspx");
				}
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
				using (SqlCommand CorrectAnswerUpdate = new SqlCommand("UPDATE WordBank SET CorrectWord = CorrectWord + 1, LastWordPractice = GETDATE()  WHERE Word = @Word", connection)) {
					CorrectAnswerUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
					CorrectAnswerUpdate.ExecuteNonQuery();
				}

				Clear();
				GenerateNewQuestion();
			}
			else {

				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "Incorrect, Try Again";
				AnswerList.Items[AnswerList.SelectedIndex].Enabled = false;
				using (SqlCommand AttemptUpdate = new SqlCommand("UPDATE WordBank SET WordAttempts = WordAttempts + 1, LastWordPractice = GETDATE() WHERE Word = @Word", connection)) {
					AttemptUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
					AttemptUpdate.ExecuteNonQuery();
				}
			}
		}

		protected void GenerateNewQuestion() {
			HintLbl.Visible = false;

			Random ran = new Random();
			var numbers = Enumerable.Range(1, 4).OrderBy(i => ran.Next()).ToList();
			List<ListItem> Answers = new List<ListItem>();

			if ((int) Session["ReSortCounter"] == 4) {
				Session["ReSort"] = false;
			}

			if ((bool)Session["ReSort"]) {
				Session["ReSortCounter"] = (int) Session["ReSortCounter"] + 1;
				using (SqlCommand PracticeWord = new SqlCommand("SELECT TOP 4 Word, Definition, Sentence1, (CorrectWord - WordAttempts) AS Difference, LastWordPractice FROM WordBank WHERE UserID = @UsernameID ORDER BY LastWordPractice, Difference", connection)) {
					PracticeWord.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);

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
			}
			else {
				Session["ReSort"] = true;
				Session["ReSortCounter"] = 0;
				using (SqlCommand PracticeWord = new SqlCommand("SELECT TOP 5 Word, Definition, Sentence1, (CorrectWord - WordAttempts) AS Difference, LastWordPractice FROM WordBank WHERE UserID = @UsernameID ORDER BY LastWordPractice DESC, Difference", connection)) {
					PracticeWord.Parameters.AddWithValue("@UsernameID", Session["UsernameID"]);

					using (SqlDataReader DataReader = PracticeWord.ExecuteReader()) {
						if (DataReader != null) {
							DataReader.Read();
							DataReader.Read();
							Answers.Add(new ListItem(DataReader.GetString(1)));
							DataReader.Read();
							Answers.Add(new ListItem(DataReader.GetString(1)));
							DataReader.Read();
							Answers.Add(new ListItem(DataReader.GetString(1)));
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
						}
					}
				}
			}

			foreach (int num in numbers) {
				AnswerList.Items.Add(Answers[num - 1]);
			}

		}
		protected void Clear() {
			AnswerList.Items.Clear();
		}

		protected void HintBtn_Click(object sender, EventArgs e) {
			HintLbl.Visible = true;
		}

		protected void Page_Unload(object sender, EventArgs e) {
			connection.Close();
		}
	}
}