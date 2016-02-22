namespace Core.Models.Resources
{
    using System;

    /// <summary>
    /// An class for all resources which have a state, which only changes by action.
    /// </summary>
    public class StateResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Resources.StateResource"/> class.
        /// </summary>
        public StateResource()
        {
            m_realValue = 0;
            LastState = DateTime.Now;
            ValidUntil = DateTime.Now;
            MaximumValue = 1;
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
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get
            {
                return m_realValue;
            }

            set
            {
                LastState = DateTime.Now;
                m_realValue = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Core.Models.Resources.StateResource"/> is valid.
        /// </summary>
        /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
        public bool Valid
        {
            get
            {
                return ValidUntil >= DateTime.Now;
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
        /// The valid until.
        /// </summary>
        public DateTime ValidUntil = DateTime.Now;

        /// <summary>
        /// The m real value.
        /// </summary>
        private double m_realValue;
    }
}