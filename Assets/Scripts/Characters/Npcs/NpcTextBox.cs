using TMPro;
using UnityEngine;

namespace BonesVr.Characters.Npcs
{
    public class NpcTextBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        protected TMP_Text Text => _text;

        public void SetText(string text)
            => Text.text = text;
    }
}
