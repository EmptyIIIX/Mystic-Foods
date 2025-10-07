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
            _tutorialscene = new TutorialScene();
            //load scene
            Page_13 = content.Load<Texture2D>("Tutorial/1หน้าทำอาหาร");
            Page_23 = content.Load<Texture2D>("Tutorial/2ผสมวัตถุดิบ");
            Page_33 = content.Load<Texture2D>("Tutorial/3การนึ่ง");
            Page_43 = content.Load<Texture2D>("Tutorial/4ตกแต่ง");
            Page_53 = content.Load<Texture2D>("Tutorial/5ถังขยะ");

            //assign max page and set count page of the tutorial
            if (TutorialScene.CountTutorial == 2)
            {
                TutorialScene.countPage = 1;
                TutorialScene.MaxPage = 5;
            }
        }

        public void Update(GameTime gameTime)
        {
            _tutorialscene.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            switch (TutorialScene.countPage)
            {
                case 1:
                    spriteBatch.Draw(Page_13, new Vector2(0, 0), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(Page_23, new Vector2(0, 0), Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(Page_33, new Vector2(0, 0), Color.White);
                    break;
                case 4:
                    spriteBatch.Draw(Page_43, new Vector2(0, 0), Color.White);
                    break;
                case 5:
                    spriteBatch.Draw(Page_53, new Vector2(0, 0), Color.White);
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
