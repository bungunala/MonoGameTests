using EscapeWok.Engine.Input;
using EscapeWok.Engine.States;
using EscapeWok.Enum;
using EscapeWok.Objects;
using EscapeWok.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EscapeWok.States.Gameplay
{
    internal class GameplayState : BaseGameState
    {
        //images
        private const string SpritePlayerFighter = @"gfx/fighter";
        private const string SpriteBackgroundTexture = @"gfx/Barren";
        private const string SpriteBulletTexture = @"gfx/bullet";
        //music
        private const string MusicFuture1 = @"music/FutureAmbient_1";
        private const string MusicFuture2 = @"music/FutureAmbient_2";
        //sounds
        private const string SoundBullet = @"sounds/bullet";

        private PlayerSprite _playerSprite;
        private Texture2D _bulletTexture;
        private List<BulletSprite> _bulletList;

        private bool _isShooting;
        private TimeSpan _lastShotAt;

        public override void HandleInput(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    NotifyEvent(Events.GAME_QUIT);
            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                { NotifyEvent(new BaseGameStateEvent.GameQuit()); }

                if (cmd is GameplayInputCommand.PlayerMoveLeft)
                {
                    _playerSprite.MoveLeft();
                    KeepPlayerInBounds();
                }

                if (cmd is GameplayInputCommand.PlayerMoveRight)
                {
                    _playerSprite.MoveRight();
                    KeepPlayerInBounds();
                }

                if (cmd is GameplayInputCommand.PlayerShoots)
                { Shoot(gameTime); }
            });
        }

        public override void UpdateGameState(GameTime gameTime)
        {
            foreach (var bullet in _bulletList)
                bullet.MoveUp();
            //no dejar disparar muy seguido
            if (gameTime.TotalGameTime - _lastShotAt > TimeSpan.FromSeconds(0.1))
                _isShooting = false;

            //borrar balas fuera de la pantalla
            var newBulletList = new List<BulletSprite>();
            foreach (var bullet in _bulletList)
            {
                var bulletStillOnScreen = bullet.Position.Y > -30;
                if (bulletStillOnScreen)
                    newBulletList.Add(bullet);
                else
                    RemoveGameObject(bullet);
            }
            _bulletList = newBulletList;
        }

        private void Shoot(GameTime gameTime)
        {
            if (!_isShooting)
            {
                CreateBullets();
                _isShooting = true;
                _lastShotAt = gameTime.TotalGameTime;
                NotifyEvent(new GameplayStateEvent.PlayerShoots());
            }
        }

        private void CreateBullets()
        {
            var bulletSpriteLeft = new BulletSprite(_bulletTexture);
            var bulletSpriteRight = new BulletSprite(_bulletTexture);
            // Position bullets around the fighter's nose when they get fired
            var bulletY = _playerSprite.Position.Y + 30;
            var bulletLeftX = _playerSprite.Position.X + _playerSprite.Width / 2 - 40;
            var bulletRightX = _playerSprite.Position.X + _playerSprite.Width / 2 + 10;
            bulletSpriteLeft.Position = new Vector2(bulletLeftX, bulletY);
            bulletSpriteRight.Position = new Vector2(bulletRightX, bulletY);
            _bulletList.Add(bulletSpriteLeft);
            _bulletList.Add(bulletSpriteRight);
            AddGameObject(bulletSpriteLeft);
            AddGameObject(bulletSpriteRight);
        }

        public override void LoadContent()
        {
            //Fondo
            AddGameObject(new TerrainBackground(LoadTexture(SpriteBackgroundTexture)));
            //Nave
            _playerSprite = new PlayerSprite(LoadTexture(SpritePlayerFighter));
            var playerPosX = _viewportWidth / 2 - _playerSprite.Width / 2;
            var playerPosY = _viewportHeight - _playerSprite.Height - 30;
            _playerSprite.Position = new Vector2(playerPosX, playerPosY);
            AddGameObject(_playerSprite);
            //balas
            _bulletTexture = LoadTexture(SpriteBulletTexture);
            _bulletList = new List<BulletSprite>();
            //audio musica
            var track1 = LoadSound(MusicFuture1).CreateInstance();
            var track2 = LoadSound(MusicFuture2).CreateInstance();
            _soundManager.SetSoundtrack(new List<SoundEffectInstance> { track1, track2 });
            //audio efectos
            var bulletSound = LoadSound(SoundBullet);
            _soundManager.RegisterSound(new GameplayStateEvent.PlayerShoots(), bulletSound);
        }
        private void KeepPlayerInBounds()
        {
            if (_playerSprite.Position.X < 0)
                _playerSprite.Position = new Vector2(0, _playerSprite.Position.Y);

            if (_playerSprite.Position.X > _viewportWidth - _playerSprite.Width)
                _playerSprite.Position = new Vector2(_viewportWidth - _playerSprite.Width, _playerSprite.Position.Y);

            if (_playerSprite.Position.Y < 0)
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, 0);

            if (_playerSprite.Position.Y > _viewportHeight - _playerSprite.Height)
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, _viewportHeight - _playerSprite.Height);

        }
        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }
    }
}
