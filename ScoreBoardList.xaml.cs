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
using System.Windows.Shapes;

namespace primer_league
{
    /// <summary>
    /// ScoreBoardList.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreBoardList : Window
    {
        public ScoreBoardList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<ScoreBoard> scoreBoards = new List<ScoreBoard>();
            string scoreboardsql = "select scoreboardNo,clubName,scoreboardWin,scoreboardLose,scoreboardEqual,scoreboardScore from scoreboard " +
               "order by scoreboardNo";
            var tempboard = MainWindow.normalUser.SqlSelectScoreboard(scoreboardsql);
            try
            {
                foreach (var board in tempboard)
                {
                    scoreBoards.Add(board);
                }
                scoreBoardListview.ItemsSource = scoreBoards;
            }
            catch(Exception ex)
            { MessageBox.Show("连接异常，请重试"); }
        }
    }
}
