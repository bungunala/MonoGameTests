using EscapeWok.Managers.Input.Base;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EscapeWok.Managers.Input
{
    internal class GameplayInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<GameplayInputCommand>();
            if (state.IsKeyDown(Keys.Escape))
            { commands.Add(new GameplayInputCommand.GameExit()); }

            if (state.IsKeyDown(Keys.Left))
            { commands.Add(new GameplayInputCommand.PlayerMoveLeft()); }

            if (state.IsKeyDown(Keys.Right))
            { commands.Add(new GameplayInputCommand.PlayerMoveRight()); }

            if (state.IsKeyDown(Keys.Space))
            { commands.Add(new GameplayInputCommand.PlayerShoots()); }

            return commands;
        }
    }
}

