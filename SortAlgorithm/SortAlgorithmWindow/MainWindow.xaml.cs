using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace SortAlgorithmWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool m_bStartBtnState;
        String m_szDataSource;
        System.Timers.Timer m_hTimer;

        const int CUR_FILE_BUF_MAX = 20;
        
        [DllImport(@".\SortAlgorithmDll.dll", EntryPoint = "SortAlgorithmStart", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SortAlgorithmStart(byte[] szFile, Int32 lSortType, Int32 lShowLiveProgress, IntPtr hWnd);

        [DllImport(@".\SortAlgorithmDll.dll", EntryPoint = "GetSortState", CallingConvention = CallingConvention.Cdecl)]
        public extern static void GetSortState(ref Int32 lProgress, ref Int32 lProgressRange, ref Int32 lIsFunctionEnd, ref Int32 lSortResult, ref Int32 lSortState, ref Int32 lSortTime);

        [DllImport(@".\SortAlgorithmDll.dll", EntryPoint = "SortAlgorithmStop", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SortAlgorithmStop();

        public MainWindow()
        {
            InitializeComponent();
            m_bStartBtnState = true;
            m_szDataSource = null;
            m_hTimer = new System.Timers.Timer();
            m_hTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            m_hTimer.Interval = 20;    // 1秒 = 1000毫秒
        }

        private void m_hStartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (m_bStartBtnState == true)
            {
                //sort type
                Int32 lSortType = m_hSortTypeSelect.SelectedIndex;
                //data source
                StringBuilder szDataSourceBuilder = new StringBuilder();
                szDataSourceBuilder.Append(m_szDataSource);
                //live progress
                Int32 lLiveProg;
                if (m_hShowLiveProgress.IsChecked == true)
                    lLiveProg = 1;
                else
                    lLiveProg = 0;
                //draw area handler
                PictureBox hDrawArea = m_hDrawArea.Child as PictureBox;
				//start sort
				byte[] sz;
				if (m_szDataSource != null)
				{
					sz = Encoding.Unicode.GetBytes(m_szDataSource);
				}
				else
				{
					sz = null;
				}
                SortAlgorithmStart(sz, lSortType, lLiveProg, hDrawArea.Handle);
                //start timer
                m_hTimer.Start();
                //set btn text to "stop"
                m_hStartBtn.Content = "Stop";
                m_bStartBtnState = false;
            }
            else
            {
                //stop function
                SortAlgorithmStop();
                //set btn text to "start"
                m_hStartBtn.Content = "Start";
                m_bStartBtnState = true;
            }
        }

        private void m_hUseFileBtn_Checked(object sender, RoutedEventArgs e)
        {
            //show a Browser to get file
            OpenFileDialog hSelectFile = new OpenFileDialog();
            hSelectFile.DefaultExt = ".png";
            hSelectFile.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|BMP Files (*.bmp)|*.bmp";
            DialogResult hResult = hSelectFile.ShowDialog();
            if (hResult == System.Windows.Forms.DialogResult.OK)
            {
                //get select file, load to m_hCurFile
                m_szDataSource = hSelectFile.FileName;
                if (m_szDataSource.Length > CUR_FILE_BUF_MAX)
                {
                    m_hCurFile.Content = "...";
                    m_hCurFile.Content += m_szDataSource.Substring(m_szDataSource.Length - CUR_FILE_BUF_MAX);
                }
                else
                {
                    m_hCurFile.Content = m_szDataSource;
                }
            }
            else
            {
                //reset to screen
                m_hUseScreenBtn.IsChecked = true;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //init
            //set "select sort type and data source" to m_hSortState
            m_hSortState.Text = "select sort type and data source";
            //init m_hSortTypeSelect index
            m_hSortTypeSelect.SelectedIndex = 0;
            //select data source to screen
            m_hUseScreenBtn.IsChecked = true;
            //show live progress
            m_hShowLiveProgress.IsChecked = false;
            m_hShowProgressManul.Text = SHOW_LIVE_PROGRESS_OFF_MANUAL;
        }

        private void m_hUseScreenBtn_Checked(object sender, RoutedEventArgs e)
        {
            //set m_hCurFile "Screen"
            m_hCurFile.Content = "Screen";
            m_szDataSource = null;
        }
        const int DLL_FUNC_FALSE = 0;
        const int DLL_SORT_STATE_GETTING_SOURCE = 0;
        const int DLL_SORT_STATE_SORTING = 1;
        const int DLL_SORT_STATE_FINISHED = 2;

		const int DLL_SORTTYPE_SELECT = 0;
		const string SORT_MANUAL_SELECT = "选择排序是最初级的排序算法，找到最小的元素，和第一个元素互换，再找第二小的元素，和第二个互换，以此类推。" +
			"优点：所有排序算法中最少的数据交换次数。缺点：极慢的速度。";
		const int DLL_SORTTYPE_INSERT = 1;
		const string SORT_MANUAL_INSERT = "插入(冒泡)排序是初级排序算法，交换相邻元素使得元素位于适当位置，当索引达到数组右端时排序完成。" +
			"优点：善于处理接近有序的数组。缺点：处理乱序或逆序数组很慢。";
		const int DLL_SORTTYPE_SHELL = 2;
		const string SORT_MANUAL_SHELL = "希尔排序是插入排序的改进，交换间隔为N的不相邻元素，直到索引达到数组最右端。多次重复这个过程并逐渐减少N至1" +
			"优点：极大幅度提高插入排序处理任何数组的性能，缺点：性能和N的选取与减小过程设计相关，不可预知的性能。";
		const int DLL_SORTTYPE_MERGE_TOP = 3;
		const string SORT_MANUAL_MERGE_TOP = "自顶归并排序是一个高级排序算法，将数组递归地分成两半排序，不断的将两个有序数组归并为一个有序数组。" +
			"优点：快速且可预测的运行时间，缺点：需要额外空间。";
		const int DLL_SORTTYPE_MERGE_BOTTON = 4;
		const string SORT_MANUAL_MERGE_BOTTON = "自底归并排序是一个高级排序算法，循序地将元素分为N个一组归并，重复这个过程直到N等于数组大小。" +
			"优点：快速且可预测的运行时间，缺点：需要额外空间。";
		const int DLL_SORTTYPE_QUICK = 5;
		const string SORT_MANUA_QUICK = "快速排序是一个高级排序算法，先进行切分整理，排定一个元素，再将数组分为两部分(不包含被排定的元素)，每部分递归这个过程。" +
	        "优点：快，不需要额外空间，缺点：脆弱，任何编写不严密将使性能低劣。";
        const int DLL_SORTTYPE_PQ = 6;
        const string SORT_MANUA_PQ = "堆排序是一种基于优先队列结构设计的排序方法，先将数组整理为有序二叉树，再依次将首节点下沉得到有序结果。" +
            "优点：稳定、编写简洁且在插入和删除最大(小)元素混合的动态场景中表现良好，缺点：缓存命中率极低，数据交换次数多。";
        const int DLL_SORTTYPE_CDEFAULT = 7;
        const string SORT_MANUA_CDEFAULT = "此为c++自带的快速排序。不会显示任何进度，因此速度略快。";
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //have to get UI items
            this.Dispatcher.Invoke(new Action(delegate {
                //sort state
                Int32 lProgress = 0;
                Int32 lProgressRange = 0;
                Int32 lIsFuncEnd = 0;
                Int32 lSortResult = 0;
                Int32 lSortState = 0;
                Int32 lSortTime = 0;
                GetSortState(ref lProgress, ref lProgressRange, ref lIsFuncEnd, ref lSortResult, ref lSortState, ref lSortTime);
                //check and change start btn state
                if ((lIsFuncEnd == DLL_FUNC_FALSE) && (m_bStartBtnState == true))
                {
                    //set btn text to "stop"
                    m_hStartBtn.Content = "Stop";
                    m_bStartBtnState = false;
                }
                else if ((lIsFuncEnd != DLL_FUNC_FALSE) && (m_bStartBtnState != true))
                {
                    //set btn text to "start"
                    m_hStartBtn.Content = "Start";
                    m_bStartBtnState = true;
                    //stop timer
                    m_hTimer.Stop();
                }
                //set progress
                if (m_hSortProg.Maximum != lProgressRange)
                    m_hSortProg.Maximum = lProgressRange;
                m_hSortProg.Value = lProgress;
                //set sort state
                switch (lSortState)
                {
                    case DLL_SORT_STATE_GETTING_SOURCE:
                        //set getting source msg
                        m_hSortState.Text = "Getting data source...";
                        break;
                    case DLL_SORT_STATE_SORTING:
                        //set sorting msg
                        m_hSortState.Text = "Sorting...";
                        break;
                    case DLL_SORT_STATE_FINISHED:
                        //set sort result msg
                        m_hSortState.Text = "Sort finished, time cost = " + lSortTime.ToString() + "ms"
                            + "\r\nCheck...";
                        if (lSortResult == 0)
                        {
                            m_hSortState.Text += "OK.";
                        }
                        else
                        {
                            m_hSortState.Text += "Error pos = " + lSortResult.ToString();
                        }
                        break;
                    default:
                        //clear msg
                        m_hSortState.Text = "";
                        break;
                    }
            }));

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //stop function
            SortAlgorithmStop();
            //stop timer
            m_hTimer.Stop();
        }

		private void M_hSortTypeSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//load sort manual
			switch (m_hSortTypeSelect.SelectedIndex)
			{
				case DLL_SORTTYPE_SELECT:
					m_hSortManual.Text = SORT_MANUAL_SELECT;
					break;
				case DLL_SORTTYPE_INSERT:
					m_hSortManual.Text = SORT_MANUAL_INSERT;
					break;
				case DLL_SORTTYPE_SHELL:
					m_hSortManual.Text = SORT_MANUAL_SHELL;
					break;
				case DLL_SORTTYPE_MERGE_TOP:
					m_hSortManual.Text = SORT_MANUAL_MERGE_TOP;
					break;
				case DLL_SORTTYPE_MERGE_BOTTON:
					m_hSortManual.Text = SORT_MANUAL_MERGE_BOTTON;
					break;
				case DLL_SORTTYPE_QUICK:
					m_hSortManual.Text = SORT_MANUA_QUICK;
					break;
                case DLL_SORTTYPE_PQ:
                    m_hSortManual.Text = SORT_MANUA_PQ;
                    break;
                case DLL_SORTTYPE_CDEFAULT:
                    m_hSortManual.Text = SORT_MANUA_CDEFAULT;
                    break;
                default:
					//clear
					m_hSortManual.Text = "";
					break;
			}
		}
        const string SHOW_LIVE_PROGRESS_ON_MANUAL = "显示实时排序状态将增加数据交换开支，影响算法性能。亦可对比出不同算法对此的适用性。";
        const string SHOW_LIVE_PROGRESS_OFF_MANUAL = "不显示实时排序状态，算法将以最大速度运行。";

        private void m_hShowLiveProgress_Click(object sender, RoutedEventArgs e)
        {
            //set manual
            if (m_hShowLiveProgress.IsChecked == true)
                m_hShowProgressManul.Text = SHOW_LIVE_PROGRESS_ON_MANUAL;
            else
                m_hShowProgressManul.Text = SHOW_LIVE_PROGRESS_OFF_MANUAL;
        }
    }
}
