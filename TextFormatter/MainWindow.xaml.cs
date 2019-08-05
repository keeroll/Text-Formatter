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
using System.Text.RegularExpressions;

namespace TextFormatter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataManager dataManager = new DataManager();
        FileManager fileManager = new FileManager();

        List<string> listOne = new List<string>();
        List<string> listTwo = new List<string>();
        List<string> listChangeText = new List<string>();

        string splitterOne = Environment.NewLine;
        string splitterTwo = Environment.NewLine;

        string UniqAllOriginal;
        string RepeatAllOriginal;
        string UniqTwoInOneOriginal;
        string RepeatTwoInOneOriginal;

        string TmpTextOne = " ";
        string TmpTextTwo = " ";

        string parameterTextFileOne = "\n";
        string parameterTextFileTwo = "\n";
        string parameterTextUniqAll = Environment.NewLine;
        string parameterTextRepeatAll = Environment.NewLine;
        string parameterTextUniqTwoInOne = Environment.NewLine;
        string parameterTextRepeatTwoInOne = Environment.NewLine;

        bool cbTrimSpacesStatus = true;

        string AboutApp = "Данное программное обеспечение предназначено для Andy.\nКоммерческое использование данного программного обеспечения в любой другой организации и его продажа запрещены!";

        public MainWindow()
        {
            InitializeComponent();
            tbCustomSplitterFileOne.Visibility = Visibility.Collapsed;
            tbCustomSplitterFileTwo.Visibility = Visibility.Collapsed;
            
            rbNewLineFOne.IsChecked = true;
        }

        //Open file 1
        private async void btnOpenFileOne_Click(object sender, RoutedEventArgs e)
        {
            tbFileOne.Text = await fileManager.OpenFileAsync();
        }

        //Open file 2
        private async void btnOpenFileTwo_Click(object sender, RoutedEventArgs e)
        {
            tbFileTwo.Text = await fileManager.OpenFileAsync();
        }

        //Data Proccessing
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFileOne.Text) && string.IsNullOrEmpty(tbFileTwo.Text))
            {
                splitterOne = ChooseSplitterOne();

                ProccessOneFileAsync(listOne, splitterOne, tbFileOne.Text);

            }
            else if (!string.IsNullOrEmpty(tbFileOne.Text) && !string.IsNullOrEmpty(tbFileTwo.Text))
            {
                splitterOne = ChooseSplitterOne();
                splitterTwo = ChooseSplitterTwo();

                ProccessTwoFilesAsync(splitterOne, splitterTwo, listOne, listTwo, tbFileOne.Text, tbFileTwo.Text);
            }
            else if (string.IsNullOrEmpty(tbFileOne.Text) && !string.IsNullOrEmpty(tbFileTwo.Text))
            {
                splitterTwo = ChooseSplitterTwo();

                ProccessOneFileAsync(listTwo, splitterTwo, tbFileTwo.Text);
            }
            else
                MessageBox.Show("Продолжение работы невозможно!\nНи одно поле не заполнено!", " ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Procces one file async
        private async void ProccessOneFileAsync(List<string> list, string splitter, string text)
        {
            list = await dataManager.SplitTextAsync(splitter, text);

            tbUniqAll.Text = await dataManager.FindUniqValuesOneFileAsync(list, cbTrimSpacesStatus);
            tbRepeatAll.Text = await dataManager.FindRepeatValuesOneFileAsync(list, cbTrimSpacesStatus);

            UniqAllOriginal = tbUniqAll.Text;
            RepeatAllOriginal = tbRepeatAll.Text;
        }

        //Proccess two files async
        private async void ProccessTwoFilesAsync(string firstSpliter, string secondSplitter, List<string> firstList, List<string> secondList, string textOne, string textTwo)
        {
            //Split text
            firstList = await dataManager.SplitTextAsync(firstSpliter, textOne);
            secondList = await dataManager.SplitTextAsync(secondSplitter, textTwo);

            //File 2 in 1
            tbRepeatTwoInOne.Text = await dataManager.FindRepeatValuesTwoInOneAsync(firstList, secondList, cbTrimSpacesStatus);
            tbUniqTwoInOne.Text = await dataManager.FindUniqValuesTwoInOneAsync(firstList, secondList, cbTrimSpacesStatus);

            //In 2 files
            tbRepeatAll.Text = await dataManager.FindRepeatValuesInTwoFilesAsync(firstList, secondList, cbTrimSpacesStatus);
            tbUniqAll.Text = await dataManager.FindUniqValuesInTwoFilesAsync(firstList, secondList, cbTrimSpacesStatus);

            UniqAllOriginal = tbUniqAll.Text;
            RepeatAllOriginal = tbRepeatAll.Text;
            UniqTwoInOneOriginal = tbUniqTwoInOne.Text;
            RepeatTwoInOneOriginal = tbRepeatTwoInOne.Text;
        }

        //Choose the splitter one
        private string ChooseSplitterOne()
        {
            if (rbNewLineFOne.IsChecked == true)
                return "\n";
            else if (rbTabFOne.IsChecked == true)
                return "\t";
            else if (rbSpecCharFOne.IsChecked == true)
                return tbCustomSplitterFileOne.Text;
            else
                return "\n";
        }

        //Choose the splitter two
        private string ChooseSplitterTwo()
        {
            if (rbNewLineFTwo.IsChecked == true)
                return "\n";
            else if (rbTabFTwo.IsChecked == true)
                return "\t";
            else if (rbSpecCharFTwo.IsChecked == true)
                return tbCustomSplitterFileOne.Text;
            else
                return "\n";
        }

        //Save files
        private async void btnSaveUniqAll_Click(object sender, RoutedEventArgs e)
        {
            await fileManager.SaveFileAsync(tbUniqAll.Text);
        }

        private async void btnSaveUniqTwoInOne_Click(object sender, RoutedEventArgs e)
        {
            await fileManager.SaveFileAsync(tbUniqTwoInOne.Text);
        }

        private async void btnSaveRepeatTwoInOne_Click(object sender, RoutedEventArgs e)
        {
            await fileManager.SaveFileAsync(tbRepeatTwoInOne.Text);
        }

        private async void btnSaveRepeateAll_Click(object sender, RoutedEventArgs e)
        {
            await fileManager.SaveFileAsync(tbRepeatAll.Text);
        }

        //Change status of radio buttons
        private void ChangeControls()
        {
           if(rbNewLineFOne.IsChecked == true && rbTabFOne.IsChecked == false && rbSpecCharFOne.IsChecked == false 
                && rbNewLineFTwo.IsChecked == false && rbTabFTwo.IsChecked == false && rbSpecCharFTwo.IsChecked == false
                || rbNewLineFOne.IsChecked == false && rbTabFOne.IsChecked == true && rbSpecCharFOne.IsChecked == false
                && rbNewLineFTwo.IsChecked == false && rbTabFTwo.IsChecked == false && rbSpecCharFTwo.IsChecked == false)
            {
                rbNewLineFTwo.IsChecked = true;
                rbNewLineUniqAll.IsChecked = true;
                rbNewLineRepeatAll.IsChecked = true;
                rbNewLineUniqTwoInOne.IsChecked = true;
                rbNewLineRepeatTwoInOne.IsChecked = true;
            }
            else if (rbNewLineFOne.IsChecked == false && rbTabFOne.IsChecked == false && rbSpecCharFOne.IsChecked == false
                 && rbNewLineFTwo.IsChecked == true && rbTabFTwo.IsChecked == false && rbSpecCharFTwo.IsChecked == false
                 || rbNewLineFOne.IsChecked == false && rbTabFOne.IsChecked == false && rbSpecCharFOne.IsChecked == false
                  && rbNewLineFTwo.IsChecked == false && rbTabFTwo.IsChecked == true && rbSpecCharFTwo.IsChecked == false)
            {
                rbNewLineFOne.IsChecked = true;
                rbNewLineUniqAll.IsChecked = true;
                rbNewLineRepeatAll.IsChecked = true;
                rbNewLineUniqTwoInOne.IsChecked = true;
                rbNewLineRepeatTwoInOne.IsChecked = true;
            }
        }

        #region Change View Of UniqAll TextBox

        private async void rbNewLineUniqAll_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqAllOriginal != null)
            {
                parameterTextUniqAll = Environment.NewLine;
                tbUniqAll.Text = "Tmp";
                tbUniqAll.Text = await dataManager.ChangeNewLineTextAsync(UniqAllOriginal, listChangeText, cbTrimSpacesStatus);
            }    
        }

        private async void rbTabUniqAll_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqAllOriginal != null)
            {
                parameterTextUniqAll = "\t";
                tbUniqAll.Text = await dataManager.ChangeTabTextAsync(UniqAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbComaUniqAll_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqAllOriginal != null)
            {
                parameterTextUniqAll = " ,  ";
                tbUniqAll.Text = await dataManager.ChangeComaTextAsync(UniqAllOriginal, listChangeText, cbTrimSpacesStatus); 
            }
        }

        private async void rbComaNoSpaceUniqAll_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqAllOriginal != null)
            {
                parameterTextUniqAll = ",";
                tbUniqAll.Text = await dataManager.ChangeComaNoSpaceTextAsync(UniqAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbSpaceUniqAll_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqAllOriginal != null)
            {
                parameterTextUniqAll = " ";
                tbUniqAll.Text = await dataManager.ChangeSpaceTextAsync(UniqAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbFullTextUniqAll_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqAllOriginal != null)
            {
                parameterTextUniqAll = @"(?<=[\n])\s+";
                tbUniqAll.Text = await dataManager.ChangeFullTextAsync(UniqAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbQuotedNewLineUniqAll_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqAllOriginal != null)
            {
                parameterTextUniqAll = Environment.NewLine;
                tbUniqAll.Text = await dataManager.ChangeQuotedTextNewLineAsync(UniqAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbQuotedInLineUniqAll_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqAllOriginal != null)
            {
                parameterTextUniqAll = "', ";
                tbUniqAll.Text = await dataManager.ChangeQuotedTextInLineAsync(UniqAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

#endregion

        #region Change View Of RepeatAll TextBox

        private async void rbNewLineRepeatAll_Checked(object sender, RoutedEventArgs e)
        {
            if(RepeatAllOriginal != null)
            {
                parameterTextRepeatAll = Environment.NewLine;
                tbRepeatAll.Text = "Tmp";
                tbRepeatAll.Text = await dataManager.ChangeNewLineTextAsync(RepeatAllOriginal, listChangeText, cbTrimSpacesStatus);
            }    
        }

        private async void rbTabRepeatAll_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatAllOriginal != null)
            {
                parameterTextRepeatAll = "\t";
                tbRepeatAll.Text = await dataManager.ChangeTabTextAsync(RepeatAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbComaRepeatAll_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatAllOriginal != null)
            {
                parameterTextRepeatAll = " ,  ";
                tbRepeatAll.Text = await dataManager.ChangeComaTextAsync(RepeatAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbComaNoSpaceRepeatAll_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatAllOriginal != null)
            {
                parameterTextRepeatAll = ",";
                tbRepeatAll.Text = await dataManager.ChangeComaNoSpaceTextAsync(RepeatAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbSpaceRepeatAll_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatAllOriginal != null)
            {
                parameterTextRepeatAll = " ";
                tbRepeatAll.Text = await dataManager.ChangeSpaceTextAsync(RepeatAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbFullTextRepeatAll_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatAllOriginal != null)
            {
                parameterTextRepeatAll = @"(?<=[\n])\s+";
                tbRepeatAll.Text = await dataManager.ChangeFullTextAsync(RepeatAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbQuotedNewLineRepeatAll_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatAllOriginal != null)
            {
                parameterTextRepeatAll = Environment.NewLine;
                tbRepeatAll.Text = await dataManager.ChangeQuotedTextNewLineAsync(RepeatAllOriginal, listChangeText, cbTrimSpacesStatus); 
            }
        }

        private async void rbQuotedInLineRepeatAll_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatAllOriginal != null)
            {
                parameterTextRepeatAll = "', ";
                tbRepeatAll.Text = await dataManager.ChangeQuotedTextInLineAsync(RepeatAllOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

#endregion

        #region Change View Of UniqTwoInOne TextBox

        private async void rbNewLineUniqTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if(UniqTwoInOneOriginal != null)
            {
                parameterTextUniqTwoInOne = Environment.NewLine;
                tbUniqTwoInOne.Text = "Tmp";
                tbUniqTwoInOne.Text = await dataManager.ChangeNewLineTextAsync(UniqTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }    
        }

        private async void rbTabUniqTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqTwoInOneOriginal != null)
            {
                parameterTextUniqTwoInOne = "\t";
                tbUniqTwoInOne.Text = await dataManager.ChangeTabTextAsync(UniqTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbComaUniqTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqTwoInOneOriginal != null)
            {
                parameterTextUniqTwoInOne = " ,  ";
                tbUniqTwoInOne.Text = await dataManager.ChangeComaTextAsync(UniqTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbComaNoSpaceUniqTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqTwoInOneOriginal != null)
            {
                parameterTextUniqTwoInOne = ",";
                tbUniqTwoInOne.Text = await dataManager.ChangeComaNoSpaceTextAsync(UniqTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbSpaceUniqTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqTwoInOneOriginal != null)
            {
                parameterTextUniqTwoInOne = " ";
                tbUniqTwoInOne.Text = await dataManager.ChangeSpaceTextAsync(UniqTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbFullTextUniqTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqTwoInOneOriginal != null)
            {
                parameterTextUniqTwoInOne = @"(?<=[\n])\s+";
                tbUniqTwoInOne.Text = await dataManager.ChangeFullTextAsync(UniqTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);  
            }
        }

        private async void rbQuotedNewLineUniqTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqTwoInOneOriginal != null)
            {
                parameterTextUniqTwoInOne = Environment.NewLine;
                tbUniqTwoInOne.Text = await dataManager.ChangeQuotedTextNewLineAsync(UniqTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbQuotedInLineUniqTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (UniqTwoInOneOriginal != null)
            {
                parameterTextUniqTwoInOne = "', ";
                tbUniqTwoInOne.Text = await dataManager.ChangeQuotedTextInLineAsync(UniqTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        #endregion

        #region Change View Of RepeatTwoInOne TextBox

        private async void rbNewLineRepeatTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatTwoInOneOriginal != null)
            {
                parameterTextRepeatTwoInOne = Environment.NewLine;
                tbRepeatTwoInOne.Text = "Tmp";
                tbRepeatTwoInOne.Text = await dataManager.ChangeNewLineTextAsync(RepeatTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbTabRepeatTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatTwoInOneOriginal != null)
            {
                parameterTextRepeatTwoInOne = "\t";
                tbRepeatTwoInOne.Text = await dataManager.ChangeTabTextAsync(RepeatTwoInOneOriginal, listChangeText, cbTrimSpacesStatus); 
            }
        }

        private async void rbComaRepeatTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatTwoInOneOriginal != null)
            {
                parameterTextRepeatTwoInOne = " ,  ";
                tbRepeatTwoInOne.Text = await dataManager.ChangeComaTextAsync(RepeatTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbComaNoSpaceRepeatTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatTwoInOneOriginal != null)
            {
                parameterTextRepeatTwoInOne = ",";
                tbRepeatTwoInOne.Text = await dataManager.ChangeComaNoSpaceTextAsync(RepeatTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbSpaceRepeatTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatTwoInOneOriginal != null)
            {
                parameterTextRepeatTwoInOne = " ";
                tbRepeatTwoInOne.Text = await dataManager.ChangeSpaceTextAsync(RepeatTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbFullTextRepeatTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatTwoInOneOriginal != null)
            {
                parameterTextRepeatTwoInOne = @"(?<=[\n])\s+";
                tbRepeatTwoInOne.Text = await dataManager.ChangeFullTextAsync(RepeatTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbQuotedNewLineRepeatTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatTwoInOneOriginal != null)
            {
                parameterTextRepeatTwoInOne = Environment.NewLine;
                tbRepeatTwoInOne.Text = await dataManager.ChangeQuotedTextNewLineAsync(RepeatTwoInOneOriginal, listChangeText, cbTrimSpacesStatus);
            }
        }

        private async void rbQuotedInLineRepeatTwoInOne_Checked(object sender, RoutedEventArgs e)
        {
            if (RepeatTwoInOneOriginal != null)
            {
                parameterTextRepeatTwoInOne = "', ";
                tbRepeatTwoInOne.Text = await dataManager.ChangeQuotedTextInLineAsync(RepeatTwoInOneOriginal, listChangeText, cbTrimSpacesStatus); 
            }
        }
        #endregion

        #region Radio Buttons of File One and File Two

        private void rbNewLineFOne_Checked(object sender, RoutedEventArgs e)
        {
            tbCustomSplitterFileOne.Visibility = Visibility.Collapsed;
            ChangeControls();

            if (tbFileOne.Text != null)
            {
                parameterTextFileOne = "\n";
                TmpTextOne = tbFileOne.Text;
                tbFileOne.Text = "Tmp";
                tbFileOne.Text = TmpTextOne;
            }
        }

        private void rbTabFOne_Checked(object sender, RoutedEventArgs e)
        {
            tbCustomSplitterFileOne.Visibility = Visibility.Collapsed;
            ChangeControls();

            if (tbFileOne.Text != null)
            {
                parameterTextFileOne = "\t";
                TmpTextOne = tbFileOne.Text;
                tbFileOne.Text = "Tmp";
                tbFileOne.Text = TmpTextOne;
            }
        }

        private void rbSpecCharFOne_Checked(object sender, RoutedEventArgs e)
        {
            tbCustomSplitterFileOne.Visibility = Visibility.Visible;
            ChangeControls();

            if (tbFileOne.Text != null)
            {
                parameterTextFileOne = tbCustomSplitterFileOne.Text;
                TmpTextOne = tbFileOne.Text;
                tbFileOne.Text = "Tmp";
                tbFileOne.Text = TmpTextOne;
            }
        }

        private void rbNewLineFTwo_Checked(object sender, RoutedEventArgs e)
        {
            tbCustomSplitterFileTwo.Visibility = Visibility.Collapsed;
            ChangeControls();

            if(tbFileTwo.Text != null)
            {
                parameterTextFileTwo = "\n";
                TmpTextTwo = tbFileTwo.Text;
                tbFileTwo.Text = "Tmp";
                tbFileTwo.Text = TmpTextTwo;
            }
        }

        private void rbTabFTwo_Checked(object sender, RoutedEventArgs e)
        {
            tbCustomSplitterFileTwo.Visibility = Visibility.Collapsed;
            ChangeControls();

            if (tbFileTwo.Text != null)
            {
                parameterTextFileTwo = "\t";
                TmpTextTwo = tbFileTwo.Text;
                tbFileTwo.Text = "Tmp";
                tbFileTwo.Text = TmpTextTwo;
            }
        }

        private void rbSpecCharFTwo_Checked(object sender, RoutedEventArgs e)
        {
            tbCustomSplitterFileTwo.Visibility = Visibility.Visible;
            ChangeControls();

            if (tbFileTwo.Text != null)
            {
                parameterTextFileTwo = tbCustomSplitterFileTwo.Text;
                TmpTextTwo = tbFileTwo.Text;
                tbFileTwo.Text = "Tmp";
                tbFileTwo.Text = TmpTextTwo;
            }
        }

#endregion

        #region Set counters to labels (conters of characters, words, spaces, phrases)

        private async void tbFileOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbCharsFileOne.Content = await dataManager.CountCharsAsync(tbFileOne.Text);
            lbSpacesFileOne.Content = await dataManager.CountSpacesAsync(tbFileOne.Text);
            lbWordsFileOne.Content = await dataManager.CountWordsAsync(tbFileOne.Text);
            lbPhrasesFileOne.Content = await dataManager.CountPhrasesAsync(tbFileOne.Text, parameterTextFileOne);
        }

        private async void tbFileTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbCharsFileTwo.Content = await dataManager.CountCharsAsync(tbFileTwo.Text);
            lbSpacesFileTwo.Content = await dataManager.CountSpacesAsync(tbFileTwo.Text);
            lbWordsFileTwo.Content = await dataManager.CountWordsAsync(tbFileTwo.Text);
            lbPhrasesFileTwo.Content = await dataManager.CountPhrasesAsync(tbFileTwo.Text, parameterTextFileTwo);
        }

        private async void tbUniqAll_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbCharsUniqAll.Content = await dataManager.CountCharsAsync(tbUniqAll.Text);
            lbSpacesUniqAll.Content = await dataManager.CountSpacesAsync(tbUniqAll.Text);
            lbWordsUniqAll.Content = await dataManager.CountWordsAsync(tbUniqAll.Text);
            lbPhrasesUniqAll.Content = await dataManager.CountPhrasesAsync(tbUniqAll.Text, parameterTextUniqAll);
        }

        private async void tbRepeatAll_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbCharsRepeatAll.Content = await dataManager.CountCharsAsync(tbRepeatAll.Text);
            lbSpacesRepeatAll.Content = await dataManager.CountSpacesAsync(tbRepeatAll.Text);
            lbWordsRepeatAll.Content = await dataManager.CountWordsAsync(tbRepeatAll.Text);
            lbPhrasesRepeatAll.Content = await dataManager.CountPhrasesAsync(tbRepeatAll.Text, parameterTextRepeatAll);
        }

        private async void tbUniqTwoInOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbCharsUniqTwoInOne.Content = await dataManager.CountCharsAsync(tbUniqTwoInOne.Text);
            lbSpacesUniqTwoInOne.Content = await dataManager.CountSpacesAsync(tbUniqTwoInOne.Text);
            lbWordsUniqTwoInOne.Content = await dataManager.CountWordsAsync(tbUniqTwoInOne.Text);
            lbPhrasesUniqTwoInOne.Content = await dataManager.CountPhrasesAsync(tbUniqTwoInOne.Text, parameterTextUniqTwoInOne);
        }

        private async void tbRepeatTwoInOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbCharsRepeatTwoInOne.Content = await dataManager.CountCharsAsync(tbRepeatTwoInOne.Text);
            lbSpacesRepeatTwoInOne.Content = await dataManager.CountSpacesAsync(tbRepeatTwoInOne.Text);
            lbWordsRepeatTwoInOne.Content = await dataManager.CountWordsAsync(tbRepeatTwoInOne.Text);
            lbPhrasesRepeatTwoInOne.Content = await dataManager.CountPhrasesAsync(tbRepeatTwoInOne.Text, parameterTextRepeatTwoInOne);
        }

        #endregion

        private void btnAboutApp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(AboutApp, "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearAllResults()
        {
            tbUniqAll.Text = null;
            tbRepeatAll.Text = null;
            tbUniqTwoInOne.Text = null;
            tbRepeatTwoInOne.Text = null;

            UniqAllOriginal = null;
            RepeatAllOriginal = null;
            UniqTwoInOneOriginal = null;
            RepeatTwoInOneOriginal = null;
        }

        private void btnClearResults_Click(object sender, RoutedEventArgs e)
        {
            ClearAllResults();
            SetLabelCountersToDefault();
        }

        //Need to set phrases labels to 0 if textBox is empty.
        private void SetLabelCountersToDefault()
        {
            lbPhrasesUniqAll.Content = "0";
            lbPhrasesRepeatAll.Content = "0";
            lbPhrasesUniqTwoInOne.Content = "0";
            lbPhrasesRepeatTwoInOne.Content = "0";
        }

        private void cbTrimSpaces_Unchecked(object sender, RoutedEventArgs e)
        {
            cbTrimSpacesStatus = false;
        }

        private void cbTrimSpaces_Checked(object sender, RoutedEventArgs e)
        {
            cbTrimSpacesStatus = true;
        }

        private async void tbCustomSplitterFileTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            parameterTextFileTwo = tbCustomSplitterFileTwo.Text;

            lbPhrasesFileTwo.Content = await dataManager.CountPhrasesAsync(tbFileTwo.Text, parameterTextFileTwo);
        }

        private async void tbCustomSplitterFileOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            parameterTextFileOne = tbCustomSplitterFileOne.Text;
            
            lbPhrasesFileOne.Content = await dataManager.CountPhrasesAsync(tbFileOne.Text, parameterTextFileOne);
        }
    }
}
