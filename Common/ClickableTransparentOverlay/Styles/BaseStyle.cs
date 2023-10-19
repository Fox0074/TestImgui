using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ClickableTransparentOverlay.Styles
{
    internal class BaseStyle
    {
        internal Vector4 MainColor = new Vector4(1, 1, 1, 1);
        internal Vector4 MainActiveColor = new Vector4(1, 1, 1, 1);
        internal Vector4 MainUnActiveColor = new Vector4(1, 1, 1, 1);
        internal Vector4 MainHoveredColor = new Vector4(1, 1, 1, 1);

        internal virtual void InitStyle() { }
    }
}
