using System.Collections.Generic;

namespace Semantics3.OAuth
{
    internal class OAuthParameters
    {
        public SortedDictionary<string, string> BaseProperties = new SortedDictionary<string, string>();
        public Dictionary<string, string> ExtraProperties = new Dictionary<string, string>();

        public string Callback
        {
            get
            {
                return safeGet(ExtraProperties, OAuthBase.OAuthCallbackKey);
            }
            set
            {
                addOrUpdate(ExtraProperties, OAuthBase.OAuthCallbackKey, value);
            }
        }

        public string ConsumerKey
        {
            get
            {
                return safeGet(BaseProperties, OAuthBase.OAuthConsumerKeyKey);
            }
            set
            {
                addOrUpdate(BaseProperties, OAuthBase.OAuthConsumerKeyKey, value);
            }
        }

        public string ConsumerSecret
        {
            get
            {
                return safeGet(ExtraProperties, OAuthBase.OAuthConsumerSecretKey);
            }
            set
            {
                addOrUpdate(ExtraProperties, OAuthBase.OAuthConsumerSecretKey, value);
            }
        }

        public string Nonce
        {
            get
            {
                return safeGet(BaseProperties, OAuthBase.OAuthNonceKey);
            }
            set
            {
                addOrUpdate(BaseProperties, OAuthBase.OAuthNonceKey, value);
            }
        }

        public string Scope
        {
            get
            {
                return safeGet(ExtraProperties, OAuthBase.OAuthScopeKey);
            }
            set
            {
                addOrUpdate(ExtraProperties, OAuthBase.OAuthScopeKey, value);
            }
        }

        public string Signature
        {
            get
            {
                return safeGet(ExtraProperties, OAuthBase.OAuthSignatureKey);
            }
            set
            {
                addOrUpdate(ExtraProperties, OAuthBase.OAuthSignatureKey, value);
            }
        }

        public string SignatureMethod
        {
            get
            {
                return safeGet(BaseProperties, OAuthBase.OAuthSignatureMethodKey);
            }
            set
            {
                addOrUpdate(BaseProperties, OAuthBase.OAuthSignatureMethodKey, value);
            }
        }

        public string Timestamp
        {
            get
            {
                return safeGet(BaseProperties, OAuthBase.OAuthTimestampKey);
            }
            set
            {
                addOrUpdate(BaseProperties, OAuthBase.OAuthTimestampKey, value);
            }
        }

        public string Token
        {
            get
            {
                return safeGet(BaseProperties, OAuthBase.OAuthTokenKey);
            }
            set
            {
                addOrUpdate(BaseProperties, OAuthBase.OAuthTokenKey, value);
            }
        }

        public string TokenSecret
        {
            get
            {
                return safeGet(ExtraProperties, OAuthBase.OAuthTokenSecretKey);
            }
            set
            {
                addOrUpdate(ExtraProperties, OAuthBase.OAuthTokenSecretKey, value);
            }
        }

        public string Verifier
        {
            get
            {
                return safeGet(BaseProperties, OAuthBase.OAuthVerifierKey);
            }
            set
            {
                addOrUpdate(BaseProperties, OAuthBase.OAuthVerifierKey, value);
            }
        }

        protected void addOrUpdate(IDictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary.ContainsKey(key))
            {
                if (value == null)
                {
                    dictionary.Remove(key);
                }
                else
                {
                    dictionary[key] = value;
                }
            }
            else if (value != null)
            {
                dictionary.Add(key, value);
            }
        }

        protected string safeGet(IDictionary<string, string> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                return null;
            }
        }
    }
}