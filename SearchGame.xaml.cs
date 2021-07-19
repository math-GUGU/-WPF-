using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// SearchGame.xaml 的交互逻辑
    /// </summary>
    public partial class SearchGame : Page
    {
        public SearchGame()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sqlstr = "select clubName from club;";
            List<string> clubs = MainWindow.coachUser.SqlSelectClub(sqlstr);
            foreach (var club in clubs)
                searchGameCombo.Items.Add(club);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchGameCombo.Text== "")
            {
                MessageBox.Show("请选择要查询的球队名称");
                return;
            }
            else
            {               
                string sqlstr1 = "select gameHost,gameGuest,gameSchedule,gameHostGoal,gameGuestGoal from game where ";
                try
                {
                    List<GameInfomation> games = MainWindow.normalUser.SqlSelectGame(sqlstr1 + "gameHost= '" +searchGameCombo.Text+"';"); 
                    List<GameInfomation> games2 = MainWindow.normalUser.SqlSelectGame(sqlstr1 + "gameGuest= '" + searchGameCombo.Text + "';");
                    games.Concat(games2);
                    if (games == null)
                    {
                        MessageBox.Show("不存在满足条件的比赛！");
                    }
                    gameInfoListview.ItemsSource = games;
                    games2.Clear();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("连接异常，请重试");
                }
               
            }
        }

        private void SearchByTimeButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime gameTime = new DateTime();
            try
            {
                gameTime = (DateTime)gameShowDate.SelectedDate;
                string sqlstr = "select gameHost,gameGuest,gameSchedule,gameHostGoal,gameGuestGoal from game where gameSchedule like '%" +
                gameTime.ToString("yyyy-MM-dd") + "%'";
                List<GameInfomation> games = new List<GameInfomation>();               
                games = MainWindow.normalUser.SqlSelectGame(sqlstr);
                if (games == null)
                {
                    MessageBox.Show("不存在满足条件的比赛！");
                }
                gameInfoListview.ItemsSource = games;
            }
            catch{ MessageBox.Show("连接异常，请重试"); }          
            
        }

        private void GameInfoListview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
            if(CheckIn.User.isCoach)
            {
                GameInfomation game = gameInfoListview.SelectedItem as GameInfomation;
                if (game == null)
                    return;
                UpdateGameInfo p1 = new UpdateGameInfo(game);
                p1.ShowDialog();
                
            }
        }

        #region listview 控件点击列头排序方法
        //
        private ListSortDirection _sortDirection;
        private GridViewColumnHeader _sortColumn;
        private void Sort_Click(object sender, RoutedEventArgs e)
        {

            GridViewColumnHeader column = e.OriginalSource as GridViewColumnHeader;
            if (column == null || column.Column == null)
            {
                return;
            }

            if (_sortColumn == column)
            {
                // Toggle sorting direction 
                _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                                   ListSortDirection.Descending :
                                                   ListSortDirection.Ascending;
            }
            else
            {
                // Remove arrow from previously sorted header 
                if (_sortColumn != null && _sortColumn.Column != null)
                {
                    _sortColumn.Column.HeaderTemplate = null;
                    _sortColumn.Column.Width = _sortColumn.ActualWidth - 20;
                }

                _sortColumn = column;
                _sortDirection = ListSortDirection.Ascending;
                column.Column.Width = column.ActualWidth + 20;
            }

            if (_sortDirection == ListSortDirection.Ascending)
            {
                column.Column.HeaderTemplate = Resources["ArrowUp"] as DataTemplate;
            }
            else
            {
                column.Column.HeaderTemplate = Resources["ArrowDown"] as DataTemplate;
            }

            string header = string.Empty;

            // if binding is used and property name doesn't match header content 
            Binding b = _sortColumn.Column.DisplayMemberBinding as Binding;
            if (b != null)
            {
                header = b.Path.Path;
            }

            ICollectionView resultDataView = CollectionViewSource.GetDefaultView(
                                                       (sender as ListView).ItemsSource);
            resultDataView.SortDescriptions.Clear();
            resultDataView.SortDescriptions.Add(
                                        new SortDescription(header, _sortDirection));
        }
        #endregion
    }
}
