using EscapeWok.Managers.Input.Base;

namespace EscapeWok.Managers.Input
{
    internal class SplashInputCommand:BaseInputCommand
    {
        public class GameExit : SplashInputCommand { };
        public class GameStart : SplashInputCommand { };        
    }
}
