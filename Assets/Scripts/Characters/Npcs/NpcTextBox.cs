using TMPro;
using UnityEngine;

namespace BonesVr.Characters.Npcs
{
    [RequireComponent(typeof(Canvas))]
    public class NpcTextBox : MonoBehaviour
    {
        protected Canvas Canvas => GetComponent<Canvas>();

        [SerializeField] private TMP_Text _text;
        protected TMP_Text Text => _text;

        protected virtual void Awake()
        {
            SetText(null);
        }

        public void SetText(string text)
        {
            Text.text = text;

            if (string.IsNullOrEmpty(text))
                HideTextBox();
            else
                ShowTextBox();
        }

        private void ShowTextBox() => Canvas.enabled = true;
        private void HideTextBox() => Canvas.enabled = false;
    }
}
