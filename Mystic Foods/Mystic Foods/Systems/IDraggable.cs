using Microsoft.Xna.Framework;
using Mystic_Foods.Managers;

namespace Mystic_Foods.Systems
{
    public interface IDraggable
    {
        Rectangle Rectangle { get; }
        Vector2 Position { get; set; }
        Rectangle GetRectangle(Vector2 cameraPos);
        Vector2 Size { get; }
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
