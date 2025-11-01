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
        private SpriteFont _font1, _font2, _font3;
        private KeyboardState keyboardState;

        public static bool ExitToMenu = false;
        public static bool isCreditActive = true;

        public static Vector2 screenCenterDefault = new Vector2(1920 / 2, 1200);
        public Vector2 screenCenter = screenCenterDefault;
        private Vector2 Yspacing_1 = new Vector2(0, 100);
        private Vector2 Yspacing_2 = new Vector2(0, 30);
        private Vector2 Yspacing_3 = new Vector2(0, 30);
        private Vector2 TextSpeed = new Vector2(0, 2);

        Hashtable nameCredit = new()
        {
            {"ST 1", "Keattikorn Samarnggoon" },
            {"ST 2", "Patison Palee" },
            {"ST 3", "Supara Grudpan" },
            {"wef", "Theerapat Boongrom 672110100" },
            {"gun", "Kanyanat Meekham 672110080" },
            {"pare", "Sirikanya Kawilawan 672110126" },
            {"prai", "Kanyanat Khumtongsuk 672110079" },
            {"min", "Premintr Singkaew 672110108" }
        };
        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font1 = content.Load<SpriteFont>("MainFont");
            _font2 = content.Load<SpriteFont>("DiaFont");
            _font3 = content.Load<SpriteFont>("BoldFont");
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                ExitToMenu = true;
                screenCenter = screenCenterDefault;
            }

            if (screenCenter.Y < -1720)
            {
                isCreditActive = false;
            } 
            else
            {
                isCreditActive = true;
            }

            if (isCreditActive)
            {
                screenCenter -= TextSpeed;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            Vector2 textsize = _font1.MeasureString((string)nameCredit["wef"]);
            //spriteBatch.Draw(GamePlayScene._rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.2f);

            spriteBatch.DrawString(_font1, "Press Esc to Exit", new Vector2(1700, 1020), Color.White);
            //gametitle
            spriteBatch.DrawString(_font2, "Mystic Food Ayothaya Sweet", screenCenter - (textsize / 2) - Yspacing_2, Color.White);
            spriteBatch.DrawString(_font2, "Group : Wolfpack", screenCenter - (textsize / 4), Color.White);

            //Game Designer
            spriteBatch.DrawString(_font1, "Game Designer", screenCenter - (textsize / 4) + (Yspacing_1 * 1), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["gun"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 1) + (Yspacing_2 * 1) + (Yspacing_3 * 1), Color.White);

            //Programmer
            spriteBatch.DrawString(_font1, "Programmer", screenCenter - (textsize / 4) + (Yspacing_1 * 2) + (Yspacing_2 * 1) + (Yspacing_3 * 1), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["min"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 2) + (Yspacing_2 * 2) + (Yspacing_3 * 2), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["wef"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 2) + (Yspacing_2 * 3) + (Yspacing_3 * 2), Color.White);

            //Artist & Animator
            spriteBatch.DrawString(_font1, "Artist & Animator", screenCenter - (textsize / 4) + (Yspacing_1 * 3) + (Yspacing_2 * 3) + (Yspacing_3 * 2), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["pare"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 3) + (Yspacing_2 * 3) + (Yspacing_3 * 4), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["gun"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 3) + (Yspacing_2 * 4) + (Yspacing_3 * 4), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["prai"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 3) + (Yspacing_2 * 5) + (Yspacing_3 * 4), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["min"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 3) + (Yspacing_2 * 6) + (Yspacing_3 * 4), Color.White);

            //Sound Designer
            spriteBatch.DrawString(_font1, "Sound Designer", screenCenter - (textsize / 4) + (Yspacing_1 * 5) + (Yspacing_2 * 4) + (Yspacing_3 * 3), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["min"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 5) + (Yspacing_2 * 4) + (Yspacing_3 * 5), Color.White);
            
            //Writer and dialogue
            spriteBatch.DrawString(_font1, "Writer & Dialogue", screenCenter - (textsize / 4) + (Yspacing_1 * 6) + (Yspacing_2 * 5) + (Yspacing_3 * 4), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["min"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 6) + (Yspacing_2 * 5) + (Yspacing_3 * 6), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["gun"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 6) + (Yspacing_2 * 6) + (Yspacing_3 * 6), Color.White);

            //Assets & Tool Used
            spriteBatch.DrawString(_font1, "Assets & Tool Used", screenCenter - (textsize / 4) + (Yspacing_1 * 7) + (Yspacing_2 * 7) + (Yspacing_3 * 5), Color.White);
            spriteBatch.DrawString(_font1, "Font : MN Plachon Lui Suan", screenCenter - (textsize / 2) + (Yspacing_1 * 7) + (Yspacing_2 * 7) + (Yspacing_3 * 7), Color.White);
            spriteBatch.DrawString(_font1, "Texture & Sprite : Wolfpack", screenCenter - (textsize / 2) + (Yspacing_1 * 7) + (Yspacing_2 * 8) + (Yspacing_3 * 7), Color.White);
            spriteBatch.DrawString(_font1, "Sound Effect : epidemicsound.com", screenCenter - (textsize / 2) + (Yspacing_1 * 7) + (Yspacing_2 * 9) + (Yspacing_3 * 7), Color.White);
            spriteBatch.DrawString(_font1, "Background Music : \n   -Lao Somdet (Lan Xang Version)\n   -Khmer Phaia Ruea\n   -Thai Mung\n   -Thai Ar Hom (Chaina)\n   -Lao Lum Dab", screenCenter - (textsize / 2) + (Yspacing_1 * 7) + (Yspacing_2 * 10) + (Yspacing_3 * 7), Color.White);
            spriteBatch.DrawString(_font1, "Engine : MonoGame 3.8.4", screenCenter - (textsize / 2) + (Yspacing_1 * 7) + (Yspacing_2 * 18) + (Yspacing_3 * 7), Color.White);

            //Special Thanks
            spriteBatch.DrawString(_font1, "Special Thanks", screenCenter - (textsize / 4) + (Yspacing_1 * 11) + (Yspacing_2 * 8) + (Yspacing_3 * 7), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["ST 1"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 11) + (Yspacing_2 * 9) + (Yspacing_3 * 8), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["ST 2"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 11) + (Yspacing_2 * 10) + (Yspacing_3 * 8), Color.White);
            spriteBatch.DrawString(_font1, $"{(string)nameCredit["ST 3"]}", screenCenter - (textsize / 2) + (Yspacing_1 * 11) + (Yspacing_2 * 11) + (Yspacing_3 * 8), Color.White);

            //End Credit
            spriteBatch.DrawString(_font3, "Thank you for playing", screenCenter - new Vector2(450, 0) + (Yspacing_1 * 16) + (Yspacing_2 * 10) + (Yspacing_3 * 10), Color.White);

            spriteBatch.End();
        }
    }
}
