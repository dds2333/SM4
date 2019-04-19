using System;
using System.IO;
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
using System.Windows.Forms;
using Org.BouncyCastle.Utilities.Encoders;

namespace SM4
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

        string tempiv = null;//临时初始向量

        /// <summary>
        /// 将文件内容转化成byte数组
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>字节数组</returns>
        private byte[] FiletoByte(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            try
            {
                byte[] databytes = new byte[fs.Length];
                fs.Read(databytes, 0, (int)fs.Length);
                return databytes;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

        }

        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// 将输入的密钥处理成16字节的伪密钥，密钥长度和分组长度都是128位（16字节）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Keyto16bytes(string key)
        {
            string fakekey;
            int length = key.Length;
            if (length == 16)
            {
                fakekey = key;
                return fakekey;
            }
            else if (length < 16 && length > 0)
            {
                int t = 16 - length;
                string s = string.Empty;
                for (int i = 0; i < t; i++)
                {
                    s += '0';
                }
                fakekey = key + s;
                return fakekey;
            }
            else
            {
                fakekey = key.Substring(0, 16);
                return fakekey;
            }
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SelFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "选择待加密或解密的文件";
            op.DefaultExt = ".txt";
            op.Filter = "All files|*.*";
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox.Clear();//清空textBox
                textBox.Text = op.FileName;
            }
            op.FilterIndex = 0;
            op.CheckFileExists = true;
            op.CheckPathExists = true;
        }

        /// <summary>
        /// CBC模式加密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CBCEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == string.Empty)
            {
                System.Windows.MessageBox.Show("请选择待加密或解密的文件！！", "提示");
                return;
            }
            else
            {
                Secretkey secretket_form = new Secretkey();
                if (secretket_form.ShowDialog().Value == true)
                {
                    SM4Utils sm4 = new SM4Utils();
                    //sm4.secretKey = "JeF8U9wHFOMfs2Y8";

                    sm4.secretKey = Keyto16bytes(secretket_form.key);

                    sm4.hexString = false;
                    tempiv = GetRandomString(16, true, true, true, false, null).ToLower();
                    sm4.iv = tempiv;
                    //sm4.iv = "UISwD9fW6cFh9SNS";
                    byte[] bytedata;
                    bytedata = FiletoByte(textBox.Text);
                    string cipher_data = sm4.Encrypt_CBC(Encoding.Default.GetString(bytedata));
                    string cipher_data_iv = sm4.iv + cipher_data;
                    byte[] cipher_bytedata = Encoding.Default.GetBytes(cipher_data);
                    byte[] cipher_bytedata_iv = Encoding.Default.GetBytes(cipher_data_iv);
                    
                    string extension = System.IO.Path.GetExtension(textBox.Text);
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "保存加密文件";
                    sfd.Filter = "All files|*" + extension;
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                        fs.Write(cipher_bytedata_iv, 0, cipher_bytedata_iv.Length);
                        textBox_EnPath.Clear();
                        textBox_EnPath.AppendText(" 加密文件路径：" + sfd.FileName);
                        fs.Close();
                        System.Windows.MessageBox.Show("已成功加密文件！！", "提示");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("请保存密文文件！！", "提示");
                        return;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("请输入一个密钥以用于加密！！", "提示");
                    return;
                }
            }
        }

        /// <summary>
        /// CBC模式解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CBCDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == string.Empty)
            {
                System.Windows.MessageBox.Show("请选择待加密或解密的文件！！", "提示");
                return;
            }
            else
            {
                Secretkey secretket_form = new Secretkey();
                if (secretket_form.ShowDialog().Value == true)
                {
                    SM4Utils sm4 = new SM4Utils();
                    //sm4.secretKey = "JeF8U9wHFOMfs2Y8";//密钥
                           
                    sm4.secretKey = Keyto16bytes(secretket_form.key);

                    sm4.hexString = false;
                    //sm4.iv = tempiv;
                    //sm4.iv = "UISwD9fW6cFh9SNS";//初始向量
                    byte[] bytedata = FiletoByte(textBox.Text);                 
                    byte[] temp_iv = new byte[16];
                    byte[] file_bytedata = new byte[bytedata.Length - 16];
                    Array.Copy(bytedata, temp_iv, 16);
                    sm4.iv = Encoding.Default.GetString(temp_iv);
                    Array.Copy(bytedata, 16, file_bytedata,0,(bytedata.Length-16));
                    string plain_data = sm4.Decrypt_CBC(Encoding.Default.GetString(file_bytedata));
                    if (plain_data == string.Empty)
                    {
                        System.Windows.MessageBox.Show("输入的密钥不正确！！", "提示");
                        return;
                    }
                    else
                    {
                        byte[] plain_bytedata = Encoding.Default.GetBytes(plain_data);
                        string extension = System.IO.Path.GetExtension(textBox.Text);
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Title = "保存解密文件";
                        sfd.Filter = "All files|*" + extension;
                        if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                            fs.Write(plain_bytedata, 0, plain_bytedata.Length);
                            textBox_DePath.Clear();
                            textBox_DePath.AppendText(" 解密文件路径：" + sfd.FileName);
                            fs.Close();
                            System.Windows.MessageBox.Show("已成功解密文件！！", "提示");
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("请保存明文文件！！", "提示");
                            return;
                        }
                    }
                    
                }
                else
                {
                    System.Windows.MessageBox.Show("请输入并确认密钥！！", "提示");
                    return;
                }
            }
        }

        /// <summary>
        /// EBC模式加密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_EBCEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == string.Empty)
            {
                System.Windows.MessageBox.Show("请选择待加密或解密的文件！！", "提示");
                return;
            }
            else
            {
                Secretkey secretket_form = new Secretkey();
                if (secretket_form.ShowDialog().Value == true)
                {
                    SM4Utils sm4 = new SM4Utils();
                    //sm4.secretKey = "JeF8U9wHFOMfs2Y8";

                    sm4.secretKey = Keyto16bytes(secretket_form.key);

                    sm4.hexString = false;

                    byte[] bytedata;
                    bytedata = FiletoByte(textBox.Text);
                    string cipher_data = sm4.Encrypt_ECB(Encoding.Default.GetString(bytedata));
                    byte[] cipher_bytedata = Encoding.Default.GetBytes(cipher_data);
                    string extension = System.IO.Path.GetExtension(textBox.Text);
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "保存加密文件";
                    sfd.Filter = "All files|*" + extension;
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                        fs.Write(cipher_bytedata, 0, cipher_bytedata.Length);
                        textBox_EnPath.Clear();
                        textBox_EnPath.AppendText(" 加密文件路径：" + sfd.FileName);
                        fs.Close();
                        System.Windows.MessageBox.Show("已成功加密文件！！", "提示");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("请保存密文文件！！", "提示");
                        return;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("请输入一个密钥以用于加密！！", "提示");
                    return;
                }           
        }
    }

        /// <summary>
        /// EBC模式解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_EBCDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == string.Empty)
            {
                System.Windows.MessageBox.Show("请选择待加密或解密的文件！！", "提示");
                return;
            }
            else
            {
                Secretkey secretket_form = new Secretkey();
                if (secretket_form.ShowDialog().Value == true)
                {
                    SM4Utils sm4 = new SM4Utils();
                    //sm4.secretKey = "JeF8U9wHFOMfs2Y8";//密钥

                    sm4.secretKey = Keyto16bytes(secretket_form.key);

                    sm4.hexString = false;
                    byte[] bytedata;
                    bytedata = FiletoByte(textBox.Text);
                    string plain_data = sm4.Decrypt_ECB(Encoding.Default.GetString(bytedata));
                    if (plain_data == string.Empty)
                    {
                        System.Windows.MessageBox.Show("输入的密钥不正确！！", "提示");
                        return;
                    }
                    else
                    {
                        byte[] plain_bytedata = Encoding.Default.GetBytes(plain_data);
                        string extension = System.IO.Path.GetExtension(textBox.Text);
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Title = "保存解密文件";
                        sfd.Filter = "All files|*" + extension;
                        if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                            fs.Write(plain_bytedata, 0, plain_bytedata.Length);
                            textBox_DePath.Clear();
                            textBox_DePath.AppendText(" 解密文件路径：" + sfd.FileName);
                            fs.Close();
                            System.Windows.MessageBox.Show("已成功解密文件！！", "提示");
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("请保存明文文件！！", "提示");
                            return;
                        }
                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("请输入并确认密钥！！", "提示");
                    return;
                }
            }
        }
    }
}
