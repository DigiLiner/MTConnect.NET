// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.Samples
{
    /// <summary>
    /// The geometric capacity of an object or container.
    /// </summary>
    public class CapacitySpatialDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.SAMPLE;
        public const string TypeId = "CAPACITY_SPATIAL";
        public const string NameId = "capSpatial";

        public CapacitySpatialDataItem()
        {
            DataItemCategory = CategoryId;
            Type = TypeId;
            Units = Devices.Units.CUBIC_MILLIMETER;
        }

        public CapacitySpatialDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            DataItemCategory = CategoryId;
            Type = TypeId;
            Name = NameId;
            Units = Devices.Units.CUBIC_MILLIMETER;
        }
    }
}