using GameActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon
{
    public interface ITicTacToeProxy: IDisposable
    {
        /// <summary>
        /// Registers the player with the server players list.
        /// </summary>
        /// <param name="requestedPlayer">Requested player type. Allotment is based on availability.</param>
        Task<bool> RegisterAsync(PlayerProfileModel model);

        /// <summary>
        /// Broadcasts the move metadata to both the clients.
        /// </summary>
        /// <param name="moveMetadata">Move metadata.</param>
        Task<bool> Move(MoveMetadata moveMetadata);

        /// <summary>
        /// Unregisters the player from the server players list.
        /// </summary>
        /// <param name="player">Player type.</param>
        Task<bool> Unregister(PlayerProfileModel model, bool earlyBailOut);

        Task CloseAsync();
    }
}
