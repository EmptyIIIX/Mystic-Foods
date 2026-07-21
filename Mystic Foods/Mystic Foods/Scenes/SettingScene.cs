using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using Mystic_Foods.Core.DI;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.Scenes;
using Mystic_Foods.Core.UI;
using Mystic_Foods.Managers;

namespace Mystic_Foods.Scenes
{
    public class SettingScene : IScene
    {
        private readonly Container _container;
        private readonly IGraphicsService _graphics;
        private readonly IInputService _input;

        private SpriteFont _font;
        private KeyboardState _prevKey;
        public bool MenuRequest { get; private set; }

        private Button _homeButton;
        private Texture2D _homeBtnTex;

        public string Name => "Setting";
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; } = true;

        public SettingScene(Container container)
        {
            _container = container;
            _graphics = container.Resolve<IGraphicsService>();
            _input = container.Resolve<IInputService>();
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("MainFont");
            _homeBtnTex = content.Load<Texture2D>("Etc/option_exit");

            _homeButton = new Button(new ButtonConfig
            {
                Texture = _homeBtnTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(50, 50, 80, 100),
                OnClick = _ => HomeButton_Click()
            });
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            _input.Update();

            _homeButton.Update(gameTime);

            var key = Keyboard.GetState();

            // ปรับ BGM
            if (key.IsKeyDown(Keys.Right) && !_prevKey.IsKeyDown(Keys.Right))
                SoundManager.SetMusicVolume(MathHelper.Clamp(SoundManager.MusicVolume + 0.1f, 0f, 1f));

            if (key.IsKeyDown(Keys.Left) && !_prevKey.IsKeyDown(Keys.Left))
                SoundManager.SetMusicVolume(MathHelper.Clamp(SoundManager.MusicVolume - 0.1f, 0f, 1f));

            // ปรับ SFX
            if (key.IsKeyDown(Keys.Up) && !_prevKey.IsKeyDown(Keys.Up))
                SoundManager.SetSfxVolume(MathHelper.Clamp(SoundManager.SfxVolume + 0.1f, 0f, 1f));

            if (key.IsKeyDown(Keys.Down) && !_prevKey.IsKeyDown(Keys.Down))
                SoundManager.SetSfxVolume(MathHelper.Clamp(SoundManager.SfxVolume - 0.1f, 0f, 1f));

            // ปุ่ม Mute
            if (key.IsKeyDown(Keys.M) && !_prevKey.IsKeyDown(Keys.M))
                SoundManager.ToggleMute();

            // ปุ่ม Save
            if (key.IsKeyDown(Keys.Enter) && !_prevKey.IsKeyDown(Keys.Enter))
                SoundManager.SaveSettings();

            _prevKey = key;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            _graphics.Begin();
            _graphics.SpriteBatch.GraphicsDevice.Clear(Color.Black);

            _homeButton.Draw(gameTime, _graphics.SpriteBatch);

            _graphics.SpriteBatch.DrawString(_font, $"BGM Volume: {(int)(SoundManager.MusicVolume * 100)}%", new Vector2(100, 100), Color.White);
            _graphics.SpriteBatch.DrawString(_font, $"SFX Volume: {(int)(SoundManager.SfxVolume * 100)}%", new Vector2(100, 140), Color.White);
            _graphics.SpriteBatch.DrawString(_font, $"Muted: {(SoundManager.IsMuted ? "Yes" : "No")}", new Vector2(100, 180), Color.White);
            _graphics.SpriteBatch.DrawString(_font, "←/→: BGM | ↑/↓: SFX | M: Mute | Enter: Save", new Vector2(100, 240), Color.Gray);

            _graphics.End();
        }

        private void HomeButton_Click()
        {
            SoundManager.PlaySfx("Click");
            MenuRequest = true;
        }

        public void OnEnter() { }
        public void OnExit() { }
        public void OnResize(int width, int height) { }
    }
}