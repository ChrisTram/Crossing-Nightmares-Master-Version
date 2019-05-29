using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePopup : MonoBehaviour {

	public static DialoguePopup Create(Vector3 position, string dialogue)
    {
        Transform dialoguePopupTransform = Instantiate(Resources.Load<Transform>("pfDialoguePopup"), position, Quaternion.identity);

        DialoguePopup dialoguePopup = dialoguePopupTransform.GetComponent<DialoguePopup>();
        dialoguePopup.Setup(dialogue);

        return dialoguePopup;
    }

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(string dialogueToLoc)
    {
        //textMesh.SetText(LocalizationManager.instance.GetLocalizedValue(dialogueToLoc));
        textMesh.SetText(dialogueToLoc);
        textColor = textMesh.color;
        disappearTimer = 1f;
    }

    private void Update()
    {
        float moveYSpeed = 20f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            // Start disappearing
            float disappearSpeed = 2f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
