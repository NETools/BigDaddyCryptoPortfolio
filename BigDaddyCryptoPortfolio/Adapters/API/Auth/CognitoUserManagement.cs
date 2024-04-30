using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using BigDaddyCryptoPortfolio.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Auth
{
    public partial class CognitoUserManagement : IUserManagement
    {
        private const string CLIENT_ID = "7id2v40a7s0babo1dfgme82qg5";

        private BasicAWSCredentials _awsCredentials = new BasicAWSCredentials("AKIAU6GD25MDRJZWOL4B", "IcvI6WrcXzWG00W0WjFnXUka7J5DL60F876KlO7O");
        private AmazonCognitoIdentityProviderClient _cognitoClient;
		private IUserSession _userSession;

        public CognitoUserManagement(IUserSession userSession)
        {
            _cognitoClient = new AmazonCognitoIdentityProviderClient(_awsCredentials, RegionEndpoint.EUCentral1);
			_userSession = userSession;
        }

		public async Task<ApiResult<bool>> ConfirmUser(Dictionary<string, string> credentials)
		{
			var result = new ApiResult<bool>();

			var username = credentials["email"];

			var regex = EmailRegex();
			credentials["email"] = regex.Replace(username, "");

			var confirmRequest = new ConfirmSignUpRequest()
			{
				ClientId = CLIENT_ID,
				Username = credentials["email"],
				ConfirmationCode = credentials["code"]
			};

			try
			{
				var response = await _cognitoClient.ConfirmSignUpAsync(confirmRequest);
				result.Result = true;
				result.Code = response.HttpStatusCode;

			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
			}

			return result;
		}
		public async Task<ApiResult<bool>> ResendCode(Dictionary<string, string> credentials)
		{
			var result = new ApiResult<bool>();

			var username = credentials["email"];

			var regex = EmailRegex();
			credentials["email"] = regex.Replace(username, "");

			var resendConfirmCodeRequest = new ResendConfirmationCodeRequest()
			{
				Username = credentials["email"],
				ClientId = CLIENT_ID
			};

			try
			{
				var response = await _cognitoClient.ResendConfirmationCodeAsync(resendConfirmCodeRequest);
				
				result.Result = true;
				result.Code = response.HttpStatusCode;

			}
			catch(Exception ex)
			{
				result.Message = ex.Message;
			}

			return result;
		}
		public async Task<ApiResult<GetUserResponse>> Login(Dictionary<string, string> credentials)
		{
			var apiResult = new ApiResult<GetUserResponse>();

			var username = credentials["USERNAME"];

			var regex = EmailRegex();
			credentials["USERNAME"] = regex.Replace(username, "");

			var authRequest = new InitiateAuthRequest()
			{
				ClientId = CLIENT_ID,
				AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
				AuthParameters = credentials
			};


			try
			{
				var authResponse = await _cognitoClient.InitiateAuthAsync(authRequest);
				var user = await _cognitoClient.GetUserAsync(new GetUserRequest()
				{
					AccessToken = authResponse.AuthenticationResult.AccessToken
				});

				apiResult.Result = user;
				apiResult.Message = "OK";
				apiResult.Code = user.HttpStatusCode;

				_userSession.StartSession(user.Username, user.UserAttributes);
			}
			catch (UserNotConfirmedException ex)
			{
				apiResult.Message = ex.Message;
				apiResult.Code = System.Net.HttpStatusCode.NotAcceptable;
			}
			catch (AmazonServiceException ex)
			{
				apiResult.Message = ex.Message;
				apiResult.Code = ex.StatusCode;
			}
			return apiResult;
		}

		public Task<ApiResult<bool>> Logout(string username)
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResult<bool>> Register(Dictionary<string, string> credentials)
		{
			var apiResult = new ApiResult<bool>();

			string username = credentials["email"];

			var regex = EmailRegex();
			username = regex.Replace(username, "");

			var signUpRequest = new SignUpRequest()
			{
				ClientId = CLIENT_ID,
				Username = username,
				Password = credentials["password"],
				UserAttributes = new List<AttributeType>()
				{
					new AttributeType()
					{
						Name = "email",
						Value = credentials["email"]
					},
					new AttributeType()
					{
						Name = "name",
						Value = credentials["email"]
					}
				}
			};

			try
			{
				var response = await _cognitoClient.SignUpAsync(signUpRequest);

				apiResult.Result = true;
				apiResult.Message = "OK";
				apiResult.Code = response.HttpStatusCode;

			}
			catch (Exception ex)
			{
				apiResult.Result = false;
				apiResult.Message = ex.Message;
				apiResult.Code = System.Net.HttpStatusCode.ServiceUnavailable;
			}

			return apiResult;
		}

		[GeneratedRegex("[^a-zA-Z0-9]")]
		private static partial Regex EmailRegex();		
	}
}