using System.IO;

namespace GeneticAlgorithmWPF.Utility
{
    public static class FileUtility
    {
        /// <summary>
        ///　指定したフォルダが無い場合のみフォルダを作成
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path);
            }
            return Directory.CreateDirectory(path);
        }
    }
}
