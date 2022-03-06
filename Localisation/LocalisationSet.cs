using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace GameCore.Localisation
{
    public class LocalisationSet
    {
        private Dictionary<string, Dictionary<string, string>> _languages;
        private Dictionary<string, string> _fallbackLanguageDict;
        private Dictionary<string, string> _currentLanguageDict;
        private string _fileLoc;
        private string _fallbackLang;

        public string CurrentLanguage { get; private set; }
        public List<string> AvailableLanguages { get; private set; }
        public Dictionary<string, LocalisationText> LocalisationTexts { get; private set; }

        public event Action<string> OnLanguageChange;

        /// <param name="fileLocation">location of localisation .json file</param>
        /// <param name="fallbackLanguage">the language id to fall back to if a key doesn't exist in the current language.</param>
        public LocalisationSet(string fileLocation, string fallbackLanguage)
        {
            LocalisationTexts = new Dictionary<string, LocalisationText>();
            AvailableLanguages = new List<string>();
            _languages = new Dictionary<string, Dictionary<string, string>>();

            _fileLoc = fileLocation;
            _fallbackLang = fallbackLanguage;
            Reload();
        }
        
        public void Reload()
        {
            AvailableLanguages.Clear();
            _languages.Clear();

            JsonDocumentOptions options = new JsonDocumentOptions()
            {
                CommentHandling = JsonCommentHandling.Skip
            };

            if (!File.Exists(_fileLoc))
            {
                throw new FileNotFoundException($"File not found! {_fileLoc}");
            }

            using (Stream file = File.Open(_fileLoc, FileMode.Open))
            {
                using (JsonDocument document = JsonDocument.Parse(file, options))
                {
                    foreach (JsonProperty language in document.RootElement.EnumerateObject())
                    {
                        Dictionary<string, string> keyValueDict = new Dictionary<string, string>();

                        string langId = language.Name;

                        RecurseProperty(keyValueDict, language, "");

                        _languages.Add(langId, keyValueDict);
                        AvailableLanguages.Add(langId);

                        if (langId == _fallbackLang)
                        {
                            _fallbackLanguageDict = keyValueDict;
                        }
                    }
                }
            }
        }

        private void RecurseProperty(Dictionary<string, string> keyValDict, JsonProperty property, string prefix)
        {
            foreach (JsonProperty key in property.Value.EnumerateObject())
            {
                string newKey = prefix + key.Name;
                if (key.Value.ValueKind == JsonValueKind.Object)
                {
                    RecurseProperty(keyValDict, key, newKey + "-");
                    continue;
                }

                keyValDict.Add(newKey, key.Value.GetString());

                if (!LocalisationTexts.ContainsKey(newKey))
                {
                    LocalisationTexts.Add(newKey, new LocalisationText(this, newKey));
                }
            }
        }

        public string GetValue(string key)
        {
            if (_currentLanguageDict.TryGetValue(key, out string value)) return value;
            if (_fallbackLanguageDict.TryGetValue(key, out string value2)) return value2;

            throw new Exception($"Key '{key}' doesn't exist!");
        }

        public LocalisationText GetLocalisationText(string key)
        {
            if (LocalisationTexts.TryGetValue(key, out LocalisationText value)) return value;

            throw new Exception($"Key '{key}' doesn't exist!");
        }

        public void SetLanguage(string language)
        {
            if (CurrentLanguage == language) return;

            if (_languages.TryGetValue(language, out var keyPairs))
            {
                _currentLanguageDict = keyPairs;
            
                CurrentLanguage = language;
                
                OnLanguageChange?.Invoke(CurrentLanguage);

                UpdateLocalisationTexts();

                return;
            }

            throw new KeyNotFoundException($"Language '{language}' does not exist!");
        }

        private void UpdateLocalisationTexts()
        {
            foreach (var kvp in LocalisationTexts)
            {
                kvp.Value.UpdateText();
            }
        }
    }
}
