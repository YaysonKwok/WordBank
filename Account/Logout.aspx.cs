﻿using System;

namespace WordBank.Account {
	public partial class Logout : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

			if (Session["Username"] == null) {
				Label1.Text = "You are not logged in!";
			}
			else {
				Session["Username"] = null;
				Label1.Text = "You have logged out";
			}

		}
	}
}