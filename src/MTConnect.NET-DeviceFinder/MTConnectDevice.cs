// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using System.Net;
using System.Net.NetworkInformation;

namespace MTConnect.DeviceFinder
{
    /// <summary>
    /// An MTConnect Device that was found using DeviceFinder
    /// </summary>
    public struct MTConnectDevice
    {
        /// <summary>
        /// The IP Address of the MTConnect Agent
        /// </summary>
        public IPAddress IpAddress { get; set; }

        /// <summary>
        /// The Port of the MTConnect Agent
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The MAC Address of the device that the MTConnect Agent is operating on
        /// </summary>
        public PhysicalAddress MacAddress { get; set; }

        /// <summary>
        /// The UUID of the MTConnect Device
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// The Name of the MTConnect Device
        /// </summary>
        public string Name { get; set; }


        public MTConnectDevice(
            IPAddress ipAddress,
            int port,
            PhysicalAddress macAddress,
            string deviceUuid,
            string deviceName
            )
        {
            IpAddress = ipAddress;
            Port = port;
            MacAddress = macAddress;
            Uuid = deviceUuid;
            Name = deviceName;
        }
    }
}