using ClientProxyCommon;
using GameActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Client.source
{
    public interface ITicTacToeView
    {
        PlayerType? PlayerChoice { get; set; }
        string GameRoom { get; set; }
        string Player { get; set; }
        ITicTacToeProxy ActorProxy { get; set; }
    }
}
