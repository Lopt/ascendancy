namespace Core.Models.Resources
{
    using System;

    /// <summary>
    /// An Class for all Resources which values increments over time. 
    /// It can be calculated by the when the value was set; the current time and the increment over time.
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
        /// <param name="actionTime">Time when the value or increment was set</param> 
        /// <param name="value">Value of the resource.</param>
        /// <param name="increments">Increment value of the resource.</param>
        public void Set(DateTime actionTime, double value, double increments)
        {
            LastState = actionTime;
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
        /// <param name="time">Current server time</param> 
        /// <returns>The value at the given time</returns>
        public double GetValue(DateTime time)
        {
            var diff = time - LastState;
            return Math.Min(Math.Max(m_realValue + (Increments * diff.TotalSeconds), 0), MaximumValue);
        }

        /// <summary>
        /// Gets the value (in percent).
        /// </summary>
        /// <param name="time">Current server time</param> 
        /// <returns>The value (in percent) at the given time</returns>
        public double GetValuePercent(DateTime time)
        {
            return GetValue(time) / MaximumValue;
        }

        /// <summary>
        /// The real value.
        /// </summary>
        private double m_realValue;
    }
}
