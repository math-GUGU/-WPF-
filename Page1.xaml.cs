using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace primer_league
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            //查询球员信息，按进球数排序，在进球榜输出前5名球员的信息
            //查询积分榜，按scoreboardNo排序，输出前5项
            //查询比赛game表，找到其中时间最晚的比赛，并依次输出5项已经有了结果的比赛

            List<PlayerInfomation> players = new List<PlayerInfomation>();
            List<ScoreBoard> scoreBoards = new List<ScoreBoard>();

            #region 射手榜部分
            string shooterListsql = "select playerId,playerName,playerNation,playerBirth,playerClub,playerPosition,playerFoot,playerNumber,playerGoal from player "+
                 "order by playerGoal desc;";
            try
            {
                var tempplayers = MainWindow.normalUser.SqlSelectPlayer(shooterListsql);
                int i = 0;
                foreach (var player in tempplayers)
                {
                    players.Add(player);
                    i++;
                    if (i >= 5)
                        break;
                }
                goalBoardListview.ItemsSource = players;
            }
            catch { MessageBox.Show("连接异常，请重试"); }
            #endregion
            
            #region 球队积分榜
            string scoreboardsql = "select scoreboardNo,clubName,scoreboardWin,scoreboardLose,scoreboardEqual,scoreboardScore from scoreboard " +
               "order by scoreboardNo";
            var tempboard = MainWindow.normalUser.SqlSelectScoreboard(scoreboardsql);
            int j = 0;
            try
            {
                foreach (var board in tempboard)
                {
                    scoreBoards.Add(board);
                    j++;
                    if (j >= 5)
                        break;
                }
                scoreBoardListview.ItemsSource = scoreBoards;
            }
            catch { MessageBox.Show("连接异常，请重试"); }
            #endregion

            #region 近期的比赛
            List<GameInfomation> games = new List<GameInfomation>();
            string gameShowSql = "select gameHost,GameGuest,gameSchedule,gameHostGoal,gameGuestGoal from game;";
            try
            {
                games = MainWindow.normalUser.SqlSelectGame(gameShowSql);
                int k = 0;
                GameInfomation[] gamesTemp = new GameInfomation[5];
                foreach (var game in games)
                {
                    if (DateTime.Compare(game.gameTime, DateTime.Now) < 0)
                    {
                        gamesTemp[k] = game;
                        k++;
                        if (k >= 5)
                            break;
                    }
                }
                gameShowGrid_1.DataContext = gamesTemp[0];
                gameShowGrid_2.DataContext = gamesTemp[1];
                gameShowGrid_3.DataContext = gamesTemp[2];
                gameShowGrid_4.DataContext = gamesTemp[3];
                gameShowGrid_5.DataContext = gamesTemp[4];
            }
            catch { MessageBox.Show("连接异常，请重试"); }
            #endregion

            #region 最新的比赛
            var newestGame = "select gameHost,GameGuest,gameSchedule,gameHostGoal,gameGuestGoal from game order by gameSchedule desc;";
            try
            {
                var newGame = MainWindow.normalUser.SqlSelectGame(newestGame);
                newestGameHost.Text = newGame.First().gameHost;
                newestGameGuest.Text = newGame.First().gameGuest;
                newestGameTime.Text = newGame.First().gameTime.ToString("yyyy.MM.dd  hh:mm");
            }
            catch { MessageBox.Show("连接异常，请重试"); }
            #endregion
        }

        private void ScoreboardButton_Click(object sender, RoutedEventArgs e)
        {
            ScoreBoardList p1 = new ScoreBoardList();
            p1.Show();
        }

        private void ShootboardButton_Click(object sender, RoutedEventArgs e)
        {
            ShooterList p1 = new ShooterList();
            p1.Show();
        }
    }
}
