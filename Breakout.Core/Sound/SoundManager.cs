
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Breakout.Core.Screen;
using Microsoft.Xna.Framework.Media;

namespace Breakout.Core.Sound
{
    public class SoundManager
    {
        #region Singleton
        private static SoundManager instance { get; set; }

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SoundManager();
                return instance;
            }
        }
        #endregion

        public SoundManager()
        {
            soundEffects = new Dictionary<string, Song>();
        }

        public void Initialize()
        { 
        }

        private ContentManager content;
        private Dictionary<string, Song> soundEffects;

        public void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            object x = content.Load<object>("Sounds/magic-chime-01");
            soundEffects.Add("bonus", content.Load<Song>("Sounds/magic-chime-01"));
            soundEffects.Add("paddle", content.Load<Song>("Sounds/paddle"));
            soundEffects.Add("brick", content.Load<Song>("Sounds/brick"));
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Update(GameTime gametime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        public bool Play( string sound )
        {
            if ( soundEffects.ContainsKey( sound ))
            {
                var soundEffect = soundEffects[sound];
                //MediaPlayer.Stop();
                //MediaPlayer.IsRepeating = false;
                //MediaPlayer.Play(soundEffect);
                return true;
                //return soundEffect.Play();
            }
            return false;
        }
    }
}
