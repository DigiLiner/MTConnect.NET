// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace MTConnect.Configurations
{
    /// <summary>
    /// Configuration for an MTConnect SHDR > MQTT Agent Application
    /// </summary>
    public class ShdrMqttAgentApplicationConfiguration : ShdrAgentApplicationConfiguration, IShdrMqttAgentApplicationConfiguration
    {
        [JsonPropertyName("server")]
        public string Server { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("useTls")]
        public bool UseTls { get; set; }

        [JsonPropertyName("retryInterval")]
        public int RetryInterval { get; set; }


        public ShdrMqttAgentApplicationConfiguration()
        {
            Server = "localhost";
            Port = 1883;
            RetryInterval = 5000;
        }
    }
}