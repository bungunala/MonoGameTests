using EscapeWok.Managers.Input.Base;
using EscapeWok.Objects;
using EscapeWok.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using EscapeWok.Managers.Input;
using EscapeWok.Enum;

namespace EscapeWok.States
{
    internal class SplashState : BaseGameState
    {
        public override void HandleInput(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            //    SwitchState(new GameplayState());
            InputManager.GetCommands(cmd => {
                if (cmd is SplashInputCommand.GameExit)
                { NotifyEvent(Events.GAME_QUIT); }

                if (cmd is SplashInputCommand.GameStart)
                { SwitchState(new GameplayState()); }
            });
        }

        public override void LoadContent(ContentManager contentManager)
        {
            AddGameObject(new SplashImage(LoadTexture(@"gfx/splash")));
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }
    }
}
