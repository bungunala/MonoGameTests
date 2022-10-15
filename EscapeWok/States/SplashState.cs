using EscapeWok.Objects;
using EscapeWok.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EscapeWok.States
{
    internal class SplashState : BaseGameState
    {
        public override void HandleInput()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                SwitchState(new GameplayState());
        }

        public override void LoadContent(ContentManager contentManager)
        {
            AddGameObject(new SplashImage(LoadTexture(@"gfx/splash")));
        }

       
    }
}
