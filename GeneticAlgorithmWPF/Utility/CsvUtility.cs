using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneitcAlgorithmWPF.Utility
{
    public static class CsvUtility
    {
        /// <summary>
        /// csvファイルの読み込み
        /// </summary>
        public static List<string[]> ReadCsv(string filePath, bool isFirstLineIgnore = false)
        {
            List<string[]> datas = new List<string[]>();

            try
            {
                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(filePath))
                {
                    if (isFirstLineIgnore)
                    {
                        sr.ReadLine();
                    }

                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        if (line != null)
                            datas.Add(line.Split(','));
                    }
                }

                return datas;
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したとき
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
