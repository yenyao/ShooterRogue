using UnityEngine;

public class Helper : MonoBehaviour
{
    public static GameObject FindComponentInChildWithTag<T>(GameObject parent, string tag)where T:Component{
           Transform t = parent.transform;
           foreach(Transform tr in t)
           {
                  if(tr.tag == tag)
                  {
                       return tr.GetComponent<GameObject>();
                  }
           }
           return null;
     }
}
