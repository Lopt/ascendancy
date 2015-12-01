namespace Core.Models.Resources
{
    using System;

    public class StateResource
    {
        public StateResource()
        {
            m_realValue = 0;
            LastState = DateTime.Now;
            ValidUntil = DateTime.Now;
            MaximumValue = 1;
        }


        public DateTime LastState
        {
            get;
            private set;
        }

        public double MaximumValue
        {
            get;
            set;
        }

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

        public bool Valid
        {
            get
            {
                return ValidUntil >= DateTime.Now;
            }
        }

        public double ValuePercent
        {
            get
            {
                return Value / MaximumValue;
            }
        }

        public DateTime ValidUntil = DateTime.Now;

        private double m_realValue;

    }
}