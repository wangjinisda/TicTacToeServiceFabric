using Microsoft.ServiceFabric.Actors;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GameActor.Interfaces
{
    /// <summary>
    /// Callback interface for TicTacToe. Server -> Client
    /// </summary>
    public interface ITicTacToeEvents : IActorEvents
    {
        /// <summary>
        /// GameStarted event is raised once both the players are connected.
        /// </summary>
        void GameStarted();

        /// <summary>
        /// Moved event is called for every move.
        /// </summary>
        /// <param name="moveMetadata">Move metadata.</param>
        void Moved(PlayerType player, MoveMetadata[][] moveMatrix);

        /// <summary>
        /// GameEnded event is raised when either of the below occurs.
        /// 1. Won
        /// 2. Tie
        /// 3. TimedOut
        /// 4. BailedOutEarly
        /// </summary>
        /// <param name="info"></param>
        void GameEnded(GameEndedInfo info);
    }   
}