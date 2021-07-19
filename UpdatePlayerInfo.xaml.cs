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
using MySql.Data.MySqlClient;

namespace primer_league
{
    /// <summary>
    /// UpdatePlayerInfo.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePlayerInfo : Page
    {
        public UpdatePlayerInfo()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlstr = "select clubName from club;";
                List<string> clubs = MainWindow.coachUser.SqlSelectClub(sqlstr);
                foreach (var club in clubs)
                {
                    clubCombobox.Items.Add(club);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
            clubCombobox.IsHitTestVisible = false;
            clubCombobox.IsReadOnly = true;
            clubCombobox.IsEditable = false;
        }

        static PlayerInfomation oldPlayer;
        static PlayerInfomation newPlayer;
        static Ability oldAbility;
        static Ability newAbility;
        static bool isableToInsert = false;

        private void SelectPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            isableToInsert = false;
            if (idTextbox.Text == "")
                MessageBox.Show("请输入要修改的球员的id");
            else
            {
                //先查询该Id是否有对应的球员存在，调用查询球员的函数若返回的结果为空则Id有误，若不为空则进一步地查询其他球员能力值信息，修改要写成事务              
                string Sqlstr = "select playerId,playerName,playerNation,playerBirth,playerClub,playerPosition,playerFoot,playerNumber,playerGoal " +
                    "from player where playerId = '" + idTextbox.Text + "';";
                
                try
                {
                    List<PlayerInfomation> players = new List<PlayerInfomation>();
                    players = MainWindow.coachUser.SqlSelectPlayer(Sqlstr);
                    if (players == null||players.Count==0)
                        MessageBox.Show("查询失败，请检查输入是否有误！");
                    else
                    {
                        PlayerInfomation player = players.First();
                        playerUpdateGrid.DataContext = player;oldPlayer = player;
                        clubCombobox.Text = player.playerClub;
                        Sqlstr = "select playerId,playerAbility,playerRate,playerPAC,playerPHY,playerDEF,playerDRI,playerPAS,playerSHO from Ability where playerId = '" + idTextbox.Text + "';";
                        List<Ability> abilitys = MainWindow.coachUser.SqlSelectAbility(Sqlstr);
                        if(abilitys==null||abilitys.Count==0)
                        {
                            MessageBox.Show("该球员的数据不完整，请补充完整");
                        }
                        Ability ability = abilitys.First();
                        abilityUpdateGrid.DataContext = ability;oldAbility = ability;
                    }
                }
                catch(Exception ex)
                { MessageBox.Show("连接异常，请重试"); }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //先查询该项是否被其他人修改，即再次查询该id（同时上锁）并将得到的结果与前一次的查询做对比，
            //若两次查询结果一致，则发送修改语句，若不一致则显示“该信息已被修改”，并重新显示信息
            isableToInsert = false;
            try
            {                
                string selectAbbstr = "select clubAbbreviation from club where clubName='" + clubCombobox.SelectedItem.ToString() + "'";
                List<string> tempclubAbb = MainWindow.coachUser.SqlSelectClub(selectAbbstr);
                string id = tempclubAbb.First() + numberTextbox.Text;
                string name = nameTextbox.Text, nation = nationTextbox.Text, position = positionTextbox.Text, club = clubCombobox.SelectedItem.ToString(), foot = footTextbox.Text;
                int goal = Convert.ToInt32(goalTextbox.Text), number = Convert.ToInt16(numberTextbox.Text);
                DateTime birth = (DateTime)birthDate.SelectedDate;
                newPlayer = new PlayerInfomation(id, name, birth, nation, goal, club, foot, number, position);

                int pas = Convert.ToInt16(pasTextbox.Text), pac = Convert.ToInt16(pacTextbox.Text), phy = Convert.ToInt16(phyTextbox.Text);
                int def = Convert.ToInt16(defTextbox.Text), dri = Convert.ToInt16(driTextbox.Text), sho = Convert.ToInt16(shoTextbox.Text);
                newAbility = new Ability(id, position, pac, phy, def, dri, pas, sho);

                if (MainWindow.coachUser.UpdatePlayerSql(oldPlayer, newPlayer, oldAbility, newAbility, idTextbox.Text))
                {
                    MessageBox.Show("修改成功！");
                    oldPlayer = new PlayerInfomation();
                    oldAbility = new Ability();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
        }

        private void DeletePlayerBuuton_Click(object sender, RoutedEventArgs e)
        {//当被删除的球员时队长时，将球队中的clubCap置为'空'；
            isableToInsert = false;
            if (MainWindow.coachUser.isClubCap(idTextbox.Text))
                MessageBox.Show("该球员为球队队长，不能直接删除");
            else 
            {
                if (MainWindow.coachUser.DeletePlayerSql(oldPlayer.playerId))
                {
                    MessageBox.Show("删除成功！");
                    nameTextbox.Text = "";
                    nationTextbox.Text = "";
                    positionTextbox.Text = "";
                    footTextbox.Text = "";
                    numberTextbox.Text = "";
                    goalTextbox.Text = "";
                    clubCombobox.Text = "";
                    birthDate.Text = "";
                    pasTextbox.Text = "";
                    pacTextbox.Text = "";
                    defTextbox.Text = "";
                    driTextbox.Text = "";
                    shoTextbox.Text = "";
                    phyTextbox.Text = "";
                }
                    
            }

        }

        private void InsertPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            isableToInsert = true;
            clubCombobox.IsHitTestVisible = true;
            clubCombobox.IsReadOnly = false;

            nameTextbox.Text = "";
            nationTextbox.Text = "";
            positionTextbox.Text = "";
            footTextbox.Text = "";
            numberTextbox.Text = "";
            goalTextbox.Text = "";
            clubCombobox.Text = "";
            birthDate.Text = "";
            pasTextbox.Text = "";
            pacTextbox.Text = "";
            defTextbox.Text = "";
            driTextbox.Text = "";
            shoTextbox.Text = "";
            phyTextbox.Text = "";
        }

        private void SendInsertPlayerBuuton_Click(object sender, RoutedEventArgs e)
        {
            PlayerInfomation player = new PlayerInfomation();
            if (!isableToInsert)
                return;
            if (nameTextbox.Text == ""|| nationTextbox.Text == ""|| numberTextbox.Text == ""|| positionTextbox.Text == ""||goalTextbox.Text==""||clubCombobox.Text==""||footTextbox.Text=="")
                MessageBox.Show("请将球员信息填写完整！");
            else if (pasTextbox.Text == "" || pacTextbox.Text == "" || phyTextbox.Text == "" || defTextbox.Text == "" || defTextbox.Text == "" || driTextbox.Text == "")
                MessageBox.Show("请将球员能力值信息填写完整");
            else
            {
                try
                {
                    string selectAbbstr = "select clubAbbreviation from club where clubName='" + clubCombobox.SelectedItem.ToString() + "'";
                    List<string> tempclubAbb = MainWindow.coachUser.SqlSelectClub(selectAbbstr);
                    string id = tempclubAbb.First() +"-"+ numberTextbox.Text;
                    string name = nameTextbox.Text, nation = nationTextbox.Text, position = positionTextbox.Text, club = clubCombobox.SelectedItem.ToString(), foot = footTextbox.Text;
                    int goal = Convert.ToInt32(goalTextbox.Text), number = Convert.ToInt16(numberTextbox.Text);
                    DateTime birth = (DateTime)birthDate.SelectedDate;
                    newPlayer = new PlayerInfomation(id, name, birth, nation, goal, club, foot, number, position);

                    int pas = Convert.ToInt16(pasTextbox.Text), pac = Convert.ToInt16(pacTextbox.Text), phy = Convert.ToInt16(phyTextbox.Text);
                    int def = Convert.ToInt16(defTextbox.Text), dri = Convert.ToInt16(driTextbox.Text), sho = Convert.ToInt16(shoTextbox.Text);
                    newAbility = new Ability(id, position, pac, phy, def, dri, pas, sho);
                     
                    if(MainWindow.coachUser.InsertPlayerSql(newPlayer,newAbility))
                    {
                        MessageBox.Show("添加球员成功！");
                        nameTextbox.Text = "";
                        nationTextbox.Text = "";
                        positionTextbox.Text = "";
                        footTextbox.Text = "";
                        numberTextbox.Text = "";
                        goalTextbox.Text = "";
                        clubCombobox.Text = "";
                        birthDate.Text = "";
                        pasTextbox.Text = "";
                        pacTextbox.Text = "";
                        defTextbox.Text = "";
                        driTextbox.Text = "";
                        shoTextbox.Text = "";
                        phyTextbox.Text = "";
                    }
                }
                catch
                {
                    MessageBox.Show("连接异常，请重试");
                }
                finally
                {
                    newPlayer = new PlayerInfomation();
                    newAbility = new Ability();
                }
            }
        }
    }
}
