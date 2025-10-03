using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Foods.Managers
{
    public static class SoundManager
    {
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, Song> songs = new Dictionary<string, Song>();

        // ค่า Volume (0.0f - 1.0f)
        public static float MusicVolume
        {
            get => MediaPlayer.Volume;
            set => MediaPlayer.Volume = MathHelper.Clamp(value, 0f, 1f);
        }

        public static float SfxVolume { get; set; } = 1f;

        // โหลดเพลง
        public static void AddSong(string key, Song song)
        {
            if (!songs.ContainsKey(key))
                songs[key] = song;
        }

        // โหลด SFX
        public static void AddSound(string key, SoundEffect sound)
        {
            if (!soundEffects.ContainsKey(key))
                soundEffects[key] = sound;
        }

        // เล่นเพลง (BGM)
        public static void PlaySong(string key, bool isRepeat = true)
        {
            if (songs.TryGetValue(key, out Song song))
            {
                MediaPlayer.IsRepeating = isRepeat;
                MediaPlayer.Play(song);
            }
        }

        // เล่น SFX
        public static void PlaySfx(string key)
        {
            if (soundEffects.TryGetValue(key, out SoundEffect sfx))
            {
                var instance = sfx.CreateInstance();
                instance.Volume = SfxVolume;
                instance.Play();
            }
        }
    }
}
