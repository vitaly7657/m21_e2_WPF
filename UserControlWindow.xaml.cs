using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using static module_21_exercise_2_WPF.AccountDataApi;
using static module_21_exercise_2_WPF.UserDataApi;

namespace module_21_exercise_2_WPF
{
    public partial class UserControlWindow : Window
    {
        public void LvRefresh(UserDataApi context)
        {
            users_lv.ItemsSource = context.GetUsers().ToObservableCollection();
        }

        public void ClearText()
        {
            userName_tb.Clear();
            password_tb.Clear();

        }
        public bool CheckText()
        {
            bool userNameBool = string.IsNullOrWhiteSpace(userName_tb.Text);
            bool passwordBool = string.IsNullOrWhiteSpace(password_tb.Text);
            
            return !(userNameBool || passwordBool);
        }
        public UserControlWindow()
        {
            InitializeComponent();
            UserDataApi contextUser = new UserDataApi();

            add_btn.IsEnabled = true;
            update_btn.IsEnabled = false;
            password_tb.IsEnabled = true;
            
            LvRefresh(contextUser);
            

            //начальное состояние флага изменения пользователя
            change_user.IsChecked = false;

            //действие при изменении сосотояния флага изменения пользователя
            change_user.Click += delegate
            {
                change_condition.Text = change_user.IsChecked.ToString();

                if (change_user.IsChecked == false)
                {
                    users_lv.IsEnabled = true;
                    add_btn.IsEnabled = true;
                    update_btn.IsEnabled = false;
                    password_tb.IsEnabled = true;
                    ClearText();
                }
                if (change_user.IsChecked == true && users_lv.SelectedItem != null)
                {
                    add_btn.IsEnabled = false;
                    update_btn.IsEnabled = true;
                    password_tb.IsEnabled = false;


                    User user = new User();
                    user = users_lv.SelectedItem as User;

                    userName_tb.Text = user.UserName;
                    users_lv.IsEnabled = false;                    
                    delete_btn.IsEnabled = false;
                }
                if (change_user.IsChecked == true && users_lv.SelectedItem == null)
                {
                    MessageBox.Show("Сначала выберите пользователя из списка");
                    change_user.IsChecked = false;
                }
            };

            //изменение пользователя
            update_btn.Click += delegate
            {
                if (change_user.IsChecked == true && users_lv.SelectedItem != null)
                {
                    User selectedUser = users_lv.SelectedItem as User;

                    User user = new User();
                    user.Id = selectedUser.Id;
                    user.UserName = userName_tb.Text;

                    contextUser.UpdateUser(user);
                    LvRefresh(contextUser);                    
                    ClearText();
                    change_user.IsChecked = false;
                    users_lv.IsEnabled = true;
                    add_btn.IsEnabled = true;
                    delete_btn.IsEnabled = true;
                    update_btn.IsEnabled = false;
                    password_tb.IsEnabled = true;
                }
            };

            //добавить пользователя
            add_btn.Click += delegate
            {
                if (change_user.IsChecked == false)
                {
                    if (CheckText())
                    {
                        string result = contextUser.AddUser(new UserForm()
                        {
                            UserName = userName_tb.Text,
                            Password = password_tb.Text
                        });

                        if (result == "fail")
                        {
                            MessageBox.Show("Ошибка при создании аккаунта\r\n" +
                                "Пользователь уже существует, либо неверно составлен пароль:\r\n" +
                                "- пароль должен состоять как минимум из 6 символов\r\n" +
                                "- пароль должен содержать как минимум одну цифру от 0 до 9\r\n" +
                                "- пароль должен содержать как минимум одну букву нижнего регистра a-z\r\n" +
                                "- пароль должен содержать как минимум одну букву верхнего регистра A-Z\r\n" +
                                "- пароль должен содержать как минимум один не буквенно-цифровой символ");
                        }
                        LvRefresh(contextUser);
                    }
                    if (!CheckText())
                    {
                        MessageBox.Show("Введите все данные пользователя");
                    }
                    ClearText();
                }
            };

            //удалить пользователя
            delete_btn.Click += delegate
            {
                User user = new User();
                {
                    if (users_lv.SelectedItem != null)
                    {
                        user = users_lv.SelectedItem as User;
                        contextUser.DeleteUser(user.Id);
                        LvRefresh(contextUser);
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не выбран");
                    }
                }
            };
        }
    }
}
