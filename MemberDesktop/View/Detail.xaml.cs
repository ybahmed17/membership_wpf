using MemberDesktop.Model;
using MemberDesktop.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Detail.xaml
    /// </summary>
    public partial class Detail : Page
    {
        MemberViewModel memberViewModel;
        Frame Frame;

        public Detail()
        {
            InitializeComponent();
        }

        

        public Detail(Frame frame, MemberViewModel memberViewModel, MemberModel member)
        {
            InitializeComponent();
            this.Frame = frame;
            this.memberViewModel = memberViewModel;
            memberViewModel.searchMembers(null, member.family_id, "", "", "", "", "", "");
            listViewMembers.ItemsSource = memberViewModel.members;
            bool hasSpouse = false;
            foreach (MemberModel memberModel in memberViewModel.members)
            {
                if (memberModel.member_id == member.member_id)
                {
                    GridAuditMember.DataContext = memberViewModel.GetAuditHx(member.member_id).DefaultView;
                    GridYearlyMember.DataContext = memberViewModel.GetYearlyMembership(member.member_id).DefaultView;

                }
                else
                {
                    hasSpouse = true;
                    GridAuditSpouse.DataContext = memberViewModel.GetAuditHx(memberModel.member_id).DefaultView;
                    GridYearlySpouse.DataContext = memberViewModel.GetYearlyMembership(memberModel.member_id).DefaultView;
                    Console.WriteLine(GridAuditSpouse.DataContext.ToString());
                }
            }
                if (hasSpouse)
                {
                    GridAuditSpouse.Visibility = Visibility.Visible;
                    GridYearlySpouse.Visibility = Visibility.Visible;
                }
                else
                {
                    GridAuditSpouse.Visibility = Visibility.Collapsed;
                    GridYearlySpouse.Visibility = Visibility.Collapsed;
                }


            

            
        }
    }
}
