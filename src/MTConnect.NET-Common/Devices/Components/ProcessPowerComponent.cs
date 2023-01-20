// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Devices.Components
{
    /// <summary>
    /// ProcessPower is a System that represents the information for a power source associated with a piece of equipment that supplies energy to the manufacturing process 
    /// separate from the Electric system.For example, this could be the power source for an EDM machining process, an electroplating line, or a welding system.
    /// </summary>
    public class ProcessPowerComponent : Component 
    {
        public const string TypeId = "ProcessPower";
        public const string NameId = "procpower";
        public new const string DescriptionText = "ProcessPower is a System that represents the information for a power source associated with a piece of equipment that supplies energy to the manufacturing process separate from the Electric system.For example, this could be the power source for an EDM machining process, an electroplating line, or a welding system.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version14;


        public ProcessPowerComponent()  { Type = TypeId; }
    }
}