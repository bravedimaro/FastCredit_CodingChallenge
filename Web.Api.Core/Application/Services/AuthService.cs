using KissLog;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Application.Interface;
using Web.Domain.Auth;

namespace Web.Api.Core.Application.Services
{
    public class AuthService : IAuthService

    {
        private readonly IKLogger _logger;
       
        private readonly IConfiguration _config;
        public AuthService(IKLogger logger, IConfiguration configuration)
        {
            
            _config = configuration;

        }
        public async Task<AuthResponse> GenerateAuth(string client_secret)
        {
            AuthResponse authResponse = new AuthResponse();
            try
            {
                // Method to generate Token to access Api
                if (string.IsNullOrEmpty(client_secret))
                {
                    authResponse.ResponseCode = "01";
                    authResponse.ResponseMessage = "client_secret is required to generate token";
                    return authResponse;
                }
                var client = new RestClient($"{_config.GetSection("Auth0").GetSection("BaseUrl").Value}");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                AuthRequest authRequest = new AuthRequest();
                authRequest.audience = _config.GetSection("Auth0").GetSection("Audience").Value;
                authRequest.client_id = _config.GetSection("Auth0").GetSection("client_id").Value;
                authRequest.client_secret = client_secret;
                authRequest.grant_type = _config.GetSection("Auth0").GetSection("grant_type").Value;
                var body = JsonConvert.SerializeObject(authRequest);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                _logger.Info($"GenerateAuth: Successful response from API call {response.Content}");
               
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    var GetResp = JsonConvert.DeserializeObject<Data>(response.Content);
                  
                    authResponse.data = GetResp;
                    authResponse.ResponseCode = "00";
                    authResponse.ResponseMessage = "SUCCESS";
                    return authResponse;
                }
                else
                {
                    var GetRespErorr = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                    authResponse.ResponseCode = "01";
                    authResponse.ResponseMessage = GetRespErorr.error;
                    return authResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"[FastCredit_CodingChallenge][GenerateAuth][Response] => {ex.Message}");

            }
            return  new AuthResponse() { ResponseCode="01", ResponseMessage= "An error occoured. Please try again later or contact admin for resolution" };
        }
    }
}
