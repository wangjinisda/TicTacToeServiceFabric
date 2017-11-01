using ClientProxyCommon;
using GameActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe.Client.source.Layer;

namespace TicTacToe.Client.source
{
    internal class PlayerProfilePresenter : IPlayerProfilePresenter
    {
        public event EventHandler<ProfileValidationEventArgs> ProfileValidationError;
        public event EventHandler<EventArgs> ConnectedToGameServer;

        private IPlayerProfileView _view;
        private ITicTacToeView _gameView;
        private ITicTacToeEvents _events;


        public PlayerProfilePresenter(IPlayerProfileView view, ITicTacToeView gameView, ITicTacToeEvents events)
        {
            _view = view;
            _gameView = gameView;
            _events = events;
        }

        public void EndGame()
        {
            //Application.Exit();
        }

        public void StartGame()
        {
            string validationMessage = string.Empty;
            if (!ValidationProfile(ref validationMessage))
            {
                ProfileValidationError(this, new ProfileValidationEventArgs { IsValid = false, ErrorMessage = validationMessage });
                return;
            }

            SetProfile();

            ConnectToGameServer();
        }

        private bool ValidationProfile(ref string validationMessage)
        {
            if (string.IsNullOrWhiteSpace(_view.GameRoom))
            {
                validationMessage = "Please enter an existing or new game room you want to join.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_view.PlayerName))
            {
                validationMessage = "Please enter the player name.";
                return false;
            }

            if (_view.PlayerType == null)
            {
                validationMessage = "Please select a player type.";
                return false;
            }

            return true;
        }

        private void SetProfile()
        {
            _gameView.PlayerChoice = _view.PlayerType;
            _gameView.GameRoom = _view.GameRoom;
            _gameView.Player = _view.PlayerName;
        }

        private void ConnectToGameServer()
        {
            ITicTacToeProxy _actorProxy;

            RetryPolicy.ExponentialRetry(
                async () =>
                {
                    _actorProxy = GetActorProxy();

                    //bool registered = await _actorProxy.RegisterAsync(_view.PlayerType.Value);
                    bool registered = await _actorProxy.RegisterAsync(new PlayerProfileModel
                    {
                        PlayerType = _view.PlayerType.Value,
                        GameRoom = _view.GameRoom,
                        PlayerName = _view.PlayerName
                    });

                    if (!registered)
                    {
                        MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Reached the maximum player limit or the requested player type is not available for selection."),
                           "TicTacToe",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Exclamation);
                        return;
                    }

                    _gameView.ActorProxy = _actorProxy;

                    ConnectedToGameServer(this, EventArgs.Empty);
                },
                (ex) =>
                {
                    var retry = false;
                    // Retry for transient fault
                    System.Fabric.FabricException fabricException = ex as System.Fabric.FabricException;
                    if (fabricException.ErrorCode == System.Fabric.FabricErrorCode.ServiceNotFound)
                    {
                        DialogResult dialogResult = MessageBox.Show("Game server is not available", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        if (dialogResult == DialogResult.Retry)
                        {
                            retry = true;
                        }
                        else
                        {
                            retry = false;
                        }
                    }
                    else
                    {
                        retry = false;
                        MessageBox.Show(ex.Message);
                    }
                    return retry;
                });
        }


        private ITicTacToeProxy GetActorProxy()
        {

            /*
            var gameId = new ActorId(_view.GameRoom);
            var game = ActorProxy.Create<ITicTacToe>(gameId, ConfigurationManager.AppSettings["TicTacToeServer"]);
            game.SubscribeAsync(_events);
            return new LocalTicTacToeProxy(game);
            */

            // for remote test
             return new WebSocketTicTacToeProxy("ws://jingamedev.eastasia.cloudapp.azure.com:8081/ws", _events);
            //return new WebSocketTicTacToeProxy("ws://localhost:8081/ws", _events);
        }
    }
}
