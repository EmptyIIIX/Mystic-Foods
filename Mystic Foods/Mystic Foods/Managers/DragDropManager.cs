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
        //hitbox for ingredients
        private static readonly Dictionary<Rectangle, Filling.FillingType> _fillingHitboxMap = new();
        private static readonly Dictionary<Rectangle, Dough.DoughType> _doughHitboxMap = new();
        private static readonly Dictionary<Rectangle, Flowers.FlowersType> _flowerHitboxMap = new();
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
        public static void RemoveDraggable(IDraggable item)
        {
            _draggables.Remove(item);
            if (_dragItem == item)
            {
                _dragItem = null;
                Mouse.SetCursor(MouseCursor.Arrow);
            }
        }
        public static void AddHitboxFilling(Rectangle rect, Filling.FillingType type)
        {
            _fillingHitboxMap[rect] = type;
        }
        public static void AddHitboxDough(Rectangle rect, Dough.DoughType type)
        {
            _doughHitboxMap[rect] = type;
        }
        public static void AddHitboxFlower(Rectangle rect, Flowers.FlowersType type)
        {
            _flowerHitboxMap[rect] = type;
        }
        public static void AddTarget(ITargetable item)
        {
            _targets.Add(item);
        }

        private static void CheckDragStart()
        {
            if (!InputManager.MouseClicked || _dragItem != null)
                return;

            //for hitbox filling
            foreach (var pair in _fillingHitboxMap)
            {
                Rectangle adjustedHitbox = new Rectangle(
                    pair.Key.X - (int)_cameraPos.X,
                    pair.Key.Y - (int)_cameraPos.Y,
                    pair.Key.Width,
                    pair.Key.Height
                );

                if (adjustedHitbox.Contains(InputManager.MousePosition))
                {
                    var matchedDraggable = _draggables.OfType<Filling>().FirstOrDefault(f => f.FillingKind == pair.Value);

                    if (matchedDraggable != null)
                    {
                        _dragItem = matchedDraggable;
                        Mouse.SetCursor(MouseCursor.Hand);
                        SoundManager.PlaySfx("Keep");
                        return;
                    }
                }

            }
            //for hitbox dough
            foreach (var pair in _doughHitboxMap)
            {
                Rectangle adjustedHitbox = new Rectangle(
                    pair.Key.X - (int)_cameraPos.X,
                    pair.Key.Y - (int)_cameraPos.Y,
                    pair.Key.Width,
                    pair.Key.Height
                );

                if (adjustedHitbox.Contains(InputManager.MousePosition))
                {
                    var matchedDraggable = _draggables.OfType<Dough>().FirstOrDefault(d => d.DoughKind == pair.Value);

                    if (matchedDraggable != null)
                    {
                        _dragItem = matchedDraggable;
                        Mouse.SetCursor(MouseCursor.Hand);
                        SoundManager.PlaySfx("Keep");
                        return;
                    }
                }
            }
            //for decoration
            foreach (var pair in _flowerHitboxMap)
            {
                Rectangle adjustedHitbox = new Rectangle(
                    pair.Key.X - (int)_cameraPos.X,
                    pair.Key.Y - (int)_cameraPos.Y,
                    pair.Key.Width,
                    pair.Key.Height
                );

                if (adjustedHitbox.Contains(InputManager.MousePosition))
                {
                    var matchedDraggable = _draggables.OfType<Flowers>().FirstOrDefault(fw => fw.FlowerKind == pair.Value);

                    if (matchedDraggable != null)
                    {
                        _dragItem = matchedDraggable;
                        Mouse.SetCursor(MouseCursor.Hand);
                        SoundManager.PlaySfx("Keep");
                        return;
                    }
                }
            }
            //for wrapper that doesn't have a hitbox
            if (_dragItem == null )
            {
                _dragItem = _draggables.OfType<Wrapper>().FirstOrDefault(w => new Rectangle(
                        (int)(w.Position.X - w.Size.X / 2),
                        (int)(w.Position.Y - w.Size.Y / 2),
                        (int)w.Size.X,
                        (int)w.Size.Y
                ).Contains(InputManager.MousePosition + _cameraPos));

                if (_dragItem != null)
                {
                    Mouse.SetCursor(MouseCursor.Hand);
                    SoundManager.PlaySfx("Woosh");
                    return;
                }
            }
            //for object that no hitbox and on the plate
            if (_dragItem == null)
            {
                _dragItem = _draggables.FirstOrDefault(d => new Rectangle(
                    (int)(d.Position.X - d.Size.X / 2),
                    (int)(d.Position.Y - d.Size.Y / 2),
                    (int)d.Size.X,
                    (int)d.Size.Y
                ).Contains(InputManager.MousePosition + _cameraPos));

                if (_dragItem != null)
                {
                    Mouse.SetCursor(MouseCursor.Hand);
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
