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
        public static int numTopic = 0;
        public static bool RecipeBookRequest = false;

        //scene tutorial
        public static Texture2D Recipe_book;
        public static Texture2D Pcook1, Pcook2, Pcook3;
        public static Texture2D Preceive;
        public static Texture2D Pserve;
        public static Texture2D PthrowAway;

        //button UI
        public static Texture2D nextPage, backPage, exitPage;
        public static Texture2D nextPage_hover, backPage_hover;
        public static Button next, back, exitpage;

        public static Texture2D topic1, topic2, topic3, topic4;
        public static Texture2D topic1_focus, topic2_focus, topic3_focus, topic4_focus;
        public static Button topic1btn, topic2btn, topic3btn, topic4btn, topicDefaultBtn;

        //check action page
        public static bool isNextPage, isBackPage, isExitPage;
        //public static bool isTopic1, isTopic2, isTopic3, isTopic4;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            //load scene
            Recipe_book = content.Load<Texture2D>("Recipe/Recipe");
            Pcook1 = content.Load<Texture2D>("Recipe/Book/cook 1");
            Pcook2 = content.Load<Texture2D>("Recipe/Book/cook 2");
            Pcook3 = content.Load<Texture2D>("Recipe/Book/cook 3");
            Preceive = content.Load<Texture2D>("Recipe/Book/receive 1");
            Pserve = content.Load<Texture2D>("Recipe/Book/Serve 1");
            PthrowAway = content.Load<Texture2D>("Recipe/Book/Throw 1");

            //load button UI
            topic1 = content.Load<Texture2D>("Recipe/Buttons/ReceiveTopic");
            topic1_focus = content.Load<Texture2D>("Recipe/Buttons/ReceiveTopic_focus");
            topic2 = content.Load<Texture2D>("Recipe/Buttons/CookTopic");
            topic2_focus = content.Load<Texture2D>("Recipe/Buttons/CookTopic_focus");
            topic3 = content.Load<Texture2D>("Recipe/Buttons/serveTopic");
            topic3_focus = content.Load<Texture2D>("Recipe/Buttons/serveTopic_focus");
            topic4 = content.Load<Texture2D>("Recipe/Buttons/throwTopic");
            topic4_focus = content.Load<Texture2D>("Recipe/Buttons/throwTopic_focus");

            nextPage = content.Load<Texture2D>("Recipe/Buttons/NextBtn");
            nextPage_hover = content.Load<Texture2D>("Recipe/Buttons/NextBtn_hover");
            backPage = content.Load<Texture2D>("Recipe/Buttons/BackBtn");
            backPage_hover = content.Load<Texture2D>("Recipe/Buttons/BackBtn_hover");
            exitPage = content.Load<Texture2D>("Recipe/exitpage");

            //button manager - fixed: Button constructor takes 4 args (texture, font, text, bounds)
                        topic1btn = new Button(topic1, _font, "", new Rectangle(266, 236, 384, 157));
                        topic1btn.Click += Topic1_click;
                        topic2btn = new Button(topic2, _font, "", new Rectangle(266, 393, 384, 157));
                        topic2btn.Click += Topic2_click;
                        topic3btn = new Button(topic3, _font, "", new Rectangle(266, 550, 384, 157));
                        topic3btn.Click += Topic3_click;
                        topic4btn = new Button(topic4, _font, "", new Rectangle(266, 707, 384, 157));
                        topic4btn.Click += Topic4_click;
                        topicDefaultBtn = new Button(topic1, _font, "", new Rectangle(266, 236, 384, 157));
                        topicDefaultBtn.Click += TopicDefault_click;

                        next = new Button(nextPage, _font, "", new Rectangle(1343, 823, 190, 81));
                        next.Click += NextButton_click;

                        back = new Button(backPage, _font, "", new Rectangle(806, 823, 190, 81));
                        back.Click += BackButton_click;

                        exitpage = new Button(exitPage, _font, "", new Rectangle(1573, 218, 78, 73));
                        exitpage.Click += ExitPageButton_click;

            //assign max page and set count page of the tutorial
            countPage = 1;
            MaxPage = 3;
        }

        public void Update(GameTime gameTime)
        {

            if (countPage == 0) countPage = 1; //this cannot be less than 1 page
            if (countPage > MaxPage) countPage = MaxPage; // this cannot be more max page

            switch (numTopic)
            {
                case 1:
                    topic2btn.Update();
                    topic3btn.Update();
                    topic4btn.Update();
                    MaxPage = 1;
                    break;
                case 2:
                    topic1btn.Update();
                    topic3btn.Update();
                    topic4btn.Update();
                    MaxPage = 3;
                    break;
                case 3:
                    topic1btn.Update();
                    topic2btn.Update();
                    topic4btn.Update();
                    MaxPage = 1;
                    break;
                case 4:
                    topic1btn.Update();
                    topic2btn.Update();
                    topic3btn.Update();
                    MaxPage = 1;
                    break;
                default:
                    //topic1btn.Update();
                    topic2btn.Update();
                    topic3btn.Update();
                    topic4btn.Update();
                    topicDefaultBtn.Update();
                    break;
            }

            if (numTopic == 2)
            {
                if (countPage < MaxPage) next.Update();
                if (countPage > 1) back.Update();
            }

            exitpage.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Begin();
            switch (numTopic)
            {
                case 1:
                    spriteBatch.Draw(Preceive, new Vector2(0, 0), Color.White);
                    break;
                case 2:
                    switch (countPage)
                    {
                        case 1:
                            spriteBatch.Draw(Pcook1, new Vector2(0, 0), Color.White); break;
                        case 2:
                            spriteBatch.Draw(Pcook2, new Vector2(0, 0), Color.White); break;
                        case 3:
                            spriteBatch.Draw(Pcook3, new Vector2(0, 0), Color.White); break;
                    }
                    break;
                case 3:
                    spriteBatch.Draw(Pserve, new Vector2(0, 0), Color.White);
                    break;
                case 4:
                    spriteBatch.Draw(PthrowAway, new Vector2(0, 0), Color.White);
                    break;
                default:
                    spriteBatch.Draw(Preceive, new Vector2(0, 0), Color.White);
                    break;
            }

            //draw topic buttons - fixed: use Draw() instead of DrawHover()
                        topic1btn.Draw(spriteBatch);
                        topic2btn.Draw(spriteBatch);
                        topic3btn.Draw(spriteBatch);
                        topic4btn.Draw(spriteBatch);

                        //draw next and back page
                        if (numTopic == 2)
                        {
                            if (countPage < MaxPage) next.Draw(spriteBatch);
                            if (countPage > 1) back.Draw(spriteBatch);

                        }
                        else if (numTopic == 0)
                        {
                            topicDefaultBtn.Draw(spriteBatch);
                        }

                        exitpage.Draw(spriteBatch);

            spriteBatch.End();
        }
        public void Topic1_click(object sender, EventArgs e)
        {
            numTopic = 1;
            SoundManager.PlaySfx("Click");
        }
        public void Topic2_click(object sender, EventArgs e)
        {
            numTopic = 2;
            SoundManager.PlaySfx("Click");
        }
        public void Topic3_click(object sender, EventArgs e)
        {
            numTopic = 3;
            SoundManager.PlaySfx("Click");
        }
        public void Topic4_click(object sender, EventArgs e)
        {
            numTopic = 4;
            SoundManager.PlaySfx("Click");
        }
        public void TopicDefault_click(object sender, EventArgs e)
        {
            numTopic = 1;
            SoundManager.PlaySfx("Click");
        }
        public void NextButton_click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Page");
            isNextPage = true;
            countPage++;
        }
        public void BackButton_click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Page");
            isBackPage = true;
            countPage--;
        }
        public void ExitPageButton_click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Page");
            isExitPage = true;
            GamePlayScene.isRecipeInGame = false;
        }
    }
}
