// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Devices.Components
{
    /// <summary>
    /// Feeder is a System that represents the information for a system that manages the delivery of materials within a piece of equipment. 
    /// For example, this could describe the wire delivery system for an EDM or welding process; 
    /// conveying system or pump and valve system distributing material to a blending station; or a fuel delivery system feeding a furnace.
    /// </summary>
    public class FeederComponent : Component 
    {
        public const string TypeId = "Feeder";
        public const string NameId = "feed";
        public new const string DescriptionText = "Feeder is a System that represents the information for a system that manages the delivery of materials within a piece of equipment. For example, this could describe the wire delivery system for an EDM or welding process; conveying system or pump and valve system distributing material to a blending station; or a fuel delivery system feeding a furnace.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version14;


        public FeederComponent()  { Type = TypeId; }
    }
}