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
    /// UpdateGameInfo.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateGameInfo : Window
    {
        static GameInfomation theGame;
        public UpdateGameInfo(GameInfomation _theGame)
        {
            InitializeComponent();
            theGame = _theGame;
        }

        public UpdateGameInfo()
        {
            theGame = null;
            //yearText.Text = ""; monthText.Text = ""; dayText.Text = ""; hourText.Text = ""; minText.Text = ""; hostGoalText.Text = ""; guestGoalText.Text = "";
            //hostCombo.Text = ""; guestCombo.Text = "";
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sqlstr = "select clubName from club;";
            List<string> clubs = MainWindow.coachUser.SqlSelectClub(sqlstr);
            foreach (var club in clubs)
            {
                hostCombo.Items.Add(club);
                guestCombo.Items.Add(club);
            }
            if(theGame!=null)
            {
                hostCombo.Text = theGame.gameHost;
                guestCombo.Text = theGame.gameGuest;
                hostGoalText.Text = theGame.gameHostGoal.ToString();
                guestGoalText.Text = theGame.gameGuestGoal.ToString();
                yearText.Text = theGame.gameTime.ToString("yyyy");
                monthText.Text = theGame.gameTime.ToString("MM");
                dayText.Text = theGame.gameTime.ToString("dd");
                
            }
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            if (hostCombo.Text == "" || guestCombo.Text == "" || yearText.Text == "" || monthText.Text == "" || dayText.Text == "" || hostGoalText.Text == "" || guestGoalText.Text == "")
            {
                MessageBox.Show("请将信息补充完整");
            }
            if (hostCombo.Text == guestCombo.Text)
                MessageBox.Show("主场球队不能和客场球队一样");
            try
            {
                DateTime dt = new DateTime(Convert.ToInt32(yearText.Text), Convert.ToInt32(monthText.Text), Convert.ToInt32(dayText.Text));
                List<string> sqlstrs = new List<string>();
                sqlstrs.Add("lock table game write;");               
                var tempSql = "insert into game(gameHost,gameGuest,gameSchedule,gameHostGoal,gameGuestGoal) values('" + hostCombo.Text + "','" + guestCombo.Text +
                    "'," + dt.ToString("yyyyMMdd")+"," + hostGoalText.Text + "," + guestGoalText.Text + ")";
                sqlstrs.Add(tempSql);
                sqlstrs.Add("unlock tables");
                if (MainWindow.coachUser.GameSql(sqlstrs))
                {
                    MessageBox.Show("添加比赛信息成功");
                }
                else
                    MessageBox.Show("该比赛已存在！");
                yearText.Text = ""; monthText.Text = ""; dayText.Text = ""; hostGoalText.Text = ""; guestGoalText.Text = "";
                hostCombo.Text = ""; guestCombo.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查输入是否有误" );
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (hostCombo.Text == "" || guestCombo.Text == "" || yearText.Text == "" || monthText.Text == "" || dayText.Text == "" ||  hostGoalText.Text == "" || guestGoalText.Text == "")
            {
                MessageBox.Show("请将信息补充完整");
            }
            if (hostCombo.Text == guestCombo.Text)
                MessageBox.Show("主场球队不能和客场球队一样");
            DateTime dt = new DateTime(Convert.ToInt32(yearText.Text), Convert.ToInt32(monthText.Text), Convert.ToInt32(dayText.Text));
            GameInfomation newGame = new GameInfomation(dt, hostCombo.Text, guestCombo.Text, Convert.ToInt32(hostGoalText.Text), Convert.ToInt32(guestGoalText.Text)); ;
            List<string> sqlstrs = new List<string>();
            sqlstrs.Add("lock table game write;");
            sqlstrs.Add("Update game set gameHost='"+newGame.gameHost+"',gameGuest='"+newGame.gameGuest+"',gameSchedule="+dt.ToString("yyyyMMdd")
                +",gameHostGoal="+newGame.gameHostGoal.ToString()+",gameGuestGoal="+newGame.gameGuestGoal.ToString()+" where gameHost='"+theGame.gameHost
                +"' and gameGuest='"+theGame.gameGuest+"' and gameSchedule="+theGame.gameTime.ToString("yyyyMMdd")+";");
            sqlstrs.Add("unlock tables;");
            if(MainWindow.coachUser.GameSql(sqlstrs))
            {
                MessageBox.Show("修改成功！");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (hostCombo.Text == "" || guestCombo.Text == "" || yearText.Text == "" || monthText.Text == "" || dayText.Text == "" ||  hostGoalText.Text == "" || guestGoalText.Text == "")
            {
                MessageBox.Show("请将信息补充完整");
            }
            if (hostCombo.Text == guestCombo.Text)
                MessageBox.Show("主场球队不能和客场球队一样");
            DateTime dt = new DateTime(Convert.ToInt32(yearText.Text), Convert.ToInt32(monthText.Text), Convert.ToInt32(dayText.Text));
            List<string> sqlstrs = new List<string>();
            sqlstrs.Add("lock table game write;");
            sqlstrs.Add("delete from game where gameHost='"+hostCombo.Text+"' and gameGuest='"+guestCombo.Text+"' and gameSchedule="+dt.ToString("yyyyMMdd")+";");
            sqlstrs.Add("unlock tables;");
            if (MainWindow.coachUser.GameSql(sqlstrs))
            {
                MessageBox.Show("删除成功！");
                hostCombo.Text = "";
                guestCombo.Text = "";
                yearText.Text = "";
                monthText.Text = "";
                dayText.Text = "";               
                hostGoalText.Text = "";
                guestGoalText.Text = "";
            }
        }
    }
}
