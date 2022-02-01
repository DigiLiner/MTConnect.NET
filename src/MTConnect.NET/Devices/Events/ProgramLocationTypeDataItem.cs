// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace MTConnect.Devices.Events
{
    /// <summary>
    /// Defines whether the logic or motion program defined by PROGRAM is being executed from the local memory of the controller or from an outside source.
    /// </summary>
    public class ProgramLocationTypeDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "PROGRAM_LOCATION_TYPE";
        public const string NameId = "pgmLocationType";

        public enum SubTypes
        {
            /// <summary>
            /// The identity of the logic or motion program currently executing.
            /// </summary>
            ACTIVE,

            /// <summary>
            /// The identity of the primary logic or motion program currently being executed.It is the starting nest level in a call structure and may contain calls to sub programs.
            /// </summary>
            MAIN,

            /// <summary>
            /// The identity of a control program that is used to specify the order of execution of other programs.
            /// </summary>
            SCHEDULE
        }


        public ProgramLocationTypeDataItem()
        {
            DataItemCategory = CategoryId;
            Type = TypeId;
        }

        public ProgramLocationTypeDataItem(
            string parentId,
            SubTypes subType = SubTypes.MAIN
            )
        {
            Id = CreateId(parentId, NameId, GetSubTypeId(subType));
            DataItemCategory = CategoryId;
            Type = TypeId;
            SubType = subType.ToString();
            Name = NameId;
        }


        /// <summary>
        /// Determine if the DataItem with the specified Value is valid in the specified MTConnectVersion
        /// </summary>
        /// <param name="mtconnectVersion">The Version of the MTConnect Standard</param>
        /// <param name="value">The value of the DataItem</param>
        /// <returns>(true) if the value is valid. (false) if the value is invalid.</returns>
        public override bool IsValid(Version mtconnectVersion, object value)
        {
            if (value != null)
            {
                // Check if Unavailable
                if (value.ToString() == Streams.DataItem.Unavailable) return true;

                // Check Valid values in Enum
                var validValues = Enum.GetValues(typeof(Streams.Events.ProgramLocationType));
                foreach (var validValue in validValues)
                {
                    if (value.ToString() == validValue.ToString()) return true;
                }
            }

            return false;
        }


        public static string GetSubTypeId(SubTypes subType)
        {
            switch (subType)
            {
                case SubTypes.ACTIVE: return "act";
                case SubTypes.MAIN: return "main";
                case SubTypes.SCHEDULE: return "sch";
            }

            return null;
        }
    }
}