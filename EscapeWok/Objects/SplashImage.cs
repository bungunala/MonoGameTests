using EscapeWok.Objects.Base;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeWok.Objects
{
    internal class SplashImage:BaseGameObject
    {
        public SplashImage(Texture2D _tex)
        {
            _texture = _tex;            
        }
    }
}
