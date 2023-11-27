using TMPro;
using UnityEngine;

namespace UI
{
    public class PopUpUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void PopUpMessage(string str)
        {
            text.text = str;
            anim.SetTrigger("Pop");
        }
    }
}
