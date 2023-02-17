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
            LVService.ItemsSource = DataBaseClass.SEBase.Service.ToList();
            cb_Price.SelectedIndex = 0;
            cb_Filter.SelectedIndex = 0;

            Filter();
        }
        void Filter()
        {
            listFilter = DataBaseClass.SEBase.Service.ToList();
            if (!string.IsNullOrWhiteSpace(tb_Search.Text)) 
            {
                listFilter = listFilter.Where(x => x.Title.ToLower().Contains(tb_Search.Text.ToLower())).ToList(); 
            }
            if (!string.IsNullOrWhiteSpace(tb_SearchDescribe.Text))  
            {
                List<Service> des = listFilter.Where(x => x.Description != null).ToList();
                if (des.Count > 0)
                {
                    listFilter = des.Where(x => x.Description.ToLower().Contains(tb_SearchDescribe.Text.ToLower())).ToList();
                }
                else
                {
                    MessageBox.Show("Нет описания");
                }
            }

            
            switch (cb_Price.SelectedIndex)
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
            switch (cb_Filter.SelectedIndex)
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
            tb_CountZapis.Text = listFilter.Count.ToString() + " из " + DataBaseClass.SEBase.Service.ToList().Count.ToString(); //количество записей

            LVService.ItemsSource = listFilter;
            if (listFilter.Count == 0)
            {
                MessageBox.Show("нет записей");
            }
        }
        private void cb_Price_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cb_Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void tb_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void tb_SearchDescribe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void tb_OldPrice_Loaded(object sender, RoutedEventArgs e)
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

        private void tb_Discount_Loaded(object sender, RoutedEventArgs e)
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

        private void bt_Update_Loaded(object sender, RoutedEventArgs e)
        {
            Button btnUpdate = sender as Button;
            if (code == "0000")
            {
                btnUpdate.Visibility = Visibility.Visible;
            }
            else
            {
                btnUpdate.Visibility = Visibility.Collapsed;
            }
        }

        private void bt_Update_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void bt_Delete_Loaded(object sender, RoutedEventArgs e)
        {
            Button btnDelete = sender as Button;
            if (code == "0000")
            {
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                btnDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void bt_Delete_Click(object sender, RoutedEventArgs e)
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
                        ClassFrame.fr.Navigate(new ListService());
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

        private void bt_Reg_Loaded(object sender, RoutedEventArgs e)
        {
            Button btnReg = sender as Button;
            if (code == "0000")
            {
                btnReg.Visibility = Visibility.Visible;
            }
            else
            {
                btnReg.Visibility = Visibility.Collapsed;
            }
        }

        private void bt_Reg_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.fr.Navigate(new UpcomingEntries());
        }

        private void btn_Admin_Loaded(object sender, RoutedEventArgs e)
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

        private void btn_ExitAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            Button butExitAdmin = sender as Button;
            if (code == "0000")
            {
                butExitAdmin.Visibility = Visibility.Visible;
            }
            else
            {

                butExitAdmin.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            //ClassFrame.fr.Navigate(new AddService());
        }

        private void btn_RegSer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

