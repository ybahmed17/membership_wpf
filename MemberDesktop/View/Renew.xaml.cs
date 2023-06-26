using MemberDesktop.Model;
using MemberDesktop.ViewModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
    /// Interaction logic for Renew.xaml
    /// </summary>
    public partial class Renew : Page
    {
        MemberViewModel memberViewModel;
        Frame Frame;
        MemberModel _member;
        MembershipRenewal _renewal;


        public Renew()
        {
            InitializeComponent();
        }


        public Renew(Frame frame, MemberViewModel memberViewModel, MemberModel member)
        {
            InitializeComponent();
            this.Frame = frame;
            this.memberViewModel = memberViewModel;
            this._member = member;
            this.DataContext = member;
            this._renewal = new MembershipRenewal();
            this._renewal.MemberID = member.member_id;
            this._renewal.FamilyID = member.family_id;
            RenewMember.DataContext = this._renewal;
            ComboRenewedBy.ItemsSource = memberViewModel.admins;
            ComboYear.ItemsSource = memberViewModel.renewal_year;
            _renewal.RenewedBy = ushort.Parse(ConfigurationManager.AppSettings["ADMIN"].ToString());




        }

        private void Renew_Click(object sender, RoutedEventArgs e)
        {
            _renewal.Comment = comment.Text;
            _renewal.ApplicationDate = (DateTime)renewal_date.SelectedDate;
            memberViewModel.RenewMembership(_renewal, _member.membership_type_db != "single");
            if (_member.membership_type_db == "single")
            {
                MessageBox.Show("Renewed membership", "Renew", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Renewed family membership","Renew", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            this.Frame.NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.NavigationService.GoBack();
        }
    }
}