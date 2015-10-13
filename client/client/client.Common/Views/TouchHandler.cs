namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using CocosSharp;

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
            
        public void ListenBegan(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_beganNodes, node, action);
        }

        public void StopListenBegan(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            RemoveFromList(m_beganNodes, node, action);
        }

        public void ListenMoved(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_movedNodes, node, action);
        }

        public void StopListenMoved(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            RemoveFromList(m_movedNodes, node, action);
        }

        public void ListenEnded(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_endedNodes, node, action);
        }

        public void StopListenEnded(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            RemoveFromList(m_endedNodes, node, action);
        }

        public void ListenCancelled(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_cancelledNodes, node, action);
        }

        public void StopListenCancelled(CCNode node, Func<List<CCTouch>, CCEvent, bool> action)
        {
            AddToList(m_cancelledNodes, node, action);
        }

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

        public void Reset()
        {
            m_beganNodes = new List<NodeAction>();
            m_endedNodes = new List<NodeAction>();
            m_cancelledNodes = new List<NodeAction>();
            m_movedNodes = new List<NodeAction>();
        }

        private List<NodeAction> m_beganNodes;
        private List<NodeAction> m_endedNodes;
        private List<NodeAction> m_cancelledNodes;
        private List<NodeAction> m_movedNodes;

        private CCEventListenerTouchAllAtOnce m_touchListener;

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static readonly Lazy<TouchHandler> Singleton =
            new Lazy<TouchHandler>(() => new TouchHandler());        

        private struct NodeAction
        {
            public CCNode Node;
            public Func<List<CCTouch>, CCEvent, bool> Action;
        }
    }
}