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
    /// ShooterList.xaml 的交互逻辑
    /// </summary>
    public partial class ShooterList : Window
    {
        
        public ShooterList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<PlayerInfomation> players = new List<PlayerInfomation>();
            string shooterListsql = "select playerId,playerName,playerNation,playerBirth,playerClub,playerPosition,playerFoot,playerNumber,playerGoal from player " +
                 "order by playerGoal desc;";
            try
            {
                var tempplayers = MainWindow.normalUser.SqlSelectPlayer(shooterListsql);
                foreach (var player in tempplayers)
                {
                    players.Add(player);
                }
                goalBoardListview.ItemsSource = players;
            }
            catch(Exception ex)
            { MessageBox.Show("连接异常，请重试"); }
        }
    }
}
