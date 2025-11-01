using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Scenes
{
    public class CreditScene : IGameScene
    {
        private SpriteFont _font;
        private KeyboardState keyboardState;

        public static bool ExitToMenu = false;

        Hashtable person = new()
        {
            {"wef", "Theerapat Boongrom 672110100" },
            {"gun", "Kanyanat Meekham 672110080" },
            {"pare", "Sirikanya Kawilawan 672110126" },
            {"prai", "Kanyanat Khumtongsuk 672110079" },
            {"min", "Premintr Singkaew 672110108" }
        };
        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("normalFont");
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                ExitToMenu = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GamePlayScene._rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.2f);
            spriteBatch.DrawString(_font, $"Hello {(string)person["wef"]}", new Vector2(500, 540), Color.White);
            spriteBatch.End();
        }
    }
}
