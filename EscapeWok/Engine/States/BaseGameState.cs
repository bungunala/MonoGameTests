using EscapeWok.Engine.Input;
using EscapeWok.Engine.Sound;
using EscapeWok.Engine.States;
using EscapeWok.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscapeWok.States.Base
{
    public abstract class BaseGameState
    {
        private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();

        public event EventHandler<BaseGameState> OnStateSwitched;
        public event EventHandler<BaseGameStateEvent> OnEventNotification;

        private const string FallBackTexture = "Empty";
        private const string FallBackSound = "empty";
        private ContentManager _contentManager;
        protected int _viewportWidth;
        protected int _viewportHeight;
        protected SoundManager _soundManager = new();
        protected InputManager InputManager { get; set; }

        public abstract void LoadContent();
        public void UnloadContent(ContentManager contentManager)
        {
            _contentManager.Unload();
        }
        public abstract void HandleInput(GameTime gametime);
        public abstract void UpdateGameState(GameTime gametime);
        public void Update(GameTime gameTime) {
            HandleInput(gameTime);
            UpdateGameState(gameTime);
            _soundManager.PlaySoundtrack();
        }
        protected void SwitchState(BaseGameState baseGameState)
        {
            OnStateSwitched?.Invoke(this, baseGameState);
        }
        protected void NotifyEvent(BaseGameStateEvent gameEvent, object argument = null)
        {
            OnEventNotification?.Invoke(this, gameEvent);
            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnNotify(gameEvent);
            }
            _soundManager?.OnNotify(gameEvent);
        }
        protected void AddGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }
        protected void RemoveGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Remove(gameObject);
        }
        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects.OrderBy(a => a.zIndex))
            {
                gameObject.Render(spriteBatch);
            }
        }
        public void Initialize(ContentManager contentManager, int viewportWidth, int viewportHeight)
        {
            _contentManager = contentManager;
            _viewportHeight = viewportHeight;
            _viewportWidth = viewportWidth;

            SetInputManager();
        }
        protected Texture2D LoadTexture(string textureName)
        {
            var texture = _contentManager.Load<Texture2D>(textureName);
            return texture ?? _contentManager.Load<Texture2D>(FallBackTexture);
        }
        protected SoundEffect LoadSound(string soundName)
        { 
            var sound = _contentManager.Load<SoundEffect>(soundName);
            return sound ?? _contentManager.Load<SoundEffect>(FallBackSound);
        }
        protected abstract void SetInputManager();
    }
}
