// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using System;

namespace MTConnect.Devices.DataItems.Samples
{
    /// <summary>
    /// The measurement of the feedrate for the axes, or a single axis, associated with a Path component-a vector.
    /// </summary>
    public class PathFeedrateDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.SAMPLE;
        public const string TypeId = "PATH_FEEDRATE";
        public const string NameId = "feed";
        public const string DefaultUnits = Devices.Units.MILLIMETER_PER_SECOND;
        public new const string DescriptionText = "The measurement of the feedrate for the axes, or a single axis, associated with a Path component-a vector.";

        public override string TypeDescription => DescriptionText;


        public enum SubTypes
        {
            /// <summary>
            /// The measured or reported value of an observation.
            /// </summary>
            ACTUAL,

            /// <summary>
            /// Directive value including adjustments such as an offset or overrides.
            /// </summary>
            COMMANDED,

            /// <summary>
            /// The feedrate specified by a logic or motion program, by a pre-set value, or set by a switch as the feedrate for a linear axis when operating in a manual state or method(jogging).
            /// </summary>
            JOG,

            /// <summary>
            /// (DEPRECATED) Operator�s overridden value.
            /// </summary>
            OVERRIDE,

            /// <summary>
            /// Directive value without offsets and adjustments.
            /// </summary>
            PROGRAMMED,

            /// <summary>
            /// Performing an operation faster or in less time than nominal rate.
            /// </summary>
            RAPID
        }


        public PathFeedrateDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
            Units = DefaultUnits;
        }

        public PathFeedrateDataItem(
            string parentId,
            SubTypes subType)
        {
            Id = CreateId(parentId, NameId, GetSubTypeId(subType));
            Category = CategoryId;
            Type = TypeId;
            SubType = subType.ToString();
            Name = NameId;
            Units = DefaultUnits;
        }


        protected override IDataItem OnProcess(IDataItem dataItem, Version mtconnectVersion)
        {
            if (SubType == SubTypes.JOG.ToString() && mtconnectVersion < MTConnectVersions.Version13) return null;
            if (SubType == SubTypes.PROGRAMMED.ToString() && mtconnectVersion < MTConnectVersions.Version13) return null;
            if (SubType == SubTypes.RAPID.ToString() && mtconnectVersion < MTConnectVersions.Version13) return null;

            return dataItem;
        }

        public override string SubTypeDescription => GetSubTypeDescription(SubType);

        public static string GetSubTypeDescription(string subType)
        {
            var s = subType.ConvertEnum<SubTypes>();
            switch (s)
            {
                case SubTypes.ACTUAL: return "The measured or reported value of an observation.";
                case SubTypes.COMMANDED: return "Directive value including adjustments such as an offset or overrides.";
                case SubTypes.JOG: return "The feedrate specified by a logic or motion program, by a pre-set value, or set by a switch as the feedrate for a linear axis when operating in a manual state or method(jogging).";
                case SubTypes.PROGRAMMED: return "Directive value without offsets and adjustments.";
                case SubTypes.RAPID: return "Performing an operation faster or in less time than nominal rate.";
            }

            return null;
        }

        public static string GetSubTypeId(SubTypes subType)
        {
            switch (subType)
            { 
                case SubTypes.ACTUAL: return "act";
                case SubTypes.COMMANDED: return "cmd";
                case SubTypes.JOG: return "jog";
                case SubTypes.PROGRAMMED: return "prg";
                case SubTypes.RAPID: return "rapid";
            }

            return null;
        }
    }
}