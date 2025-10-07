using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;
using static System.Net.Mime.MediaTypeNames;

namespace Mystic_Foods.Scenes
{
    public class TutorialScene2 : IGameScene
    {
        private SpriteFont _font;
        public static Texture2D Page_12, Page_22, Page_32, Page_42, Page_52;
        private TutorialScene _tutorialscene;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _tutorialscene = new TutorialScene();
            //load scene
            Page_12 = content.Load<Texture2D>("Tutorial/1หน้ารับออเดอร์");
            Page_22 = content.Load<Texture2D>("Tutorial/2หนังสือเมนู");
            Page_32 = content.Load<Texture2D>("Tutorial/3วันและเวลา");
            Page_42 = content.Load<Texture2D>("Tutorial/4การสั่งซื้อของลูกค้า");
            Page_52 = content.Load<Texture2D>("Tutorial/5ความประทับใจ");

            //assign max page and set count page of the tutorial
            if (TutorialScene.CountTutorial == 1)
            {
                TutorialScene.countPage = 1;
                TutorialScene.MaxPage = 5;
            }
        }

        public void Update(GameTime gameTime) 
        {
            if (TutorialScene.countPage == 0) TutorialScene.countPage = 1; //this cannot be less than 1 page
            if (TutorialScene.countPage > TutorialScene.MaxPage) TutorialScene.countPage = TutorialScene.MaxPage; // this cannot be more max page

            if (Game1.wasTutorial == false)
            {
                if (TutorialScene.countPage != TutorialScene.MaxPage) TutorialScene.next.Update();
                if (TutorialScene.countPage > 1) TutorialScene.back.Update();

                if (TutorialScene.countPage == TutorialScene.MaxPage) TutorialScene.exitpage.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            switch (TutorialScene.countPage)
            {
                case 1:
                    spriteBatch.Draw(Page_12, new Vector2(0, 0), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(Page_22, new Vector2(0, 0), Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(Page_32, new Vector2(0, 0), Color.White);
                    break;
                case 4:
                    spriteBatch.Draw(Page_42, new Vector2(0, 0), Color.White);
                    break;
                case 5:
                    spriteBatch.Draw(Page_52, new Vector2(0, 0), Color.White);
                    break;
            }

            //draw next and back page
            if (TutorialScene.countPage < TutorialScene.MaxPage) TutorialScene.next.Draw(spriteBatch);
            if (TutorialScene.countPage > 1) TutorialScene.back.Draw(spriteBatch);
            if (TutorialScene.countPage == TutorialScene.MaxPage) TutorialScene.exitpage.Draw(spriteBatch);

            spriteBatch.End();
        }

    }
}
