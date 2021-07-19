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
    /// PlayerInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerInfo : Window
    {
        
        readonly PlayerInfomation player;
        public PlayerInfo(PlayerInfomation _player)
        {
            InitializeComponent();
            player = _player;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           BitmapImage bitmap=new BitmapImage(new Uri("\\PHOTO\\球员能力值背景.jpeg", UriKind.Relative));
           backGround.Source = bitmap;  
            
           bigNameTextBox.Text = player.playerName;
           try
           {
               playerShowGrid.DataContext = player;
               string sqlstr = "select playerId,playerAbility,playerRate,playerPAC,playerPHY,playerDEF,playerDRI,playerPAS,playerSHO from Ability where playerId = '" + player.playerId + "';";
               List< Ability> ability = MainWindow.normalUser.SqlSelectAbility(sqlstr);
               if (ability == null||ability.Count==0)
               {
                   throw new Exception("查询球员能力值失败，请返回重试！");
               }
               AbilityShowGrid.DataContext = ability;
               string clubAbb = "\\PHOTO\\球队logo\\";

                switch (player.playerClub)
                {
                    case "阿森纳":clubAbb += "阿森纳.jpeg";break;
                    case "阿斯顿维拉":clubAbb += "阿斯顿维拉.jpg"; break;
                    case "埃弗顿": clubAbb += "埃弗顿.jpg"; break;
                    case "伯恩利": clubAbb += "伯恩利.jpg"; break;
                    case "伯恩茅斯": clubAbb += "伯恩茅斯.png"; break;
                    case "布莱顿": clubAbb += "布莱顿.jpg"; break;
                    case "莱斯特城": clubAbb += "莱斯特城.jpg"; break;
                    case "狼队": clubAbb += "狼队.png"; break;
                    case "利物浦": clubAbb += "利物浦.jpeg"; break;
                    case "曼城": clubAbb += "曼城.jpg"; break;
                    case "曼联": clubAbb += "曼联.jpg"; break;
                    case "南安普顿": clubAbb += "南安普顿.jpg"; break;
                    case "纽卡斯尔": clubAbb += "纽卡斯尔.jpg"; break;
                    case "诺维奇": clubAbb += "诺维奇.jpg"; break;
                    case "切尔西": clubAbb += "切尔西.jpg"; break;
                    case "水晶宫": clubAbb += "水晶宫.jpg"; break;
                    case "托特纳姆热刺": clubAbb += "托特纳姆热刺.jpg"; break;
                    case "沃特福德": clubAbb += "沃特福德.jpg"; break;
                    case "西汉姆联": clubAbb += "西汉姆联.jpg"; break;
                    case "谢菲尔德联": clubAbb += "谢菲尔德联.jpeg"; break;
                }
                BitmapImage bm = new BitmapImage(new Uri(clubAbb, UriKind.Relative));
                logoImage.Source = bm;
     

            }
            catch { MessageBox.Show("连接异常，请重试"); }
        }
    }
}
