// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// Identifier of a part or product moving through the manufacturing process.
    /// </summary>
    public class PartNumberDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "PART_NUMBER";
        public const string NameId = "partNumber";
        public new const string DescriptionText = "Identifier of a part or product moving through the manufacturing process.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MaximumVersion => MTConnectVersions.Version16;
        public override System.Version MinimumVersion => MTConnectVersions.Version14;


        public PartNumberDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public PartNumberDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}