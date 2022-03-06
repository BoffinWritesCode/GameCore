using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GameCore.Localisation
{
    public class LocalisationText
    {
        protected LocalisationSet _Localisation;
        protected string _key;

        protected string _currentText;
        protected string _currentTextLowerCase;

        public string Value => _currentText;
        public string ValueInLowerCase => _currentTextLowerCase;
        public event Action OnChange;

        public LocalisationText(LocalisationSet localisation, string key)
        {
            _Localisation = localisation;
            _key = key;
        }

        public void UpdateText()
        {
            _currentText = _Localisation.GetValue(_key);
            _currentTextLowerCase = _currentText.ToLower();
            OnChange?.Invoke();
        }
    }
}
