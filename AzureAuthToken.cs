﻿using System;
using System.Net;

namespace TranslationRobot
{
    public class AzureAuthToken

    {

        /// URL of the token service

        private static readonly Uri ServiceUrl = new Uri("https://api.cognitive.microsoft.com/sts/v1.0/issueToken");

        /// Name of header used to pass the subscription key to the token service

        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        /// After obtaining a valid token, this class will cache it for this duration.

        /// Use a duration of 5 minutes, which is less than the actual token lifetime of 10 minutes.

        private static readonly TimeSpan TokenCacheDuration = new TimeSpan(0, 5, 0);



        /// Cache the value of the last valid token obtained from the token service.

        private string storedTokenValue = string.Empty;

        /// When the last valid token was obtained.

        private DateTime storedTokenTime = DateTime.MinValue;



        /// Gets the subscription key.

        public string SubscriptionKey { get; private set; }



        /// Gets the HTTP status code for the most recent request to the token service.

        public HttpStatusCode RequestStatusCode { get; private set; }



        /// <summary>

        /// Creates a client to obtain an access token.

        /// </summary>

        /// <param name="key">Subscription key to use to get an authentication token.</param>

        public AzureAuthToken(string key)

        {

            if (string.IsNullOrEmpty(key))

            {

                throw new ArgumentNullException("key", "A subscription key is required");

            }

            SubscriptionKey = key;

            RequestStatusCode = HttpStatusCode.InternalServerError;

        }



        /// <summary>

        /// Gets a token for the specified subscription.

        /// </summary>

        /// <returns>The encoded JWT token prefixed with the string "Bearer ".</returns>

        /// <remarks>

        /// This method uses a cache to limit the number of request to the token service.

        /// A fresh token can be re-used during its lifetime of 10 minutes. After a successful

        /// request to the token service, this method caches the access token. Subsequent 

        /// invocations of the method return the cached token for the next 5 minutes. After

        /// 5 minutes, a new token is fetched from the token service and the cache is updated.

        /// </remarks>

        public string GetAccessToken()

        {

            // Re-use the cached token if there is one.

            if ((DateTime.Now - storedTokenTime) < TokenCacheDuration)

            {

                return storedTokenValue;

            }



            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Accept", "application/jwt");
                client.Headers.Add(OcpApimSubscriptionKeyHeader, SubscriptionKey);
             
                string result = client.UploadString(ServiceUrl, "");

                var token = result;

                storedTokenTime = DateTime.Now;

                storedTokenValue = "Bearer " + token;

                return storedTokenValue;



            }
        }

    }
}