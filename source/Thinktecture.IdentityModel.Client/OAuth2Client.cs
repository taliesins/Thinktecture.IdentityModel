﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Client
{
	public class OAuth2Client
	{
		protected HttpClient _client;
		protected ClientAuthenticationStyle _authenticationStyle;
		protected Uri _address;
		protected string _clientId;
		protected string _clientSecret;

		public enum ClientAuthenticationStyle
		{
			BasicAuthentication,
			PostValues,
			None
		};

		public OAuth2Client(Uri address)
			: this(address, new HttpClientHandler())
		{ }

        public OAuth2Client(Uri address, HttpMessageHandler innerHttpClientHandler)
            : this(address, innerHttpClientHandler, ClientAuthenticationStyle.None)
        { }

        public OAuth2Client(Uri address, HttpMessageHandler innerHttpClientHandler, ClientAuthenticationStyle style)
            : this(address, null, null, innerHttpClientHandler, style)
		{ }

		public OAuth2Client(Uri address, string clientId, string clientSecret, ClientAuthenticationStyle style = ClientAuthenticationStyle.BasicAuthentication)
			: this(address, clientId, clientSecret, new HttpClientHandler(), style)
		{ }

		public OAuth2Client(Uri address, string clientId, string clientSecret, HttpMessageHandler innerHttpClientHandler, ClientAuthenticationStyle style = ClientAuthenticationStyle.BasicAuthentication)
		{
            if (innerHttpClientHandler == null)
            {
                throw new ArgumentNullException("innerHttpClientHandler");
            }

            _client = new HttpClient(innerHttpClientHandler)
            {
                BaseAddress = address
            };

            _address = address;
            _authenticationStyle = style;

			if (style == ClientAuthenticationStyle.BasicAuthentication)
			{
				_client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
			}
			else if (style == ClientAuthenticationStyle.PostValues)
			{
				_clientId = clientId;
				_clientSecret = clientSecret;
			}
		}

		public string CreateCodeFlowUrl(string clientId, string scope = null, string redirectUri = null, string state = null, Dictionary<string, string> additionalValues = null)
		{
			return CreateAuthorizeUrl(
				clientId,
				OAuth2Constants.ResponseTypes.Code,
				scope,
				redirectUri,
				state,
				additionalValues);
		}

		public string CreateImplicitFlowUrl(string clientId, string scope = null, string redirectUri = null, string state = null, Dictionary<string, string> additionalValues = null)
		{
			return CreateAuthorizeUrl(
				clientId,
				OAuth2Constants.ResponseTypes.Token,
				scope,
				redirectUri,
				state,
				additionalValues);
		}

		public string CreateAuthorizeUrl(string clientId, string responseType, string scope = null, string redirectUri = null, string state = null, Dictionary<string, string> additionalValues = null)
		{
			var values = new Dictionary<string, string>
			{
				{ OAuth2Constants.ClientId, clientId },
				{ OAuth2Constants.ResponseType, responseType }
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				values.Add(OAuth2Constants.Scope, scope);
			}

			if (!string.IsNullOrWhiteSpace(redirectUri))
			{
				values.Add(OAuth2Constants.RedirectUri, redirectUri);
			}

			if (!string.IsNullOrWhiteSpace(state))
			{
				values.Add(OAuth2Constants.State, state);
			}

			return CreateAuthorizeUrl(_address, Merge(values, additionalValues));
		}

		public static string CreateAuthorizeUrl(Uri endpoint, Dictionary<string, string> values)
		{
			var qs = string.Join("&", values.Select(kvp => String.Format("{0}={1}", WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value))).ToArray());
			return string.Format("{0}?{1}", endpoint.AbsoluteUri, qs);
		}

		public Task<TokenResponse> RequestResourceOwnerPasswordAsync(string userName, string password, string scope = null, Dictionary<string, string> additionalValues = null)
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.Password },
				{ OAuth2Constants.UserName, userName },
				{ OAuth2Constants.Password, password }
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				fields.Add(OAuth2Constants.Scope, scope);
			}

			return Request(Merge(fields, additionalValues));
		}

		public Task<TokenResponse> RequestAuthorizationCodeAsync(string code, string redirectUri, Dictionary<string, string> additionalValues = null)
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.AuthorizationCode },
				{ OAuth2Constants.Code, code },
				{ OAuth2Constants.RedirectUri, redirectUri }
			};

			return Request(Merge(fields, additionalValues));
		}

		public Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken, Dictionary<string, string> additionalValues = null)
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.RefreshToken },
				{ OAuth2Constants.RefreshToken, refreshToken }
			};

			return Request(Merge(fields, additionalValues));
		}

		public Task<TokenResponse> RequestClientCredentialsAsync(string scope = null, Dictionary<string, string> additionalValues = null)
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.ClientCredentials }
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				fields.Add(OAuth2Constants.Scope, scope);
			}

			return Request(Merge(fields, additionalValues));
		}

		public Task<TokenResponse> RequestAssertionAsync(string assertionType, string assertion, string scope = null, Dictionary<string, string> additionalValues = null)
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, assertionType },
				{ OAuth2Constants.Assertion, assertion },
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				fields.Add(OAuth2Constants.Scope, scope);
			}

			return Request(Merge(fields, additionalValues));
		}

		public async Task<TokenResponse> Request(Dictionary<string, string> form)
		{
			var response = await _client.PostAsync(string.Empty, new FormUrlEncodedContent(form)).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			return new TokenResponse(content);
		}

		private Dictionary<string, string> Merge(Dictionary<string, string> explicitValues, Dictionary<string, string> additionalValues = null)
		{
			var merged = explicitValues;

			if (_authenticationStyle == ClientAuthenticationStyle.PostValues)
			{
				merged.Add(OAuth2Constants.ClientId, _clientId);
				merged.Add(OAuth2Constants.ClientSecret, _clientSecret);
			}

			if (additionalValues != null)
			{
				merged =
					explicitValues.Concat(additionalValues.Where(add => !explicitValues.ContainsKey(add.Key)))
										 .ToDictionary(final => final.Key, final => final.Value);
			}

			return merged;
		}
	}
}