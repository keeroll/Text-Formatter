using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace TextFormatter
{
    class FileManager
    {
        public async Task<string> OpenFileAsync()
        {
             return await Task.Factory.StartNew(() => {
                 return OpenFile();
            });
        }

        private string OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Text Documents(*.txt)|*.txt", ValidateNames = true, Multiselect = false };
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    string textFromFile = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
                    textFromFile = textFromFile.Replace("\r", string.Empty);
                    return textFromFile;
                }
            }
            return null;
        }

        public async Task SaveFileAsync(string result)
        {
            await Task.Factory.StartNew(() =>
            {
                SaveFile(result);
            });
        }

        private void SaveFile(string result)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true };
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                    {
                        streamWriter.WriteLineAsync(result);
                        MessageBox.Show("Сохранено!", " ", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
