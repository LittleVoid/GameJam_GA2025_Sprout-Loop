using UnityEngine;

public class Butterflycontroller : MonoBehaviour
{
    public void FixedUpdate()
    {
        if (App.Instance.IsPlaying)
            this.transform.position = App.Instance.GetButterflyPosition();
    }
}
