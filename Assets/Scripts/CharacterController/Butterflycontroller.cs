using UnityEngine;

public class Butterflycontroller : MonoBehaviour
{
    private Game _app;

    public void Setup(Game app)
    {
        _app = app;
    }

    public void FixedUpdate()
    {
        if (_app.IsPlaying)
            this.transform.position = _app.GetButterflyPosition();
    }
}
