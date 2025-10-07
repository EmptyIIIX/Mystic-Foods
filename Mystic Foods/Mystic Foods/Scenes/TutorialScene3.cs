using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Scenes
{
    public class TutorialScene3 : IGameScene
    {
        private SpriteFont _font;
        public static Texture2D Page_13, Page_23, Page_33, Page_43, Page_53;
        private TutorialScene _tutorialscene;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            //load scene
            Page_13 = content.Load<Texture2D>("Tutorial/1หน้าทำอาหาร");
            Page_23 = content.Load<Texture2D>("Tutorial/2ผสมวัตถุดิบ");
            Page_33 = content.Load<Texture2D>("Tutorial/3การนึ่ง");
            Page_43 = content.Load<Texture2D>("Tutorial/4ตกแต่ง");
            Page_53 = content.Load<Texture2D>("Tutorial/5ถังขยะ");

        }

        public void Update(GameTime gameTime)
        {
            _tutorialscene.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _tutorialscene.Draw(spriteBatch);
        }
    }
}
