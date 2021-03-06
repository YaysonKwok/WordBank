﻿using System;
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
	public partial class DefinitionPractice : Page {
		readonly static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank"].ConnectionString);
		string[] Word = new string[10];
		string[] Definition = new string[10];
		string[] Hint = new string[10];

		protected void Page_Load(object sender, EventArgs e) {
			CheckLoggedIn();
			CheckResortValue();
			if (!IsPostBack) {
				CheckWordTotal();
				Clear();
				GenerateNewQuestion();
			}
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

		private void CheckLoggedIn() {
			if (Session["Username"] == null) {
				string OriginalUrl = HttpContext.Current.Request.RawUrl;
				string LoginPageUrl = "~/Account/Login.aspx";
				HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
			}
		}

		protected void SubmitBtn_Click(object sender, EventArgs e) {
			Session["SelectedAnswer"] = AnswerList.SelectedValue;

			if (AnswerList.SelectedIndex == -1) {
				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "You must choose an answer!";
			}
			else if (Session["SelectedAnswer"].ToString().Equals(Session["Answer"].ToString())) {
				Responselbl.Attributes.Add("class", "alert alert-success");
				Responselbl.Text = "Correct! Here's a new definition";
				CorrectAnswerUpdate();
				Clear();
				GenerateNewQuestion();
			}
			else {
				Responselbl.Attributes.Add("class", "alert alert-danger");
				Responselbl.Text = "Incorrect, Try Again";
				AnswerList.Items[AnswerList.SelectedIndex].Enabled = false;
				AttemptUpdate();
			}
		}

		private void AttemptUpdate() {
			connection.Open();
			using (SqlCommand AttemptUpdate = new SqlCommand("UPDATE WordBank SET IncorrectDefinition = IncorrectDefinition + 1, LastDefPractice = GETDATE() WHERE Word = @Word", connection)) {
				AttemptUpdate.Parameters.AddWithValue("@Word", Session["Word"].ToString());
				AttemptUpdate.ExecuteNonQuery();
			}
			connection.Close();
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

		protected void GenerateNewQuestion() {
			HintLbl.Visible = false;

			int index = (int)Session["DefIndex"];
			int resortValue = (int)Session["resortValue"];

			if (Session["DefinitionArray"] != null) {
				Word = (string[])Session["WordArray"];
				Definition = (string[])Session["DefinitionArray"];
				Hint = (string[])Session["HintArray"];
			}

			if (index == (resortValue - 1) || index == 0) {
				PracticeWord();
			}

			Definitionlbl.Text = Definition[index];
			Session["Word"] = Word[index];
			Session["Answer"] = Word[index];

			HintCreation(index);
			List<ListItem> Answers = new List<ListItem>();
			
			Answers.Add(new ListItem(Word[index]));

			GenerateWrongAnswers(Answers);
		}

		private List<ListItem> GenerateWrongAnswers(List<ListItem> Answers) {
			Random ran = new Random();
			var numbers = Enumerable.Range(1, 4).OrderBy(i => ran.Next()).ToList();

			connection.Open();
			using (SqlCommand WrongWord = new SqlCommand("SELECT TOP 3 Word, Definition, Sentence1 FROM WordBank WHERE Username = @Username ORDER BY NEWID()", connection)) {
				WrongWord.Parameters.AddWithValue("@Username", Session["Username"]);
				using (SqlDataReader DataReader = WrongWord.ExecuteReader()) {
					if (DataReader != null) {
						DataReader.Read();
						Answers.Add(new ListItem(DataReader.GetString(0)));
						DataReader.Read();
						Answers.Add(new ListItem(DataReader.GetString(0)));
						DataReader.Read();
						Answers.Add(new ListItem(DataReader.GetString(0)));
					}
				}
			}
			connection.Close();

			foreach (int num in numbers) {
				AnswerList.Items.Add(Answers[num - 1]);
			}

			return Answers;
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
			HintLbl.Text = input;
		}

		private void PracticeWord() {
			connection.Open();
			using (SqlCommand PracticeWord = new SqlCommand("SELECT TOP 10 Word, Definition, Sentence1, (CorrectDefinition - IncorrectDefinition) AS Difference FROM WordBank WHERE Username = @Username ORDER BY Difference ASC", connection)) {
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

			Session["DefIndex"] = 0;
			Session["WordArray"] = Word;
			Session["DefinitionArray"] = Definition;
			Session["HintArray"] = Hint;
			connection.Close();
		}

		protected void CheckWordTotal() {
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

		protected void HintBtn_Click(object sender, EventArgs e) {
			HintLbl.Visible = true;
		}

		protected void Clear() {
			AnswerList.Items.Clear();
		}

		protected void Page_Unload(object sender, EventArgs e) {
			connection.Close();
		}
	}
}