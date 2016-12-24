using GameActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameId = new ActorId("LoadTest");
            var game = ActorProxy.Create<ITicTacToe>(gameId, "fabric:/TicTacToe");

            game.Register(PlayerType.Cross).GetAwaiter().GetResult();
            game.Register(PlayerType.Zero).GetAwaiter().GetResult();

            game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.First)).GetAwaiter().GetResult();
            game.Move(new MoveMetadata(PlayerType.Zero, CellNumber.Second)).GetAwaiter().GetResult();
            game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.Third)).GetAwaiter().GetResult();
            game.Move(new MoveMetadata(PlayerType.Zero, CellNumber.Forth)).GetAwaiter().GetResult();
            game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.First)).GetAwaiter().GetResult();
            game.Move(new MoveMetadata(PlayerType.Zero, CellNumber.Sixth)).GetAwaiter().GetResult();
            game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.Seventh)).GetAwaiter().GetResult();
            game.Move(new MoveMetadata(PlayerType.Zero, CellNumber.Eighth)).GetAwaiter().GetResult();
            game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.Ninth)).GetAwaiter().GetResult();
            

            game.Unregister(PlayerType.Cross, false).GetAwaiter().GetResult();
            game.Unregister(PlayerType.Zero, false).GetAwaiter().GetResult();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
