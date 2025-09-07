
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Managers
{
    public class GameManager
    {
        private readonly List<Food> _food = new();
        private Socket _plate;

        public GameManager()
        {
        }

        public void LoadContent(ContentManager content)
        {
            var foodTexture = content.Load<Texture2D>("Environments/Food");
            var plateTexture = content.Load<Texture2D>("Environments/Plate");

            for (int i = 0; i < 3; i++)
            {
                _food.Add(new Food(foodTexture, new(460 + i * 460, 200)));
            }
            _plate = new Socket(plateTexture, new(940, 600));
        }

        public void Update()
        {
            InputManager.Update();
            DragDropManager.Update();
        }

        public void Draw()
        {
            _plate.Draw();
            
            foreach (var item in  _food)
            {
                item.Draw();
            }
        }
    }
}
