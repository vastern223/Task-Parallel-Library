using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace zavd3
{
   
    public partial class MainWindow : Window
    {
        public int count_Sentences = 0;
        public int count_characters;
        public int count_interrogative_sentences = 0;
        public int count_exclamation_points = 0;
        static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token = cancelTokenSource.Token;

        public MainWindow()
        {

            InitializeComponent();

        }


        private void start_Click(object sender, RoutedEventArgs e)
        {
            count_Sentences = 0;
            count_characters = 0;
            count_interrogative_sentences = 0;
            count_exclamation_points = 0;

            Task task1 = new Task(
                () => NumberOfSentencesIn_the_text()
            );

            // задача продолжения
            Task task2 = task1.ContinueWith(Number_of_characters);

            Task task3 = task2.ContinueWith(Number_of_words);

            Task task4 = task3.ContinueWith(Number_of_interrogative_sentences);

            Task task5 = task4.ContinueWith(Number_of_exclamation_points);

            Task task6 = task5.ContinueWith(Report);

            task1.Start();


        }


        public void Report(Task t)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                if (radiobt1.IsChecked == true)
                {
                    string mainstr = null;

                    if (CheckBox1.IsChecked == true)
                        mainstr += $"count Sentences: {count_Sentences}\n";
                    if (CheckBox2.IsChecked == true)
                        mainstr += $"count characters: {count_characters}\n";
                    if (CheckBox3.IsChecked == true)
                        mainstr += $"count interrogative sentences: {count_interrogative_sentences}\n";
                    if (CheckBox4.IsChecked == true)
                        mainstr += $"count exclamation points: {count_exclamation_points}\n";

                    MessageBox.Show(mainstr);
                }
                else if (radiobt2.IsChecked == true)
                {
                    using (StreamWriter sw = new StreamWriter("text.txt", false, System.Text.Encoding.Default))
                    {
                        string mainstr = null;

                        if (CheckBox1.IsChecked == true)
                            mainstr += $"count Sentences: {count_Sentences}\n";
                        if (CheckBox2.IsChecked == true)
                            mainstr += $"count characters: {count_characters}\n";
                        if (CheckBox3.IsChecked == true)
                            mainstr += $"count interrogative sentences: {count_interrogative_sentences}\n";
                        if (CheckBox4.IsChecked == true)
                            mainstr += $"count exclamation points: {count_exclamation_points}\n";
                        sw.Write(mainstr);
                    }
                }
            }));
        }

        public void NumberOfSentencesIn_the_text()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                for (int i = 0; i < Textbox.Text.Length; i++)
                {
                    if (Textbox.Text[i] == '.' || Textbox.Text[i] == '!' || Textbox.Text[i] == '?')
                    {
                        if ((i - 1) >= 0)
                        {
                            count_Sentences++;
                        }
                    }
                }

            }));
            // MessageBox.Show(count_Sentences.ToString());
        }

        public void Number_of_characters(Task t)//символи без пробілів
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                string temp_string = Textbox.Text;
                temp_string = temp_string.Replace(" ", "");
                count_characters = temp_string.Length;

            }));
            // MessageBox.Show(count_characters.ToString());
        }

        public void Number_of_words(Task t)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                int count_words = 0;
                char[] wordsSplit = new char[] { ' ', ',', '!', '?', '.' };
                string[] words = Textbox.Text.Split(wordsSplit, StringSplitOptions.RemoveEmptyEntries);
                count_words = words.Count();
            }));
            //  MessageBox.Show(count_words.ToString());
        }

        public void Number_of_interrogative_sentences(Task t)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                for (int i = 0; i < Textbox.Text.Length; i++)
                {
                    if (Textbox.Text[i] == '?')
                    {
                        if ((i - 1) >= 0)
                        {
                            count_interrogative_sentences++;
                        }
                    }
                }

            }));
        }

        public void Number_of_exclamation_points(Task t)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                for (int i = 0; i < Textbox.Text.Length; i++)
                {
                    if (Textbox.Text[i] == '!')
                    {
                        if ((i - 1) >= 0)
                        {
                            count_exclamation_points++;
                        }
                    }
                }

            }));
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource.Cancel();
        }
    }
}
