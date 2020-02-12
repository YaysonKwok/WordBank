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
		}
    }
}