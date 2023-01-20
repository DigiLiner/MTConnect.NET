// Copyright (c) 2023 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace MTConnect.Devices.Configurations
{
    /// <summary>
    /// The process of transforming to the origin position of the coordinate system from a parent coordinate system using Translation and Rotation.
    /// </summary>
    public interface ITransformation
    {
        /// <summary>
        /// Translations along X, Y, and Z axes are expressed as x,y, and z respectively within a 3-dimensional vector.      
        /// </summary>
        string Translation { get; }

        /// <summary>
        /// Rotations about X, Y, and Z axes are expressed in A, B, and C respectively within a 3-dimensional vector.
        /// </summary>
        string Rotation { get; }
    }
}