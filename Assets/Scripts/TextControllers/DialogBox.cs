using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour{
    [SerializeField] private TextMeshPro textArea;

    public void SetColor(Color color){
        textArea.color = color;
    }
    
    public void SetText(string text){
        textArea.text = text;
    }
}
