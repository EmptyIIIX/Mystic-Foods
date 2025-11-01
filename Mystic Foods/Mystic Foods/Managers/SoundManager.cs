using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mystic_Foods.Managers
{
    public static class SoundManager
    {
        private static readonly Dictionary<string, SoundEffect> soundEffects = new();
        private static readonly Dictionary<string, Song> songs = new();
        private static readonly Dictionary<string, SoundEffectInstance> activeSfx = new();
        private static SettingsData settings = new SettingsData();

        // 🔊 ค่า Volume (0.0f - 1.0f)
        public static float MusicVolume
        {
            get => MediaPlayer.Volume;
            set => MediaPlayer.Volume = MathHelper.Clamp(value, 0f, 1f);
        }

        public static float SfxVolume { get; set; } = 1f;

        // 🎵 โหลดเพลง
        public static void AddSong(string key, Song song)
        {
            if (!songs.ContainsKey(key))
                songs[key] = song;
        }

        // 💥 โหลด SFX
        public static void AddSound(string key, SoundEffect sound)
        {
            if (!soundEffects.ContainsKey(key))
                soundEffects[key] = sound;
        }

        // ▶️ เล่นเพลง (BGM)
        public static void PlaySong(string key, bool isRepeat = true)
        {
            if (songs.TryGetValue(key, out Song song))
            {
                MediaPlayer.IsRepeating = isRepeat;
                MediaPlayer.Play(song);
            }
        }

        // ⏸️ หยุดเพลง
        public static void StopMusic() => MediaPlayer.Stop();
        public static void PauseMusic() => MediaPlayer.Pause();
        public static void ResumeMusic() => MediaPlayer.Resume();

        // 💫 Fade out เพลง (แบบนุ่มนวล)
        public static async Task FadeOutMusic(float durationSeconds = 1f)
        {
            float startVolume = MediaPlayer.Volume;
            int steps = 20;
            for (int i = 0; i < steps; i++)
            {
                MediaPlayer.Volume = MathHelper.Lerp(startVolume, 0f, (float)i / steps);
                await Task.Delay((int)(durationSeconds * 1000 / steps));
            }
            MediaPlayer.Stop();
            MediaPlayer.Volume = startVolume;
        }

        // 💥 เล่น SFX (และสามารถควบคุม loop ได้)
        public static void PlaySfx(string key, bool loop = false)
        {
            if (soundEffects.TryGetValue(key, out SoundEffect sfx))
            {
                var instance = sfx.CreateInstance();
                instance.Volume = SfxVolume;
                instance.IsLooped = loop;
                instance.Play();

                // เก็บไว้เพื่อควบคุมภายหลัง
                activeSfx[key] = instance;
            }
        }

        // ⏸️ Pause / Resume / Stop เฉพาะ SFX
        public static void PauseSfx(string key)
        {
            if (activeSfx.TryGetValue(key, out var instance))
                instance.Pause();
        }

        public static void ResumeSfx(string key)
        {
            if (activeSfx.TryGetValue(key, out var instance))
                instance.Resume();
        }

        public static void StopSfx(string key)
        {
            if (activeSfx.TryGetValue(key, out var instance))
            {
                instance.Stop();
                instance.Dispose();
                activeSfx.Remove(key);
            }
        }

        // ⛔ หยุด SFX ทั้งหมด
        public static void StopAllSfx()
        {
            foreach (var kv in activeSfx)
            {
                kv.Value.Stop();
                kv.Value.Dispose();
            }
            activeSfx.Clear();
        }

        #region Setting
        public static void LoadSettings()
        {
            settings = SettingsData.Load();
            ApplySettings();
        }

        public static void SaveSettings()
        {
            settings.Save();
        }

        public static void ApplySettings()
        {
            MusicVolume = settings.IsMuted ? 0 : settings.MusicVolume;
            SfxVolume = settings.IsMuted ? 0 : settings.SfxVolume;
        }

        public static void SetMusicVolume(float volume)
        {
            settings.MusicVolume = MathHelper.Clamp(volume, 0f, 1f);
            if (!settings.IsMuted) MusicVolume = settings.MusicVolume;

            SaveSettings();
            //ApplySettings();
        }

        public static void SetSfxVolume(float volume)
        {
            settings.SfxVolume = MathHelper.Clamp(volume, 0f, 1f);
            if (!settings.IsMuted) SfxVolume = settings.SfxVolume;

            SaveSettings();
            //ApplySettings();
        }

        public static void ToggleMute()
        {
            settings.IsMuted = !settings.IsMuted;
            ApplySettings();
        }
        #endregion
    }
}
