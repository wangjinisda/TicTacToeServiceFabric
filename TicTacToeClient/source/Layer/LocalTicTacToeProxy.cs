using ClientProxyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActor.Interfaces;

namespace TicTacToe.Client.source.Layer
{
    public class LocalTicTacToeProxy : ITicTacToeProxy
    {
        private ITicTacToe _ticTacToe;

        public LocalTicTacToeProxy(ITicTacToe ticTacToe)
        {
            _ticTacToe = ticTacToe;
        }

        public void CloseAsync()
        {
            return;
        }

        public void Dispose()
        {
            return;
        }

        public Task<bool> Move(MoveMetadata moveMetadata)
        {
            return _ticTacToe.Move(moveMetadata);
        }

        public Task<bool> RegisterAsync(PlayerProfileModel model)
        {
            return _ticTacToe.Register(model);
        }

        public Task<bool> Unregister(PlayerProfileModel model, bool earlyBailOut)
        {
            return _ticTacToe.Unregister(model, earlyBailOut);
        }
    }
}
