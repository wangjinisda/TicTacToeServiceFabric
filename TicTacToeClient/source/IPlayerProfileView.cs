using GameActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Client.source
{
    internal interface IPlayerProfileView
    {
        PlayerType? PlayerType { get; }

        string GameRoom { get; }

        string PlayerName { get; }
    }
}
