namespace NewSkin.Util
{
    internal class Pegasus
    {
        /// <summary>
        ///     Sign in to PegasusCRM using the specified crendentials.
        /// </summary>
        /// <param name="user">The user to sign in to.</param>
        /// <returns>The Browser instance.</returns>
        public static Browser Login(string user)
        {
            var credentials = XmlReader.Read("Config/Credentials.xml")[user];
            string username = credentials.username;
            string password = credentials.password;
            return Login(username, password);
        }

        /// <summary>
        ///     Sign in to the PegasusCRM test site with the specified username/password.
        /// </summary>
        /// <param name="user">The username to use for logging in.</param>
        /// <param name="pass">The password to use for logging in.</param>
        /// <returns>The Browser instance.</returns>
        private static Browser Login(string user, string pass)
        {
            string url = XmlReader.Read("Config/Config.xml").dotinfo;
            var login = new LocatorReader("Login.xml");

            var browser = new Browser().GoToUrl(url).Maximize();
            browser.FillForm(login.Get("username"), user);
            browser.FillForm(login.Get("password"), pass);
            browser.Click(login.Get("submit-button"));
            browser.ImplicitWait = 10;
            return browser;
        }

        /// <summary>
        ///     Sign in to PegasusCRM .info test site using the specified crendentials.
        /// </summary>
        /// <param name="user">The user to sign in to.</param>
        /// <returns>The Browser instance.</returns>
        public static Browser LoginCom(string user)
        {
            var credentials = XmlReader.Read("Config/Credentials.xml")[user];
            string username = credentials.username;
            string password = credentials.password;
            return LoginCom(username, password);
        }
        /// <summary>
        ///     Sign in to MyPegasusCRM .com main site using the specified crendentials.
        /// </summary>
        /// <param name="user">The user to sign in to.</param>
        /// <returns>The Browser instance.</returns>
        public static Browser LoginMyPeg(string user)
        {
            var credentials = XmlReader.Read("Config/Credentials.xml")[user];
            string username = credentials.username;
            string password = credentials.password;
            return LoginMyPeg(username, password);
        }

        /// <summary>
        ///     Sign in to the PegasusCRM .info test site with the specified username/password.
        /// </summary>
        /// <param name="user">The username to use for logging in.</param>
        /// <param name="pass">The password to use for logging in.</param>
        /// <returns>The Browser instance.</returns>
        private static Browser LoginCom(string user, string pass)
        {
            string url = XmlReader.Read("Config/Config.xml").dotcom;
            var login = new LocatorReader("Login.xml");

            var browser = new Browser().GoToUrl(url).Maximize();
            browser.FillForm(login.Get("username"), user);
            browser.FillForm(login.Get("password"), pass);
            browser.Click(login.Get("submit-button"));
            browser.ImplicitWait = 10;
            return browser;
        }
        /// <summary>
        ///     Sign in to the MyPegasusCRM .com test site with the specified username/password.
        /// </summary>
        /// <param name="user">The username to use for logging in.</param>
        /// <param name="pass">The password to use for logging in.</param>
        /// <returns>The Browser instance.</returns>
        private static Browser LoginMyPeg(string user, string pass)
        {
            string url = XmlReader.Read("Config/Config.xml").mypeg;
            var login = new LocatorReader("Login.xml");

            var browser = new Browser().GoToUrl(url).Maximize();
            browser.FillForm(login.Get("username"), user);
            browser.FillForm(login.Get("password"), pass);
            browser.Click(login.Get("submit-button"));
            browser.ImplicitWait = 10;
            return browser;
        }
    }
}