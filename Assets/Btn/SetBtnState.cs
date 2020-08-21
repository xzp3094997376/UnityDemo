using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class SetBtnState:MonoBehaviour
{
    public List<Sprite> allNormalSprite = new List<Sprite>();
    public List<Sprite> allPressedSprite = new List<Sprite>();
    public List<Button> allButton = new List<Button>(); 
    public List<Button> allButtons=new List<Button>();
    [Button]
    public void QuickSetBtn()
    {
        allButtons.AddRange(this.GetComponentsInChildren<Button>(true));
        
        for (int i = 0; i < allButtons.Count; i++)
        {
            int j = i;
            //Debug.LogError(allButtons[j].name);
            Button cBtn = allButtons[j];
            if(cBtn.transform.childCount!=0)
                DestroyImmediate(cBtn.transform.GetChild(0).gameObject);
            cBtn.GetComponent<Image>().sprite = allNormalSprite[j];
            cBtn.GetComponent<Image>().type = Image.Type.Simple;
            cBtn.GetComponent<Image>().SetNativeSize();
            cBtn.transition = Selectable.Transition.SpriteSwap;
            SpriteState sp = new SpriteState();
            sp.highlightedSprite = allPressedSprite[j];
            sp.pressedSprite = allNormalSprite[j];
            cBtn.spriteState = sp;
        }
    }
    [Button]
    public void CollectSprite()
    {
        allButtons.AddRange(this.GetComponentsInChildren<Button>(true));
        for (int i = 0; i < allButtons.Count; i++)
        {
            int j = i;
            allNormalSprite.Add(allButtons[j].GetComponent<Image>().sprite);
            allPressedSprite.Add(allButtons[j].spriteState.highlightedSprite);
        }
    }
}
