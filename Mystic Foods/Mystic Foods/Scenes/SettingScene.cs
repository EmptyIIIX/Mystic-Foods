using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;
using System;
using System.Threading.Tasks;

namespace Mystic_Foods.Scenes
{
    public class SettingScene : IGameScene
    {
        private SpriteFont _font;
        private KeyboardState prevKey;

        private Texture2D bg, header, settingBG;
        private Texture2D musicIcon, muteMusicIcon;
        private Texture2D sfxIcon, muteSfxIcon;
        private Texture2D barBg, barFill, knob;
        private Button homeButton;

        private Vector2 musicBarPos = new Vector2(600, 425);
        private Vector2 sfxBarPos = new Vector2(600, 675);
        private const float barScale = 1.0f;

        public bool MenuRequest = false;

        private bool _draggingMusic = false;
        private bool _draggingSfx = false;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("MainFont");

            bg = content.Load<Texture2D>("Environments/BG/MenuBG");
            header = content.Load<Texture2D>("UI/setting/Setting_Word");
            settingBG = content.Load<Texture2D>("UI/setting/Setting_BG");

            musicIcon = content.Load<Texture2D>("UI/setting/Music_UI");
            muteMusicIcon = content.Load<Texture2D>("UI/setting/MuteSong");
            sfxIcon = content.Load<Texture2D>("UI/setting/SoundEffect");
            muteSfxIcon = content.Load<Texture2D>("UI/setting/MuteSoundEffect");

            barBg = content.Load<Texture2D>("UI/setting/IncreaseSound_BG_UI");
            barFill = content.Load<Texture2D>("UI/setting/IncreaseSound_UI");
            knob = content.Load<Texture2D>("UI/setting/SoundButton");

            var homeBtnTex = content.Load<Texture2D>("UI/previous");
            homeButton = new Button(homeBtnTex, homeBtnTex, _font, "", new Rectangle(50, 50, 80, 100));
            homeButton.Click += HomeButton_Click;
        }

        public void Update(GameTime gameTime)
        {
            homeButton.Update();
            var key = Keyboard.GetState();
            var mouse = Mouse.GetState();

            Vector2 musicBarPos = new Vector2(600, 425);
            Vector2 sfxBarPos = new Vector2(600, 675);

            float mx = mouse.X;
            float my = mouse.Y;

            Rectangle musicBarRect = new Rectangle((int)musicBarPos.X, (int)musicBarPos.Y, barBg.Width, barBg.Height);
            Rectangle sfxBarRect = new Rectangle((int)sfxBarPos.X, (int)sfxBarPos.Y, barBg.Width, barBg.Height);

            // --- Mouse Drag Volume ---
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (!_draggingMusic && !_draggingSfx)
                {
                    if (musicBarRect.Contains(mx, my))
                        _draggingMusic = true;
                    else if (sfxBarRect.Contains(mx, my))
                        _draggingSfx = true;
                }
            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                _draggingMusic = false;
                _draggingSfx = false;
            }

            if (_draggingMusic)
            {
                float newVol = MathHelper.Clamp((mx - musicBarRect.X) / (float)musicBarRect.Width, 0f, 1f);
                SoundManager.SetMusicVolume(newVol);
            }
            else if (_draggingSfx)
            {
                float newVol = MathHelper.Clamp((mx - sfxBarRect.X) / (float)sfxBarRect.Width, 0f, 1f);
                SoundManager.SetSfxVolume(newVol);
            }
            #region Keyboadrd
            if (key.IsKeyDown(Keys.Right) && !prevKey.IsKeyDown(Keys.Right))
                SoundManager.SetMusicVolume(SoundManager.MusicVolume + 0.1f);

            if (key.IsKeyDown(Keys.Left) && !prevKey.IsKeyDown(Keys.Left))
                SoundManager.SetMusicVolume(SoundManager.MusicVolume - 0.1f);

            if (key.IsKeyDown(Keys.Up) && !prevKey.IsKeyDown(Keys.Up))
                SoundManager.SetSfxVolume(SoundManager.SfxVolume + 0.1f);

            if (key.IsKeyDown(Keys.Down) && !prevKey.IsKeyDown(Keys.Down))
                SoundManager.SetSfxVolume(SoundManager.SfxVolume - 0.1f);

            if (key.IsKeyDown(Keys.M) && !prevKey.IsKeyDown(Keys.M))
                SoundManager.ToggleMute();
            prevKey = key;
            #endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            spriteBatch.Draw(bg, new Vector2((1920 - bg.Width) / 2, (1080 - bg.Height) / 2), Color.White);
            spriteBatch.Draw(settingBG, new Vector2(224, 175), Color.White);

            spriteBatch.Draw(header, new Vector2((1920 - header.Width) / 2, 120), Color.White);
            homeButton.Draw(spriteBatch);

            DrawVolumeBar(spriteBatch, musicBarPos, SoundManager.MusicVolume, SoundManager.MusicVolume > 0, musicIcon, muteMusicIcon);
            DrawVolumeBar(spriteBatch, sfxBarPos, SoundManager.SfxVolume, SoundManager.SfxVolume > 0, sfxIcon, muteSfxIcon);

            spriteBatch.End();
        }

        private void DrawVolumeBar(SpriteBatch spriteBatch, Vector2 position, float volume, bool notMuted, Texture2D normalIcon, Texture2D muteIcon)
        {
            spriteBatch.Draw(notMuted ? normalIcon : muteIcon, new Vector2(position.X - 250, position.Y - 30), Color.White);

            spriteBatch.Draw(barBg, position, null, Color.White, 0f, Vector2.Zero, barScale, SpriteEffects.None, 0f);
            float fillWidth = barFill.Width * volume;
            Rectangle sourceRect = new Rectangle(0, 0, (int)fillWidth, barFill.Height);

            spriteBatch.Draw(barFill, position, sourceRect, Color.White, 0f, Vector2.Zero, barScale, SpriteEffects.None, 0f);

            Vector2 knobPos = new Vector2(position.X + fillWidth - knob.Width / 2, position.Y - 5);
            spriteBatch.Draw(knob, knobPos, Color.White);

            spriteBatch.DrawString(_font, $"{(int)(volume * 100)}%", new Vector2(position.X + barBg.Width + 50, position.Y), Color.Black);
        }

        private async void HomeButton_Click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            await Task.Delay(100);
            MenuRequest = true;
        }
    }
}
