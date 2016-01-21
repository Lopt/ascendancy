namespace Core.Models.Resources
{
    using System;

    /// <summary>
    /// Incremental resource.
    /// </summary>
    public class IncrementalResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Resources.IncrementalResource"/> class.
        /// </summary>
        /// <param name="value">Value of the resource.</param>
        /// <param name="maximumValue">Maximum value.</param>
        /// <param name="increments">Increments value.</param>
        public IncrementalResource(double value, double maximumValue, double increments)
        {
            m_realValue = value;
            LastState = DateTime.Now;
            MaximumValue = maximumValue;
            Increments = increments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Resources.IncrementalResource"/> class.
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
            Increments += increments;
        }

        /// <summary>
        /// Gets the last state.
        /// </summary>
        /// <value>The last state.</value>
        public DateTime LastState
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public double MaximumValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the increments.
        /// </summary>
        /// <value>The increments.</value>
        public double Increments
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get
            {
                var diff = DateTime.Now - LastState;
                return Math.Min(Math.Max(m_realValue + Increments * diff.TotalSeconds, 0), MaximumValue);
            }
        }

        /// <summary>
        /// Gets the value percent.
        /// </summary>
        /// <value>The value percent.</value>
        public double ValuePercent
        {
            get
            {
                return Value / MaximumValue;
            }
        }

        /// <summary>
        /// The m real value.
        /// </summary>
        private double m_realValue;
    }
}
