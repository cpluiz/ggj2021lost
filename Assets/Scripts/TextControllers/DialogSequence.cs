using UnityEngine;

namespace TextControllers{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class DialogSequence : ScriptableObject{
        public string[] dialogIDs;
    }
}