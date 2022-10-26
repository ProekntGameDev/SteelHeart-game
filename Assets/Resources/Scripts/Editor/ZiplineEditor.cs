using UnityEditor;

[CustomEditor(typeof(Zipline))]
public class ZiplineEditor : Editor
{
    private void OnSceneGUI()
    {
        Zipline zipline = target as Zipline;
        if (zipline.point1 == null || zipline.point2 == null) return;

        Handles.DrawLine(zipline.point1.transform.position, zipline.point2.transform.position, 5f);
    }
}
