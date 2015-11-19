using UnityEngine;
using System.Collections;

public static class ClearSave {

    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
    }

    
}
