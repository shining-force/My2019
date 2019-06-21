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
using Newtonsoft.Json;

namespace DatabaseConsole
{
    /// <summary>
    /// JsonSheet.xaml 的交互逻辑
    /// </summary>
    public partial class JsonSheet : UserControl
    {
        public JsonSheet()
        {
            InitializeComponent();
        }

        public string getSerializedBuffer()
        {
            return mJsonDataBox.Text;
        }

        public J getObject<J>()
        {
            try
            {
                J j = JsonConvert.DeserializeObject<J>(mJsonDataBox.Text);

                return j;
            }
            catch(Exception e)
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    mJsonDataBox.Text += "\r\n" + e.Message;
                }));
                return default(J);
            }
            

        }

        public void layObject(object obj)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(obj, jsSettings);

            this.Dispatcher.Invoke(new Action(delegate
            {
                mJsonDataBox.Text = json;
            }));
        }
    }
}
