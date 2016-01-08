﻿using AutoHome.Universal.Models;
using AutoHome.Universal.Services.SettingsServices;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AutoHome.Universal.Services.DomoticzConnectionServices
{
    public class DomoticzConnection
    {
        public static DomoticzConnection Instance { get; }

        SettingsService _settings;

        static DomoticzConnection()
        {
            // implement singleton pattern
            Instance = Instance ?? new DomoticzConnection();
        }

        private DomoticzConnection()
        {
            _settings = SettingsService.Instance;
        }

        private async Task<T> ExecuteDomoticzApiCallAsync<T>(string command)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"http://{_settings.DomoticzServer}:{_settings.DomoticzPort}/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = new TimeSpan(0, 0, 0, 10, 0);

                    HttpResponseMessage response = await client.GetAsync(command, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode(); //throw exception if not succesfull

                    //this will only be executed on succesfull response
                    string responseContent = await response.Content.ReadAsStringAsync();

                    T responseDeserialized = await JsonConvert.DeserializeObjectAsync<T>(responseContent).ConfigureAwait(false);
                    return responseDeserialized;
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        public async Task<TestDomoticzResponse> TestDomoticzConnection()
        {
            TestDomoticzResponse response = await ExecuteDomoticzApiCallAsync<TestDomoticzResponse>("json.htm?type=command&param=addlogmessage&message=Hello from AutoHome").ConfigureAwait(false);
            return response;
        }
    }
}
