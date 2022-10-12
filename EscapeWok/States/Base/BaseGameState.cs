﻿using EscapeWok.Enum;
using EscapeWok.Objects.Base;
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
        public event EventHandler<Events> OnEventNotification;

        public abstract void LoadContent(ContentManager contentManager);
        public abstract void UnloadContent(ContentManager contentManager);
        public abstract void HandleInput();
        protected void SwitchState(BaseGameState baseGameState)
        {
            OnStateSwitched?.Invoke(this, baseGameState);
        }
        protected void NotifyEvent(Events eventType, object argument = null)
        {
            OnEventNotification?.Invoke(this, eventType);
            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnNotify(eventType);
            }
        }
        protected void AddGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }
        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects.OrderBy(a=>a.zIndex))
            {
                gameObject.Render(spriteBatch);
            }
        }
    }
}