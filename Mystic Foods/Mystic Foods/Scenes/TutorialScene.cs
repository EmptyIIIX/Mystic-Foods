using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Systems;


namespace Mystic_Foods.Scenes
{
    public class TutorialScene : IGameScene
    {
        private SpriteFont _font;
        public static int countPage, MaxPage;

        //scene tutorial
        public static Texture2D Page_1, Page_2, Page_3, Page_4;

        //button UI
        public static Texture2D nextPage, backPage, exitPage, topicPoint;
        public static Button next, back, exitpage;

        //check action page
        public static bool isNextPage, isBackPage, isExitPage;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            //load scene
            Page_1 = content.Load<Texture2D>("Tutorial/levelscene");
            Page_2 = content.Load<Texture2D>("Tutorial/หนังสือที่จะขึ้นก่อนเริ่มเกม");
            Page_3 = content.Load<Texture2D>("Tutorial/หนังสือแบบเลือกดูประวัติได้");
            Page_4 = content.Load<Texture2D>("Tutorial/levelscene");

            //load button UI
            nextPage = content.Load<Texture2D>("Tutorial/nextpage");
            backPage = content.Load<Texture2D>("Tutorial/backpage");
            exitPage = content.Load<Texture2D>("Tutorial/exitpage");
            topicPoint = content.Load<Texture2D>("Tutorial/topicPoint");

            //button manager
            next = new Button(nextPage, _font, "", new Rectangle(1674, 838, 136, 134));
            next.Click += NextButton_click;

            back = new Button(backPage, _font, "", new Rectangle(110, 838, 136, 134));
            back.Click += BackButton_click;

            exitpage = new Button(exitPage, _font, "", new Rectangle(1720, 106, 90, 79));
            exitpage.Click += ExitPageButton_click;

            //assign max page and set count page of the tutorial
            countPage = 1;
            MaxPage = 4;
        }

        public void Update(GameTime gameTime)
        {
            if (countPage == 0) countPage = 1; //this cannot be less than 1 page
            if (countPage > MaxPage) countPage = MaxPage; // this cannot be more max page


            if (Game1.wasTutorial == false)
            {
                next.Update();
                back.Update();

                if (countPage == MaxPage) exitpage.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            switch (countPage)
            {
                case 1:
                    spriteBatch.Draw(Page_1, new Vector2(0, 0), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(Page_2, new Vector2(0, 0), Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(Page_3, new Vector2(0, 0), Color.White);
                    break;
                case 4:
                    spriteBatch.Draw(Page_4, new Vector2(0, 0), Color.White);
                    break;

            }

            //draw next and back page
            if (countPage < MaxPage) next.Draw(spriteBatch);
            if (countPage > 1) back.Draw(spriteBatch);
            if (countPage == MaxPage) exitpage.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void NextButton_click(object sender, EventArgs e)
        {
            isNextPage = true;
            countPage++;
        }
        public void BackButton_click(object sender, EventArgs e)
        {
            isBackPage = true;
            countPage--;
        }
        public void ExitPageButton_click(object sender, EventArgs e)
        {
            isExitPage = true;
            Game1.wasTutorial = true;
        }
    }
}
