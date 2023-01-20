// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// Accumulation of the number of times an operation has attempted to, or is planned to attempt to, transfer materials, 
    /// parts, or other items from one location to another.
    /// </summary>
    public class TransferCountDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "TRANSFER_COUNT";
        public const string NameId = "transferCount";
        public new const string DescriptionText = "Accumulation of the number of times an operation has attempted to, or is planned to attempt to, transfer materials, parts, or other items from one location to another.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version18;

        public enum SubTypes
        {
            /// <summary>
            /// An accumulation representing all actions, items, or activities being counted independent of the outcome.ALL is the default subType.
            /// </summary>
            ALL,

            /// <summary>
            /// An accumulation representing actions, items, or activities being counted that do not conform to specification or expectation.
            /// </summary>
            BAD,

            /// <summary>
            /// An accumulation representing actions, items, or activities being counted that conform to specification or expectation.
            /// </summary>
            GOOD,

            /// <summary>
            /// The goal of the operation or process.
            /// </summary>
            TARGET,

            /// <summary>
            /// An accumulation representing actions, items, or activities yet to be counted.
            /// </summary>
            REMAINING,

            /// <summary>
            /// An accumulation representing actions, items, or activities that have been completed, independent of the outcome.
            /// </summary>
            COMPLETE,

            /// <summary>
            /// An accumulation representing actions or activities that were attempted, but failed to complete or resulted in an unexpected or unacceptable outcome.
            /// </summary>
            FAILED,

            /// <summary>
            /// An accumulation representing actions or activities that were attempted, but terminated before they could be completed.
            /// </summary>
            ABORTED
        }


        public TransferCountDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public TransferCountDataItem(
            string parentId,
            SubTypes subType
            )
        {
            Id = CreateId(parentId, NameId, GetSubTypeId(subType));
            Category = CategoryId;
            Type = TypeId;
            SubType = subType.ToString();
            Name = NameId;
            Units = Devices.Units.COUNT;
        }

        public override string SubTypeDescription => GetSubTypeDescription(SubType);

        public static string GetSubTypeDescription(string subType)
        {
            var s = subType.ConvertEnum<SubTypes>();
            switch (s)
            {
                case SubTypes.ALL: return "An accumulation representing all actions, items, or activities being counted independent of the outcome.ALL is the default subType.";
                case SubTypes.BAD: return "An accumulation representing actions, items, or activities being counted that do not conform to specification or expectation.";
                case SubTypes.GOOD: return "An accumulation representing actions, items, or activities being counted that conform to specification or expectation.";
                case SubTypes.TARGET: return "The goal of the operation or process.";
                case SubTypes.REMAINING: return "An accumulation representing actions, items, or activities yet to be counted.";
                case SubTypes.COMPLETE: return "An accumulation representing actions, items, or activities that have been completed, independent of the outcome.";
                case SubTypes.FAILED: return "An accumulation representing actions or activities that were attempted, but failed to complete or resulted in an unexpected or unacceptable outcome.";
                case SubTypes.ABORTED: return "An accumulation representing actions or activities that were attempted, but terminated before they could be completed.";
            }

            return null;
        }

        public static string GetSubTypeId(SubTypes subType)
        {
            switch (subType)
            {
                case SubTypes.ALL: return "all";
                case SubTypes.BAD: return "bad";
                case SubTypes.GOOD: return "good";
                case SubTypes.TARGET: return "target";
                case SubTypes.REMAINING: return "remaining";
                case SubTypes.COMPLETE: return "complete";
                case SubTypes.FAILED: return "failed";
                case SubTypes.ABORTED: return "aborted";
            }

            return null;
        }
    }
}