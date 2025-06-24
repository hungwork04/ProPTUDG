using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectcharact : MonoBehaviour
{
    public List<Image> ChaImgs;
    public int curIndex=-1;
    public void OnSelectCharacterUpdateUI(int playerIndex){
        for(int i=0;i<ChaImgs.Count;i++){
            ChaImgs[i].color=Color.white;
            if(playerIndex==i){
                ChaImgs[playerIndex].color=Color.red;
                curIndex=playerIndex;
            }
        }
    }
    public void chooseCharacter(){
        if(curIndex<0||curIndex>ChaImgs.Count){
            curIndex=0;
        }
        GameController.Instance.playerIndex=curIndex;
        this.gameObject.SetActive(false);
    }
}
