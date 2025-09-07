using Microsoft.Xna.Framework;
using Mystic_Foods.Managers;

namespace Mystic_Foods.Systems
{
    public interface ITargetable
    {
        Rectangle Rectangle { get; }
        Vector2 Position { get; set; }

        void RegisterTargetable()
        {
            DragDropManager.AddTarget(this);
        }
    }
}
