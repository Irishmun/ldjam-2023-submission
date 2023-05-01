using System.Collections;
using System.Collections.Generic;
using Translations.Languages;
using UnityEngine;
using UnityEngine.Networking;

namespace Translations
{
    public class LanguageHandler : MonoBehaviour
    {

        private LanguageBase _currentLanguage;
        private Dictionary<string, string> _currentLanguageDict;

        public static LanguageHandler Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            SetLanguage(new Language_English());
        }

        public void SetLanguage(LanguageBase newLanguage)
        {
            _currentLanguage = newLanguage;
            _currentLanguageDict = _currentLanguage.GetLanguageText();
            //Channels.LanguageEvents.OnLanguageChanged?.Invoke(_currentLanguage);
        }
        /// <summary>Gets and returns the text associated with the given ID</summary>
        /// <param name="Id">the ID for the desired text</param>
        /// <param name="text">the text that belongs to the ID</param>
        /// <returns>whether the ID exists in the current language</returns>
        /// <remarks>will return the ID as text if it can't be found in the current language</remarks>
        public bool GetText(string Id, out string text)
        {//prevent nokeyexceptions
            if (!_currentLanguageDict.ContainsKey(Id.ToUpper().Trim()))
            {
                Debug.Log($"Key \"{Id}\" was not found");
                text = Id;
                return false;
            }
            text = _currentLanguageDict[Id];
            return true;
        }
        public LanguageBase CurrentLanguage { get => _currentLanguage; set => _currentLanguage = value; }
        public Dictionary<string, string> CurrentLanguageDict { get => _currentLanguageDict; }
    }
}