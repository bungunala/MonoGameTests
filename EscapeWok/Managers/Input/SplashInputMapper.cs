using EscapeWok.Managers.Input.Base;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeWok.Managers.Input
{
    internal class SplashInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<SplashInputCommand>();
            if (state.IsKeyDown(Keys.Escape))
            { commands.Add(new SplashInputCommand.GameExit()); }

            if (state.IsKeyDown(Keys.Enter))
            { commands.Add(new SplashInputCommand.GameStart()); }

            return commands;
        }
    }
}
