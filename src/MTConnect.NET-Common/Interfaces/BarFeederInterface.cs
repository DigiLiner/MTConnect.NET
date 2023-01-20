// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Interfaces
{
    /// <summary>
    /// BarFeederInterface provides the set of information used to coordinate the operations between a Bar Feeder and another piece of equipment.
    /// </summary>
    public class BarFeederInterface : Interface 
    {
        public const string TypeId = "BarFeederInterface";


        public BarFeederInterface()
        {
            Type = TypeId;
        }
    }
}