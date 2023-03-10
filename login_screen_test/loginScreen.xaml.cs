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
using System.Data.SQLite;
using System.Diagnostics;


using login_screen_test.Global.SQL;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.Design.Serialization;
using System.DirectoryServices.ActiveDirectory;
using login_screen_test.Global.Datatypes;

namespace login_screen_test
{
    /// <summary>
    /// Interaction logic for loginScreen.xaml
    /// </summary>
    public partial class loginScreen : Window
    {
        static string cs = @"URI=file:main.db";
        public loginScreen()
        {
            InitializeComponent();

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);


            // this will create the users table if it does not currently exists
            // eg if the database was just created
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'users' ('userID' INTEGER  PRIMARY KEY NOT NULL,    'username'  TEXT,    'password'  TEXT,    'salt'  TEXT);";
            cmd.ExecuteNonQuery();

            con.Close();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            string username = username_box.Text;

            Trace.WriteLine("");


            if (!userExists(username)) // if no user exist with this username
            {
                MessageBox.Show("Username not in use");
                return; // return as no user exists so no point checking database more
            }

            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = $"SELECT salt FROM users WHERE username='{username}'"; // get the salt stored for this user
            var reader = cmd.ExecuteReader(); // in the database
            reader.Read(); // this is needed to verify the password of the user as it will be hashed

            string salt = reader[0].ToString();

            string password = Password.HashString(password_box.Password, salt);

            if (userExists(username, password))
            {
                MessageBox.Show("Data correct");
                GameWindow mainWindow = new GameWindow(new Global.Account(username, password));
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Data incorrect");
                this.Close();
            }

        }

        protected bool userExists(string username, string password = null)
        {
            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = $"SELECT userID FROM users WHERE username='{username}'";
            if (password != null)
            {
                cmd.CommandText += $"AND password='{password}'";
                Debug.WriteLine(cmd.CommandText);
            }
            var reader = cmd.ExecuteReader();
            bool exists = false;
            if (reader.HasRows) // there is a userID for this user
            {
                // so it exists
                exists = true;
            }
            reader.Close(); // close the reader
            return exists;
        }

        private void Create_user_button_Click(object sender, RoutedEventArgs e)
        {
            string username = username_box.Text;
            string salt = Password.generateSalt();
            string password = Password.HashString(password_box.Password, Password.generateSalt());

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);


            if (userExists(username))
            {
                return;
            }

            cmd.CommandText = $"INSERT INTO users (username, password, salt) VALUES (\"{username}\", \"{password}\", \"{salt}\")";
            cmd.ExecuteNonQuery();

            MessageBox.Show($"Added User {username}");
            this.Close();
        }
    }
}
