using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Google.GData.Documents;
using Google.GData.Client;
using System.Diagnostics;

namespace gDriveTest
{
    class gDriveTest
    {
        bool hasOauthToken()
        {
            string token = getPersistedOAuthToken;
            return !(token == null || token.Trim().Length == 0);
        }

        string getPersistedOAuthToken()
        {
            return Properties.Settings.Default.OAuthToken;
        }

        OAuth2Parameters getParameters()
        {
            OAuth2Parameters parameters = new OAuth2Parameters();
            parameters.ClientId = ConfigurationManager.AppSettings.Get("ClientId");
            Console.WriteLine(parameters.ClientId);
            parameters.ClientSecret = ConfigurationManager.AppSettings.Get("ClientSecret");
            parameters.Scope = ConfigurationManager.AppSettings.Get("Scope");
            parameters.RedirectUri = ConfigurationManager.AppSettings.Get("RedirectURI");
            if (!(hasOauthToken()))
            {
                Properties.Settings.Default.OAuthToken = getOauthToken(parameters);
                Properties.Settings.Default.Save();
            }
            parameters.AccessToken = getPersistedOAuthToken();
            return parameters;
        }

        string getOauthToken(OAuth2Parameters parameters)
        {
            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            Console.WriteLine(authorizationUrl);
            Console.WriteLine("Please visit the URL above (if it wasn't opened automatically) "
              + "to authorize your OAuth request token.  Once that is complete, type in your access code to "
              + "continue...");
            Process.Start(authorizationUrl);
            parameters.AccessCode = Console.ReadLine();
            OAuthUtil.GetAccessToken(parameters);
            string accessToken = parameters.AccessToken;
            return accessToken;
        }

        void printDocumentList(OAuth2Parameters parameters)
        {
            GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "MyDocumentsListIntegration-v1", parameters);
            DocumentsService service = new DocumentsService("MyDocumentsListIntegration-v1");
            service.RequestFactory = requestFactory;
            DocumentsListQuery query = new DocumentsListQuery();
            DocumentsFeed feed = service.Query(query);
            foreach (DocumentEntry entry in feed.Entries)
            {
                Console.WriteLine(entry.Title.Text);
            }
            Console.ReadKey();
        }

        public void handleAuthAndPrintDocuments()
        {
            printDocumentList(getParameters());
        }

    }
}
