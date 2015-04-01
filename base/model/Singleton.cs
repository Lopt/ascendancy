using System;

namespace @base.model
{
	public class Singleton
	{
		private static Singleton m_instance;

		private Singleton() {}

		public static Singleton Instance
		{
			get 
			{
                if (m_instance == null)
				{
					m_instance = new Singleton();
				}
                return m_instance;
			}
		}
	}
}
