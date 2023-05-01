using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Translations.Languages
{
    public abstract class LanguageBase
    {
        /// <summary>The code for the given language (e.g. english = "en")</summary>
        public abstract string Language { get; }
        /// <summary>Name of the current language</summary>
        public abstract string Name { get; }
        private const string _filePrefix = "lang_";
        private const string _audioFolder = "audio/";
        private const string _fileFolder = "Languages/";

        private const string _audioObjectName = "LANG_AUDIO";
        private const string _langFolderName = "LANG_FOLDER";

        /// <summary>Returns a key,value pair of the translated text and their deisgnated codes</summary>
        public Dictionary<string, string> GetLanguageText()
        {
            return GetLanguageContent(TextFilePath);
        }

        /// <summary>Returns a key,value pair of the language associated audio files and their designated codes</summary>
        public Dictionary<string, string> GetLanguageAudio()
        {
            return GetLanguageContent(AudioFilePath);
        }

        /// <summary>Returns the contents of the json file at path as a string,string Dictionary</summary>
        /// <param name="path">The file's path from the resources folder</param>
        public Dictionary<string, string> GetLanguageContent(string path)
        {
            TextAsset file = Resources.Load<TextAsset>(path);
            if (file == null)
            { return new Dictionary<string, string>(); }
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(file.text);
        }

        /// <summary>The FilePath (From Resources) to this language's file</summary>
        public string TextFilePath => $"{_fileFolder}{_filePrefix}{Language}";//{_fileType}";
        public string AudioFolderPath => $"{_audioFolder}{_fileFolder}";
        /// <summary>The FilePath (From Resources) to this language's audio file folders</summary>
        public string AudioFilePath => $"{AudioFolderPath}{_filePrefix}{Language}";
        public static string FilePrefix => _filePrefix;

    }
}