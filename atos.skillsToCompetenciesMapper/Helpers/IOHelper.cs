using atos.skillsToCompetenciesMapper.Core;
using atos.skillsToCompetenciesMapper.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace atos.skillsToCompetenciesMapper.Helpers
{
    internal enum DataFile { SkillMatrix }
    static class IOHelper
    {
        // ref string path
        internal static bool SaveFileDialog(string data, string filter = null) {
            var sfd = new SaveFileDialog {
                Filter = filter
            };

            try
            {
                if (sfd.ShowDialog() == true)
                    File.WriteAllText(sfd.FileName, data);
            }
            catch (IOException IoEx) {
                MessageBox.Show(IoEx.Message, "File I/O", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
           
        }

        internal async static Task<string> OpenFileDialogAsync(string filter = null)
        {
            var result = String.Empty;
            var ofd = new OpenFileDialog {
                Filter = filter
            };
            
            try
            {
                if (ofd.ShowDialog() == true)
                    result = await Task.Run(() => File.ReadAllText(ofd.FileName));

            }
            catch (IOException IoEx)
            {
                MessageBox.Show(IoEx.Message, "File I/O", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        internal async static Task<T> ReadDataFileAsync<T>(DataFile dataFile)
        {
            string localPath = null;
            switch (dataFile)
            {
                case DataFile.SkillMatrix:
                    localPath = Constants.SKILLMATRIX_PATH;
                    break;
            }

            using (StreamReader r = new StreamReader(localPath))
                return JsonConvert.DeserializeObject<T>(await r.ReadToEndAsync(), Role.GetConverter());
        }
    }
}
