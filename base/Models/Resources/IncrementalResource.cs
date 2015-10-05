namespace Core.Models.Resources
{
    using System;

    public class IncrementalResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Resource"/> class.
        /// </summary>
        public IncrementalResource(double value, double maximumValue, double increments)
        {
            m_realValue = value;
            LastState = DateTime.Now;
            MaximumValue = maximumValue;
            Increments = increments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Resource"/> class.
        /// </summary>
        public IncrementalResource()
        {
            m_realValue = 0;
            LastState = DateTime.Now;
            MaximumValue = 1;
            Increments = 0;
        }

        /// <summary>
        /// Set the specified value and increments.
        /// </summary>
        /// <param name="value">Value of the resource.</param>
        /// <param name="increments">Increment value of the resource.</param>
        public void Set(double value, double increments)
        {
            LastState = DateTime.Now;
            m_realValue = value;
            Increments = increments;
        }

        public DateTime LastState
        {
            get;
            private set;
        }

        public double MaximumValue
        {
            get;
            private set;
        }

        public double Increments
        {
            get;
            private set;
        }

        public double Value
        {
            get
            {
                var diff = DateTime.Now - LastState;
                return Math.Min(Math.Max(m_realValue + Increments * diff.Seconds, 0), MaximumValue);
            }
        }

        public double ValuePercent
        {
            get
            {
                return Value / MaximumValue;
            }
        }

        private double m_realValue;
    }
}

