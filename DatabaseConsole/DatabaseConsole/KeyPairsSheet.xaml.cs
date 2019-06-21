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
    /// KeyPairsSheet.xaml 的交互逻辑
    /// </summary>
    public partial class KeyPairsSheet : UserControl
    {
        public KeyPairsSheet()
        {
            InitializeComponent();
        }

        public Dictionary<string, string> getKeyPairs()
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (KeyPairCtrl pair in mPairsBox.Children)
            {
                pairs.Add(pair.getKeyPair().Key, pair.getKeyPair().Value);
            }

            return pairs;
        }

        private void mAddBtn_Click(object sender, RoutedEventArgs e)
        {
            mPairsBox.Children.Add(new KeyPairCtrl(delPair));
        }

        private void delPair(KeyPairCtrl pair)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                mPairsBox.Children.RemoveAt(mPairsBox.Children.IndexOf(pair));
            }));            
        }
    }
}
