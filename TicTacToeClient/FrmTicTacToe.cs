﻿using ClientProxyCommon;
using GameActor.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe.Client.source;

namespace TicTacToe.Client
{
    internal partial class FrmTicTacToe : Form, ITicTacToeEvents, ITicTacToeView
    {
        private System.Threading.SynchronizationContext _synchronizationContext;// Synchronization context for UI thread
        private WinVector _winVector = new WinVector();                         // Holds instructions for painting on the screen.
        private MoveMetadata[][] _moveMatrix = new MoveMetadata[3][] {          // 3x3 matrix to hold the move data of both the players.
             new MoveMetadata[3],
             new MoveMetadata[3],
             new MoveMetadata[3]
        };         
        private Graphics _graphics;                                             // Graphics instance for painting.
        private bool _closeSilently;                                            // Indicates whether to close the form silently.
        private bool _isYourTurn;                                               // Indicates whether it is your turn.
        private bool _gameStated;                                               // Indicates whether both the players has joined.

        #region Constructor

        public FrmTicTacToe()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void frmTicTacToe_Load(object sender, System.EventArgs e)
        {
            InitializeGraphics();

            if (!_gameStated)
                toolStripStatus.Text = "Other player has not joined yet.";

            //System.Threading.SynchronizationContext.SetSynchronizationContext(System.Threading.SynchronizationContext.Current);
            _synchronizationContext = System.Threading.SynchronizationContext.Current;

            // Set window title
            Text = string.Format(CultureInfo.InvariantCulture, "TicTacToe - {0} - {1}", PlayerChoice.Value.ToString(), Player);            
        }

        private void frmTicTacToe_Paint(object sender, PaintEventArgs e)
        {
            PaintBorders();

            PaintSymbols();

            PaintWinVectors();
        }

        private async void frmTicTacToe_Click(object sender, EventArgs e)
        {
            if (!_gameStated)
            {
                MessageBox.Show("Plese wait for other player to join",
                                "TicTacToe", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                return;
            }

            if (_isYourTurn)
            {
                MouseEventArgs eargs = e as MouseEventArgs;
                Point clickedPoint = new Point(eargs.X, eargs.Y);

                if (!HasValueInCell(clickedPoint))
                {
                    MoveMetadata moveMetadata = new MoveMetadata(PlayerChoice.Value, GetCellNumber(clickedPoint));
                    moveMetadata.PlayerProfileModel = new PlayerProfileModel
                    {
                        PlayerType = PlayerChoice.Value,
                        GameRoom = GameRoom,
                        PlayerName = Player

                    };

                    await ActorProxy.Move(moveMetadata);
                }
                else
                {
                    MessageBox.Show("Click on a blank region.",
                                    "TicTacToe",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Not your turn.",
                                "TicTacToe",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }
        
        private void frmTicTacToe_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_closeSilently)
            {
                DialogResult result = MessageBox.Show("Do you wish to quit the game?",
                                                        "TicTacToe", 
                                                        MessageBoxButtons.YesNo, 
                                                        MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    //ActorProxy.Unregister(PlayerChoice.Value, true);

                    ActorProxy.Unregister(new PlayerProfileModel
                    {
                        PlayerType = PlayerChoice.Value,
                        GameRoom = GameRoom,
                        PlayerName = Player

                    }, true);
                }
            }
        }

        private void frmTicTacToe_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Since frmPlayerChoice is hidden.
            ActorProxy.CloseAsync()
                .ContinueWith(_ => Application.Exit())
                .ConfigureAwait(false);   
        }

        #endregion

        #region Members

        #region Graphics

        private void InitializeGraphics()
        {
            _graphics = CreateGraphics();
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        private void PaintWinVectors()
        {
            // Paint Win Vectors.
            Pen greenPen = new Pen(Color.Green, 4F);
            switch (_winVector)
            {
                case WinVector.TOP:
                    _graphics.DrawLine(greenPen, new Point(0, 50), new Point(300, 50));
                    break;
                case WinVector.CENTER:
                    _graphics.DrawLine(greenPen, new Point(0, 150), new Point(300, 150));
                    break;
                case WinVector.BOTTOM:
                    _graphics.DrawLine(greenPen, new Point(0, 250), new Point(300, 250));
                    break;
                case WinVector.LEFT:
                    _graphics.DrawLine(greenPen, new Point(50, 0), new Point(50, 300));
                    break;
                case WinVector.MIDDLE:
                    _graphics.DrawLine(greenPen, new Point(150, 0), new Point(150, 300));
                    break;
                case WinVector.RIGHT:
                    _graphics.DrawLine(greenPen, new Point(250, 0), new Point(250, 300));
                    break;
                case WinVector.BACK_DIAGONAL:
                    _graphics.DrawLine(greenPen, new Point(0, 0), new Point(300, 300));
                    break;
                case WinVector.FORWARD_DIAGONAL:
                    _graphics.DrawLine(greenPen, new Point(0, 300), new Point(300, 0));
                    break;
                default:
                    break;
            }
        }

        private void PaintSymbols()
        {
            // Horizontal
            for (byte c = 0; c <= 2; c++)
            {
                // Vertical
                for (byte r = 0; r <= 2; r++)
                {
                    MoveMetadata moveMetadata = _moveMatrix[c][r];

                    if (moveMetadata != null)
                    {
                        switch (moveMetadata.CellNumber)
                        {
                            case CellNumber.First:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                            case CellNumber.Second:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                            case CellNumber.Third:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                            case CellNumber.Forth:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                            case CellNumber.Fifth:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                            case CellNumber.Sixth:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                            case CellNumber.Seventh:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                            case CellNumber.Eighth:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                            case CellNumber.Ninth:
                                PaintSymbol(moveMetadata, GetPointToPaint(moveMetadata.CellNumber));
                                break;
                        }
                    }
                }
            }
        }

        private void PaintSymbol(MoveMetadata moveMetadata, Point coordinate)
        {
            _graphics.DrawString((moveMetadata.Player == PlayerType.Zero ? "0" : "X"),
                                    new Font("Arial", 30F),
                                    new SolidBrush((moveMetadata.Player == PlayerType.Zero ? Color.Blue : Color.Red)),
                                    coordinate.X,
                                    coordinate.Y);
        }

        private void PaintBorders()
        {
            Pen grayPen = new Pen(Color.Gray, 1F);
            _graphics.DrawLine(grayPen, new Point(100, 0), new Point(100, 300));
            _graphics.DrawLine(grayPen, new Point(200, 0), new Point(200, 300));
            _graphics.DrawLine(grayPen, new Point(0, 100), new Point(300, 100));
            _graphics.DrawLine(grayPen, new Point(0, 200), new Point(300, 200));
        }

        /// <summary>
        /// 1, 2, 3
        /// 4, 5, 6
        /// 7, 8, 9
        /// </summary>
        /// <param name="clickedPoint"></param>
        /// <returns></returns>
        private static Point GetPointToPaint(CellNumber cellNumber)
        {
            Point pointToPaint = Point.Empty;
            int number = 20;

            // Find the center of the cell.
            switch (cellNumber)
            {
                case CellNumber.First:
                    pointToPaint = new Point(50, 50);
                    break;
                case CellNumber.Second:
                    pointToPaint = new Point(150, 50);
                    break;
                case CellNumber.Third:
                    pointToPaint = new Point(250, 50);
                    break;
                case CellNumber.Forth:
                    pointToPaint = new Point(50, 150);
                    break;
                case CellNumber.Fifth:
                    pointToPaint = new Point(150, 150);
                    break;
                case CellNumber.Sixth:
                    pointToPaint = new Point(250, 150);
                    break;
                case CellNumber.Seventh:
                    pointToPaint = new Point(50, 250);
                    break;
                case CellNumber.Eighth:
                    pointToPaint = new Point(150, 250);
                    break;
                case CellNumber.Ninth:
                    pointToPaint = new Point(250, 250);
                    break;
            }

            // Subtract to position correctly.
            pointToPaint = Point.Subtract(pointToPaint, new Size(number, number));

            return pointToPaint;
        }

        #endregion
                      
        private bool HasValueInCell(Point clickedPoint)
        {
            MoveMetadata hasValueInCell = null;
            CellNumber cellNumber = GetCellNumber(clickedPoint);

            switch (cellNumber)
            {
                case CellNumber.First:
                    hasValueInCell = _moveMatrix[0][0];
                    break;
                case CellNumber.Second:
                    hasValueInCell = _moveMatrix[0][1];
                    break;
                case CellNumber.Third:
                    hasValueInCell = _moveMatrix[0][2];
                    break;
                case CellNumber.Forth:
                    hasValueInCell = _moveMatrix[1][0];
                    break;
                case CellNumber.Fifth:
                    hasValueInCell = _moveMatrix[1][1];
                    break;
                case CellNumber.Sixth:
                    hasValueInCell = _moveMatrix[1][2];
                    break;
                case CellNumber.Seventh:
                    hasValueInCell = _moveMatrix[2][0];
                    break;
                case CellNumber.Eighth:
                    hasValueInCell = _moveMatrix[2][1];
                    break;
                case CellNumber.Ninth:
                    hasValueInCell = _moveMatrix[2][2];
                    break;
                case CellNumber.None:
                    break;
            }

            return (hasValueInCell != null);
        }

        private static CellNumber GetCellNumber(Point clickedPoint)
        {
            CellNumber cellNumber = CellNumber.None;

            if (clickedPoint.Between(new Point(0, 0), new Point(100, 100)))
            {
                cellNumber = CellNumber.First;
            }

            if (clickedPoint.Between(new Point(100, 0), new Point(200, 100)))
            {
                cellNumber = CellNumber.Second;
            }

            if (clickedPoint.Between(new Point(200, 0), new Point(300, 100)))
            {
                cellNumber = CellNumber.Third;
            }

            if (clickedPoint.Between(new Point(0, 100), new Point(100, 200)))
            {
                cellNumber = CellNumber.Forth;
            }

            if (clickedPoint.Between(new Point(100, 100), new Point(200, 200)))
            {
                cellNumber = CellNumber.Fifth;
            }

            if (clickedPoint.Between(new Point(200, 100), new Point(300, 200)))
            {
                cellNumber = CellNumber.Sixth;
            }

            if (clickedPoint.Between(new Point(0, 200), new Point(100, 300)))
            {
                cellNumber = CellNumber.Seventh;
            }

            if (clickedPoint.Between(new Point(100, 200), new Point(200, 300)))
            {
                cellNumber = CellNumber.Eighth;
            }

            if (clickedPoint.Between(new Point(200, 200), new Point(300, 300)))
            {
                cellNumber = CellNumber.Ninth;
            }
             
            return cellNumber;
        }

        #endregion

        #region ITicTacToeCallback Members

        public void GameStarted()
        {
             _gameStated = true;

            if (PlayerChoice.Value == PlayerType.Cross)
            {
                _isYourTurn = true;
                toolStripStatus.Text = "Your turn";
            }
            else
            {
                _isYourTurn = false;
                toolStripStatus.Text = "Not your turn";
            }
            //return Task.CompletedTask;
        }

        public void Moved(PlayerType player, MoveMetadata[][] moveMatrix)
        {
            _moveMatrix = moveMatrix;

            Invalidate();

            _isYourTurn = (PlayerChoice.Value == player) ? false : true;

            toolStripStatus.Text = _isYourTurn ? "Your turn" : "Not your turn";
           // return Task.CompletedTask;
        }

        public void GameEnded(GameEndedInfo info)
        {
            switch (info.EventType)
            {
                case GameEndedEventType.Won:
                    _winVector = info.WinVector;
                    Invalidate();

                    GameEndedCommon(info.Player, "{0} has won the game !");
                    break;
                case GameEndedEventType.Tie:
                    GameEndedCommon(info.Player, "It's a tie !");
                    break;
                case GameEndedEventType.BailedOutEarly:
                    GameEndedCommon(info.Player, "{0} has end the game.");
                    break;
                case GameEndedEventType.TimedOut:
                    GameEndedCommon(info.Player, "{0} has end the game.");
                    break;
            }

            //return Task.CompletedTask;
        }
        private void GameEndedCommon(PlayerType player, string message)
        {
            MessageBox.Show(string.Format(CultureInfo.InvariantCulture, message, player.ToString()),
                "TicTacToe",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            //ActorProxy.Unregister(PlayerChoice.Value, true);

            ActorProxy.Unregister(new PlayerProfileModel
            {
                PlayerType = PlayerChoice.Value,
                GameRoom = GameRoom,
                PlayerName = Player

            }, true);

            _closeSilently = true;

            // Close Window from UI thread
            //System.Threading.SynchronizationContext.Current.Send((state) =>
            _synchronizationContext.Send((state) =>
            {
                Close();
            }, null);

        }

        #endregion

        #region Properties

        public PlayerType? PlayerChoice { get;  set; }

        public string GameRoom { get;  set; }

        public string Player { get;  set; }

        public ITicTacToeProxy ActorProxy { get;  set; }

        #endregion
    }

}
