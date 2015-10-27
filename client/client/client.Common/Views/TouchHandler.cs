namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using CocosSharp;

    /// <summary>
    /// The Touch handler class.
    /// </summary>
    public sealed class TouchHandler
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static TouchHandler Instance
        {
            get
            {
                return Singleton.Value;
            }
        }

        /// <summary>
        /// Lists a touch began event.
        /// </summary>
        /// <param name="node"> Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        public void ListenBegan(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_beganNodes, node, action);
        }

        /// <summary>
        /// Removes a touch began event from the list.
        /// </summary>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        public void StopListenBegan(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            RemoveFromList(m_beganNodes, node, action);
        }

        /// <summary>
        /// Lists a touch moved event.
        /// </summary>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        public void ListenMoved(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_movedNodes, node, action);
        }

        /// <summary>
        /// Removes a touch moved event from the list.
        /// </summary>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        public void StopListenMoved(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            RemoveFromList(m_movedNodes, node, action);
        }

        /// <summary>
        /// Lists a touch ended event.
        /// </summary>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        public void ListenEnded(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_endedNodes, node, action);
        }

        /// <summary>
        /// Removes a touch ended event from the list.
        /// </summary>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        public void StopListenEnded(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            RemoveFromList(m_endedNodes, node, action);
        }

        /// <summary>
        /// Lists a touch cancelled event.
        /// </summary>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        public void ListenCancelled(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_cancelledNodes, node, action);
        }

        /// <summary>
        /// Removes a touch cancelled event from the list.
        /// </summary>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        public void StopListenCancelled(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_cancelledNodes, node, action);
        }
            
        /// <summary>
        /// Initialize the specified scene.
        /// </summary>
        /// <param name="scene">the Scene.</param>
        public void Init(GameScene scene)
        {
            m_touchListener = new CCEventListenerTouchAllAtOnce();
            m_touchListener.OnTouchesBegan = OnTouchesBegan;
            m_touchListener.OnTouchesCancelled = OnTouchesCancelled;
            m_touchListener.OnTouchesEnded = OnTouchesEnded;
            m_touchListener.OnTouchesMoved = OnTouchesMoved;

            scene.WorldLayer.AddEventListener(m_touchListener);
        }

        /// <summary>
        /// On the touches moved event. Set the gesture to move or scale and scale or move the map.
        /// </summary>
        /// <param name="touches">Touches, where the user touched.</param>
        /// <param name="touchEvent">Touch event.</param>
        public void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (var node in m_movedNodes)
            {
                if (node.Action(touches, touchEvent))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Raises the touches ended event.
        /// </summary>
        /// <param name="touches">List of Touches.</param>
        /// <param name="touchEvent">Touch event.</param>
        public void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (var node in m_endedNodes)
            {
                if (node.Action(touches, touchEvent))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Raises the touches cancelled event.
        /// </summary>
        /// <param name="touches">List of Touches.</param>
        /// <param name="touchEvent">Touch event.</param>
        public void OnTouchesCancelled(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (var node in m_cancelledNodes)
            {
                if (node.Action(touches, touchEvent))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Raises the touches began event.
        /// </summary>
        /// <param name="touches">List of Touches.</param>
        /// <param name="touchEvent">Touch event.</param>
        public void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (var node in m_beganNodes)
            {
                if (node.Action(touches, touchEvent))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            m_beganNodes = new List<NodeAction>();
            m_endedNodes = new List<NodeAction>();
            m_cancelledNodes = new List<NodeAction>();
            m_movedNodes = new List<NodeAction>();
        }

        /// <summary>
        /// Adds an item to a list.
        /// </summary>
        /// <param name="list">Handle to a List.</param>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        private void AddToList(List<NodeAction> list, CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            var nodeAction = new NodeAction();
            var index = 0;

            nodeAction.Node = node;
            nodeAction.Action = action;
            if (list.Count != 0)
            {
                index = list.FindIndex((x) => x.Node.ZOrder >= node.ZOrder);
            }
            else
            {
                index = 0;
            }
            list.Insert(index, nodeAction);
        }

        /// <summary>
        /// Removes an item from a list.
        /// </summary>
        /// <param name="list">Handle to a List.</param>
        /// <param name="node">Handle to a Node.</param>
        /// <param name="action">an Action.</param>
        private void RemoveFromList(List<NodeAction> list, CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            list.RemoveAll((x) => x.Node == node && x.Action == action);
        }
            
        /// <summary>
        /// Prevents a default instance of the <see cref="TouchHandler"/> class from being created.
        /// </summary>
        private TouchHandler()
        {
            Reset();
        }

        /// <summary>
        /// The list for all touch began events.
        /// </summary>
        private List<NodeAction> m_beganNodes;

        /// <summary>
        /// The list for all touch ended events.
        /// </summary>
        private List<NodeAction> m_endedNodes;

        /// <summary>
        /// The list for all touch cancelled events.
        /// </summary>
        private List<NodeAction> m_cancelledNodes;

        /// <summary>
        /// The list for all touch moved events.
        /// </summary>
        private List<NodeAction> m_movedNodes;

        /// <summary>
        /// The touch event listener.
        /// </summary>
        private CCEventListenerTouchAllAtOnce m_touchListener;

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static readonly Lazy<TouchHandler> Singleton =
            new Lazy<TouchHandler>(() => new TouchHandler());        

        /// <summary>
        /// Struct that contains a Node with a related action.
        /// </summary>
        private struct NodeAction
        {
            /// <summary>
            /// a node.
            /// </summary>
            public CCNode Node;

            /// <summary>
            /// The related action to a Node.
            /// </summary>
            public Func<List<CCTouch>, CCEvent, bool> Action;
        }
    }
}