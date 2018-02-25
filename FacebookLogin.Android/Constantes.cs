namespace FacebookLogin.Android
{
    public static class Constantes
    {
        public const string FACEBOOK_URL_AUTH = "https://www.facebook.com/v2.12/dialog/oauth/";

        public const string CLIENTE_ID_FACEBOOK = "589052281463506";

        public const string SCOPE_LOGIN_FACEBOOK = "public_profile user_friends email";

        public const string URL_GRAPHI_API_PERMISSIONS = "https://graph.facebook.com/me?fields=name,email,birthday,picture.height(900)";

        public const string DEFAULT_REDIRECT_URL_FACEBOOK = "https://www.facebook.com/connect/login_success.html";
    }
}