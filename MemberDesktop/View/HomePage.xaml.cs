using MemberDesktop.ViewModel;
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

namespace MemberDesktop.View
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private Frame Frame;
        MemberViewModel memberViewModel;

        public HomePage()
        {
            InitializeComponent();
        }

        public HomePage(Frame frame1, MemberViewModel memberViewModel)
        {
            InitializeComponent();
            this.Frame = frame1;
            this.memberViewModel = memberViewModel;
        }

    
    }
}
