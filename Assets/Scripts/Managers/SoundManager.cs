using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using RunnerBoi.Component;

namespace RunnerBoi.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private Sound[] Sounds;

        private List<Sound> playing;

        private void Awake()
        {
            Instance = this;

            foreach(Sound s in Sounds)
            {
                s.Source = this.gameObject.AddComponent<AudioSource>();
                s.Source.clip = s.Clip;
                s.Source.volume = s.Volume;
                s.Source.pitch = s.Pitch;
            }
            playing = new List<Sound>();
            PlaySoundLoop("MainMenu", true);
        }

        private void Start()
        {
            Actions.Instance.OnGameStateChange += HandleGameStateChange;
        }

        public void PlaySound(string soundName)
        {
            PlaySoundLoop(soundName, false);
        }

        public void PlaySoundLoop(string soundName, bool loop)
        {
            Sound s = System.Array.Find(Sounds, sound => sound.Name == soundName);
            s.Source.Play();
            s.Source.loop = loop;
            playing.Add(s);
        }
        
        public void StopSounds()
        {
            foreach(Sound s in playing)
            {
                if(s.Source.isPlaying)
                    s.Source.Stop();
            }
            playing.Clear();
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.OnReady:
                    StopSounds();
                    PlaySoundLoop("OnReady", true);
                    break;
                case GameState.Racing:
                    StopSounds();
                    PlaySoundLoop("Racing", true);
                    break;
                case GameState.RaceLost:
                    break;
                case GameState.RaceWon:
                    StopSounds();
                    PlaySoundLoop("RaceWon", true);
                    break;
                case GameState.RaceFinished:
                    break;
                case GameState.Wallpaint:
                    break;
                case GameState.Endgame:
                    StopSounds();
                    PlaySoundLoop("MainMenu", true);
                    break;
                case GameState.MainMenu:
                    StopSounds();
                    PlaySoundLoop("MainMenu", true);
                    break;
                default:
                    break;
            }
        }
    }
}

