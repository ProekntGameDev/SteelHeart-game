using UnityEngine;

public class WaitForSceneLoad : CustomYieldInstruction
{
    public override bool keepWaiting => _keepWaiting;

    private bool _keepWaiting;

    public WaitForSceneLoad(AsyncOperation operation)
    {
        _keepWaiting = true;

        operation.completed += OnEventTriggered;
    }

    private void OnEventTriggered(AsyncOperation operation)
    {
        _keepWaiting = false;
    }
}
