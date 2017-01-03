﻿using GameActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class GameActor : Actor, ITicTacToe, IRemindable
    {
        private const string REMINDER_NAME = "ClearGameStateOnTimeout";
        private const int TIMEOUT_INTERVAL = 5;
        private IActorReminder _reminderRegistration;

        /// <summary>
        /// Initializes a new instance of GameActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public GameActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public async Task<bool> Move(MoveMetadata moveMetadata)
        {
            var gameState = await StateManager.GetStateAsync<GameState>("GameState");

            if (moveMetadata.Player != gameState.NextPlayer)
                return await Task.FromResult(false);

            StoreGameState(gameState, moveMetadata);

            var events = GetEvent<ITicTacToeEvents>();
            events.Moved(moveMetadata.Player, gameState.Matrix);

            await StateManager.SetStateAsync("GameState", gameState);
            
            // Check game status
            await CheckStatusAndRaiseEvents(events);

            return await Task.FromResult(true);
        }

        public async Task<bool> Register(PlayerType requestedPlayer)
        {
            var gameState = await StateManager.GetStateAsync<GameState>("GameState");
            var players = gameState.Players;
            
            if (players.Count == 2)
                return await Task.FromResult(false); // TODO: consider returning exception.

            if (players.Contains(requestedPlayer))
                return await Task.FromResult(false); // TODO: consider returning exception.

            players.Add(requestedPlayer);

            if (players.Count == 2)
            {
                var events = GetEvent<ITicTacToeEvents>();
                events.GameStarted();

                // Register reminder

                _reminderRegistration = await RegisterReminderAsync(
                 REMINDER_NAME,
                 BitConverter.GetBytes(0),
                 TimeSpan.FromMinutes(TIMEOUT_INTERVAL),
                 TimeSpan.FromMilliseconds(-1));
            }
            
            await StateManager.SetStateAsync("GameState", gameState);
            
            return await Task.FromResult(true);
        }

        public async Task<bool> Unregister(PlayerType player, bool earlyBailOut)
        {
            var gameState = await StateManager.GetStateAsync<GameState>("GameState");
            var players = gameState.Players;

            bool removed = players.Remove(player);

            // Reset state if all users are exited
            if (players.Count == 0)
            {
                gameState.Matrix = new MoveMetadata[3][]
                                    {
                                        new MoveMetadata[3],
                                        new MoveMetadata[3],
                                        new MoveMetadata[3]
                                    };

                gameState.NextPlayer = PlayerType.Cross;

                await UnregisterReminderAsync(_reminderRegistration);
            }

            if (earlyBailOut)
            {
                var events = GetEvent<ITicTacToeEvents>();
                events.GameEnded(new GameEndedInfo { Player = player, EventType = GameEndedEventType.BailedOutEarly });
            }

            await StateManager.SetStateAsync("GameState", gameState);

            return await Task.FromResult(removed);
        }

        public async Task ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName.Equals(REMINDER_NAME))
            {
                var gameState = await StateManager.GetStateAsync<GameState>("GameState");
                gameState.Matrix = new MoveMetadata[3][]
                            {
                                        new MoveMetadata[3],
                                        new MoveMetadata[3],
                                        new MoveMetadata[3]
                            };
                await StateManager.SetStateAsync("GameState", gameState);

                var events = GetEvent<ITicTacToeEvents>();
                events.GameEnded(new GameEndedInfo { EventType = GameEndedEventType.TimedOut });
            }
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            MoveMetadata[][] _moveMatrix = new MoveMetadata[3][] {          // 3x3 matrix to hold the move data of both the players.
                 new MoveMetadata[3],
                 new MoveMetadata[3],
                 new MoveMetadata[3]
            };   

            return StateManager.TryAddStateAsync("GameState", new GameState
            {
                NextPlayer = PlayerType.Cross,
                Matrix = _moveMatrix,
                Players = new List<PlayerType>()
            });
        }
        
        private async Task CheckStatusAndRaiseEvents(ITicTacToeEvents events)
        {
            var gameStatus = await CheckGameStatus();

            if (gameStatus.Item1 != WinVector.NONE)
            {
                events.GameEnded(
                    new GameEndedInfo
                    {
                        Player = gameStatus.Item3,
                        EventType = GameEndedEventType.Won,
                        WinVector = gameStatus.Item1
                    });
            }
            else if (gameStatus.Item2)
            {
                events.GameEnded(
                    new GameEndedInfo
                    {
                        EventType = GameEndedEventType.Tie
                    });
            }
        }

        private void StoreGameState(GameState gameState, MoveMetadata moveMetadata)
        {
            MoveMetadata[][] moveMatrix = gameState.Matrix;

            switch (moveMetadata.CellNumber)
            {
                case CellNumber.First:
                    moveMatrix[0][ 0] = moveMetadata;
                    break;
                case CellNumber.Second:
                    moveMatrix[0][1] = moveMetadata;
                    break;
                case CellNumber.Third:
                    moveMatrix[0][2] = moveMetadata;
                    break;
                case CellNumber.Forth:
                    moveMatrix[1][0] = moveMetadata;
                    break;
                case CellNumber.Fifth:
                    moveMatrix[1][1] = moveMetadata;
                    break;
                case CellNumber.Sixth:
                    moveMatrix[1][2] = moveMetadata;
                    break;
                case CellNumber.Seventh:
                    moveMatrix[2][0] = moveMetadata;
                    break;
                case CellNumber.Eighth:
                    moveMatrix[2][1] = moveMetadata;
                    break;
                case CellNumber.Ninth:
                    moveMatrix[2][2] = moveMetadata;
                    break;
            }

            gameState.NextPlayer = (moveMetadata.Player == PlayerType.Cross) ? PlayerType.Zero : PlayerType.Cross;
        }

        private async Task<Tuple<WinVector, bool, PlayerType>> CheckGameStatus()
        {
            WinVector winVector = WinVector.NONE;
            PlayerType? player = null;
            bool isDraw = false;

            var gameState = await StateManager.GetStateAsync<GameState>("GameState");
            var matrix = gameState.Matrix;
            var nextPlayer = gameState.NextPlayer;

            player = PlayerType.Cross;
            if (!FindWinner(matrix, ref winVector, player.Value))
            {
                player = PlayerType.Zero;
                if (!FindWinner(matrix, ref winVector, player.Value))
                {
                    if (matrix[0][0] != null && matrix[0][1] != null && matrix[0][2] != null &&
                        matrix[1][0] != null && matrix[1][1] != null && matrix[1][2] != null &&
                        matrix[2][0] != null && matrix[2][1] != null && matrix[2][2] != null)
                    {
                        isDraw = true;
                    }
                }
            }

            return new Tuple<WinVector, bool, PlayerType>(
                winVector, isDraw, player.Value);
        }

        private static bool FindWinner(
            MoveMetadata[][] moveMatrix, 
            ref WinVector winVector, 
            PlayerType player)
        {
            // Horizontal
            for (byte i = 0; i <= 2; i++)
            {
                if ((moveMatrix[i][0] != null && moveMatrix[i][0].Player == player) &&
                    (moveMatrix[i][1] != null && moveMatrix[i][1].Player == player) &&
                    (moveMatrix[i][2] != null && moveMatrix[i][2].Player == player))
                {
                    if (i == 0)
                        winVector = WinVector.TOP;
                    else if (i == 1)
                        winVector = WinVector.CENTER;
                    else if (i == 2)
                        winVector = WinVector.BOTTOM;

                    return true;
                }
            }

            // Vertical
            for (byte i = 0; i <= 2; i++)
            {
                if ((moveMatrix[0][i] != null && moveMatrix[0][i].Player == player) &&
                    (moveMatrix[1][i] != null && moveMatrix[1][i].Player == player) &&
                    (moveMatrix[2][i] != null && moveMatrix[2][i].Player == player))
                {
                    if (i == 0)
                        winVector = WinVector.LEFT;
                    else if (i == 1)
                        winVector = WinVector.MIDDLE;
                    else if (i == 2)
                        winVector = WinVector.RIGHT;

                    return true;
                }
            }
            

            // Diagonal
            if ((moveMatrix[0][0] != null && moveMatrix[0][0].Player == player) &&
                (moveMatrix[1][1] != null && moveMatrix[1][1].Player == player) &&
                (moveMatrix[2][2] != null && moveMatrix[2][2].Player == player))
            {
                winVector = WinVector.BACK_DIAGONAL;
                return true;
            }

            if ((moveMatrix[0][2] != null && moveMatrix[0][2].Player == player) &&
                (moveMatrix[1][1] != null && moveMatrix[1][1].Player == player) &&
                (moveMatrix[2][0] != null && moveMatrix[2][0].Player == player))
            {
                winVector = WinVector.FORWARD_DIAGONAL;
                return true;
            }

            return false;
        }
    }
}
