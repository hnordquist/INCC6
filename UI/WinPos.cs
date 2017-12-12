using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    public class WinPos
    {
        public double height, width, top, left;
        public double maxh = System.Windows.SystemParameters.VirtualScreenHeight - 100;
        public double maxw = System.Windows.SystemParameters.VirtualScreenWidth - 100;
        public WinPos()
        {
            width = 1200;
            height = 800;
            top = 100;
            left = 100;
        }
        public WinPos GetChildPos(double h, double w)
        {
            WinPos child = new WinPos();
            child.height = h < maxh ? h : maxh;
            child.top = this.top + 200;
            child.width = w < maxw ? w : maxw;
            //move to right of main window if won't go off the screen
            child.left = this.left + w < maxw ? this.left + w : this.left + 200;
            return child;
        }
        public void SizeToFit()
        {
            if (height > .8 * System.Windows.SystemParameters.VirtualScreenHeight)
            {
                height = .8 * System.Windows.SystemParameters.VirtualScreenHeight;
            }

            if (width > .8 * System.Windows.SystemParameters.VirtualScreenWidth)
            {
                width = .8 * System.Windows.SystemParameters.VirtualScreenWidth;
            }
        }

        public void MoveIntoView()
        {
            if (top + height / 2 > System.Windows.SystemParameters.VirtualScreenHeight)
            {
                top = System.Windows.SystemParameters.VirtualScreenHeight - height;
            }

            if (left + width / 2 > System.Windows.SystemParameters.VirtualScreenWidth)
            {
                left = System.Windows.SystemParameters.VirtualScreenWidth - width;
            }

            if (top < 0)
            {
                top = 0;
            }

            if (left < 0)
            {
                left = 0;
            }
        }
    }
}
