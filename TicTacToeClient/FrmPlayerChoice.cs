using GameActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe.Client
{
    internal partial class FrmPlayerChoice : Form, IPlayerProfileView
    {
        private FrmTicTacToe _frmTicTacToe = new FrmTicTacToe();
        private IPlayerProfilePresenter _presentor;

        public PlayerType? PlayerType
        {
            get
            {
                PlayerType player;
                if (Enum.TryParse(cbPlayerChoice.Text, out player))
                {
                    return player;
                }
                else
                {
                    return null;
                }
            }
        }

        public string GameRoom
        {
            get
            {
                return txtGameRoom.Text;
            }
        }

        public string PlayerName
        {
            get
            {
                return txtPlayer.Text;
            }
        }
        
        public FrmPlayerChoice()
        {
            InitializeComponent();
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            _presentor.StartGame();
        }

        private void btnEndGame_Click(object sender, EventArgs e)
        {
            _presentor.EndGame();
        }

        private async void btnLoadTest_Click(object sender, EventArgs e)
        {
            //foreach (int i in System.Linq.Enumerable.Range(0, 1000))
            //{
            //    var gameId = new ActorId("LoadTest" + i);
            //    var game = ActorProxy.Create<ITicTacToe>(gameId, ConfigurationManager.AppSettings["TicTacToeServer"]);

            //    await game.Register(PlayerType.Cross);
            //    await game.Register(PlayerType.Zero);

            //    await game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.First));
            //    await game.Move(new MoveMetadata(PlayerType.Zero, CellNumber.Second));
            //    await game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.Third));
            //    await game.Move(new MoveMetadata(PlayerType.Zero, CellNumber.Forth));
            //    await game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.First));
            //    await game.Move(new MoveMetadata(PlayerType.Zero, CellNumber.Sixth));
            //    await game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.Seventh));
            //    await game.Move(new MoveMetadata(PlayerType.Zero, CellNumber.Eighth));
            //    await game.Move(new MoveMetadata(PlayerType.Cross, CellNumber.Ninth));

            //    await game.Unregister(PlayerType.Cross, false);
            //    await game.Unregister(PlayerType.Zero, false);
            //}
        }

        private void FrmPlayerChoice_Load(object sender, EventArgs e)
        {
            _presentor = new PlayerProfilePresenter(this, _frmTicTacToe, _frmTicTacToe);
            _presentor.ProfileValidationError += _presentor_ProfileValidationError;
            _presentor.ConnectedToGameServer += _presentor_ConnectedToGameServer;
        }

        private void _presentor_ConnectedToGameServer(object sender, EventArgs e)
        {
            Hide();
            _frmTicTacToe.Show();
        }

        private void _presentor_ProfileValidationError(object sender, ProfileValidationEventArgs e)
        {
            if (!e.IsValid)
            {
                MessageBox.Show(e.ErrorMessage, "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    internal interface IPlayerProfilePresenter
    {
        void StartGame();
        void EndGame();

        event EventHandler<ProfileValidationEventArgs> ProfileValidationError;

        event EventHandler<EventArgs> ConnectedToGameServer;
    }

    public class ProfileValidationEventArgs : EventArgs
    {
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; }
    }

    internal interface IPlayerProfileView
    {
        PlayerType? PlayerType { get; }

        string GameRoom { get; }

        string PlayerName { get; }
    }

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
            Application.Exit();
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
            ITicTacToe _actorProxy;

            RetryPolicy.ExponentialRetry(
                async () =>
                {
                    _actorProxy = GetActorProxy();

                    bool registered = await _actorProxy.Register(_view.PlayerType.Value);

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


        private ITicTacToe GetActorProxy()
        {
            var gameId = new ActorId(_view.GameRoom);
            var game = ActorProxy.Create<ITicTacToe>(gameId, ConfigurationManager.AppSettings["TicTacToeServer"]);
            game.SubscribeAsync(_events);
            return game;
        }
    }

    public class RetryPolicy
    {
        /// <summary>
        /// Retry on transient error using exponential backoff algorithm.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionLogic"></param>
        public async static void ExponentialRetry(Action action, Func<Exception, bool> exceptionLogic)
        {
            const int MAX_RETRIES = 3;
            const int MAX_WAIT_INTERVAL = 60000;
            int retries = 0;
            bool retry = false;

            do
            {
                try
                {
                    int waitTime = Math.Min(GetWaitTimeExp(retries), MAX_WAIT_INTERVAL);

                    await Task.Delay(waitTime);

                    action();
                }
                catch (Exception ex)
                {
                    retry = exceptionLogic(ex);
                }
            } while (retry && (retries++ < MAX_RETRIES));
        }

        /// <summary>
        /// Returns the next wait interval, in milliseconds, using an exponential backoff algorithm.
        /// </summary>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        private static int GetWaitTimeExp(int retryCount)
        {
            int waitTime = (int)(Math.Pow(2, retryCount) * 100);

            return waitTime;
        }
    }
}

