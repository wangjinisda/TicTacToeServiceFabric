using GameActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe.Client.source;

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

 
}

