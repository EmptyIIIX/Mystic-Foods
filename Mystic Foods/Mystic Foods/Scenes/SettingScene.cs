using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Foods.Scenes
{
    public class SettingScene : IGameScene
    {
        private SpriteFont _font;
        private KeyboardState prevKey;
        public bool MenuRequest = false;

        Button homeButton;
        private Texture2D homeBtn;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("MainFont");

            homeBtn = content.Load<Texture2D>("Etc/option_exit");

            homeButton = new Button(homeBtn, _font, "", new Rectangle(50, 50, 80, 100));
            homeButton.Click += HomeButton_Click;
        }

        public void Update(GameTime gameTime)
        {
            homeButton.Update();

            var key = Keyboard.GetState();

            // ปรับ BGM
            if (key.IsKeyDown(Keys.Right) && !prevKey.IsKeyDown(Keys.Right))
                SoundManager.SetMusicVolume(SoundManager.MusicVolume + 0.1f);

            if (key.IsKeyDown(Keys.Left) && !prevKey.IsKeyDown(Keys.Left))
                SoundManager.SetMusicVolume(SoundManager.MusicVolume - 0.1f);

            // ปรับ SFX
            if (key.IsKeyDown(Keys.Up) && !prevKey.IsKeyDown(Keys.Up))
                SoundManager.SetSfxVolume(SoundManager.SfxVolume + 0.1f);

            if (key.IsKeyDown(Keys.Down) && !prevKey.IsKeyDown(Keys.Down))
                SoundManager.SetSfxVolume(SoundManager.SfxVolume - 0.1f);

            // ปุ่ม Mute
            if (key.IsKeyDown(Keys.M) && !prevKey.IsKeyDown(Keys.M))
                SoundManager.ToggleMute();

            // ปุ่ม Save
            if (key.IsKeyDown(Keys.Enter) && !prevKey.IsKeyDown(Keys.Enter))
                SoundManager.SaveSettings();

            prevKey = key;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            homeButton.Draw(spriteBatch);

            spriteBatch.DrawString(_font, $"BGM Volume: {(int)(SoundManager.MusicVolume * 100)}%", new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(_font, $"SFX Volume: {(int)(SoundManager.SfxVolume * 100)}%", new Vector2(100, 140), Color.White);
            spriteBatch.DrawString(_font, $"Muted: {(MediaPlayer.Volume == 0 ? "Yes" : "No")}", new Vector2(100, 180), Color.White);
            spriteBatch.DrawString(_font, "←/→: BGM | ↑/↓: SFX | M: Mute | Enter: Save", new Vector2(100, 240), Color.Gray);

            spriteBatch.End();
        }

        public async void HomeButton_Click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            await Task.Delay(100);
            MenuRequest = true;
        }
    }
}
