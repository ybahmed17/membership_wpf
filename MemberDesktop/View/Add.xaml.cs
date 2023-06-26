using MemberDesktop.Model;
using MemberDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for Add.xaml
    /// </summary>
    public partial class Add : Page
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


        public Add()
        {
            InitializeComponent();
        }


        public Add(Frame frame, MemberViewModel memberViewModel)
        {
            InitializeComponent();
            this.Frame = frame;
            this.memberViewModel = memberViewModel;
            MemberModel member = new MemberModel();
            member.spouse = new SpouseModel();

            this.DataContext = member;


            _member = member;
            MEMBERSHIP_GRID.DataContext = _member;
            MEMBERDEMO_GRID.DataContext = _member;
            SPOUSEDEMO_GRID.DataContext = _member.spouse;
            ADDRESS_GRID.DataContext = _member;
            APPLICATION_GRID.DataContext = _member;

            _member.created_by_db = ushort.Parse(ConfigurationManager.AppSettings["ADMIN"].ToString());

            MembershipType = memberViewModel.membershipType;
            MembershipStatus = memberViewModel.membershipStatus;
            MembershipReason = memberViewModel.membershipReason;
            Reviewers = memberViewModel.reviewers;
            Admins = memberViewModel.admins;
            ContactPreference = memberViewModel.contactPreference;
            Title = memberViewModel.title; 
            Gender = memberViewModel.gender;
            ComboContactPref.ItemsSource = ContactPreference;
            ComboMemberType.ItemsSource = MembershipType;
           
            ComboCreatedBy.ItemsSource = Admins;
           
            ComboTitle.ItemsSource = Title;
            ComboGender.ItemsSource = Gender;
            ComboTitleSpouse.ItemsSource = Title;
            ComboGenderSpouse.ItemsSource = Gender;
            ComboContactPrefSpouse.ItemsSource = ContactPreference;

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string err = "";
                if (_member.membership_type_db == null || string.IsNullOrEmpty(_member.membership_type_db))
                {
                    err = "\nMembership type is required";
                }
                if (string.IsNullOrEmpty(_member.last_name.Trim()))
                {
                    err += "\nLast name is required";
                }
                if (string.IsNullOrEmpty(_member.first_name.Trim()))
                {
                    err += "\nFirst name is required";
                }
                if (ApplicationDate.SelectedDate == null) { err +=  "\nApplication Date is required"; }

                if (ComboMemberType.SelectedIndex <1 ) { err += "\nMembership type is required"; }

                if (ComboTitle.SelectedIndex < 1) { err += "\nTitle is required"; }

                if (ComboGender.SelectedIndex < 1) { err += "\nGender is required"; }

                if ((bool) SpouseCheckBox.IsChecked)
                {

                    if (ComboTitleSpouse.SelectedIndex < 1) { err += "\nSpouse title is required"; }

                    if (ComboGenderSpouse.SelectedIndex < 1) { err += "\nSpouse gender is required"; }

                    if (string.IsNullOrEmpty(_member.spouse.last_name.Trim()))
                    {
                        err += "\nSpouse last name is required";
                    }
                    if (string.IsNullOrEmpty(_member.spouse.first_name.Trim()))
                    {
                        err += "\nSpouse first name is required";
                    }
                }
                

                if (!string.IsNullOrEmpty(err))
                {
                    MessageBox.Show("Incomplete Data" + err ,"Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                _member.application_date = (DateTime) ApplicationDate.SelectedDate;
                _member.year_started = (short) DateTime.Now.Year;
               
                _member.membership_year = (short)((DateTime)ApplicationDate.SelectedDate).Year;

                memberViewModel.AddMember(_member, (bool) SpouseCheckBox.IsChecked);
                MessageBox.Show("Member added successfully");
                this.Frame.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add member" + "\n" + ex.ToString(), "Add", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.NavigationService.GoBack();
        }

        private void SpouseCheckBox_Click(object sender, RoutedEventArgs e)
        {
            SPOUSEDEMO_GRID.Visibility = (bool)SpouseCheckBox.IsChecked ?
                                                Visibility.Visible :
                                                Visibility.Collapsed;
        }

        private void ComboMemberType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboMemberType.SelectedIndex == 2) // Single
            {
                SPOUSEDEMO_GRID.Visibility = Visibility.Collapsed;
                SpouseCheckBox.Visibility = Visibility.Collapsed;
            }
            else
            {
               // SPOUSEDEMO_GRID.Visibility = Visibility.Visible;
                SpouseCheckBox.Visibility = Visibility.Visible;
                SpouseCheckBox.IsChecked = false;
            }
        }
    }
}