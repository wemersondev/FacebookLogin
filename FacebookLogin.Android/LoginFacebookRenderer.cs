using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using FacebookLogin.Android;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FacebookLogin.Android
{
    public abstract class LoginFacebookRenderer : PageRenderer
    {
        Account _account = new Account();
        private string _token = "";
        
        public LoginFacebookRenderer(Context context, string FacebookIdClient, string ScopePermissions, Uri FacebookUrlAuth, Uri UrlRedirect, ContentPage nextPage, ContentPage loginPage) : base(context)
        {
            var activity = Context as Activity;
            var auth = new OAuth2Authenticator(clientId: FacebookIdClient,
                                                         scope: ScopePermissions,
                                                         authorizeUrl: FacebookUrlAuth,
                                                         redirectUrl: UrlRedirect);

            auth.Completed += (sender, eventArgs) =>
            {
                new NavigationPage().PushAsync(nextPage);
                _account = eventArgs.Account;
                _token = eventArgs.Account.Properties["access_token"].ToString();
            };

            auth.Error += (sender, eventArgs) =>
            {
                new NavigationPage().PushAsync(loginPage);
            };

            activity.StartActivity(auth.GetUI(activity));
        }

        public Account GetAccount()
        {
            return _account;
        }

        public string GetUserToken()
        {
            return _token;
        }

        public async Task<string> GetFacebookData(Account account)
        {
            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=id,name,picture.height(318),email"), null, account);
            var response = await request.GetResponseAsync();
            var obj = JObject.Parse(response.GetResponseText());
            var id = obj["id"].ToString().Replace("\"", "");
            var name = obj["name"].ToString().Replace("\"", "");
            var email = obj["email"].ToString().Replace("\"", "");
            dynamic picture = obj["picture"];
            dynamic data = picture["data"];
            var url = data["url"].ToString().Replace("{", "").Replace("}", "");

            return JsonConvert.SerializeObject(new
            {
                Name = name,
                Email = email,
                UrlProfilePhoto = url,
                FacebookId = id,
                AccessToken = account.Properties["access_token"].ToString()
            });
        }

        async void VerifificaValidadeToken(AuthenticatorCompletedEventArgs eventArgs)
        {
            //TODO: Implementar refresh token se necessário.
            //var accessToken = eventArgs.Account.Properties["access_token"].ToString();
            //var expiresIn = Convert.ToDouble(eventArgs.Account.Properties["expires_in"]);
            //var expiryDate = DateTime.Now + TimeSpan.FromSeconds(expiresIn);
        }
    }
}
