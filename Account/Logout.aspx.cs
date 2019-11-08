using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordBank.Account {
	public partial class Logout : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

			if (Session["UsernameID"] == null || Session["Username"] == null) {
				Label1.Text = "You are not logged in!";
			}
			else {
				Session["UsernameID"] = null;
				Session["Username"] = null;
				Label1.Text = "You have logged out";
			}

		}
	}
}