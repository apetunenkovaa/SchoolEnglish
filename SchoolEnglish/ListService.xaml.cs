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

namespace SchoolEnglish
{
    /// <summary>
    /// Логика взаимодействия для ListService.xaml
    /// </summary>
    public partial class ListService : Page
    {
        public static string code;

        List<Service> listFilter;
        public ListService()
        {
            InitializeComponent();
            LService.ItemsSource = DataBaseClass.SEBase.Service.ToList();
            cbPrice.SelectedIndex = 0;
            cbFilter.SelectedIndex = 0;

            Filter();
        }

        void Filter() 
        {
            listFilter = DataBaseClass.SEBase.Service.ToList();
            if (!string.IsNullOrWhiteSpace(tbSearch.Text))  
            {
                listFilter = listFilter.Where(x => x.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList(); 
            }
            if (!string.IsNullOrWhiteSpace(tbSearchDescribe.Text))  
            {
                List<Service> des = listFilter.Where(x => x.Description != null).ToList();
                if (des.Count > 0)
                {
                    listFilter = des.Where(x => x.Description.ToLower().Contains(tbSearchDescribe.Text.ToLower())).ToList();
                }
                else
                {
                    MessageBox.Show("Описание отстутствует");
                }
            }

           
            switch (cbPrice.SelectedIndex)
            {
                case 1:
                    listFilter.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    break;
                case 2:
                    listFilter.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    listFilter.Reverse();
                    break;
            }

            //фильтр
            switch (cbFilter.SelectedIndex)
            {
                case 1:
                    listFilter = listFilter.Where(z => z.Discount >= 0 && z.Discount < 0.05).ToList();
                    break;
                case 2:
                    listFilter = listFilter.Where(z => z.Discount >= 0.05 && z.Discount < 0.15).ToList();
                    break;
                case 3:
                    listFilter = listFilter.Where(z => z.Discount >= 0.15 && z.Discount < 0.30).ToList();
                    break;
                case 4:
                    listFilter = listFilter.Where(z => z.Discount >= 0.30 && z.Discount < 0.70).ToList();
                    break;
                case 5:
                    listFilter = listFilter.Where(z => z.Discount >= 0.70 && z.Discount < 1).ToList();
                    break;
            }
            CountZapis.Text = listFilter.Count.ToString() + " из " + DataBaseClass.SEBase.Service.ToList().Count.ToString(); 

            LService.ItemsSource = listFilter;
            if (listFilter.Count == 0)
            {
                MessageBox.Show("нет записей");
            }
        }

        //private void btnUpdate_Click(object sender, RoutedEventArgs e) 
        //{
        //    Button btn = (Button)sender;
        //    int index = Convert.ToInt32(btn.Uid);
        //    Service service = DataBaseClass.SEBase.Service.FirstOrDefault(z => z.ID == index);
        //    ClassFrame.MainFrame.Navigate(new AddService(service));
        //}

        private void btnDelete_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                Button btn = (Button)sender;
                int index = Convert.ToInt32(btn.Uid);
                if (MessageBox.Show("Вы действительно хотите удалить данную услугу?", "Системное сообщение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (DataBaseClass.SEBase.ClientService.FirstOrDefault(x => x.ServiceID == index) == null)
                    {
                        foreach (ServicePhoto servicePhoto in DataBaseClass.SEBase.ServicePhoto.ToList())
                        {
                            if (servicePhoto.ServiceID == index)
                            {
                                DataBaseClass.SEBase.ServicePhoto.Remove(servicePhoto);
                            }
                        }
                        Service service = DataBaseClass.SEBase.Service.FirstOrDefault(x => x.ID == index);
                        DataBaseClass.SEBase.Service.Remove(service);
                        DataBaseClass.SEBase.SaveChanges();
                        MessageBox.Show("Успешное удаление!");
                        //ClassFrame.MainFrame.Navigate(new Pages.ListOfService());
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно удалить эту услугу!");
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так..");
            }
        }

        private void cbPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void tbSearchDes_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void tbSkidka_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            if (tb.Uid != null)
            {
                tb.Visibility = Visibility.Visible;
            }
            else
            {
                tb.Visibility = Visibility.Collapsed;
            }
        }

        private void tbOldPrice_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            if (tb.Uid != null)
            {
                tb.Visibility = Visibility.Visible;
            }
            else
            {
                tb.Visibility = Visibility.Collapsed;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e) //переход на добавление услуги
        {

            ClassFrame.MainFrame.Navigate(new AddService());
        }

        private void btnUpdate_Loaded(object sender, RoutedEventArgs e)
        {
            Button btnUp = sender as Button;
            if (code == "0000")
            {
                btnUp.Visibility = Visibility.Visible;
            }
            else
            {
                btnUp.Visibility = Visibility.Collapsed;
            }
        }

        private void btnDelete_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (code == "0000")
            {
                btn.Visibility = Visibility.Visible;
            }
            else
            {
                btn.Visibility = Visibility.Collapsed;
            }
        }

        //private void btnZapis_Click(object sender, RoutedEventArgs e) //переход на окно запись на услугу
        //{
        //    Button btn = (Button)sender;
        //    int index = Convert.ToInt32(btn.Uid);
        //    Service service = DataBaseClass.SEBase.Service.FirstOrDefault(z => z.ID == index);

        //    WindowSigningUp windowSigning = new WindowSigningUp(service);
        //    windowSigning.ShowDialog();
        //    ClassFrame.MainFrame.Navigate(new ListService());
        //}

        private void btnZapis_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (code == "0000")
            {
                btn.Visibility = Visibility.Visible;
            }
            else
            {
                btn.Visibility = Visibility.Collapsed;
            }
        }

        private void btnZap_Click(object sender, RoutedEventArgs e) //переход на ближайшие записи
        {
            ClassFrame.MainFrame.Navigate(new UpcomingEntries());
        }

        //private void btnAdmin_Click(object sender, RoutedEventArgs e)
        //{
        //    WindowsAdmin windowsAdmin = new WindowsAdmin();
        //    windowsAdmin.ShowDialog();
        //    ClassFrame.frameL.Navigate(new Pages.ListOfService());
        //}

        private void btnAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            Button btnAdmin = sender as Button;
            if (code == "0000")
            {
                btnAdmin.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnAdmin.Visibility = Visibility.Visible;
            }
        }

        //private void btnExitAdmin_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult k = MessageBox.Show("Вы действительно хотите выйти из режима администратора?", "Системное сообщение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (k == MessageBoxResult.Yes)
        //    {
        //        MessageBox.Show("Режим администратора выключен");
        //        ClassFrame.MainFrame.Navigate(new Pages.ListOfService());
        //        Admin.Visibility = Visibility.Visible;
        //        ExitAdmin.Visibility = Visibility.Collapsed;
        //        Add.Visibility = Visibility.Collapsed;
        //        Zapis.Visibility = Visibility.Collapsed;
        //        code = "00";
        //    }
        //}

        private void btnExitAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (code == "0000")
            {
                button.Visibility = Visibility.Visible;
            }
            else
            {

                button.Visibility = Visibility.Collapsed;
            }
        }
    }
}

