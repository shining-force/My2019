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

namespace DatabaseConsole
{
    /// <summary>
    /// KeyPair.xaml 的交互逻辑
    /// </summary>
    public partial class KeyPairCtrl : UserControl
    {
        private Action<KeyPairCtrl> mOnDel;
        public KeyPairCtrl(Action<KeyPairCtrl> onDel)
        {
            InitializeComponent();
            mOnDel = onDel;
        }

        public KeyValuePair<string, string> getKeyPair()
        {
            return new KeyValuePair<string, string>(mKeyBox.Text, mValueBox.Text);
        }

        private void mDelBtn_Click(object sender, RoutedEventArgs e)
        {
            mOnDel(this);
        }
    }
}
