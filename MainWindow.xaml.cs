using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace module_21_exercise_2_WPF
{
    public partial class MainWindow : Window
    {      
        //метод заполнения таблицы абонентов телефонной книги
        public void LvRefresh(SubscriberDataApi context)
        {
            phonebook_lv.ItemsSource = context.GetSubscribers().ToObservableCollection();
        }

        //метод проверки заполнения полей
        public bool CheckText()
        {
            bool nameBool = string.IsNullOrWhiteSpace(name_tb.Text);
            bool surnameBool = string.IsNullOrWhiteSpace(surname_tb.Text);
            bool patronymicBool = string.IsNullOrWhiteSpace(patronymic_tb.Text);
            bool phoneNumberBool = string.IsNullOrWhiteSpace(phone_number_tb.Text);
            bool addressBool = string.IsNullOrWhiteSpace(address_tb.Text);
            bool descriptionBool = string.IsNullOrWhiteSpace(description_tb.Text);
            return !(nameBool || surnameBool || patronymicBool || phoneNumberBool || addressBool || descriptionBool);
        }

        //метод очистки полей
        public void ClearText()
        {
            name_tb.Clear();
            surname_tb.Clear();
            patronymic_tb.Clear();
            phone_number_tb.Clear();
            address_tb.Clear();
            description_tb.Clear();
        }
        public MainWindow()
        {
            InitializeComponent();            
            SubscriberDataApi context = new SubscriberDataApi();
            AccountDataApi contextAccount = new AccountDataApi();
                     

            stackPanel.IsEnabled = false;

            login_btn.Visibility = Visibility.Visible;
            logout_btn.Visibility = Visibility.Collapsed;
            reg_btn.Visibility = Visibility.Collapsed;


            //редактирование пользователей
            usersControl_btn.Visibility = Visibility.Collapsed;

            reg_check.IsChecked = false;

            test_log.Text = null;
            user_tb.Text = null;

            //логин
            login_btn.Click += delegate
            {
                string resultLogin = contextAccount.Login(login_tb.Text, password_tb.Text);
                if (resultLogin == "ok")
                {
                    if (login_tb.Text.ToLower() == "admin")
                    {
                        usersControl_btn.Visibility = Visibility.Visible;                        
                    }
                    user_tb.Text = $"текущий пользователь {login_tb.Text}";
                    stackPanel.IsEnabled = true;
                    login_btn.Visibility = Visibility.Collapsed;
                    logout_btn.Visibility = Visibility.Visible;
                    login_tb.Text = null;
                    password_tb.Text = null;
                    test_log.Text = "логин успешен";
                }
                if (resultLogin != "ok")
                {
                    test_log.Text = "логин неуспешен, проверьте данные для ввода";
                }
            };

            //логаут
            logout_btn.Click += delegate
            {
                stackPanel.IsEnabled = false;
                login_btn.Visibility = Visibility.Visible;
                logout_btn.Visibility = Visibility.Collapsed;
                usersControl_btn.Visibility = Visibility.Collapsed;
                test_log.Text = null;
                user_tb.Text = null;
            };

            reg_check.Click += delegate
            {

                if (reg_check.IsChecked == false)
                {
                    login_btn.Visibility = Visibility.Visible;
                    logout_btn.Visibility = Visibility.Collapsed;
                    reg_btn.Visibility = Visibility.Collapsed;
                }

                if (reg_check.IsChecked == true)
                {
                    login_btn.Visibility = Visibility.Collapsed;
                    logout_btn.Visibility = Visibility.Collapsed;
                    reg_btn.Visibility = Visibility.Visible;

                    //регистрация
                    reg_btn.Click += delegate
                    {
                        string resultReg = contextAccount.Registartion(login_tb.Text, password_tb.Text);
                        if (resultReg == "ok")
                        {
                            user_tb.Text = $"зарегистрирован пользователь {login_tb.Text}";
                            login_tb.Text = null;
                            password_tb.Text = null;
                            test_log.Text = "регистрация успешна";

                        }
                        if (resultReg == "fail")
                        {
                            test_log.Text = "регистрация неуспешна, проверьте данные для ввода";
                            MessageBox.Show("Ошибка при создании аккаунта\r\n" +
                                "Пользователь уже существует, либо неверно составлен пароль:\r\n" +
                                "- пароль должен состоять как минимум из 6 символов\r\n" +
                                "- пароль должен содержать как минимум одну цифру от 0 до 9\r\n" +
                                "- пароль должен содержать как минимум одну букву нижнего регистра a-z\r\n" +
                                "- пароль должен содержать как минимум одну букву верхнего регистра A-Z\r\n" +
                                "- пароль должен содержать как минимум один не буквенно-цифровой символ");
                        }
                    };
                }
                test_log.Text = null;
                user_tb.Text = null;
            };

            //вызов окна редактирования пользователей
            usersControl_btn.Click += delegate
            {
                UserControlWindow userControlWindow = new UserControlWindow();
                userControlWindow.Show();
            };
            

            add_btn.IsEnabled = true;
            update_btn.IsEnabled = false;

            LvRefresh(context);

            //начальное состояние флага изменения абонента
            change_subscriber.IsChecked = false;

            //действие при изменении сосотояния флага изменения абонента
            change_subscriber.Click += delegate
            {
                change_condition.Text = change_subscriber.IsChecked.ToString();

                if(change_subscriber.IsChecked == false)
                {
                    phonebook_lv.IsEnabled = true;
                    add_btn.IsEnabled = true;
                    update_btn.IsEnabled = false;
                    ClearText();
                }
                if (change_subscriber.IsChecked == true && phonebook_lv.SelectedItem != null)
                {
                    add_btn.IsEnabled = false;
                    update_btn.IsEnabled = true;
                    Subscriber sub = new Subscriber();
                    sub = phonebook_lv.SelectedItem as Subscriber;
                    name_tb.Text = sub.Name;
                    surname_tb.Text = sub.Surname;
                    patronymic_tb.Text = sub.Patronymic;
                    phone_number_tb.Text = sub.PhoneNumber;
                    address_tb.Text = sub.Address;
                    description_tb.Text = sub.Description;
                    phonebook_lv.IsEnabled = false;
                    delete_btn.IsEnabled = false;
                }
                if (change_subscriber.IsChecked == true && phonebook_lv.SelectedItem == null)
                {
                    MessageBox.Show("Сначала выберите абонента из списка");
                    change_subscriber.IsChecked = false;
                }
            };

            //изменение абонента
            update_btn.Click += delegate
            {
                if (change_subscriber.IsChecked == true && phonebook_lv.SelectedItem != null)
                {
                    Subscriber selectedSub = phonebook_lv.SelectedItem as Subscriber;
                    Subscriber sub = new Subscriber();
                    sub.Id = selectedSub.Id;
                    sub.Name = name_tb.Text;
                    sub.Surname = surname_tb.Text;
                    sub.Patronymic = patronymic_tb.Text;
                    sub.PhoneNumber = phone_number_tb.Text;
                    sub.Address = address_tb.Text;
                    sub.Description = description_tb.Text;                                      
                    context.UpdateSubscriber(sub);
                    LvRefresh(context);
                    ClearText();
                    change_subscriber.IsChecked = false;
                    phonebook_lv.IsEnabled = true;
                    add_btn.IsEnabled = true;
                    delete_btn.IsEnabled = true;
                    update_btn.IsEnabled = false;                    
                }
            };

            //добавить абонента
            add_btn.Click += delegate
            {
                if(change_subscriber.IsChecked == false)
                {
                    if(CheckText())
                    {
                        context.AddSubscriber(new Subscriber()
                        {
                            Name = name_tb.Text,
                            Surname = surname_tb.Text,
                            Patronymic = patronymic_tb.Text,
                            PhoneNumber = phone_number_tb.Text,
                            Address = address_tb.Text,
                            Description = description_tb.Text
                        });                        
                        LvRefresh(context);
                    }
                    if(!CheckText())
                    {
                        MessageBox.Show("Введите все данные абонента");
                    }
                    ClearText();
                }                           
            };

            //удалить абонента
            delete_btn.Click += delegate 
            {
                Subscriber sub = new Subscriber();
                {
                    if(phonebook_lv.SelectedItem != null)
                    {
                        sub = phonebook_lv.SelectedItem as Subscriber;
                        context.DeleteSubscriber(sub.Id);
                        LvRefresh(context);
                    }
                    else
                    {
                        MessageBox.Show("Абонент не выбран");
                    }
                }
            };
        }
    }
}
