using Microsoft.ServiceFabric.Actors;

namespace GameActor.Interfaces
{
    /// <summary>
    /// Callback interface for TicTacToe. Server -> Client
    /// </summary>
    public interface ITicTacToeEvents : IActorEvents
    {
        /// <summary>
        /// GameStarted event is called once both the players are connected.
        /// </summary>
        void GameStarted();

        /// <summary>
        /// Moved event is called for every move.
        /// </summary>
        /// <param name="moveMetadata">Move metadata.</param>
        void Moved(MoveMetadata moveMetadata, MoveMetadata[][] moveMatrix);

        /// <summary>
        /// BailedOutEarly event is called to inform the other player that your have aborted.
        /// </summary>
        /// <param name="player">Aborted player.</param>
        void BailedOutEarly(PlayerType player);
    }
}
