// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Streams.Samples
{
    /// <summary>
    /// The measurement of the degree to which a substance opposes the passage of an electric current.
    /// </summary>
    public class ResistanceValue : SampleValue
    {
        protected override string MetricUnits => "OHM";
        protected override string InchUnits => "OHM";


        public ResistanceValue(double resistance)
        {
            Value = resistance;
        }
    }
}