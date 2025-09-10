using Microsoft.Xna.Framework;
using Mystic_Foods.Managers;

namespace Mystic_Foods.Systems
{
    public interface IDraggable
    {
        Rectangle Rectangle { get; }
        Vector2 Position { get; set; }

        void RegisterDraggable()
        {
            DragDropManager.AddDraggable(this);
        }

        void UnregisterDraggable()
        {
            DragDropManager.RemoveDraggable(this);
        }
    }
}
