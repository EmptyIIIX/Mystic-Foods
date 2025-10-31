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
    public class Recipe : IGameScene
    {
        private SpriteFont _font;
        public static int countPage, MaxPage;
        public static bool RecipeBookRequest = false;
        public static Texture2D _rectTexture1;

        //scene tutorial
        public static Texture2D Recipe_book;

        //button UI
        public static Texture2D nextPage, backPage, exitPage, topicPoint;
        public static Button next, back, exitpage;

        //check action page
        public static bool isNextPage, isBackPage, isExitPage;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _rectTexture1 = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _rectTexture1.SetData(new[] { Color.White });
            //load scene
            Recipe_book = content.Load<Texture2D>("Recipe/Recipe");

            //load button UI
            nextPage = content.Load<Texture2D>("Recipe/nextpage");
            backPage = content.Load<Texture2D>("Recipe/backpage");
            exitPage = content.Load<Texture2D>("Recipe/exitpage");

            //button manager
            next = new Button(nextPage, nextPage, _font, "", new Rectangle(1674, 838, 136, 134));
            next.Click += NextButton_click;

            back = new Button(backPage, backPage, _font, "", new Rectangle(110, 838, 136, 134));
            back.Click += BackButton_click;

            exitpage = new Button(exitPage, exitPage, _font, "", new Rectangle(1720, 106, 90, 79));
            exitpage.Click += ExitPageButton_click;

            //assign max page and set count page of the tutorial
            countPage = 1;
            MaxPage = 9;
        }

        public void Update(GameTime gameTime)
        {
            if (countPage == 0) countPage = 1; //this cannot be less than 1 page
            if (countPage > MaxPage) countPage = MaxPage; // this cannot be more max page

            if (countPage < MaxPage) next.Update();
            if (countPage > 1) back.Update();

            exitpage.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            switch (countPage)
            {
                case 1:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 1), 850 * (countPage - 1), 1410, 850), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 1), 850 * (countPage - 2), 1410, 850), Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 1), 850 * (countPage - 3), 1410, 850), Color.White);
                    break;
                case 4:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 4), 850 * (countPage - 3), 1410, 850), Color.White);
                    break;
                case 5:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 4), 850 * (countPage - 4), 1410, 850), Color.White);
                    break;
                case 6:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 4), 850 * (countPage - 5), 1410, 850), Color.White);
                    break;
                case 7:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 7), 850 * (countPage - 5), 1410, 850), Color.White);
                    break;
                case 8:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 7), 850 * (countPage - 6), 1410, 850), Color.White);
                    break;
                case 9:
                    spriteBatch.Draw(Recipe_book, new Vector2(255, 115), new Rectangle(1410 * (countPage - 7), 850 * (countPage - 7), 1410, 850), Color.White);
                    break;
            }

            ////draw next and back page
            if (countPage < MaxPage) next.Draw(spriteBatch);
            if (countPage > 1) back.Draw(spriteBatch);

            exitpage.Draw(spriteBatch);

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
            GamePlayScene.isRecipeInGame = false;
        }
    }
}
