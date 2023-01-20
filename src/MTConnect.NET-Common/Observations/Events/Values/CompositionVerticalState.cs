// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Observations.Events.Values
{
    public enum CompositionVerticalState
    {
        /// <summary>
        /// The position of the Composition element is oriented in a downward direction to the point of a positive confirmation
        /// </summary>
        DOWN,

        /// <summary>
        /// The position of the Composition element is oriented in an upward direction to the point of a positive confirmation
        /// </summary>
        UP,

        /// <summary>
        /// The position of the Composition element is not oriented in an upward direction to the point of a positive confirmation and is not oriented in a downward direction to the point of a positive confirmation. 
        /// It is in an intermediate position.
        /// </summary>
        TRANSITIONING
    }
}