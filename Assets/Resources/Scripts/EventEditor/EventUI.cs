using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUI : MonoBehaviour
{
    static public Transform cv_transform;
    static private Transform search_window_transform;
    static Transform v3_constructor;
    static Canvas cv;
    //
    public static GameObject clicked_object;
    //
    public static int menu_level = 1;
    static GameObject[] menu_lvl;
    //
    bool isEditorActivated = false;

    public void Start()
    {
        if (cv == null)
        {
            //v3_constructor = GameObject.Find("V3_Constructor").transform;
            GameObject cv_obj = GameObject.Find("Canvas");
            cv_transform = cv_obj.transform;
            cv = cv_obj.GetComponent<Canvas>();
        }

        menu_lvl = new GameObject[gameObject.transform.childCount];
        int index = 0;
        foreach (Transform child in gameObject.transform)
        {
            menu_lvl[index] = child.gameObject;
            index += 1;
        }
        //menu level layout's parent objects^

        SearchWindow.Init(menu_lvl[0]);
        ContextMenu.Init();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E)) isEditorActivated = !isEditorActivated;

        if (isEditorActivated)
        {
            SearchWindow.UpdatePointerValue();
            SearchWindow.Search();
            SearchWindow.UpdateText();

            if (Input.GetMouseButtonUp(0))
            {
                if (CheckIfMouseCursorHoverOverCanvasObject() && SearchWindow.selected_obj == null && SearchWindow.choices.Count > 0)
                {
                    SearchWindow.Select();
                    clicked_object = null;
                }
                else CheckIfMouseCursorHoverOverObject();
            }
            if (Input.GetMouseButton(0) && EventUI.clicked_object != null)
                ContextMenu.SetActive(true);
        }
    }

    static public void DecreaseMenuLevel()
    {
        if (menu_level == 1) SearchWindow.selected_obj = null;
        //
        int level;
        level = menu_level - 1;
        menu_lvl[menu_level].SetActive(false);
        menu_lvl[level].SetActive(true);
        menu_lvl[level].transform.position = menu_lvl[menu_level].transform.position;
        menu_level = level;
    }
    static public void IncreaseMenuLevel()
    {
        int level;
        level = menu_level + 1;
        menu_lvl[menu_level].SetActive(false);
        menu_lvl[level].SetActive(true);
        menu_lvl[level].transform.position = menu_lvl[menu_level].transform.position;
        menu_level = level;
    }

    static public void CheckIfMouseCursorHoverOverObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider == null) clicked_object = null;
        else clicked_object = hit.collider.gameObject;
    }
    static public bool CheckIfMouseCursorHoverOverCanvasObject()
    {
        UnityEngine.EventSystems.PointerEventData ptr_e_data = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        ptr_e_data.position = Input.mousePosition;
        List<UnityEngine.EventSystems.RaycastResult> raycast_result = new List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(ptr_e_data, raycast_result);
        return raycast_result.Count > 0;
    }

    public static class ContextMenu
    {
        public static GameObject parent;
        static UnityEngine.UI.Button button;
        public static void SetActive(bool state)
        {
            parent.SetActive(state);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            parent.transform.position = ray.GetPoint(-parent.transform.parent.transform.position.z);
            //parent.transform.localPosition -= Vector3.forward * parent.transform.localPosition.z;
        }
        public static void Init()
        {
            parent = GameObject.Find("ContextMenu");
            //parent.transform.GetComponentsInChildren(typeof(Transform));
        }
    }

    public static class SearchWindow
    {
        static UnityEngine.UI.InputField inputField;
        static UnityEngine.UI.Text text;
        //
        public static int pointer = 0;
        //
        static int search_depth = 0;
        static bool isByTypeSearch = false;
        //state^
        public static List<string> choices = new List<string>();
        static Component selected_component = null;
        public static GameObject selected_obj = null;
        //temp^
        public static void Init(GameObject child_source)
        {
            inputField = child_source.transform.Find("InputField").GetComponent<UnityEngine.UI.InputField>();
            text = child_source.transform.Find("SearchedText").GetComponent<UnityEngine.UI.Text>();
        }
        public static void ChangeSearchType() { isByTypeSearch = !isByTypeSearch; }
        static public void UpdatePointerValue()
        {
            int menu_pts = choices.Count;
            if (menu_pts < 1) return;
            pointer -= (int)Input.mouseScrollDelta.y;
            pointer = (pointer < 0) ? pointer + menu_pts : pointer;
            pointer = (pointer >= menu_pts) ? pointer % menu_pts : pointer;
        }
        public static void UpdateText()
        {
            if (choices.Count <= 1) text.text = "no matches";
            else
            {
                text.text = "";
                int range = 3;

                for (int i = pointer - range; i <= pointer + range; ++i)
                {
                    int _i = i;
                    if (i <= 0) { text.text += "\n"; continue; }
                    if (i > choices.Count - 1) break;

                    if (Mathf.Abs(i - pointer) < 1) text.text += ">";
                    if (Mathf.Abs(i - pointer) < 2) text.text += " ";
                    if (Mathf.Abs(i - pointer) < 3) text.text += "  ";
                    text.text += choices[_i] + "\n";
                }
            }
        }
        static public void Select()
        {
            if (selected_obj == null) selected_obj = GameObject.Find(choices[pointer]);
            else if (selected_component == null) selected_component =
                selected_obj.GetComponent(System.Type.GetType(choices[pointer]));
            //else eventEditor...(choices[pointer])
            IncreaseMenuLevel();
        }
        static public void Search()
        {
            string input = inputField.text;
            if (search_depth == 0 && isByTypeSearch) SearchObjectByComponent(System.Type.GetType(input));
            else if (search_depth == 0) SearchObjectByName(input);
            else if (search_depth == 1 && isByTypeSearch) SearchComponent();
        }
        static void SearchObjectByName(string name)
        {
            GameObject[] obj = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            choices.Clear();
            for (int i = 0; i < obj.Length; ++i)
                if (obj[i].name.Contains(name)) choices.Add(obj[i].name);
        }
        static void SearchObjectByComponent(System.Type type)
        {
            Component[] components = MonoBehaviour.FindObjectsOfType(type) as Component[];
            choices.Clear();
            for (int i = 0; i < components.Length; ++i) choices.Add(components[i].gameObject.name);
        }
        //depth 0
        static void SearchObjectByEssential(GameObject gameObject = null) //excluding from choices
        {
            GameObject game_obj = (gameObject == null) ? EventUI.clicked_object : gameObject;
            //
            Component[] comp = game_obj.GetComponents(typeof(Component));
            for (int i0 = 0; i0 < choices.Count; ++i0)
            {
                Component[] comp_other = GameObject.Find(choices[i0]).GetComponents(typeof(Component));
                if (comp.Length != comp_other.Length) { choices.RemoveAt(i0); continue; }
                //
                for (int i1 = 0; i1 < choices.Count; ++i1)
                {
                    if (comp[i1].name != comp_other[i1].name)
                        choices.RemoveAt(i0);
                }
            }
        }
        //depth 1
        static void SearchComponent()
        {
            Component[] components = selected_obj.GetComponents(typeof(Component));
            choices.Clear();
            for (int i = 0; i < components.Length; ++i)
                choices.Add(components[i].name);
        }
        //depth 2
        static void SearchField(string name) { }
        static void SearchMethod(string name) { }
        //depth 3
    }
}

