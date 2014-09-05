Tenjin Unity plugin
=========
- Allows unity developers to quickly integrate with Tenjin's install API

Tenjin install/session integration:
-------
- Include the `.unitypackage` in your Unity Project
- In your project's first scene locate your `Start()` method write and the following `Tenjin.getInstance("<API_KEY>").Connect();`

Here's an example of the code:
```
using UnityEngine;
using System.Collections;

public class TenjinExampleScript : MonoBehaviour {

  // Use this for initialization
  void Start () {
    Tenjin.getInstance ("API_KEY").Connect();
  }
  
  // Update is called once per frame
  void Update () {
  
  }

  void OnApplicationPause(bool pauseStatus){
    if(pauseStatus){
      //do nothing
    }
    else
    {
      Tenjin.getInstance ("API_KEY").Connect();  
    }
  }
}
```
Additional Notes:
------

- This package contains example code in the `/Example` folder of the `.unitypackage`. This can be removed.
- This package contains a `.manifest` for Android and can be modified for your specific project. Make sure to include the permissions found here: https://github.com/ordinance/tenjin-android-sdk
