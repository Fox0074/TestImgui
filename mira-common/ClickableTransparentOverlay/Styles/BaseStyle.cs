using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ClickableTransparentOverlay.Styles
{
    public class BaseStyle
    {
        public Vector4 MainColor = new Vector4(1, 1, 1, 1);
        public Vector4 MainActiveColor = new Vector4(1, 1, 1, 1);
        public Vector4 MainUnActiveColor = new Vector4(1, 1, 1, 1);
        public Vector4 MainHoveredColor = new Vector4(1, 1, 1, 1);

        public virtual void InitStyle() { }
    }
}
