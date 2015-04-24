using System;
using CocosSharp;
using System.Collections.Generic;

namespace client.Common
{
	public class TouchTestScene : CCScene
	{
		CCLayer mTouchTestLayer;

		public TouchTestScene (CCWindow _MainWindow)
			: base (_MainWindow)
		{
			mTouchTestLayer = new MultiTouchTestLayer ();

			this.AddChild (mTouchTestLayer);
		}
	}

	public class MultiTouchTestLayer : CCLayer
	{

		CCLabel title;

		public MultiTouchTestLayer ()
		{
			var listener = new CCEventListenerTouchAllAtOnce ();
			listener.OnTouchesBegan = onTouchesBegan;
			listener.OnTouchesMoved = onTouchesMoved;
			listener.OnTouchesEnded = onTouchesEnded;

			AddEventListener (listener);   

			title = new CCLabel ("Please touch the screen!", "arial", 22);

			AddChild (title);
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			title.PositionX = this.VisibleBoundsWorldspace.MinX + 20;
			title.PositionY = this.VisibleBoundsWorldspace.MaxY - 40;
			title.AnchorPoint = CCPoint.AnchorUpperLeft;
		}

		private CCColor3B[] s_TouchColors = new CCColor3B[] {
			CCColor3B.Yellow,
			CCColor3B.Blue,
			CCColor3B.Green,
			CCColor3B.Red,
			CCColor3B.Magenta
		};

		private static Dictionary<int, TouchPoint> s_dic = new Dictionary<int, TouchPoint> ();

		void onTouchesBegan (List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (var item in touches) {
				CCTouch touch = (item);
				TouchPoint touchPoint = TouchPoint.TouchPointWithParent (this);
				CCPoint location = touch.Location;
				touchPoint.SetTouchPos (location);
				touchPoint.SetTouchColor (s_TouchColors [touch.Id % 5]);

				AddChild (touchPoint);
				s_dic.Add (touch.Id, touchPoint);
			}
		}

		void onTouchesMoved (List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (var item in touches) {
				CCTouch touch = item;
				TouchPoint pTP = s_dic [touch.Id];
				CCPoint location = touch.LocationOnScreen;
				location = Layer.ScreenToWorldspace (location);
				pTP.SetTouchPos (location);
			}
		}

		void onTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (var item in touches) {
				CCTouch touch = item;
				TouchPoint pTP = s_dic [touch.Id];
				RemoveChild (pTP, true);
				s_dic.Remove (touch.Id);
			}
		}

		void onTouchesCancelled (List<CCTouch> touches, CCEvent touchEvent)
		{
			onTouchesEnded (touches, touchEvent);
		}

	}

	public class TouchPoint : CCNode
	{

		protected override void Draw ()
		{
			CCDrawingPrimitives.Begin ();
			CCDrawingPrimitives.DrawColor = new CCColor4B (_touchColor.R, _touchColor.G, _touchColor.B, 255);
			CCDrawingPrimitives.LineWidth = 10;
			CCDrawingPrimitives.DrawLine (new CCPoint (0, _touchPoint.Y), new CCPoint (ContentSize.Width, _touchPoint.Y));
			CCDrawingPrimitives.DrawLine (new CCPoint (_touchPoint.X, 0), new CCPoint (_touchPoint.X, ContentSize.Height));
			CCDrawingPrimitives.LineWidth = 1;
			CCDrawingPrimitives.PointSize = 30;
			CCDrawingPrimitives.DrawPoint (_touchPoint);
			CCDrawingPrimitives.End ();
		}

		public void SetTouchPos (CCPoint pt)
		{
			_touchPoint = pt;
		}

		public void SetTouchColor (CCColor3B color)
		{
			_touchColor = color;
		}

		public static TouchPoint TouchPointWithParent (CCNode pParent)
		{
			TouchPoint pRet = new TouchPoint ();
			pRet.ContentSize = pParent.ContentSize;
			pRet.AnchorPoint = CCPoint.AnchorLowerLeft;
			return pRet;
		}

		private CCPoint _touchPoint;
		private CCColor3B _touchColor;
	}

}

