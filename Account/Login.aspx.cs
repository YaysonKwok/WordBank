using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using WordBank.Models;

namespace WordBank.Account {
	public partial class Login : Page {
		static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WordBank.Properties.Settings.ConnectionString"].ConnectionString);

		protected void Page_Load(object sender, EventArgs e) {
			RegisterHyperLink.NavigateUrl = "Register";
			// Enable this once you have account confirmation enabled for password reset functionality
			//ForgotPasswordHyperLink.NavigateUrl = "Forgot";
			OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
			var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
			if (!String.IsNullOrEmpty(returnUrl)) {
				RegisterHyperLink.NavigateUrl = "?ReturnUrl=" + returnUrl;
			}
		}

		protected void LogIn(object sender, EventArgs e) {
			connection.Open();
			using (SqlCommand ExistingUsername = new SqlCommand("SELECT Id FROM Username WHERE Username = @Username", connection)) {
				ExistingUsername.Parameters.AddWithValue("@Username", Email.Text);
				SqlDataReader dr = ExistingUsername.ExecuteReader();
				dr.Read();
				Session["UsernameID"] = dr.GetValue(0);
				Session["Username"] = Email.Text;
				Session["InputRedirect"] = false;
				connection.Close();

				Label1.Text = "You have logged in!";

			}
			/*
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
                                                        Request.QueryString["ReturnUrl"],
                                                        RememberMe.Checked),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "Invalid login attempt";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
			*/
		}
	}
}