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

Tenjin custom event integration:
-------
- Include the Assets folder in your Unity project
- In your projects method for the custom event write the following for a named event: `Tenjin.getInstance("<API_KEY>").SendEvent("name")` and the following for a named event with a value: `Tenjin.getInstance("<API_KEY>").SendEvent("nameWithValue","0.99")`

Here's an example of the code:
```
void MethodWithCustomEvent(){
    //event with name
    Tenjin.getInstance("API_KEY").SendEvent("name");

    //event with name and value
    Tenjin.getInstance("API_KEY").SendEvent("nameWithValue", "Value");
}
```
