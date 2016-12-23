using GameActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nelson.TicTacToe.Client
{
    internal partial class FrmPlayerChoice : Form
    {
        private ITicTacToe _actorProxy;                                         // Actor proxy.
        private FrmTicTacToe _frmTicTacToe = new FrmTicTacToe();

        public FrmPlayerChoice()
        {
            InitializeComponent();
        }

        private async void btnStartGame_Click(object sender, EventArgs e)
        {
            // Validate fields
            if (string.IsNullOrWhiteSpace(txtGameRoom.Text))
            {
                MessageBox.Show("Please enter an existing or new game room you want to join.", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGameRoom.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPlayer.Text))
            {
                MessageBox.Show("Please enter the player name.", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPlayer.Focus();
                return;
            }

            string playerChoice = cbPlayerChoice.SelectedItem as string;
            if (playerChoice == null)
            {
                MessageBox.Show("Please select a player type.", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbPlayerChoice.Focus();
                return;
            }

            // Store
            if (playerChoice == "Cross")
            {
                _frmTicTacToe.PlayerChoice = PlayerType.Cross;
            }
            else if (playerChoice == "Zero")
            {
                _frmTicTacToe.PlayerChoice = PlayerType.Zero;
            }

            _frmTicTacToe.GameRoom = txtGameRoom.Text;
            _frmTicTacToe.Player = txtPlayer.Text;

            // Register and retry on transient error using exponential backoff algorithm
            const int MAX_RETRIES = 3;
            const int MAX_WAIT_INTERVAL = 60000;
            int retries = 0;
            bool retry = false;

            do
            {
                try
                {
                    int waitTime = Math.Min(GetWaitTimeExp(retries), MAX_WAIT_INTERVAL);

                    // Wait for the result.
                    await Task.Delay(waitTime);

                    _actorProxy = GetActorProxy();
                    _frmTicTacToe.ActorProxy = _actorProxy;

                    // Register Player.
                    bool registered = await _actorProxy.Register(_frmTicTacToe.PlayerChoice.Value);

                    if (registered)
                    {
                        _frmTicTacToe.Registered(_frmTicTacToe.PlayerChoice.Value == PlayerType.Cross);
                    }
                    else
                    {
                        MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Reached the maximum player limit or the requested player type is not available for selection."),
                       "TicTacToe",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Exclamation);

                        Close();
                    }

                    Hide();
                    _frmTicTacToe.Show();
                }
                catch (Exception ex)
                {
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
                            Close();
                        }
                    }
                    else
                    {
                        retry = false;
                        Close();
                    }
                }
            } while (retry && (retries++ < MAX_RETRIES));
        }

        private ITicTacToe GetActorProxy()
        {
            var gameId = new ActorId(txtGameRoom.Text);
            var game = ActorProxy.Create<ITicTacToe>(gameId, "fabric:/TicTacToe");
            game.SubscribeAsync(_frmTicTacToe);
            return game;
        }

        /// <summary>
        /// Returns the next wait interval, in milliseconds, using an exponential backoff algorithm.
        /// </summary>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        private int GetWaitTimeExp(int retryCount)
        {
            int waitTime = (int)(Math.Pow(2, retryCount) * 100);

            return waitTime;
        }

        private void btnEndGame_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
