using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace GameActor.Interfaces
{
    /// <summary>
    /// Service Contract for TicTacToe. Client -> Server
    /// </summary>
    public interface ITicTacToe : IActor, IActorEventPublisher<ITicTacToeEvents>
    {
        /// <summary>
        /// Registers the player with the server players list.
        /// </summary>
        /// <param name="requestedPlayer">Requested player type. Allotment is based on availability.</param>
        Task<bool> Register(PlayerType requestedPlayer);

        /// <summary>
        /// Broadcasts the move metadata to both the clients.
        /// </summary>
        /// <param name="moveMetadata">Move metadata.</param>
        Task Move(MoveMetadata moveMetadata);

        /// <summary>
        /// Unregisters the player from the server players list.
        /// </summary>
        /// <param name="player">Player type.</param>
        Task<bool> Unregister(PlayerType player, bool earlyBailOut);

        Task<GameStatus> CheckGameStatus();
    }
}
