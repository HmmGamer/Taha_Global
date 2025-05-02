///
/// you can use this instead of manual singleton coding
/// 
/* example :
 * 
 * public class Sample : Singleton_Abs<Sample>
 * {
 *    private void Start()
 *   {
 *       print(_instance.name);
 *   }
 * }
 */
using UnityEngine;

public abstract class Singleton_Abs<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _instance;

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
    }
}
