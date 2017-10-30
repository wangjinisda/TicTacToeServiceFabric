using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Client.source
{
    internal interface IPlayerProfilePresenter
    {
        void StartGame();

        void EndGame();

        event EventHandler<ProfileValidationEventArgs> ProfileValidationError;

        event EventHandler<EventArgs> ConnectedToGameServer;
    }
}
