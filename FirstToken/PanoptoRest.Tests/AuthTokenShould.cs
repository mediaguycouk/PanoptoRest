using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using PanoptoRest.Auth;
using Xunit;

namespace PanoptoRest.Tests
{
    public class AuthTokenShould
    {
        [Fact] // Uncomment this to run the test, but don't leave it in production
        //[Fact(Skip = "User/pass specific. Run in IDE only.")]
        public void GetTokenReturnsToken()
        {
            // Client ID and Secret are shown when creating a new API key https://drive.google.com/file/d/1ACiPoc_qSuxZI6jGnRyndG9_NsZ0ZARi/view?usp=drivesdk
            var clientId = "fd284390-23d9-4d2d-9a01-8865c06b01f4";
            var clientSecret = "0uyyRytIKT0jja52igwgzNEgXr/qWVvMwgFC8Y=";

            // This is your standard username and password https://drive.google.com/file/d/1eK-qBdJf9e10fG4dN6BxIVIIAS1UhMtj/view?usp=drivesdk
            // If you are using an integration user, you will need to add the integration\ infront of the username. See 2.1.c https://support.panopto.com/s/article/oauth2-for-services
            var username = "testUser0001";
            var password = "3-MONEY-mine-SLEEP-mercury";

            // This is the base Url of your site. It includes https:// and includes everything up to the first / https://drive.google.com/file/d/1JL0U3T3rRq9_b11J6i3FHMNRxoCxp1py/view?usp=drivesdk
            var baseUrl = "https://servername.cloud.panopto.eu";

            var token = new Token(clientId, clientSecret, username, password, baseUrl);
            var adminToken = token.GetAdminToken();

            Debug.WriteLine($"Your access token is {adminToken.AccessToken} and it expires at {adminToken.Expires}");

            // These tests should pass if your client, user and url details are correct
            Assert.True(string.IsNullOrEmpty(adminToken.Error));
            Assert.True(!string.IsNullOrEmpty(adminToken.AccessToken));
        }
    }
}
