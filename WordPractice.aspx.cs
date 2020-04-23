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

namespace WordBank {
	public partial class WordPractice : Page {
		const string NumTotal_word2def = "NumTotal_word2def";
		readonly static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank"].ConnectionString);
		string[] Word = new string[10];
		string[] Definition = new string[10];
		string[] Hint = new string[10];

		protected void Page_Load(object sender, EventArgs e) {
			CheckLoggedIn();

			connection.Open();
			using (SqlCommand CheckResortValue = new SqlCommand("SELECT Resort FROM Login WHERE Username = @Username", connection)) {
				CheckResortValue.Parameters.AddWithValue("@Username", Session["Username"]);

				using (SqlDataReader DataReader = CheckResortValue.ExecuteReader()) {
					if (DataReader != null) {
						DataReader.Read();
						Session["resortValue"] = (int)DataReader.GetValue(0);
					}
				}
			}
			connection.Close();

			if (!IsPostBack) {
				Session[NumTotal_word2def] = new NumTotal(); // v0.15

				CheckWordTotal();
				Clear();
				GenerateNewQuestion();
			}
		}

		private void CheckLoggedIn() {
			if (Session["Username"] == null) {
				string OriginalUrl = HttpContext.Current.Request.RawUrl;
				string LoginPageUrl = "~/Account/Login.aspx";
				HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
			}
		}

		private void CheckWordTotal() {
			connection.Open();
			using (SqlCommand WordCheck = new SqlCommand("SELECT COUNT(*) FROM WordBank WHERE Username = @Username", connection)) {
				WordCheck.Parameters.AddWithValue("@Username", Session["Username"]);
				int WordAmount = (int)WordCheck.ExecuteScalar();
				if (WordAmount < 4) {
					Session["RedirectFromPractice"] = true;
					Response.Redirect("Input.aspx");
				}
			}
			connection.Close();
		}

		protected void SubmitBtn_Click(object sender, EventArgs e) {
			Session["SelectedAnswer"] = AnswerList.SelectedValue;

			if (AnswerList.SelectedIndex == -1) {
				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "You must choose an answer!";
			}
			else if (Session["SelectedAnswer"].ToString().Equals(Session["Answer"].ToString())) {
				Responselbl.Attributes.Add("class", "alert alert-success");
				Responselbl.Text = "Correct! Here's a new word";
				LabelNumTotal.Text = NumTotal.Inc(Session, NumTotal_word2def, 1); // Canny v0.15

				connection.Open();
				using (SqlCommand CorrectAnswerUpdate = new SqlCommand("UPDATE WordBank SET CorrectWord = CorrectWord + 1, LastWordPractice = GETDATE() WHERE Word = @Word AND Username = @Username", connection)) {
					CorrectAnswerUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
					CorrectAnswerUpdate.Parameters.AddWithValue("@Username", Session["Username"]);
					CorrectAnswerUpdate.ExecuteNonQuery();
					if ((int)Session["WordIndex"] == 9) {
						Session["WordIndex"] = 0;
					}
					else {
						Session["WordIndex"] = (int)Session["WordIndex"] + 1;
					}
				}

				if (HintLbl.Visible == true) {
					using (SqlCommand HintIncUpdate = new SqlCommand("UPDATE WordBank SET Sen_hint_inc = Sen_hint_inc + 1 WHERE Word = @Word AND Username = @Username", connection)) {
						HintIncUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
						HintIncUpdate.Parameters.AddWithValue("@Username", Session["Username"]);
						HintIncUpdate.ExecuteNonQuery();
					}
				}
				else {
					using (SqlCommand HintIncUpdate = new SqlCommand("UPDATE WordBank SET Sen_hint_dec = CASE WHEN Sen_hint_dec < Sen_hint_inc THEN Sen_hint_dec + 1 ELSE Sen_hint_dec END WHERE Word = @Word AND Username = @Username", connection)) {
						HintIncUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
						HintIncUpdate.Parameters.AddWithValue("@Username", Session["Username"]);
						HintIncUpdate.ExecuteNonQuery();
					}
				}
				Clear();
				GenerateNewQuestion();
			} else {
				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "Incorrect, Try Again";
				LabelNumTotal.Text = NumTotal.Inc(Session, NumTotal_word2def, 0); // Canny v0.15
				AnswerList.Items[AnswerList.SelectedIndex].Enabled = false;
				using (SqlCommand AttemptUpdate = new SqlCommand("UPDATE WordBank SET IncorrectWord = IncorrectWord + 1, LastWordPractice = GETDATE() WHERE Word = @Word", connection)) {
					AttemptUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
					AttemptUpdate.ExecuteNonQuery();
				}
				LabelNumTotal.Text = NumTotal.Inc(Session, NumTotal_word2def, 0); // Canny v0.15

			}
			connection.Close();
		}

		protected void GenerateNewQuestion() {
			if (connection.State != ConnectionState.Open) {
				connection.Close();
				connection.Open();
			}
			HintLbl.Visible = false;

			int index = (int)Session["WordIndex"];
			int resortValue = (int)Session["resortValue"];

			if (Session["WordArray"] != null) {
				Word = (string[])Session["WordArray"];
				Definition = (string[])Session["DefinitionArray"];
				Hint = (string[])Session["HintArray"];
			}

			if (index == (resortValue-1) || index == 0) {
				using (SqlCommand PracticeWord = new SqlCommand("SELECT TOP 10 Word, Definition, Sentence1, (CorrectWord - IncorrectWord) AS WordDifference, (Sen_hint_dec - Sen_hint_inc) As HintDifference FROM WordBank WHERE Username = @Username ORDER BY WordDifference ASC, HintDifference ASC ", connection)) {
					PracticeWord.Parameters.AddWithValue("@Username", Session["Username"]);

					using (SqlDataReader DataReader = PracticeWord.ExecuteReader()) {
						for (int i = 0; i < 10; i++) {
							DataReader.Read();
							Word[i] = DataReader.GetString(0);
							Definition[i] = DataReader.GetString(1);
							Hint[i] = DataReader.GetString(2);
						}
					}
				}
				Session["WordIndex"] = 0;
				Session["WordArray"] = Word;
				Session["DefinitionArray"] = Definition;
				Session["HintArray"] = Hint;
			}

			Wordlbl.Text = Word[index];
			Session["Word"] = Wordlbl.Text;
			Session["Answer"] = Definition[index];
			string input = Hint[index];
			string[] sKeywords = Wordlbl.Text.Split(' ');
			foreach (string sKeyword in sKeywords) {
				try {
					input = Regex.Replace(input, sKeyword, string.Format("<span style=\"background-color:yellow\">{0}</span>", "$0"), RegexOptions.IgnoreCase);
				}
				catch {
					//
				}
			}
			HintLbl.Text = input;

			Random ran = new Random();
			var numbers = Enumerable.Range(1, 4).OrderBy(i => ran.Next()).ToList();
			List<ListItem> Answers = new List<ListItem>();

			Answers.Add(new ListItem(Definition[index]));

			using (SqlCommand PracticeWord = new SqlCommand("SELECT TOP 3 Word, Definition, Sentence1 FROM WordBank WHERE Username = @Username ORDER BY NEWID()", connection)) {
				PracticeWord.Parameters.AddWithValue("@Username", Session["Username"]);
				using (SqlDataReader DataReader = PracticeWord.ExecuteReader()) {
					if (DataReader != null) {
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
			ResortLbl.Text = "(resort)";

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