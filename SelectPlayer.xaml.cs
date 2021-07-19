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
    /// SelectPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class SelectPlayer : Page
    {
        public SelectPlayer()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            searchCombobox.Items.Add("姓名");
            searchCombobox.Items.Add("国籍");
            searchCombobox.Items.Add("所属俱乐部");
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "")
                MessageBox.Show("请输入要搜索的内容");
            else if (searchCombobox.SelectedItem == null)
                MessageBox.Show("请选择要搜索的主题");
            else
            {
                string searchType = "";
                switch (searchCombobox.SelectedItem.ToString())
                {
                    case "姓名":searchType = "playerName";break;
                    case "国籍":searchType = "playerNation";break;
                    case "所属俱乐部":searchType = "playerClub";break;           
                }
                string sqlstr = "select playerId,playerName,playerNation,playerBirth,playerClub,playerPosition,playerFoot,playerNumber,playerGoal from player " +
                    "where " + searchType + " = ";                 
                if(searchType=="playerName"||searchType=="playerClub"||searchType=="playerNation")
                {
                    sqlstr = sqlstr + "'" + searchTextBox.Text + "';";
                }
                try
                {
                    List<PlayerInfomation> players = MainWindow.normalUser.SqlSelectPlayer(sqlstr);
                    playerInfoListview.ItemsSource = players;
                    
                }
                catch { MessageBox.Show("连接异常，请重试"); }              
            }
        }

        private void PlayerInfoListview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PlayerInfomation thePlayer = playerInfoListview.SelectedItem as PlayerInfomation;
            if (thePlayer == null)
                return;
            PlayerInfo p1 = new PlayerInfo(thePlayer);
            p1.ShowDialog();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
