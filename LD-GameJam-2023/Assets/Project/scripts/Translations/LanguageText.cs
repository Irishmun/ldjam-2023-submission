using TMPro;
using Translations.Languages;
using UnityEngine;

namespace Translations
{
    [RequireComponent(typeof(TMP_Text))]
    public class LanguageText : MonoBehaviour
    {
        //private const string PLACEHOLDER_TEXT = "Placeholder_text_(Script)";
        private const string PLACEHOLDER_TEXT = "PLACEHOLDER";
        private const int MAX_VISIBLE_CHARACTERS = 200;


        [SerializeField] private string textId;
        [SerializeField] private bool typeWriterEffect = false;
        [SerializeField] private float typeWriterTime = 0.1f;

        private TextMeshProUGUI _myText;
        private int _currentMaxCharacters = 0, _textLength;
        private float _timeSinceLastChar = 0;
        private bool _finishedTyping = true;


        private void Awake()
        {
            _myText = gameObject.GetComponent<TextMeshProUGUI>();
        }
        private void OnEnable()
        {
            //Channels.LanguageEvents.OnLanguageChanged += OnLanguageChanged;
            if (typeWriterEffect == true)
            {
                SetMaxVisibleChars(0);
            }
            else
            {
                SetMaxVisibleChars(MAX_VISIBLE_CHARACTERS);
            }
            if (LanguageHandler.Instance == null)
            { return; }
            UpdateText();
        }
        private void Start()
        {
            UpdateText();
        }
        private void OnDisable()
        {
            //Channels.LanguageEvents.OnLanguageChanged -= OnLanguageChanged;
        }
        private void LateUpdate()
        {
            if (_finishedTyping == false)
            {
                if (this.enabled == true && typeWriterEffect == true)
                {
                    if (_currentMaxCharacters < _textLength)
                    {
                        if (_timeSinceLastChar <= 0)
                        {
                            SetMaxVisibleChars(_currentMaxCharacters + 1);
                            _timeSinceLastChar = typeWriterTime;
                        }
                        else
                        {
                            _timeSinceLastChar -= Time.deltaTime;
                        }
                    }
                    else
                    {
                        _finishedTyping = true;
                    }
                }
            }

        }

        private void OnLanguageChanged(LanguageBase newLanguage)
        {
            UpdateText();
        }
        private void SetMaxVisibleChars(int maxValue)
        {
            if (maxValue > MAX_VISIBLE_CHARACTERS)
            {
                maxValue = MAX_VISIBLE_CHARACTERS;
            }
            _currentMaxCharacters = maxValue;
            _myText.maxVisibleCharacters = _currentMaxCharacters;
        }
        /// <summary>Updates the text to the new <see cref="TextID"/> if available</summary>
        public void UpdateText()
        {
            if (_myText == null)
            { return; }
            if (string.IsNullOrWhiteSpace(textId))
            {
                //_myText.text = PLACEHOLDER_TEXT;
            }
            else
            {
                LanguageHandler.Instance.GetText(textId, out string newText);
                //Debug.Log($"{textId}: {newText}");
                _myText.text = newText;
            }
            _textLength = _myText.text.Length > MAX_VISIBLE_CHARACTERS ? MAX_VISIBLE_CHARACTERS : _myText.text.Length;
            if (typeWriterEffect == true)
            {
                SetMaxVisibleChars(0);
                _finishedTyping = false;
            }
            else
            {
                ForceFinishText();
            }
        }
        /// <summary>Forces the typewriter to finish typing instantly</summary>
        public void ForceFinishText()
        {
            SetMaxVisibleChars(_textLength);
            _finishedTyping = true;
        }

        public string TextID { get => textId; set => textId = value; }
        public bool FinishedTyping => _finishedTyping;
    }
}