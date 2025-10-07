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
    public class TutorialScene2 : IGameScene
    {
        private SpriteFont _font;
        public static Texture2D Page_12, Page_22, Page_32, Page_42, Page_52;
        private TutorialScene _tutorialscene;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            //load scene
            Page_12 = content.Load<Texture2D>("Tutorial/1หน้ารับออเดอร์");
            Page_22 = content.Load<Texture2D>("Tutorial/2หนังสือเมนู");
            Page_32 = content.Load<Texture2D>("Tutorial/3วันและเวลา");
            Page_42 = content.Load<Texture2D>("Tutorial/4การสั่งซื้อของลูกค้า");
            Page_52 = content.Load<Texture2D>("Tutorial/5ความประทับใจ");

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
