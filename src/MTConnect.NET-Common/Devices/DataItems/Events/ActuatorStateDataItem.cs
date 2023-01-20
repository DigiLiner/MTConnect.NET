// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using MTConnect.Observations;
using MTConnect.Observations.Input;
using System;

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// Represents the operational state of an apparatus for moving or controlling a mechanism or system.
    /// </summary>
    public class ActuatorStateDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "ACTUATOR_STATE";
        public const string NameId = "actuatorState";
        public new const string DescriptionText = "Represents the operational state of an apparatus for moving or controlling a mechanism or system.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version12;


        public ActuatorStateDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public ActuatorStateDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }


        /// <summary>
        /// Determine if the DataItem with the specified Observation is valid in the specified MTConnectVersion
        /// </summary>
        /// <param name="mtconnectVersion">The Version of the MTConnect Standard</param>
        /// <param name="observation">The Observation to validate</param>
        /// <returns>A DataItemValidationResult indicating if Validation was successful and a Message</returns>
        protected override ValidationResult OnValidation(Version mtconnectVersion, IObservationInput observation)
        {
            if (observation != null && !observation.Values.IsNullOrEmpty())
            {
                // Get the Result Value for the Observation
                var result = observation.GetValue(ValueKeys.Result);
                if (result != null)
                {
                    // Check Valid values in Enum
                    var validValues = Enum.GetValues(typeof(Observations.Events.Values.ActuatorState));
                    foreach (var validValue in validValues)
                    {
                        if (result == validValue.ToString())
                        {
                            return new ValidationResult(true);
                        }
                    }

                    return new ValidationResult(false, "'" + result + "' is not a valid value");
                }
                else
                {
                    return new ValidationResult(false, "No Result is specified for the Observation");
                }
            }

            return new ValidationResult(false, "No Observation is Specified");
        }
    }
}