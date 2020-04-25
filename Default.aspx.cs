using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WordBank {
	public partial class _Default : Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank"].ConnectionString);
		protected void Page_Load(object sender, EventArgs e) {
            Session["EditRedirect"] = false;
            Session["InputRedirect"] = false;
		}
    }
}