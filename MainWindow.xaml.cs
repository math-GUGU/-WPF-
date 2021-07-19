using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;

namespace primer_league
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly static  NormalUserConnection _normalUser = new NormalUserConnection();
        public static NormalUserConnection normalUser => _normalUser;
        private readonly static CoachUserConnection _coachUser = new CoachUserConnection();
        public static CoachUserConnection coachUser => _coachUser;
        static bool iscoach = false;
       
        public MainWindow()
        {
            CheckIn checkIn = new CheckIn(false);
            checkIn.ShowDialog();
            iscoach = CheckIn.User.isCoach;
            if (CheckIn.User.isLogin == false)
                this.Close();
            InitializeComponent();             
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!iscoach)
            {
                button_3.Visibility = Visibility.Hidden;
                button_4.Visibility = Visibility.Hidden;
            }
            BitmapImage bitmap = new BitmapImage(new Uri("\\PHOTO\\ChNy21rWqceANk1AAAOWhlDDKgM803.jpg", UriKind.Relative));
            backGround.Source = bitmap;
            BitmapImage bt = new BitmapImage(new Uri("\\PHOTO\\切尔西.jpg", UriKind.Relative));
            HeadPortrait.Source = bt;
            textBox_name.Text = CheckIn.User.name;
            if (!CheckIn.User.isCoach)
                textBox_previledge.Visibility = Visibility.Hidden;
            Page1 p1 = new Page1();
            mainMuneFrame.Content = p1;
           
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            Page1 p1 = new Page1();
            mainMuneFrame.Content = p1;
        }

        private void SelectPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            SelectPlayer p1 = new SelectPlayer();
            mainMuneFrame.Content = p1;
        }

        private void SelectGameButton_Click(object sender, RoutedEventArgs e)
        {
            SearchGame p1 = new SearchGame();
            mainMuneFrame.Content = p1;
        }

        private void Button_4_Click(object sender, RoutedEventArgs e)
        {
            UpdateGameInfo p1 = new UpdateGameInfo();
            p1.ShowDialog();
        }

        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            UpdatePlayerInfo p1 = new UpdatePlayerInfo();
            mainMuneFrame.Content = p1;
        }

        private void Button_5_Click(object sender, RoutedEventArgs e)
        {
            球场 sr = new 球场();
            sr.Show();
        }

        private void Button_6_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.premierleague.com/");
        }
    }

    public class NormalUserConnection
    {
        /// <summary>
        /// 用于在普通用户登陆时连接数据库,并完成普通用户的所有连接数据库的操作
        /// </summary>
        readonly string connectionString = "Server=localhost;"                  //Server是服务器地址，如果是本地的数据库可以直接写localhost，远程连接数据库时须在数据库内创建远程用户
                                         + "database=pl;"                     //数据库名称
                                         + "user id=normalUser;"                //登录数据库的用户名
                                         + "password='password';"                 //数据库的登录密码
                                         + "Connect Timeout=5";                 //数据超时时间

        public List<PlayerInfomation> SqlSelectPlayer(string sqlstr)
        {
            //传入要执行的sql语句，返回完整的player对象查询结果
            // PreCmd();       
            List<PlayerInfomation> players = new List<PlayerInfomation>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand sqlcmd = new MySqlCommand();            
            sqlcmd.CommandType = System.Data.CommandType.Text;
            sqlcmd.Connection = conn;
            try
            {
                conn.Open();
                sqlcmd.CommandText = sqlstr;
                var playerDR = sqlcmd.ExecuteReader();
                var playerId = "";
                var playerName = "";
                var playerNation = "";
                DateTime playerBirth = DateTime.Now;
                var playerClub = "";
                var playerPosition = "";
                var playerFoot = "";
                int playerGoal = -1;
                int playerNumber = -1;
                int no = 1;
                if(playerDR.HasRows)
                {
                    while(playerDR.Read())
                    {
                        for(int i=0;i<playerDR.FieldCount;i++)
                        {
                            switch (i%9)
                            {
                                case 0:playerId = playerDR[i].ToString();break;
                                case 1:playerName= playerDR[i].ToString(); break;
                                case 2:playerNation= playerDR[i].ToString(); break;
                                case 3:playerBirth = (DateTime)playerDR[i];break;
                                case 4:playerClub= playerDR[i].ToString(); break;
                                case 5:playerPosition= playerDR[i].ToString(); break;
                                case 6:playerFoot= playerDR[i].ToString(); break;
                                case 7:playerNumber = Convert.ToInt32(playerDR[i].ToString());break;
                                case 8:playerGoal= Convert.ToInt32(playerDR[i].ToString());
                                    players.Add(new PlayerInfomation(playerId,playerName,playerBirth,playerNation,playerGoal,playerClub,playerFoot,playerNumber,playerPosition,no)); no++;
                                       break;
                            }                            
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接异常，请重试！");
            }
            finally
            {
                conn.Close();
            }

            return players;
        }

        public List<GameInfomation> SqlSelectGame(string sqlstr)
        {
            List<GameInfomation> games = new List<GameInfomation>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand selectCmd = new MySqlCommand();
            selectCmd.CommandText = sqlstr;
            selectCmd.CommandType = System.Data.CommandType.Text;
            selectCmd.Connection = conn;
            try
            {
                conn.Open();
                var gameInfos = selectCmd.ExecuteReader();
                string hometeamTemp = "";
                string awayteamTemp = "";
                int hostGoalTemp = -1;
                int guestGoalTemp = -1;
                DateTime gametimeTemp = new DateTime();
                if (gameInfos.HasRows)
                {
                    while (gameInfos.Read())
                    {
                        for (int i = 0; i < gameInfos.FieldCount; i++)
                        {
                            switch (i % 5)
                            {
                                case 0: hometeamTemp = gameInfos[i].ToString(); break;
                                case 1: awayteamTemp = gameInfos[i].ToString(); break;
                                case 2: gametimeTemp = (DateTime)gameInfos[i]; break;
                                case 3: hostGoalTemp = (int)gameInfos[i]; break;
                                case 4: guestGoalTemp = (int)gameInfos[i];
                                    games.Add(new GameInfomation(gametimeTemp, hometeamTemp, awayteamTemp, hostGoalTemp, guestGoalTemp));
                                    break;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
            finally
            {
                conn.Close();
            }           
            return games;
        }

        public List<ScoreBoard> SqlSelectScoreboard(string sqlstr)
        {
            List<ScoreBoard> scoreboards = new List<ScoreBoard>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sqlstr;
            int no = -1, win = -1, lose = -1, equal = -1, score = -1;
            string name = "";
            try
            {
                conn.Open();
                var s = cmd.ExecuteReader();
                if(s.HasRows)
                {
                    while(s.Read())
                    {
                        for(int i=0;i<s.FieldCount;i++)
                        {
                            switch (i % 6)
                            {
                                case 0:no = Convert.ToInt32(s[i]);break;
                                case 1:name = s[i].ToString();break;
                                case 2:win= Convert.ToInt32(s[i]); break;
                                case 3:lose= Convert.ToInt32(s[i]); break;
                                case 4:equal= Convert.ToInt32(s[i]); break;
                                case 5:score= Convert.ToInt32(s[i]);
                                    scoreboards.Add(new ScoreBoard(no, name, win, lose, equal, score));
                                    break; 
                            }                           
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
            finally
            {
                conn.Close();
            }
            return scoreboards;
        }

        public List<Ability> SqlSelectAbility(string sqlstr)
        {
            List<Ability> abilitys = new List<Ability>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = sqlstr;
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;

            string id = "";
            float ability = -1, rate = -1;
            int pac = -1, phy = -1, def = -1, dri = -1, pas = -1, sho = -1;
            try
            {
                conn.Open();
                var a = cmd.ExecuteReader();
                if(a.HasRows)
                {
                    while(a.Read())
                    {
                        for(int i=0;i<a.FieldCount;i++)
                        {
                            switch(i%9)
                            {
                                case 0: id = a[i].ToString();break;
                                case 1: ability = Convert.ToSingle(a[i].ToString()); break;
                                case 2: rate = Convert.ToSingle(a[i].ToString()); break;
                                case 3: pac= Convert.ToInt32(a[i].ToString()); break;
                                case 4: phy= Convert.ToInt32(a[i].ToString()); break;
                                case 5: def= Convert.ToInt32(a[i].ToString()); break;
                                case 6: dri= Convert.ToInt32(a[i].ToString()); break;
                                case 7: pas= Convert.ToInt32(a[i].ToString()); break;
                                case 8: sho= Convert.ToInt32(a[i].ToString());
                                    if(id==""||ability==-1||rate==-1||pac==-1||phy==-1||def==-1||dri==-1||pas==-1||sho==-1)
                                    { }
                                    else
                                    {
                                        abilitys.Add(new Ability(id, ability, rate, pac, phy, def, dri, pas, sho));
                                        id = ""; ability = -1;rate = -1; pac = -1; phy = -1; def = -1; dri = -1; pas = -1; sho = -1;
                                    }
                                    break; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex){ MessageBox.Show("连接异常，请重试"); }
            finally
            {
                conn.Close();
            }
            return abilitys;
        }

    }

    public class CoachUserConnection:NormalUserConnection
    {
        /// <summary>
        /// 用于在普通用户登陆时连接数据库,并完成普通用户的所有连接数据库的操作
        /// </summary>

         readonly string connectionString = "Server=localhost;"                  //Server是服务器地址，如果是本地的数据库可以直接写localhost，远程连接数据库时须在数据库内创建远程用户
                                         + "database=pl;"                     //数据库名称
                                         + "user id=coachUser;"                //登录数据库的用户名
                                         + "password='password';"                 //数据库的登录密码
                                         + "Connect Timeout=5";                 //数据超时时间

        public new List<PlayerInfomation> SqlSelectPlayer(string sqlstr)
        {
            //传入要执行的sql语句，返回完整的player对象查询结果
                 
            List<PlayerInfomation> players = new List<PlayerInfomation>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.CommandType = System.Data.CommandType.Text;
            sqlcmd.Connection = conn;
            try
            {
                conn.Open();
                sqlcmd.CommandText = sqlstr;
                var playerDR = sqlcmd.ExecuteReader();
                var playerId = "";
                var playerName = "";
                var playerNation = "";
                DateTime playerBirth = DateTime.Now;
                var playerClub = "";
                var playerPosition = "";
                var playerFoot = "";
                int playerGoal = -1;
                int playerNumber = -1;
                int no = 1;
                if (playerDR.HasRows)
                {
                    while (playerDR.Read())
                    {
                        for (int i = 0; i < playerDR.FieldCount; i++)
                        {
                            switch (i % 9)
                            {
                                case 0: playerId = playerDR[i].ToString(); break;
                                case 1: playerName = playerDR[i].ToString(); break;
                                case 2: playerNation = playerDR[i].ToString(); break;
                                case 3: playerBirth = Convert.ToDateTime(playerDR[i].ToString()); break;
                                case 4: playerClub = playerDR[i].ToString(); break;
                                case 5: playerPosition = playerDR[i].ToString(); break;
                                case 6: playerFoot = playerDR[i].ToString(); break;
                                case 7: playerNumber = Convert.ToInt32(playerDR[i].ToString()); break;
                                case 8: playerGoal = Convert.ToInt32(playerDR[i].ToString());
                                    players.Add(new PlayerInfomation(playerId, playerName, playerBirth, playerNation, playerGoal, playerClub, playerFoot, playerNumber, playerPosition,no));no++;
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
            finally
            {
                conn.Close();
            }

            return players;
        }

        public new List<GameInfomation> SqlSelectGame(string sqlstr)
        {
            List<GameInfomation> games = new List<GameInfomation>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand selectCmd = new MySqlCommand();
            selectCmd.CommandText = sqlstr;
            selectCmd.CommandType = System.Data.CommandType.Text;
            selectCmd.Connection = conn;
            try
            {
                conn.Open();
                var gameInfos = selectCmd.ExecuteReader();
                string hometeamTemp = "";
                string awayteamTemp = "";
                int hostGoalTemp = -1;
                int guestGoalTemp = -1;
                DateTime gametimeTemp = new DateTime();
                if (gameInfos.HasRows)
                {
                    while (gameInfos.Read())
                    {
                        for (int i = 0; i < gameInfos.FieldCount; i++)
                        {
                            switch (i % 5)
                            {
                                case 0: hometeamTemp = gameInfos[i].ToString(); break;
                                case 1: awayteamTemp = gameInfos[i].ToString(); break;
                                case 2: gametimeTemp = (DateTime)gameInfos[i]; break;
                                case 3: hostGoalTemp = (int)gameInfos[i]; break;
                                case 4:
                                    guestGoalTemp = (int)gameInfos[i];
                                    games.Add(new GameInfomation(gametimeTemp, hometeamTemp, awayteamTemp, hostGoalTemp, guestGoalTemp));
                                    break;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
            finally
            {
                conn.Close();
            }
            return games;
        }

        public new List<ScoreBoard> SqlSelectScoreboard(string sqlstr)
        {
            List<ScoreBoard> scoreboards = new List<ScoreBoard>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sqlstr;
            int no = -1, win = -1, lose = -1, equal = -1, score = -1;
            string name = "";
            try
            {
                conn.Open();
                var s = cmd.ExecuteReader();
                if (s.HasRows)
                {
                    while (s.Read())
                    {
                        for (int i = 0; i < s.FieldCount; i++)
                        {
                            switch (i % 6)
                            {
                                case 0: no = Convert.ToInt32(s[i]); break;
                                case 1: name = s[i].ToString(); break;
                                case 2: win = Convert.ToInt32(s[i]); break;
                                case 3: lose = Convert.ToInt32(s[i]); break;
                                case 4: equal = Convert.ToInt32(s[i]); break;
                                case 5:
                                    score = Convert.ToInt32(s[i]);
                                    scoreboards.Add(new ScoreBoard(no, name, win, lose, equal, score));
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
            finally
            {
                conn.Close();
            }
            return scoreboards;
        }

        public new List<Ability> SqlSelectAbility(string sqlstr)
        {
            List<Ability> abilitys = new List<Ability>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = sqlstr;
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;

            string id = "";
            float ability = -1, rate = -1;
            int pac = -1, phy = -1, def = -1, dri = -1, pas = -1, sho = -1;
            try
            {
                conn.Open();
                var a = cmd.ExecuteReader();
                if (a.HasRows)
                {
                    while (a.Read())
                    {
                        for (int i = 0; i < a.FieldCount; i++)
                        {
                            switch (i % 9)
                            {
                                case 0: id = a[i].ToString(); break;
                                case 1: ability = Convert.ToSingle(a[i].ToString()); break;
                                case 2: rate = Convert.ToSingle(a[i].ToString()); break;
                                case 3: pac = Convert.ToInt32(a[i].ToString()); break;
                                case 4: phy = Convert.ToInt32(a[i].ToString()); break;
                                case 5: def = Convert.ToInt32(a[i].ToString()); break;
                                case 6: dri = Convert.ToInt32(a[i].ToString()); break;
                                case 7: pas = Convert.ToInt32(a[i].ToString()); break;
                                case 8:
                                    sho = Convert.ToInt32(a[i].ToString());
                                    if (id == "" || ability == -1 || rate == -1 || pac == -1 || phy == -1 || def == -1 || dri == -1 || pas == -1 || sho == -1)
                                    { }
                                    else
                                    {
                                        abilitys.Add(new Ability(id, ability, rate, pac, phy, def, dri, pas, sho));
                                        id = ""; ability = -1; rate = -1; pac = -1; phy = -1; def = -1; dri = -1; pas = -1; sho = -1;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("连接异常，请重试"); }
            finally
            {
                conn.Close();
            }
            return abilitys;
        }

        public List<String> SqlSelectClub(string sqlstr)
        {//用于查询单列的字符串类型的数据
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlstr;
            cmd.CommandType = System.Data.CommandType.Text;
            conn.Open();
            var datareader = cmd.ExecuteReader();
            List<string> clubs = new List<string>();
            if (datareader.HasRows)
            {
                while (datareader.Read())
                {
                    for (int i = 0; i < datareader.FieldCount; i++)
                        clubs.Add(datareader[i].ToString());
                }
            }
            datareader.Close();
            conn.Close();
            return clubs;
        }

        public bool isClubCap(string playerID)//判断该球员是否为球队队长,是则返回true
        {
            bool flag = false;
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "use pl;";
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "select count(*) from club where clubCap='" + playerID + "'";
                long a = (long)cmd.ExecuteScalar();
                if (a == 1)
                    flag = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }

        public bool UpdatePlayerSql(PlayerInfomation oldPlayer,PlayerInfomation newPlayer,Ability oldAbility,Ability newAbility,string id)
        {  //修改球员信息函数,oldPlayer为前一次查询所得的球员信息，newPlayer为要插入的新的球员信息,id为搜索时的id，先验证搜索id所得的信息与oldplayer是否一致
            //若一致，则将球员信息信息修改为newplayer和newAbility；当修改的球员为某一球队的队长时须将球队的信息一并修改 
            //因为playerId同时是club表和Ability表的外码，所以当需要修改playerId时不能直接修改球员的playerId，故采取先插入信息的player信息，在修改club和Ability表，最后在删除原有球员信息的方法
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();                
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();
            MySqlTransaction tran = conn.BeginTransaction();
            try
            {
                PlayerInfomation selectplayer = new PlayerInfomation();
                Ability selectability = new Ability();

                cmd.CommandText = "lock tables player write,ability write,club write;";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "select playerId,playerName,playerNation,playerBirth,playerClub,playerPosition,playerFoot,playerNumber,playerGoal from player " +
                    "where playerId = '" + id + "';";
                var dataps = cmd.ExecuteReader();
               
                if (!dataps.HasRows)
                    throw new Exception("球员信息已被修改，请返回重试！");
                string playerId = "", playerName = "", playerNation = "", playerClub = "", playerPosition = "", playerFoot = "";
                int playerGoal = -1, playerNumber = -1;
                DateTime playerBirth = DateTime.MinValue;
                if (dataps.Read())
                {
                    for (int i = 0; i < dataps.FieldCount; i++)
                    {
                        switch (i % 9)
                        {
                            case 0: playerId = dataps[i].ToString(); break;
                            case 1: playerName = dataps[i].ToString(); break;
                            case 2: playerNation = dataps[i].ToString(); break;
                            case 3: playerBirth = (DateTime)dataps[i]; break;
                            case 4: playerClub = dataps[i].ToString(); break;
                            case 5: playerPosition = dataps[i].ToString(); break;
                            case 6: playerFoot = dataps[i].ToString(); break;
                            case 7: playerNumber = Convert.ToInt32(dataps[i].ToString()); break;
                            case 8:
                                playerGoal = Convert.ToInt32(dataps[i].ToString());
                                selectplayer = new PlayerInfomation(playerId, playerName, playerBirth, playerNation, playerGoal, playerClub, playerFoot, playerNumber, playerPosition);
                                break;
                        }
                    }
                }
                dataps.Close();
                cmd.CommandText = "select playerId,playerAbility,playerRate,playerPAC,playerPHY,playerDEF,playerDRI,playerPAS,playerSHO from Ability where playerId = '" + id + "';";
                var dataas = cmd.ExecuteReader();

                float ability = -1, rate = -1;
                int pac = -1, phy = -1, def = -1, dri = -1, pas = -1, sho = -1;
                if (dataas.Read())
                {
                    for (int i = 0; i < dataas.FieldCount; i++)
                    {
                        switch (i % 9)
                        {
                            case 0: id = dataas[i].ToString(); break;
                            case 1: ability = Convert.ToSingle(dataas[i].ToString()); break;
                            case 2: rate = Convert.ToSingle(dataas[i].ToString()); break;
                            case 3: pac = Convert.ToInt32(dataas[i].ToString()); break;
                            case 4: phy = Convert.ToInt32(dataas[i].ToString()); break;
                            case 5: def = Convert.ToInt32(dataas[i].ToString()); break;
                            case 6: dri = Convert.ToInt32(dataas[i].ToString()); break;
                            case 7: pas = Convert.ToInt32(dataas[i].ToString()); break;
                            case 8:
                                sho = Convert.ToInt32(dataas[i].ToString());
                                if (id == "" || ability == -1 || rate == -1 || pac == -1 || phy == -1 || def == -1 || dri == -1 || pas == -1 || sho == -1)
                                { }
                                else
                                {
                                    selectability = new Ability(id, ability, rate, pac, phy, def, dri, pas, sho);
                                }
                                break;
                        }
                    }
                }
                dataas.Close();
                if (oldPlayer != selectplayer || oldAbility != selectability)   //再上一次查询后到进行修改时，这个球员的信息没有被修改，则接下来可以进行数据的修改
                {
                    //修改player信息
                    if (newPlayer.playerId != oldPlayer.playerId)  //但这两项的playerId不同时，先插入新的player，再修改club和ability中playerId为该值的项的player，再删除旧的player
                    {
                        cmd.CommandText = "insert into player (playerId,playerName,playerNation,playerBirth,playerNumber,playerClub,playerPosition,playerFoot,playerGoal)" +
                            "values('" + newPlayer.playerId + "','" + newPlayer.playerName + "','" + newPlayer.playerNation + "','" + newPlayer.playerBirth.ToString("yyyyMMdd") +
                            "'," + newPlayer.playerNumber.ToString() + ",'" + newPlayer.playerClub + "','" + newPlayer.playerPosition + "','" + newPlayer.playerFoot +
                            "'," + newPlayer.playerGoal.ToString() + ");";
                        if ((cmd.ExecuteNonQuery()) != 1)
                            throw new Exception("修改失败！");

                        cmd.CommandText = "update ability set playerId='" + newPlayer.playerId + "',playerAbility=" + newAbility.playerAbility.ToString() +
                            ",playerRate=" + newAbility.playerRate.ToString() + ",playerPAS=" + newAbility.playerPAS.ToString() +
                            ",playerPAC=" + newAbility.playerPAC.ToString() + ",playerDEF=" + newAbility.playerDEF + ",playerDRI=" + newAbility.playerDRI.ToString() +
                            ",playerPHY=" + newAbility.playerPHY.ToString() + ",playerSHO=" + newAbility.playerSHO + " where playerId='" + id + "';";
                        if ((cmd.ExecuteNonQuery()) != 1)
                            throw new Exception("修改失败！");

                        cmd.CommandText = "delete from player where playerId ='" + id + "';";
                        if (cmd.ExecuteNonQuery() != 1)
                            throw new Exception("修改失败");

                    }
                    else
                    {
                        cmd.CommandText = "update player set playerName ='" + newPlayer.playerName + "',playerNation ='" +
                        newPlayer.playerNation + "',playerBirth='" + newPlayer.playerBirth.ToString("yyyyMMdd") + "',playerNumber =" + newPlayer.playerNumber.ToString()
                        + ",playerClub='" + newPlayer.playerClub + "',playerPosition='" + newPlayer.playerPosition + "',playerFoot ='" + newPlayer.playerFoot + "',playerGoal="
                        + newPlayer.playerGoal.ToString() + " where playerId ='" + newPlayer.playerId + "';";
                        int i = cmd.ExecuteNonQuery();  //返回受影响的元组数，应为1
                        if (i != 1)
                            throw new Exception("修改失败！");
                        //修改Ability信息
                        cmd.CommandText = "update ability set playerAbility=" + newAbility.playerAbility.ToString() + ",playerRate=" + newAbility.playerRate.ToString() + "," +
                            "playerPAS=" + newAbility.playerPAS.ToString() + ",playerPAC=" + newAbility.playerPAC.ToString() + ",playerPHY=" + newAbility.playerPHY.ToString()
                            + ",playerDRI=" + newAbility.playerDRI.ToString() + ",playerSHO=" + newAbility.playerSHO.ToString() + ",playerDEF=" + newAbility.playerDEF.ToString()
                            + " where playerId='" + newPlayer.playerId + "';";
                        i = cmd.ExecuteNonQuery();
                        if (i != 1)
                            throw new Exception("修改失败！");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {
                cmd.CommandText = "unlock tables;";
                cmd.ExecuteNonQuery();
                tran.Commit();
                conn.Close();
            }
            return true;

        }     

        public bool DeletePlayerSql(string playerID)//先查询该id所对应的球员是否为球队队长，若是则将该球队队长值置为'空'，若不是，则直接删除
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "use pl;";
            conn.Open();
            MySqlTransaction tran = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.CommandText = "lock tables player write,ability write;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "delete from ability where playerId='" + playerID + "'";
                if (cmd.ExecuteNonQuery() != 1)
                    throw new Exception("删除操作失败！");

                cmd.CommandText = "delete from player where playerId ='" + playerID + "'";
                if (cmd.ExecuteNonQuery() != 1)
                    throw new Exception("删除操作失败！");

                cmd.CommandText = "unlock tables;";
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {
                tran.Commit();
                conn.Close();
            }
            return true;
        }

        public bool InsertPlayerSql(PlayerInfomation player,Ability ability)
        {
            bool flag = false;
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "use pl;";
            conn.Open();
            cmd.ExecuteNonQuery();
            MySqlTransaction tran = conn.BeginTransaction();
            try
            {
                cmd.CommandText = "lock table player write,ability write;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "insert into player(playerId,playerName,playerNation,playerBirth,playerNumber,playerClub,playerPosition,playerFoot,playerGoal) " +
                    "values ('" + player.playerId + "','" + player.playerName + "','" + player.playerNation + "'," + player.playerBirth.ToString("yyyyMMdd") + ","
                    + player.playerNumber.ToString() + ",'" + player.playerClub + "','" + player.playerPosition + "','" + player.playerFoot + "'," + player.playerGoal.ToString() + ");";
                if (cmd.ExecuteNonQuery() != 1)
                    throw new Exception("添加球员失败，请返回重试！");

                cmd.CommandText = "insert into ability(playerId,playerAbility,playerRate,playerPAS,playerPAC,playerPHY,playerDEF,playerDRI,playerSHO)" +
                    "values('" + ability.playerId + "'," + ability.playerAbility.ToString() + "," + ability.playerRate.ToString() + "," + ability.playerPAS.ToString()
                    + "," + ability.playerPAC.ToString() + "," + ability.playerPHY.ToString() + "," + ability.playerDEF.ToString() + "," + ability.playerDRI.ToString() + "," + ability.playerSHO.ToString() + ");";
                if (cmd.ExecuteNonQuery() != 1)
                    throw new Exception("添加球员失败，请返回重试！");

                flag = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                flag = false;
            }
            finally
            {
                cmd.CommandText = "unlock tables;";
                cmd.ExecuteNonQuery();
                tran.Commit();
                conn.Close();
            }
            return flag;
        }

        public bool GameSql(List<string> sqls)
        {
            bool flag = false ;
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            cmd.CommandText = "use pl;";
            conn.Open();
            MySqlTransaction tran = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();                
                foreach(var sqlstr in sqls)
                {
                    cmd.CommandText = sqlstr;
                    cmd.ExecuteNonQuery();
                }
                flag = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接异常，请重试");
            }
            finally
            {
                tran.Commit();
                conn.Close();
            }
            return flag;
        }
    }

    public class GameInfomation
    {
        int _gameHostGoal;               //主场球队得分
        public int gameHostGoal => _gameHostGoal;
        int _gameGuestGoal;              //客场球队得分
        public int gameGuestGoal => _gameGuestGoal;
        string _gameHost;                //主场球队
        public string gameHost => _gameHost;
        string _gameGuest;               //客场球队
        public string gameGuest => _gameGuest;
        DateTime _gameTime;              //比赛时间
        public DateTime gameTime => _gameTime;
        public string gameTimeShow
        {
            get
            {
                return _gameTime.ToString("MM.dd");
            }
        }
        public string gameScore
        {
            get
            {
                return (gameHostGoal.ToString() + " - " + gameGuestGoal.ToString());
            }
        }

        public GameInfomation(DateTime gamedt, string home, string guest, int hostGoal, int guestGoal)
        {
            _gameTime = gamedt;
            _gameHost = home;
            _gameGuest = guest;
            _gameHostGoal = hostGoal;
            _gameGuestGoal = guestGoal;
        }

        public GameInfomation()
        {
        }
    }

    public class PlayerInfomation
    {
        /// <summary>
        /// 在查询球员信息时使用，用于保留在此窗口中需要用到的球员信息
        /// </summary>
        private string _playerId;              //球员ID
        public string playerId
        { get { return _playerId; } set { _playerId = value; } }
        private DateTime _playerBirth ;        //球员生日
        public DateTime playerBirth
        {get{ return _playerBirth; } set { _playerBirth= value; } }
        public string playerBIRTH
        { get { return _playerBirth.ToString("yyyy-MM-dd"); } set { } }
        private string _playerName;            //球员姓名
        public string playerName
        {get{    return _playerName;}set { _playerName= value; } }
        private string _playerNation;        //球员国籍
        public string playerNation
        {get{  return _playerNation;}set { _playerNation= value; } }
        private int _playerNumber;            //球员号码
        public int playerNumber
        { get{ return _playerNumber; }set { _playerNumber= value; } }
        public int playerAge{ get; set; }
        private int _playerGoal;               //球员进球数
        public int playerGoal
        {get{  return _playerGoal; } set { _playerGoal= value; } }
        private string _playerClub;           //所属俱乐部
        public string playerClub
        {get{ return _playerClub; }set { _playerClub= value; } }       
        private string _playerFoot;           //球员惯用脚，属性返回值待定
        public string playerFoot
        {get{  return _playerFoot;} set { _playerFoot= value; } }
        string _playerPosition;               //球员位置,属性返回值待定
        public string playerPosition
        {get {  return _playerPosition; }set { _playerPosition= value; } }
        private int _playerNo;              //球员射手榜排名
        public int playerNo
        {
            get
            {
                return _playerNo;
            }
            set
            {
                _playerNo = value;
            }
        }

        public PlayerInfomation(string id,string name,DateTime birth,string nation,int goal,string club,string foot,int number,string position)
        {
            _playerBirth = birth;
            _playerName = name;
            _playerNation = nation;
            _playerPosition = position;
            _playerFoot = foot;
            _playerGoal = goal;
            _playerClub = club;
            _playerId = id;
            _playerNumber = number;
            playerAge = (Convert.ToInt32(DateTime.Now.ToString("yyyy"))-Convert.ToInt32(birth.ToString("yyyy")));
        }
        public PlayerInfomation(string id, string name, DateTime birth, string nation, int goal, string club, string foot, int number, string position,int No)
        {
            _playerBirth = birth;
            _playerName = name;
            _playerNation = nation;
            _playerPosition = position;
            _playerFoot = foot;
            _playerGoal = goal;
            _playerClub = club;
            _playerId = id;
            _playerNumber = number;
            playerAge = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(birth.ToString("yyyy")));
            playerNo = No;
        }
        public PlayerInfomation()
        {
        }
    }

    public class CoachInfomation
    {
        private string _coachName;      //管理员姓名
        public string coachName
        {
            get
            {
                return _coachName;
            }
        }
        string _coachNation;            //管理员国籍
        public string coachNation
        {
            get
            {
                return _coachNation;
            }
        }
        string _clubName;               //执教球队
        public string clubName
        {
            get
            {
                return _clubName;
            }
        }

        public CoachInfomation(string name,string nation,string club)
        {
            _coachName = name;
            _clubName = club;
            _coachNation = nation;
        }

    }

    public class ClubInfomation
    {
        string _clubName;           //俱乐部名字
        public string clubName => _clubName;
        string _clubAbbreviation;   //俱乐部缩写
        public string clubAbbreviation => _clubAbbreviation;
        string _clubChairman;       //俱乐部主席名
        public string clubChairman => _clubChairman;
        string _coachName;          //俱乐部名字
        public string coachName => _coachName;
        string _clubScoreFied;      //俱乐部主场名
        public string clubScoreFied => _clubScoreFied;
        string _clubCity;           //所在城市
        public string clubCity => _clubCity;
        string _clubQuality;        //球队资格
        public string clubQuality => _clubQuality;
        string _clubCap;            //球队队长
        public string clubCap => _clubCap;

        public ClubInfomation(string quality,string name,string abb,string chair,string coach,string fied,string city,string cap)
        {
            _clubAbbreviation = abb;
            _clubCap = cap;
            _clubChairman = chair;
            _clubCity = city;
            _clubName = name;
            _clubScoreFied = fied;
            _clubQuality = quality;
            _coachName = coach;
        }
    }

    public class Ability
    {
        string _playerId;
        public string playerId
        {
            get
            {
                return _playerId;
            }
            set
            {
                _playerId = value;
            }
        }
        float _playerAbility;
        public float playerAbility
        {
            get
            {
                return _playerAbility;
            }
            set
            {
                _playerAbility = value;
            }
        }
        float _playerRate;
        public float playerRate
        {
            get
            {
                return _playerRate;
            }
            set
            {
                _playerRate = value;
            }
        }
        int _playerPAC;
        public int playerPAC
        {
            get
            {
                return _playerPAC;
            }
            set
            {
                _playerPAC = value;
            }
        }
        int _playerPHY;
        public int playerPHY
        {
            get
            {
                return _playerPHY;
            }
            set
            {
                _playerPHY = value;
            }
        }
        int _playerDEF;
        public int playerDEF
        {
            get
            {
                return _playerDEF;
            }
            set
            {
                _playerDEF = value;
            }
        }
        int _playerDRI;
        public int playerDRI
        {
            get
            {
                return _playerDRI;
            }
            set
            {
                _playerDRI = value;
            }
        }
        int _playerPAS;
        public int playerPAS
        {
            get
            {
                return _playerPAS;
            }
            set
            {
                _playerPAS = value;
            }
        }
        int _playerSHO;
        public int playerSHO
        {
            get
            {
                return _playerSHO;
            }
            set
            {
                _playerSHO = value;
            }
        }

        public Ability(string id,string position,int pac,int phy,int def,int dri,int pas,int sho)  //改为通过公式计算Ability
        {
            _playerId = id;      _playerDEF = def;   _playerDRI = dri;
            _playerPAC = pac;    _playerPAS = pas;   _playerPHY = phy;
            _playerSHO = sho;    _playerRate = def/sho;
            switch(position)
            {
                case "CF": _playerAbility =(float)( 0.4 * sho + 0.4 * dri + 0.2 * pas);break;
                case "CM": _playerAbility = (float)(0.4 * pas + 0.4 * dri + 0.2 * pac); break;
                case "LB": _playerAbility = (float)(0.4 * phy + 0.4 * def + 0.2 * dri); break;
                case "RB": _playerAbility = (float)(0.4 * phy + 0.4 * def + 0.2 * dri); break;
                case "GK": _playerAbility = (float)(0.4 * pac + 0.4 * phy + 0.2 * dri); break;

            }
        }
        public Ability(string id, float rate,float ability, int pac, int phy, int def, int dri, int pas, int sho)  //改为通过公式计算Ability
        {
            _playerId = id;
            _playerAbility = ability;
            _playerRate = rate;
            _playerDEF = def;
            _playerDRI = dri;
            _playerPAC = pac;
            _playerPAS = pas;
            _playerPHY = phy;
            _playerSHO = sho;
        }

        public Ability()
        {
        }
    }

    public class ScoreBoard
    {
        private int _scoreboardNo;      //积分榜排名
        public int scoreboardNo => _scoreboardNo;
        string _scoreboardName;         //球队名
        public string scoreboardName => _scoreboardName;
        int _scoreboardWin;             //球队胜场
        public int scoreboardWin => _scoreboardWin;
        int _scoreboardLose;            //球队负场
        public int scoreboardLose => _scoreboardLose;
        int _scoreboardEqual;           //平场
        public int scoreboardEqual => _scoreboardEqual;
        int _scoreScore;                //积分
        public int scoreScore => _scoreScore;

        public ScoreBoard(int no,string name,int win,int lose,int equal,int score)
        {
            _scoreboardNo = no;
            _scoreboardName = name;
            _scoreboardWin = win;
            _scoreboardLose = lose;
            _scoreboardEqual = equal;
            _scoreScore = score;
        }
    }
}
