// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Devices.Components
{
    /// <summary>
    /// WasteDisposal is an Auxiliary that represents the information for a unit comprised of all the parts involved in removing manufacturing byproducts from a piece of equipment.
    /// </summary>
    public class WasteDisposalComponent : Component 
    {
        public const string TypeId = "WasteDisposal";
        public const string NameId = "waste";
        public new const string DescriptionText = "WasteDisposal is an Auxiliary that represents the information for a unit comprised of all the parts involved in removing manufacturing byproducts from a piece of equipment.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version14;


        public WasteDisposalComponent()  { Type = TypeId; }
    }
}