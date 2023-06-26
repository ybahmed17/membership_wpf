using MemberDesktop.Model;
using MemberDesktop.ViewModel;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MemberDesktop.View
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Page
    {
        MemberViewModel memberViewModel;
        Frame Frame;


        public Search()
        {
            InitializeComponent();
        }

        public Search(Frame frame, MemberViewModel memberViewModel)
        {
            InitializeComponent();
            this.Frame = frame;
            this.memberViewModel = memberViewModel;
            this.DataContext = memberViewModel;
            listViewMembers.ItemsSource = memberViewModel.members;
            this.Loaded += SearchPage_Loaded;
        }

        public ObservableCollection<MemberModel> MemberCollection
        {
            get
            {
                return memberViewModel.members;
            }
            set { memberViewModel.members = value; }
        }

        private void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

            memberViewModel.searchMembers(string.IsNullOrWhiteSpace(txtID.Text) ? null : int.Parse(txtID.Text),
                string.IsNullOrWhiteSpace(txtFamilyID.Text) ? null : int.Parse(txtFamilyID.Text), txtFirstName.Text,
                txtLastName.Text, txtEmail.Text, txtPhone.Text, txtStreet.Text, txtZip.Text);
        }

        private void gridTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Renew_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MemberModel member = button.DataContext as MemberModel;

            Frame.Navigate(new Renew(Frame, memberViewModel, member));

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MemberModel member = button.DataContext as MemberModel;
            Frame.Navigate(new Edit(Frame, memberViewModel, member));
            
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MemberModel member = button.DataContext as MemberModel;
            Frame.Navigate(new Detail(Frame, memberViewModel, member));


        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.NavigationService.GoBack();
        }
    }
}
