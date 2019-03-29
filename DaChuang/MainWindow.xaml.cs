using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace DaChuang
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /*
         * rateMoody为穆迪的输出，rateSP为标准普尔的输出
         */
        string rateMoody = "";
        string rateSP = "";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //先逐一判断输入框输入是否为数字，若不为数字，则聚焦至该输入框，并返回函数
            if (!checkNumber(textColumn1) || !checkNumber(textColumn2) || !checkNumber(textColumn3) || !checkNumber(textColumn4))
            {
                return;
            }
            string pyexePath = @"main\main.exe";
            double dataPreManage = double.Parse(textColumn2.Text)/10000;//处理大数值
            string strArgs = textColumn1.Text + " " + dataPreManage.ToString() + " " + textColumn3.Text + " " + textColumn4.Text;

            Process p = new Process();
            p.StartInfo.FileName = pyexePath;           //需要执行的文件路径
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;  //输出参数设定
            p.StartInfo.RedirectStandardInput = true;   //传入参数设定
            p.StartInfo.RedirectStandardError = true;   //重定向错误输出 
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = strArgs;
            p.Start();          
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            p.WaitForExit();//关键，等待外部程序退出后才能往下执行
            Console.Write(output);
            Console.Write(error);
            if (error!=string.Empty)
            {
                MessageBox.Show("请输入真实数值");
            }
            getRate(output);//将输出转化为穆迪和标准普尔的输出格式
            if (radioMoody.IsChecked==true)
            {
                textRate.Text = rateMoody;
            }
            else
            {
                textRate.Text = rateSP;
            }            
            p.Close();
            Console.Write("done");
        }
        /*
         * Function:    checkNumber
         * Description: 判断输入框的文本是否为数字，若为空或不为数字，都会聚焦到该文本框
         * Input:       textbox:需要进行判断的文本框
         * output:      返回bool值，若文本为数字，返回true，若不为数字，返回false
         */
        private bool checkNumber(TextBox textbox)
        {
            if (textbox.Text == string.Empty)
            {
                MessageBox.Show("输入不能为空！");
                textbox.Focus();
                return false;
            }
            else
            {
                float temp;
                if (float.TryParse(textbox.Text, out temp)==false)
                {
                    MessageBox.Show("请输入数字！");
                    textbox.Focus();
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /*
         * Function:    getRate
         * Description: 对输入的string进行语义转化，对rateMoody穆迪和rateSP标准普尔进行赋值
         * Input:       strRate:需要进行语义转化的string，此处应为python执行文件的输出结果
         */
        private void getRate(string strRate)
        {
            int rate = int.Parse(strRate);
            switch (rate)
            {
                case 0:
                    rateMoody = "Aaa";
                    rateSP = "AAA";
                    break;
                case 1:
                    rateMoody = "Aa1 OR Aa2 OR Aa3";
                    rateSP = "AA+ OR AA OR AA-";
                    break;
                case 2:
                    rateMoody = "A1 OR A2 OR A3";
                    rateSP = "A+ OR A OR A-";
                    break;
                case 3:
                    rateMoody = "Baa1 OR Baa2 OR Baa3";
                    rateSP = "BBB+ OR BBB OR BBB-";
                    break;
                case 4:
                    rateMoody = "Ba1 OR Ba2 OR Ba3";
                    rateSP = "BB+ OR BB OR BB-";
                    break;
                case 5:
                    rateMoody = "B1 OR B2 OR B3";
                    rateSP = "B+ OR B OR B-";
                    break;
                case 6:
                    rateMoody = "Caa";
                    rateSP = "CCC";
                    break;
                case 7:
                    rateMoody = "Ca";
                    rateSP = "CC";
                    break;
                case 8:
                    rateMoody = rateSP = "C";
                    break;
                case 9:
                    rateMoody = rateSP = "D";
                    break;
                default:
                    MessageBox.Show("BUG COMING!");
                    break;               
            }
        }

        private void radioMoody_Click(object sender, RoutedEventArgs e)
        {
            textRate.Text = rateMoody;
        }

        private void radioSP_Click(object sender, RoutedEventArgs e)
        {
            textRate.Text = rateSP;
        }
    }
}
