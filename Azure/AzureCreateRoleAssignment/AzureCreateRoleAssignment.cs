using Ayehu.Sdk.ActivityCreation.Extension;
using Ayehu.Sdk.ActivityCreation.Interfaces;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;

namespace AzureCreateRoleAssignment
{
    class AzureCreateRoleAssignment : IActivity
    {
        public string tenantId;
        public string clientId;
        public string clientSecret;
        public string subscriptionId;
        public string roleGUID;
        public string principalID;
        public string resourceId;

        public ICustomActivityResult Execute()
        {
            string authContextURL = "https://login.windows.net/" + tenantId;
            var authenticationContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(authContextURL);
            var credential = new ClientCredential(clientId, clientSecret);
            var result = authenticationContext.AcquireTokenAsync(resource: "https://management.azure.com/", clientCredential: credential).Result;

            if (result == null)
            {
                return this.GenerateActivityResult("Failed to obtain JWT token.");
            }

            string token = result.AccessToken;

            string apiURL = "https://management.azure.com" + resourceId + "/providers/Microsoft.Authorization/roleAssignments/" + roleGUID + "?api-version=2015-07-01";

            HttpWebRequest request1 = (HttpWebRequest)HttpWebRequest.Create(apiURL);
            request1.Method = "PUT";
            request1.Headers["Authorization"] = "Bearer " + token;
            request1.ContentType = "application/json";

            string jsonBody = "{\"properties\":{\"roleDefinitionId\": \"/subscriptions/" + subscriptionId + "/providers/Microsoft.Authorization/roleDefinitions/" + roleGUID + "\",\"principalId\": \"" + principalID + "\"}}";   

            try
            {
                using(var streamWriter = new StreamWriter(request1.GetRequestStream()))
                {
                    streamWriter.Write(jsonBody);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)request1.GetResponse();

                    using(var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var responseString = streamReader.ReadToEnd();

                        return this.GenerateActivityResult("Success");
                    }
                }
            }
            catch(WebException e)
            {
                return this.GenerateActivityResult("Error (" + e.Message + ")");
            }
        }
    }
}