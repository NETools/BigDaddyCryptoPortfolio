using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.Globalization
{
    public class LanguageGlobalizationResourceAdapter : INotifyPropertyChanged
    {
        public Dictionary<string, string> Strings { get; private set; } = [];

        public event PropertyChangedEventHandler? PropertyChanged;

        public async Task LoadLanguage(string file)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(file);
            using var reader = new StreamReader(stream);

            if (reader == null)
                return;

            Strings.Clear();

            string? currentLine = "";
            while ((currentLine = await reader.ReadLineAsync()) != null)
            {
                var keyValuePair = currentLine.Split('=');
                if (Strings.ContainsKey(keyValuePair[0]))
                    continue;
                Strings.Add(keyValuePair[0], keyValuePair[1]);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Strings)));

        }
    }
}
