using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace TowerDefense.GamePlay.Sound
{
    public static class SoundManager
    {
        private static SoundEffect _bombExplosion;
        private static SoundEffect _bombFire;
        private static SoundEffect _creepDeath;
        private static SoundEffect _missle;
        private static SoundEffect _pelletFire;
        private static SoundEffect _sellTurret;
        private static SoundEffect _turretPlacement;
        private static SoundEffect _basicFire;

        private static Song _music;

        private static bool _musicPlaying;

        private static List<SoundPackage> _soundEffects;
        public static void Init(ContentManager content)
        {
            _bombExplosion = content.Load<SoundEffect>("Sounds/bombExplode");
            _bombFire = content.Load<SoundEffect>("Sounds/bombFire");
            _creepDeath = content.Load<SoundEffect>("Sounds/creepDeath");
            _missle = content.Load<SoundEffect>("Sounds/missle");
            _pelletFire = content.Load<SoundEffect>("Sounds/pelletFire");
            _sellTurret = content.Load<SoundEffect>("Sounds/sellTurret");
            _turretPlacement = content.Load<SoundEffect>("Sounds/turretPlacement");
            _basicFire = content.Load<SoundEffect>("Sounds/basicFire");
            _music = content.Load<Song>("Sounds/music");

            
            MediaPlayer.Volume = .1f;
            MediaPlayer.IsRepeating = true;
            _soundEffects = new List<SoundPackage>();
        }

        public static void PlayMusic()
        {
            if (!_musicPlaying)
            {
                _musicPlaying = true;
                MediaPlayer.Play(_music);
            }
        }





        public static void PauseMusic()
        {
            if (_musicPlaying)
            {
                _musicPlaying = false;
                MediaPlayer.Pause();
            }
        }

        public static void Update()
        {
            try
            {
                for (int x = 0; x < _soundEffects.Count; x++)
                {
                    if (!_soundEffects[x].Play)
                    {
                        continue;
                    }
                    else
                    {
                        _soundEffects[x].Play = false;
                        _soundEffects[x].Sound.Play();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Sounds crashed");
                _soundEffects.Clear();
            }

            _soundEffects.RemoveAll(t => !t.Play && t.Sound.State == SoundState.Stopped);

        }

        public static void ShootPellet()
        {
            var instance = _pelletFire.CreateInstance();
            instance.Volume = .1f;
            _soundEffects.Add(new SoundPackage(true, instance));
        }
        public static void BombExplosion()
        {
            _soundEffects.Add(new SoundPackage(true, _bombExplosion.CreateInstance()));
        }
        public static void BombFire()
        {
            _soundEffects.Add(new SoundPackage(true, _bombFire.CreateInstance()));
        }
        public static void CreepDeath()
        {
            _soundEffects.Add(new SoundPackage(true, _creepDeath.CreateInstance()));
        }
        public static void Missle()
        {
           // _soundEffects.Add(new SoundPackage(true, _missle.CreateInstance()));
        }
        public static void SellTurret()
        {
            _soundEffects.Add(new SoundPackage(true, _sellTurret.CreateInstance()));
        }
        public static void PlaceTurret()
        {
            _soundEffects.Add(new SoundPackage(true, _turretPlacement.CreateInstance()));
        }
        public static void ShootBasic()
        {
            _soundEffects.Add(new SoundPackage(true, _basicFire.CreateInstance()));
        }


        private class SoundPackage
        {
            public bool Play;
            public SoundEffectInstance Sound;
            public SoundPackage(bool play, SoundEffectInstance sound)
            {
                this.Play = play;
                this.Sound = sound;
            }
        }
    }
}
