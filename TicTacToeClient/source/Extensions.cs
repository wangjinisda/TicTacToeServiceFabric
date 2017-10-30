using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Client.source
{

    internal static class Extensions
    {
        public static bool Between(this Point clickedPoint, Point first, Point second)
        {
            return ((clickedPoint.X >= first.X && clickedPoint.X <= second.X) &&
                (clickedPoint.Y >= first.Y && clickedPoint.Y <= second.Y));
        }
    }
}
