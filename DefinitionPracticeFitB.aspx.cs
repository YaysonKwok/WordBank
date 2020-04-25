using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

namespace WordBank {
	public partial class DefinitionPracticeFitB : System.Web.UI.Page {
		const string NumTotal_def2word = "NumTotal_def2word";
		readonly static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank"].ConnectionString);
		string[] Word = new string[10];
		string[] Definition = new string[10];
		string[] Hint = new string[10];
		char[] hintArray;

		protected void Page_Load(object sender, EventArgs e) {
			CheckLoggedIn();
			CheckResortValue();
			if (!IsPostBack) {
				Session[NumTotal_def2word] = new NumTotal(); // v0.15
				CheckWordTotal();
				Clear();
				GenerateNewQuestion();
			}
			SubmittedAnswer.Focus(); // Canny, v0.1.1
		}

		private void CheckResortValue() {
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
		}

		protected void SubmitBtn_Click(object sender, EventArgs e) {
			Session["SelectedAnswer"] = SubmittedAnswer.Text;

			if (SubmittedAnswer.Text == "") {
				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "You cannot sumbit a blank answer!";
			}
			else if (IsAnagram(Session["SelectedAnswer"].ToString(), Session["Word"].ToString()) && Session["SelectedAnswer"].ToString() != Session["Answer"].ToString()) {
				Responselbl.Attributes.Add("class", "alert alert-warning");
				Responselbl.Text = "Check spelling!";
			}
			else if (Session["SelectedAnswer"].ToString().Equals(Session["Answer"].ToString())) {
				Responselbl.Attributes.Add("class", "alert alert-success");
				Responselbl.Text = "Correct! Here's a new definition";
				LabelNumTotal.Text = NumTotal.Inc(Session, NumTotal_def2word, 1); // Canny v0.15
				CorrectAnswerUpdate();
				Clear();
				GenerateNewQuestion();
			}
			else {
				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "Incorrect, Try Again";
				LabelNumTotal.Text = NumTotal.Inc(Session, NumTotal_def2word, 0); // Canny v0.15
				SubmittedAnswer.Text = "";
				AttemptUpdate();
				SubmittedAnswer.Text = "";
			}
		}

		private void CorrectAnswerUpdate() {
			connection.Open();
			using (SqlCommand CorrectAnswerUpdate = new SqlCommand("UPDATE WordBank SET CorrectDefinition = CorrectDefinition + 1, LastDefPractice = GETDATE() WHERE Word = @Word", connection)) {
				CorrectAnswerUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
				CorrectAnswerUpdate.ExecuteNonQuery();
				Session["DefIndex"] = (int)Session["DefIndex"] + 1;
			}
			connection.Close();
		}

		private void AttemptUpdate() {
			connection.Open();
			using (SqlCommand AttemptUpdate = new SqlCommand("UPDATE WordBank SET IncorrectDefinition = IncorrectDefinition + 1, LastDefPractice = GETDATE() WHERE Word = @Word", connection)) {
				AttemptUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
				AttemptUpdate.ExecuteNonQuery();
			}
			connection.Close();
		}

		private bool IsAnagram(string a, string b) {
			if (a.Length != b.Length) {
				return false;
			}

			var aFrequency = CalculateFrequency(a);
			var bFrequency = CalculateFrequency(b);

			foreach (var key in aFrequency.Keys) {
				if (!bFrequency.ContainsKey(key)) return false;
				if (aFrequency[key] != bFrequency[key]) return false;
			}

			return true;
		}

		private Dictionary<char, int> CalculateFrequency(string input) {
			var frequency = new Dictionary<char, int>();
			foreach (var c in input) {
				if (!frequency.ContainsKey(c)) {
					frequency.Add(c, 0);
				}
				++frequency[c];
			}
			return frequency;
		}

		private void GenerateNewQuestion() {
			LetterHintLbl.Visible = false;
			SentenceHintLbl.Visible = false;

			int index = (int)Session["DefIndex"];
			int resortValue = (int)Session["resortValue"];

			if (Session["DefinitionArray"] != null) {
				Word = (string[])Session["WordArray"];
				Definition = (string[])Session["DefinitionArray"];
				Hint = (string[])Session["HintArray"];
			}
			ResortLbl.Text = ""; // Canny: v0.1.2, Indicates whether there's a resort event.
			if (index == resortValue || index == 0) { // Canny: v0.1.2, Changed from (resortValue-1)

				// Canny v0.1.2, Added resortValue 
				PracticeWord(resortValue);
				ResortLbl.Text = "(Resort)"; // v0.1.2, Indicates a resort event.
				index = 0; // Canny v0.1.2
				Session["DefIndex"] = 0;
				Session["WordArray"] = Word;
				Session["DefinitionArray"] = Definition;
				Session["HintArray"] = Hint;
			}

			Definitionlbl.Text = Definition[index];
			Session["Word"] = Word[index];
			Session["Answer"] = Word[index];
			HintCreation(index);
		}

		private void PracticeWord(int resortValue) {
			connection.Open();
			string sqlStmt = $"SELECT TOP {resortValue} Word, Definition, Sentence1, (CorrectDefinition - IncorrectDefinition) AS Difference FROM WordBank WHERE Username = @Username ORDER BY Difference ASC";
			using (SqlCommand PracticeWord = new SqlCommand(sqlStmt, connection)) {
				PracticeWord.Parameters.AddWithValue("@Username", Session["Username"]);


				using (SqlDataReader DataReader = PracticeWord.ExecuteReader()) {
					for (int i = 0; i < resortValue; i++) { // Canny: 0.1.2, Changed from i < 10
						DataReader.Read();
						Word[i] = DataReader.GetString(0);
						Definition[i] = DataReader.GetString(1);
						Hint[i] = DataReader.GetString(2);
					}
				}
			}
			connection.Close();
		}

		private void HintCreation(int index) {
			string input = Hint[index];
			string[] words = input.Split(' ');
			string[] sKeywords = Word[index].Split(' ');

			for (int i = 0; i < words.Length; i++) {
				if (words[i].Equals(sKeywords[0]) && i != 0) {
					if (words[i - 1].Equals("a") || words[i - 1].Equals("an")) {
						words[i - 1] = "a/an";
					}
				}
			}

			input = String.Join(" ", words);

			foreach (string sKeyword in sKeywords) {
				try {
					input = Regex.Replace(input, sKeyword, string.Format("______"), RegexOptions.IgnoreCase);
				}
				catch {
					//
				}
			}
			SentenceHintLbl.Text = input;
			Session["HintButtonIndex"] = 0;
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

		protected void Clear() {
			SubmittedAnswer.Text = "";
			LetterHintLbl.Text = "";
			LetterHintBtn.Text = "Letter Hint";
			LetterHintBtn.Enabled = true;
		}

		protected void SentenceHintBtn_Click(object sender, EventArgs e) {
			SentenceHintLbl.Visible = true;
		}

		protected void LetterHintBtn_Click(object sender, EventArgs e) {
			string s = Session["Word"].ToString();
			hintArray = s.ToCharArray();

			if ((int)Session["HintButtonIndex"] < hintArray.Length - 1) {
				LetterHintLbl.Text = LetterHintLbl.Text + hintArray[(int)Session["HintButtonIndex"]].ToString();
				Session["HintButtonIndex"] = (int)Session["HintButtonIndex"] + 1;
			}
			else {
				LetterHintBtn.Text = "No more hints!";
				LetterHintBtn.Enabled = false;
			}
			LetterHintLbl.Visible = true;
		}

		protected void Page_Unload(object sender, EventArgs e) {
			connection.Close();
		}
	}
}