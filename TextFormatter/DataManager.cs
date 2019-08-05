using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TextFormatter
{
    class DataManager
    {
        List<string> resultList = new List<string>();

        //Split text
        public List<string> SplitText(string splitter, string text)
        {
            text = text.Replace("\r", splitter);
            text = text.Replace("\n", splitter);
            text = text.Replace("\r\n", splitter);

            if (splitter.Length > 1)
                resultList = Regex.Split(text, splitter).ToList();
            else if (splitter.Length == 1)
                resultList = text.Split(Convert.ToChar(splitter)).ToList();

            return resultList;
        }

        //Split text async
        public async Task<List<string>> SplitTextAsync(string splitter, string text)
        {
            return await Task.Factory.StartNew(() =>
            {
                return SplitText(splitter, text);
            });
        }

        #region Find Unique and Repeat Values

        private string FindUniqValuesOneFile(List<string> list, bool status)
        {
            if (status)
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            resultList = list.Where(s => !string.IsNullOrEmpty(s)).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
            string uniqText = String.Join(Environment.NewLine, resultList);

            return uniqText;
        }

        //Find uniq values from file async
        public async Task<string> FindUniqValuesOneFileAsync(List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return FindUniqValuesOneFile(list, status);
            });
        }

        private string FindUniqValuesInTwoFiles(List<string> listOne, List<string> listTwo, bool status)
        {
            if (status)
            {
                string tmp = " ";

                for (int i = 0; i < listOne.Count; i++)
                {
                    tmp = listOne[i].ToString();
                    listOne[i] = tmp.Trim();
                }
                
                for(int j = 0; j < listTwo.Count; j++)
                {
                    tmp = listTwo[j].ToString();
                    listTwo[j] = tmp.Trim();
                }
            }

            resultList = listOne.Concat(listTwo).ToList();
            resultList = resultList.Where(s => !string.IsNullOrEmpty(s)).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();

            string uniqTextAll = String.Join(Environment.NewLine, resultList);

            return uniqTextAll;
        }

        //Find uniq values from two files async
        public async Task<string> FindUniqValuesInTwoFilesAsync(List<string> listOne, List<string> listTwo, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return FindUniqValuesInTwoFiles(listOne, listTwo, status);
            });
        }

        private string FindUniqValuesTwoInOne(List<string> listOne, List<string> listTwo, bool status)
        {
            if (status)
            {
                string tmp = " ";

                for (int i = 0; i < listOne.Count; i++)
                {
                    tmp = listOne[i].ToString();
                    listOne[i] = tmp.Trim();
                }

                for (int j = 0; j < listTwo.Count; j++)
                {
                    tmp = listTwo[j].ToString();
                    listTwo[j] = tmp.Trim();
                }
            }

            resultList = listTwo.Where(s => !string.IsNullOrEmpty(s)).Except(listOne, StringComparer.CurrentCultureIgnoreCase).ToList();

            string uniqTextTwoInOne = String.Join(Environment.NewLine, resultList);

            return uniqTextTwoInOne;
        }

        //Find uniq values of file two for file one async
        public async Task<string> FindUniqValuesTwoInOneAsync(List<string> listOne, List<string> listTwo, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return FindUniqValuesTwoInOne(listOne, listTwo, status);
            });
        }

        private string FindRepeatValuesOneFile(List<string> list, bool status)
        {
            if (status)
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            resultList = list.Where(s => !string.IsNullOrEmpty(s)).GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            string repeatText = String.Join(Environment.NewLine, resultList);

            return repeatText;
        }

        //Find repeat values in one file async
        public async Task<string> FindRepeatValuesOneFileAsync(List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return FindRepeatValuesOneFile(list, status);
            });
        }

        private string FindRepeatValuesInTwoFiles(List<string> listOne, List<string> listTwo, bool status)
        {
            if (status)
            {
                string tmp = " ";

                for (int i = 0; i < listOne.Count; i++)
                {
                    tmp = listOne[i].ToString();
                    listOne[i] = tmp.Trim();
                }

                for (int j = 0; j < listTwo.Count; j++)
                {
                    tmp = listTwo[j].ToString();
                    listTwo[j] = tmp.Trim();
                }
            }

            resultList = listOne.Concat(listTwo).ToList();
            resultList = resultList.Where(s => !string.IsNullOrEmpty(s)).GroupBy(x => x)
                 .Where(x => x.Count() > 1)
                 .Select(x => x.Key)
                 .ToList();

            string repeatTextTwoInOne = String.Join(Environment.NewLine, resultList);

            return repeatTextTwoInOne;
        }

        //Find repeat values in two files async
        public async Task<string> FindRepeatValuesInTwoFilesAsync(List<string> listOne, List<string> listTwo, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return FindRepeatValuesInTwoFiles(listOne, listTwo, status);
            });
        }

        private string FindRepeatValuesTwoInOne(List<string> listOne, List<string> listTwo, bool status)
        {
            if (status)
            {
                string tmp = " ";
                for (int i = 0; i < listOne.Count; i++)
                {
                    tmp = listOne[i].ToString();
                    listOne[i] = tmp.Trim();
                }

                for (int j = 0; j < listTwo.Count; j++)
                {
                    tmp = listTwo[j].ToString();
                    listTwo[j] = tmp.Trim();
                }
            }

            resultList = listTwo.Where(s => !string.IsNullOrEmpty(s)).Intersect(listOne, StringComparer.CurrentCultureIgnoreCase).ToList();
            string repeatTextAll = String.Join(Environment.NewLine, resultList);
            return repeatTextAll;
        }

        //Find repeat values of file two in file one async
        public async Task<string> FindRepeatValuesTwoInOneAsync(List<string> listOne, List<string> listTwo, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return FindRepeatValuesTwoInOne(listOne, listTwo, status);
            });
        }
        #endregion

        #region Change View Of Result TextBox

        //Change view to coma
        private string ChangeComaText(string text, List<string> list, bool status)
        {
            list = Regex.Split(text, Environment.NewLine).ToList();
            string tmp = " ";
            if (status)
                for(int i = 0; i < list.Count; i++)
                {
                    tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            return text = String.Join(" ,  ", list);
        }

        public async Task<string> ChangeComaTextAsync(string text, List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ChangeComaText(text, list, status);
            });
        }

        //Change view to coma and without spaces
        private string ChangeComaNoSpaceText(string text, List<string> list, bool status)
        {
            text = text.Replace(" ", string.Empty);
            list = Regex.Split(text, Environment.NewLine).ToList();

            if (status)
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            return text = String.Join(",", list);
        }

        public async Task<string> ChangeComaNoSpaceTextAsync(string text, List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ChangeComaNoSpaceText(text, list, status);
            });
        }

        //Change view to spaces
        private string ChangeSpaceText(string text, List<string> list, bool status)
        {
            text = text.Replace(" ", string.Empty);
            list = Regex.Split(text, Environment.NewLine).ToList();

            if (status)
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            return text = String.Join(" ", list);
        }

        public async Task<string> ChangeSpaceTextAsync(string text, List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ChangeSpaceText(text, list, status);
            });
        }

        //Change view to full text without any characters
        private string ChangeFullText(string text, List<string> list, bool status)
        {
            text = text.Replace(" ", string.Empty);
            list = Regex.Split(text, Environment.NewLine).ToList();
            
            if (status)
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            return text = String.Join(string.Empty, list);
        }

        public async Task<string> ChangeFullTextAsync(string text, List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ChangeFullText(text, list, status);
            });
        }

        //Change view to quoted text on new line
        private string ChangeQuotedTextNewLine(string text, List<string> list, bool status)
        {
            list = Regex.Split(text, Environment.NewLine).ToList();
            
            if (status)
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            return text = String.Join(Environment.NewLine, list.Select(x => string.Format("'{0}'", x)).ToList());
        }

        public async Task<string> ChangeQuotedTextNewLineAsync(string text, List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ChangeQuotedTextNewLine(text, list, status);
            });
        }

        //Change view to quoted text in line with coma
        private string ChangeQuotedTextInLine(string text, List<string> list, bool status)
        {
            list = Regex.Split(text, Environment.NewLine).ToList();
            
            if (status)
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            return text = String.Join(", ", list.Select(x => string.Format("'{0}'", x)).ToList());
        }

        public async Task<string> ChangeQuotedTextInLineAsync(string text, List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ChangeQuotedTextInLine(text, list, status);
            });
        }

        //Change view to tab text
        private string ChangeTabText(string text, List<string> list, bool status)
        {
            //text = text.Replace(" ", string.Empty);
            list = Regex.Split(text, Environment.NewLine).ToList();
            
            if (status)
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }

            return text = String.Join("\t", list);
        }

        public async Task<string> ChangeTabTextAsync(string text, List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ChangeTabText(text, list, status);
            });
        }

        //Change view to NewLine
        private string ChangeNewLineText(string text, List<string> list, bool status)
        {
            list = Regex.Split(text, Environment.NewLine).ToList();
            
            if (status)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string tmp = list[i].ToString();
                    list[i] = tmp.Trim();
                }
                return text = String.Join(Environment.NewLine, list);
            }
            else
                return text;
        }

        public async Task<string> ChangeNewLineTextAsync(string text, List<string> list, bool status)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ChangeNewLineText(text, list, status);
            });
        }

        #endregion

        #region Count Characters, Spaces, Words and Sentences

        private string CountChars(string text)
        {
            if (text.Length < 1)
                return "0";

            text = text.Replace(Environment.NewLine, string.Empty);
            return text.Length.ToString();
        }
        
        public async Task<string> CountCharsAsync(string text)
        {
            return await Task.Factory.StartNew(() =>
            {
                return CountChars(text);
            });
        }

        private string CountSpaces(string text)
        {
            if (text.Length < 1)
                return "0";

            return (text.Split(' ').Length - 1).ToString();
        }

        public async Task<string> CountSpacesAsync(string text)
        {
            return await Task.Factory.StartNew(() =>
            {
                return CountSpaces(text);
            });
        }

        private string CountPhrases(string text, string parameter)
        {
            if (text.Length < 1)
                return "0";

            List<string> result = new List<string>();
            int count = 0;

            if (parameter.Length > 1)
            {
                result = Regex.Split(text, parameter).ToList();
                count = result.Count;
            }
            else if (parameter.Length == 1)
            {
                result = text.Split(Convert.ToChar(parameter)).ToList();
                count = result.Count;
            }

            return count.ToString();
        }

        public async Task<string> CountPhrasesAsync(string text, string parameter)
        {
            return await Task.Factory.StartNew(() =>
            {
                return CountPhrases(text, parameter);
            });
        }

        private string CountWords(string text)
        {
            if (text.Length < 1)
                return "0";

            var words = Regex.Matches(text, @"[^\s.?,]+", RegexOptions.CultureInvariant | RegexOptions.Multiline
                | RegexOptions.IgnoreCase);

            return words.Count.ToString();
        }

        public async Task<string> CountWordsAsync(string text)
        {
            return await Task.Factory.StartNew(() =>
            {
                return CountWords(text);
            });
        }
        
#endregion
    }
}
