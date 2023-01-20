// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Devices.DataItems.Samples
{
    /// <summary>
    /// The measurement of the amount of a substance remaining compared to the planned maximum amount of that substance.
    /// </summary>
    public class FillLevelDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.SAMPLE;
        public const string TypeId = "FILL_LEVEL";
        public const string NameId = "fillLevel";
        public const string DefaultUnits = Devices.Units.PERCENT;
        public new const string DescriptionText = "The measurement of the amount of a substance remaining compared to the planned maximum amount of that substance.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version12;


        public FillLevelDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
            Units = DefaultUnits;
        }

        public FillLevelDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
            Units = DefaultUnits;
        }
    }
}