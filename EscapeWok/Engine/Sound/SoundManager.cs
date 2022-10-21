
using EscapeWok.Engine.States;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace EscapeWok.Engine.Sound
{
    public class SoundManager
    {
        private int _soundtrackIndex = -1;
        //Lista de canciones de soundtrack
        private List<SoundEffectInstance> _soundtracks = new List<SoundEffectInstance>();
        //lista de efectos sonidos 
        private Dictionary<Type, SoundEffect> _soundBank = new();

        public void SetSoundtrack(List<SoundEffectInstance> tracks)
        {
            _soundtracks = tracks;
            _soundtrackIndex = _soundtracks.Count - 1;
        }

        public void PlaySoundtrack()
        {
            var numberOfTracks = _soundtracks.Count;
            if (numberOfTracks <= 0)
                return;
            var currentTrack = _soundtracks[_soundtrackIndex];
            var nextTrack = _soundtracks[(_soundtrackIndex + 1) % numberOfTracks];
            if (currentTrack.State == SoundState.Stopped)
            { 
                nextTrack.Play();
                _soundtrackIndex++;
                if (_soundtrackIndex >= numberOfTracks)
                    _soundtrackIndex = 0;
            }
        }

        public void RegisterSound(BaseGameStateEvent gameEvent, SoundEffect sound)
        {
            _soundBank.Add(gameEvent.GetType(), sound);
        }

        public void OnNotify(BaseGameStateEvent gameEvent)
        {
            if (_soundBank.ContainsKey(gameEvent.GetType()))
            { 
                var sound = _soundBank[gameEvent.GetType()];
                sound.Play();
            }
        }


    }
}
