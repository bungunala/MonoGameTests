using EscapeWok.Engine.Input;

namespace EscapeWok.States.Splash
{
    internal class SplashInputCommand : BaseInputCommand
    {
        public class GameExit : SplashInputCommand { };
        public class GameStart : SplashInputCommand { };
    }
}
