﻿using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinFormsApp.Models;

namespace WinFormsApp.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly SessionService _sessionService;

        public AuthenticationService(HttpClient httpClient, SessionService sessionService)
        {
            _httpClient = httpClient;
            _sessionService = sessionService;
        }

        public async Task<bool> Login(string username, string password)
        {
            var url = $"{Configuration.Configuration.URL}/auth/admin/login";
            var payload = new { username, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jsonNode = JsonDocument.Parse(json).RootElement;

                    var token = jsonNode.GetProperty("admin").GetProperty("token").GetString();
                    var userId = jsonNode.GetProperty("admin").GetProperty("id").GetInt32();

                    _sessionService.SetToken(token);

                    var user = new User
                    {
                        Id = userId,
                        Username = username
                        // Populate other fields if necessary
                    };

                    _sessionService.SetUser(user);

                    return true;
                }
                else
                {
                    var errorMessage = await ExtractErrorMessage(response);
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during login: {ex.Message}", ex);
            }
        }

        public async Task Logout()
        {
            var url = $"{Configuration.Configuration.URL}/auth/logout";
            try
            {
                await _httpClient.PostAsync(url, null);
            }
            finally
            {
                _sessionService.ClearSession();
            }
        }

        private async Task<string> ExtractErrorMessage(HttpResponseMessage response)
        {
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                var jsonNode = JsonDocument.Parse(json).RootElement;
                return jsonNode.GetProperty("error").GetString() ?? "An unexpected error occurred.";
            }
            catch
            {
                return "An unexpected error occurred while processing the error response.";
            }
        }
    }
}
