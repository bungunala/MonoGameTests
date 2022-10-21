using EscapeWok.Objects;
using EscapeWok.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using EscapeWok.States.Gameplay;
using EscapeWok.Engine.Input;
using EscapeWok.Engine.States;

namespace EscapeWok.States.Splash
{
    internal class SplashState : BaseGameState
    {
        private const string SplashScreen = @"gfx/splash";
        public override void HandleInput(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            //    SwitchState(new GameplayState());
            InputManager.GetCommands(cmd =>
            {
                if (cmd is SplashInputCommand.GameExit)
                { NotifyEvent(new BaseGameStateEvent.GameQuit()); }

                if (cmd is SplashInputCommand.GameStart)
                { SwitchState(new GameplayState()); }
            });
        }

        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture(SplashScreen)));
        }

        public override void UpdateGameState(GameTime gametime)
        {
            //throw new System.NotImplementedException();
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }
    }
}
