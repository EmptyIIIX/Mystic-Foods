using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Managers
{
    public static class DragDropManager
    {
        private static readonly List<IDraggable> _draggables = new();
        private static readonly List<ITargetable> _targets = new();
        private static IDraggable _dragItem;
        public static event Action<IDraggable, ITargetable> OnDrop;
        public static event Action<IDraggable> OnDragFailed;

        public static void AddDraggable(IDraggable item)
        {
            _draggables.Add(item);
        }

        public static void RemoveDraggable(IDraggable item)
        {
            _draggables.Remove(item);
            if (_draggables == item)
            {
                _dragItem = null;
                Mouse.SetCursor(MouseCursor.Arrow);
            }
        }

        public static void AddTarget(ITargetable item)
        {
            _targets.Add(item);
        }

        private static void CheckDragStart()
        {
            if (InputManager.MouseClicked)
            {
                foreach (var item in  _draggables)
                {
                    if (item.Rectangle.Contains(InputManager.MousePosition))
                    {
                        _dragItem = item;
                        Mouse.SetCursor(MouseCursor.Hand);
                        break;
                    }
                }
            }
        }

        private static void CheckTarget()
        {
            if (_dragItem == null) return;

            bool droppedOnTarget = false;
            foreach (var item in _targets)
            {
                if (item.Rectangle.Contains(InputManager.MousePosition))
                {
                    _dragItem.Position = item.Position;
                    OnDrop?.Invoke(_dragItem, item);
                    droppedOnTarget = true;
                    break;
                }
            }
            if (!droppedOnTarget)
            {
                OnDragFailed?.Invoke(_dragItem);
            }
        }

        private static void CheckDragStop()
        {
            if (InputManager.MouseReleased)
            {
                CheckTarget();
                _dragItem = null;
                Mouse.SetCursor(MouseCursor.Arrow);
            }
        }

        public static void Update()
        {
            CheckDragStart();

            if (_dragItem is not  null)
            {
                _dragItem.Position = InputManager.MousePosition;
                CheckDragStop();
            }
        }
    }
}
