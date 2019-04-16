using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Kentico.Membership;
using CMS.EventLog;
using Models.Administrative;
using System.Web;

namespace Controllers
{
    public class SignInManagerController : Controller
    {
        /// <summary>
        /// Provides access to the Kentico.Membership.SignInManager instance.
        /// </summary>
        public SignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<SignInManager>();
            }
        }

        /// <summary>
        /// Provides access to the Kentico.Membership.UserManager instance.
        /// </summary>
        public UserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<UserManager>();
            }
        }

        public User CurrentUser
        {
            get
            {
                return UserManager.FindByName(User.Identity.Name);
            }
        }

        /// <summary>
        /// Provides access to the Microsoft.Owin.Security.IAuthenticationManager instance.
        /// </summary>
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Basic action that displays the sign-in form.
        /// </summary>
        public ActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        /// Handles authentication when the sign-in form is submitted. Accepts parameters posted from the sign-in form via the SignInViewModel.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> SignIn(LoginViewModel model, string returnUrl)
        {
            // Validates the received user credentials based on the view model
            if (!ModelState.IsValid)
            {
                // Displays the sign-in form if the user credentials are invalid
                return View();
            }

            // Attempts to authenticate the user against the Kentico database
            SignInStatus signInResult = SignInStatus.Failure;
            try
            {
                signInResult = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.StaySignedIn, false);
            }
            catch (Exception ex)
            {
                // Logs an error into the Kentico event log if the authentication fails
                EventLogProvider.LogException("SignInManager", "SignIn", ex);
            }

            // If the authentication was not successful, displays the sign-in form with an "Authentication failed" message
            if (signInResult != SignInStatus.Success)
            {
                ModelState.AddModelError(String.Empty, "Authentication failed");
                return View();
            }

            // If the authentication was successful, redirects to the return URL when possible or to a different default action
            string decodedReturnUrl = Server.UrlDecode(returnUrl);
            if (!string.IsNullOrEmpty(decodedReturnUrl) && Url.IsLocalUrl(decodedReturnUrl))
            {
                return Redirect(decodedReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Action for signing out users. The Authorize attribute allows the action only for users who are already signed in.
        /// </summary>
        [Authorize]
        [HttpPost]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            // Signs out the current user
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Redirects to a different action after the sign-out
            return RedirectToAction("Index", "Home");
        }
    }
}