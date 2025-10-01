using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Managers
{
    public static class DragDropManager
    {
        private static GameManager _gameManager;
        private static Sprite _sprite;

        private static readonly List<IDraggable> _draggables = new();
        private static readonly List<Rectangle> _hitboxes = new();
        private static readonly List<ITargetable> _targets = new();
        private static IDraggable _dragItem;

        public static event Action<IDraggable, ITargetable> OnDrop;
        public static event Action<IDraggable> OnDragFailed;

        private static Vector2 _cameraPos = Vector2.Zero;

        // เรียกจาก Scene.Update() เพื่อ sync cameraPos
        public static void SetCamera(Vector2 cameraPos)
        {
            _cameraPos = cameraPos;
        }
        public static void AddDraggable(IDraggable item)
        {
            if (!_draggables.Contains(item))
                _draggables.Add(item);
        }
        public static void AddHitbox(Rectangle rect)
        {
            _hitboxes.Add(rect);
        }
        public static void RemoveDraggable(IDraggable item)
        {
            _draggables.RemoveAll(d => d == item);
            if (_dragItem == item)
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
            //if (InputManager.MouseClicked)
            //{
            //    foreach (var item in _draggables)
            //    {
            //        if (item.GetRectangle(_cameraPos).Contains(InputManager.MousePosition))
            //        {
            //            _dragItem = item;
            //            Mouse.SetCursor(MouseCursor.Hand);
            //            break;
            //        }
            //    }
            //}
            if (!InputManager.MouseClicked) return;

            if (InputManager.MouseClicked)
            {
                Mouse.SetCursor(MouseCursor.Hand);
                foreach (var hitbox in _hitboxes)
                {
                    Rectangle adjustedHitbox = new Rectangle(
                        hitbox.X - (int)_cameraPos.X,
                        hitbox.Y - (int)_cameraPos.Y,
                        hitbox.Width,
                        hitbox.Height
                    );

                    if (adjustedHitbox.Contains(InputManager.MousePosition))
                    {
                        Vector2 hitCenter = new Vector2(adjustedHitbox.Center.X, adjustedHitbox.Center.Y);

                        _dragItem = _draggables.OrderBy(d => Vector2.Distance(d.Position, hitCenter)).FirstOrDefault();

                        if (_dragItem != null)
                        {
                            break;
                        }
                    }
                    else
                    {
                        _dragItem = _draggables.FirstOrDefault(
                            d => new Rectangle(
                                (int)(d.Position.X - d.Size.X / 2),
                                (int)(d.Position.Y - d.Size.Y / 2),
                                (int)d.Size.X,
                                (int)d.Size.Y
                            ).Contains(InputManager.MousePosition + _cameraPos)
                        );
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
                if (item.GetRectangle(_cameraPos).Contains(InputManager.MousePosition))
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
            if (InputManager.MouseReleased && _dragItem != null)
            {
                CheckTarget();
                _dragItem = null;
                Mouse.SetCursor(MouseCursor.Arrow);
            }
        }
        public static void Update()
        {
            CheckDragStart();

            if (_dragItem is not null)
            {
                _dragItem.Position = InputManager.MousePosition + _cameraPos;

                if (_gameManager != null)
                {
                    Vector2 originPos = _dragItem switch
                    {
                        Filling => _gameManager._originSai,
                        Dough => _gameManager._originPang,
                        _ => _dragItem.Position
                    };

                    _sprite.Visible = Vector2.Distance(_dragItem.Position, originPos) > 1f;
                }
                CheckDragStop();
            }
        }
    }
}
