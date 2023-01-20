// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// A reference to the offset variables for a work piece or part associated with a Path in a Controller type component.
    /// </summary>
    public class WorkOffsetDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "WORK_OFFSET";
        public const string NameId = "workOffset";
        public new const string DescriptionText = "A reference to the offset variables for a work piece or part associated with a Path in a Controller type component.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version14;


        public WorkOffsetDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public WorkOffsetDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}