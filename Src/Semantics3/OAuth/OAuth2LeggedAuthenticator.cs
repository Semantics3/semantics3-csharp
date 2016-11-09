using System;
using System.Net.Http;
using System.Text;

namespace Semantics3.OAuth
{
    internal class OAuth2LeggedAuthenticator
    {
        private readonly string _constumerKey;
        private readonly string _constumerSecret;

        public OAuth2LeggedAuthenticator(string consumerKey, string consumerSecret)
        {
            this._constumerKey = consumerKey;
            this._constumerSecret = consumerSecret;
        }

        public HttpRequestMessage CreateHttpWebRequest(HttpMethod httpMethod, Uri targetUri)
        {
            var request = new HttpRequestMessage(httpMethod, targetUri);

            var authorizationHeader = GenerateAuthorizationHeader(request.RequestUri, this._constumerKey, this._constumerSecret, null, null, request.Method);
            request.Headers.Add("User-Agent", "Semantics3 C# Lib/1.0.0.24");
            request.Headers.Add("Authorization", authorizationHeader);

            return request;
        }

        private static string GenerateAuthorizationHeader(
            Uri uri,
            string consumerKey,
            string consumerSecret,
            string token,
            string tokenSecret,
            HttpMethod httpMethod)
        {
            var parameters = new OAuthParameters
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Token = token,
                TokenSecret = tokenSecret,
                SignatureMethod = OAuthBase.HMACSHA1SignatureType,
                Timestamp = OAuthBase.GenerateTimeStamp(),
                Nonce = OAuthBase.GenerateNonce()
            };

            var signature = OAuthBase.GenerateSignature(uri, httpMethod, parameters);

            var sb = new StringBuilder();
            sb.AppendFormat("OAuth {0}=\"{1}\",", OAuthBase.OAuthVersionKey, OAuthBase.OAuthVersion);
            sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthNonceKey, OAuthBase.EncodingPerRFC3986(parameters.Nonce));
            sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthTimestampKey, OAuthBase.EncodingPerRFC3986(parameters.Timestamp));
            sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthConsumerKeyKey, OAuthBase.EncodingPerRFC3986(parameters.ConsumerKey));
            if (parameters.BaseProperties.ContainsKey(OAuthBase.OAuthVerifierKey))
            {
                sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthVerifierKey, OAuthBase.EncodingPerRFC3986(parameters.BaseProperties[OAuthBase.OAuthVerifierKey]));
            }
            if (!string.IsNullOrEmpty(parameters.Token))
            {
                sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthTokenKey, OAuthBase.EncodingPerRFC3986(parameters.Token));
            }
            if (parameters.BaseProperties.ContainsKey(OAuthBase.OAuthCallbackKey))
            {
                sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthCallbackKey, OAuthBase.EncodingPerRFC3986(parameters.BaseProperties[OAuthBase.OAuthCallbackKey]));
            }
            sb.AppendFormat("{0}=\"{1}\",", OAuthBase.OAuthSignatureMethodKey, OAuthBase.HMACSHA1SignatureType);
            sb.AppendFormat("{0}=\"{1}\"", OAuthBase.OAuthSignatureKey, OAuthBase.EncodingPerRFC3986(signature));

            return sb.ToString();
        }
    }
}