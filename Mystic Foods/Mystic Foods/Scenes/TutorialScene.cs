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
    public class TutorialScene : IGameScene
    {
        private SpriteFont _font;
        public static int countPage, MaxPage;

        //scene tutorial
        public static Texture2D Page_1;

        //button UI
        public static Texture2D nextPage, backPage, exitPage, topicPoint;
        public static Button next, back, exitpage;

        //check action page
        public static bool isNextPage, isBackPage, isExitPage;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            //load scene
            Page_1 = content.Load<Texture2D>("Tutorial/T1_page_1");

            //load button UI
            nextPage = content.Load<Texture2D>("Tutorial/nextpage");
            backPage = content.Load<Texture2D>("Tutorial/backpage");
            exitPage = content.Load<Texture2D>("Tutorial/exitpage");
            topicPoint = content.Load<Texture2D>("Tutorial/topicPoint");

            //button manager
            next = new Button(nextPage, nextPage, _font, "", new Rectangle(1674, 838, 136, 134));
            next.Click += NextButton_click;

            back = new Button(backPage, backPage, _font, "", new Rectangle(110, 838, 136, 134));
            back.Click += BackButton_click;

            exitpage = new Button(exitPage, exitPage, _font, "", new Rectangle(1720, 106, 90, 79));
            exitpage.Click += ExitPageButton_click;

            //assign max page and set count page of the tutorial
            countPage = 1;
            MaxPage = 1;
        }

        public void Update(GameTime gameTime)
        {
            if (countPage == 0) countPage = 1; //this cannot be less than 1 page
            if (countPage > MaxPage) countPage = MaxPage; // this cannot be more max page

            if (Game1.wasTutorial == false)
            {
                if (countPage != MaxPage) next.Update();
                if (countPage > 1) back.Update();

                if (countPage == MaxPage) exitpage.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Page_1, new Vector2(0, 0), Color.White);

            //draw next and back page
            if (countPage < MaxPage) next.Draw(spriteBatch);
            if (countPage > 1) back.Draw(spriteBatch);
            if (countPage == MaxPage) exitpage.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void NextButton_click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            isNextPage = true;
            countPage++;
        }
        public void BackButton_click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            isBackPage = true;
            countPage--;
        }
        public void ExitPageButton_click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            isExitPage = true;
            //Game1.wasTutorial = true;
        }
    }
}
