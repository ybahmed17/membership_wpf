using MemberDesktop.Model;
using MemberDesktop.ViewModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Page
    {
        MemberViewModel memberViewModel;
        Frame Frame;
        MemberModel _member;
        ObservableCollection<Category> MembershipType;
        ObservableCollection<Category> MembershipStatus;
        ObservableCollection<Category> MembershipReason;
        ObservableCollection<PersonModel> Reviewers;
        ObservableCollection<Category> ContactPreference;
        ObservableCollection<PersonModel> Admins;
        ObservableCollection<Category> Title;
        ObservableCollection<Category> Gender;


        public Edit()
        {
            InitializeComponent();
        }


        public Edit(Frame frame, MemberViewModel memberViewModel, MemberModel member)
        {
            InitializeComponent();
            this.Frame = frame;
            this.memberViewModel = memberViewModel;
            this.DataContext = member;


            _member = member;

            MEMBER.DataContext = _member;
            member.updated_by_db = ushort.Parse(ConfigurationManager.AppSettings["ADMIN"].ToString());

            MembershipType = memberViewModel.membershipType;
            MembershipStatus = memberViewModel.membershipStatus;
            MembershipReason = memberViewModel.membershipReason;
            Reviewers = memberViewModel.reviewers;
            Admins = memberViewModel.admins;
            ContactPreference = memberViewModel.contactPreference;
            Title = memberViewModel.title;
            Gender = memberViewModel.gender;

            ComboTitle.ItemsSource = Title;
            ComboGender.ItemsSource = Gender;
            ComboContactPref.ItemsSource = ContactPreference;
            ComboMemberType.ItemsSource = MembershipType;
            ComboMemberStatus.ItemsSource = MembershipStatus;
            ComboMemberReason.ItemsSource = MembershipReason;
            ComboUpdatedBy.ItemsSource = Admins;
            ComboReviewer1.ItemsSource = Reviewers;
            ComboReviewer2.ItemsSource = Reviewers;



        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                memberViewModel.UpdateMember(_member);
                MessageBox.Show("Member udpdated successfully");
                this.Frame.NavigationService.GoBack();
            }
            catch (Exception ex) { 
                MessageBox.Show("Failed to update member" + "\n" + ex.ToString(),"Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.NavigationService.GoBack();
        }
    }
}
