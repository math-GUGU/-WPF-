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
using MySql.Data.MySqlClient;


namespace primer_league
{
    /// <summary>
    /// 登录窗口，注册时查询名称是否重复，登录时查询是否存在该用户
    /// </summary> 
    public partial class CheckIn : Window
    {     
              
        static public class User
        {
            static public string name;
            static public bool isLogin = false;
            static bool iscoach  = false;
            static bool isnormal = false;
            static public bool isCoach
            {
                get
                {
                    return iscoach;
                }
                set
                {
                    if (isnormal == false && value == true)
                        iscoach = true;
                    else
                        iscoach = false;
                }
            }
            static public bool isNormal
            {
                get
                {
                    return isnormal;
                }
                set
                {
                    if (iscoach == false && value == true)
                        isnormal = true;
                    else
                        isnormal = false;
                }
            }
        }
        static bool isExit = false;
        class Tool
        {
            /// <summary>
            /// 用于校验登录的数据库账户
            /// </summary>
            static readonly string mysqlConnect = "Server=localhost;"          //连接数据库的连接字符串
                      + "database=pl;"                         //数据库名称
                      + "user id=loginuser;"                   //登录数据库的用户名
                      + "password='loginpassword';"                   //数据库的登录密码
                      + "Connect Timeout=5";                    // 数据超时时间; 


            static public bool login(string userId,string userPassword,string status)
            {
                bool flag = false;
                MySqlConnection conn = new MySqlConnection(mysqlConnect);
                try
                {
                    conn.Open();
                    MySqlCommand cmdLogin = new MySqlCommand();
                    cmdLogin.CommandText = "select count(*) from userlist where userName = '" + userId + "' and userpassword = '" + userPassword  + "' and Status = '" + status + "'";
                    cmdLogin.Connection = conn;
                    cmdLogin.CommandType = System.Data.CommandType.Text;
                    int i = Convert.ToInt16(cmdLogin.ExecuteScalar().ToString());
                    if (i == 1)
                    {
                        flag = true;
                    }
                    else
                    {
                        MessageBox.Show("名称或密码错误！");
                        return false;
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
                return flag;
            }
            static public bool Registered(string userName, string userPassword,string status)
            {
                MySqlConnection conn = new MySqlConnection(mysqlConnect);
                try
                {
                    conn.Open();
                    MySqlCommand cmdRegistered = new MySqlCommand();
                    cmdRegistered.CommandText = "select count(*) from userlist where userName = '" + userName + "'";
                    cmdRegistered.Connection = conn;
                    cmdRegistered.CommandType = System.Data.CommandType.Text;
                    int i = Convert.ToInt16(cmdRegistered.ExecuteScalar().ToString());
                    if (i >= 1)
                    {
                        MessageBox.Show("该用户名已被使用，请更改!");
                        return false;
                    }
                    else
                    {                       
                        cmdRegistered.CommandText = "insert into userlist value('"+userName+"','" +userPassword + "','"+status+"')";                                              
                        if(cmdRegistered.ExecuteNonQuery()>0)
                        {
                            MessageBox.Show("注册成功！");
                        }
                        return true;
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
                return true;
            }
        }
        
        public CheckIn(bool _isExit)
        {
            isExit = _isExit;
            InitializeComponent();           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox1.Items.Add("普通用户");
            comboBox1.Items.Add("管理员");

            BitmapImage bitmap = new BitmapImage(new Uri("\\PHOTO\\登陆界面.jpeg",UriKind.Relative));
            backGround.Source = bitmap;
        }

        private void RegisteredButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox1.Text == "管理员")
            {
                User.isNormal = false;
                User.isCoach = true;
            }
            else
            {
                User.isCoach = false;
                User.isNormal = true;
            }
            if (nickNameTextBox.Text != "" && passwordTextBox.Password != "" && comboBox1.Text != "")
            {
                string status = "";
                if (User.isNormal)
                    status = "普通用户";
                else if (User.isCoach)
                    status = "管理员";
                var isRegistered = Tool.Registered(nickNameTextBox.Text, passwordTextBox.Password, status);
                if (isRegistered)
                    LoginButton_Click(sender, e);
            }
            else if (nickNameTextBox.Text == "")
                MessageBox.Show("名称不能为空！");
            else if (passwordTextBox.Password == "")
                MessageBox.Show("密码不能为空！");
            else
                MessageBox.Show("请选择身份！");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox1.Text == "管理员")
            {
                User.isNormal=false;
                User.isCoach = true;
            }
            else 
            {
                User.isCoach = false;
                User.isNormal = true;
            }
            if (nickNameTextBox.Text !="" && passwordTextBox.Password != "")
            {
                if (comboBox1.SelectedItem == null)
                    MessageBox.Show("请选择“普通用户”或“管理员”！");
                else
                {
                    string status = "";
                    if (User.isCoach)
                    {
                        status = "管理员";
                    }
                    else 
                        status = "普通用户";
                    User.isLogin = Tool.login(nickNameTextBox.Text, passwordTextBox.Password,status);
                    if(User.isLogin)
                    {
                        User.name = nickNameTextBox.Text;
                        this.Close();
                    }
                    
                }                
            }
            else if (nickNameTextBox.Text == "")
                MessageBox.Show("名称不能为空！");
            else
                MessageBox.Show("密码不能为空！");

        }
    }
}
