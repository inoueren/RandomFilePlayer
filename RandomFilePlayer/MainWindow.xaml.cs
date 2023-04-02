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
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace RandomFilePlayer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private int OnOff = 1;
        private string FolderPath;
        /// <summary>
        /// メディアプレイヤー
        /// </summary>
        private System.Windows.Media.MediaPlayer Player
        {
            get;
            set;
    } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // キーが押されたときに起こるイベント
            KeyDown += new KeyEventHandler(OnKeyDownHandler);
        }

        /// <summary>
        /// 再生ボタンをクリックしたときのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButtonClick(object sender, EventArgs arguments)
        {
            RandomPlay();
        }

        private void StopButtonClick(object sender, EventArgs arguments)
        {
            PauseAndStart();
        }

        private void NextPlay(object sender, EventArgs auguments)
        {
            RandomPlay();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                RandomPlay();
            }
            else if (e.Key == Key.Space)
            {
                PauseAndStart();
            }
            else if (e.Key == Key.C)
            {
                FolderSelect();
            }
        }

        public void FolderSelect()
        {
            var file_dlg = new CommonOpenFileDialog
            {
                Title = "Please select a folder",
                InitialDirectory = @"C:",
                // フォルダ選択モードにする
                IsFolderPicker = true,
            };

            if (file_dlg.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            this.textboxDirPath.Text = file_dlg.FileName;
            FolderPath = file_dlg.FileName;

            RandomPlay();
        }

        public void FolderSelectButtonClick(object sender, EventArgs arguments)
        {
            FolderSelect();
        }

        public void RandomPlay()
        {
            // MediaPlayerがインスタンス化されているかをあらかじめチェックしておく
            if (Player != null)
            {
                // 再生を停止する
                Player.Stop();
                // ファイルを閉じる
                Player.Close();
                // 初期化する
                Player = null;
            }

            if (FolderPath != null)
            {
                // ディレクトリ内のm4aファイルをリスト化
                // 取得できる拡張子（mp3,m4a,wav）
                List<string> location = new List<string>();
                string[] extList = new string[] { "*.mp3", "*.m4a", "*.wav" };
                foreach (string ext in extList)
                {
                    location.AddRange(System.IO.Directory.GetFiles(FolderPath, ext, System.IO.SearchOption.AllDirectories));
                }

                // Randomをインスタンス化
                Random random = new Random();
                // リストからランダムなファイルを取得
                int num = random.Next(0, location.Count());
                string file = location[num];

                // ラベルにファイル名を表示
                string fileName = System.IO.Path.GetFileName(file);
                label1.Content = fileName;

                // MediaPlayerをインスタンス化
                Player = new System.Windows.Media.MediaPlayer();
                // m4aファイルを開く
                Player.Open(new System.Uri(file));
                // m4aファイルを再生する
                Player.Play();

                // メディアが終了したときに起こるイベント
                Player.MediaEnded += new EventHandler(NextPlay);
            }
        }

        public void PauseAndStart()
        {
            // MediaPlayerがインスタンス化されているかをあらかじめチェックしておく
            if (Player != null && OnOff == 1)
            {
                // 再生を一次停止する
                Player.Pause();
                OnOff = 0;

            }
            else if (Player != null && OnOff == 0)
            {
                // 再生を開始する
                Player.Play();
                OnOff = 1;
            }
        }
    }
}
