using System;
using CocosSharp;
using @base.model;

namespace client.Common.Helper
{
	/// <summary>
	/// Modify
	/// </summary>
	public class Modify
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="client.Common.Helper.Modify"/> class.
		/// </summary>
		public Modify()
		{
		}

		/// <summary>
		/// Gets the scale factor.
		/// </summary>
		/// <returns>The scale factor.</returns>
		/// <param name="content">Content.</param>
		/// <param name="space">Space.</param>
		public static float GetScaleFactor(CCSize content, CCSize space)
		{
			var contentSquare = content.Height * content.Width;
			var spaceSquare = space.Height * space.Width;
			var scaleFactor = spaceSquare / contentSquare;

			return scaleFactor;
		}
 
	}
}

