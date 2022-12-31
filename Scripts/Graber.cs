using UnityEngine;

public class Graber : MonoBehaviour {
    string tagObstacles;
    int PrevColorCount = 0;
    bool firstTransionLeft = true;
    bool firstTransionRight = true;
    bool firstClicked = false;
    bool leftClicked = false;
    bool rightClicked = false;
    bool scrolled = false;
    [SerializeField] float burnFuelRate = 1f;
    [SerializeField] float rotatescale = .5f;
    [SerializeField] float dragSpeed = .5f;

    private GameObject selectedObject;
    RaycastHit hit;
    Vector3 prePositionLeft;
    Vector3 prePositionRight;
    Vector2 prevScrolle;
    Vector2 newScrolle;

    Color prevColor;
    public Material materialSecond;
    public Material materialFirst;

    private void Start() {
        // materials = GetComponent<Material>();
    }

    private void Update() {
        newScrolle = Input.mouseScrollDelta; 

        applySelectingObject();
        
        applymouseClicked();

        applyMouseReleased();

        applyingMouseChange();

        firstClicked = true;
        prevScrolle = newScrolle;
    }

    void applySelectingObject()
    {
        if (Input.GetMouseButtonDown(0) ) {
            if(selectedObject == null) {
                Debug.Log("is null");
                hit = CastRay();
                
                if(hit.collider != null) {
                    tagObstacles = hit.collider.gameObject.tag;
                    if (hit.collider.gameObject.layer != 6) {
                        return;
                    }
                    selectedObject = hit.collider.gameObject;
                    selectedObject.transform.Translate(0,0,-.25f);
                    // if(PrevColorCount == 0){
                    //     PrevColorCount++;
                    //     prevColor = selectedObject.GetComponent<MeshRenderer>().material.color;
                    // }
                    // PrevColorCount = false;
                    // Debug.Log(prevColor);
                    // selectedObject.GetComponent<MeshRenderer>().material.SetColor("_yellow",Color.yellow);

                    firstTransionLeft = true;
                    firstTransionRight = true;
                    firstClicked = false;
                    // Cursor.visible = false;
                }
            }
        }
    }

    void applymouseClicked()
    {
        if(Input.GetMouseButtonDown(0) && firstClicked){
             if(selectedObject != null) {
                Debug.Log("selected is not null");
                hit = CastRay();
                
                if(hit.collider == null) {
                    selectedObject.transform.Translate(0,0,.25f);
                    selectedObject = null;
                    return;
                    // Cursor.visible = false;
                }else{
                    if (!hit.collider.CompareTag(tagObstacles)) {
                        selectedObject.transform.Translate(0,0,.25f);
                        selectedObject = null;
                        return;
                    }
                }
               
                 leftClicked = true;
            } 
        }
        else if(Input.GetMouseButtonDown(1) && firstClicked){
             if(selectedObject != null) {
                Debug.Log("selected is not null");
                hit = CastRay();
                
                if(hit.collider == null) {
                    selectedObject.transform.Translate(0,0,.25f);
                    selectedObject = null;
                    return;
                    // Cursor.visible = false;
                }else{
                    if (!hit.collider.CompareTag(tagObstacles)) {
                        selectedObject.transform.Translate(0,0,.25f);
                        selectedObject = null;
                        return;
                    }
                }
               
                 rightClicked = true;
            } 
        }
        else if( !newScrolle.Equals(prevScrolle) && firstClicked){
             if(selectedObject != null) {
                Debug.Log("scrolled");
                hit = CastRay();
                
                if(hit.collider == null) {
                    selectedObject.transform.Translate(0,0,.25f);
                    selectedObject = null;
                    return;
                    // Cursor.visible = false;
                }else{
                    if (!hit.collider.CompareTag(tagObstacles)) {
                        selectedObject.transform.Translate(0,0,.25f);
                        selectedObject = null;
                        return;
                    }
                }
               
                 scrolled = true;
            } 
        }
    }

    void applyMouseReleased()
    {
        if(Input.GetMouseButtonUp(button: 0) && leftClicked)
        {
            // Debug.Log("mouse released");
            // Debug.Log(message: materials.color);
            // selectedObject.GetComponent<MeshRenderer>().material.SetColor("_origin",Color.blue);
            selectedObject.transform.Translate(0,0,.25f);
            selectedObject = null;
             leftClicked = false;
        }
        else if(Input.GetMouseButtonUp(button: 1) && rightClicked)
        {
            // Debug.Log("mouse released");
            // Debug.Log(message: materials.color);
            // selectedObject.GetComponent<MeshRenderer>().material.SetColor("_origin",Color.blue);
            selectedObject.transform.Translate(0,0,.25f);
            selectedObject = null;
             rightClicked = false;
        }
    }

    void applyingMouseChange(){
        if(selectedObject != null && leftClicked) {
            // Debug.Log("transitioning");
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            if(!firstTransionLeft){
                // Vector3 newPos = (worldPosition - prePositionLeft) * dragSpeed;
                float newPosX = (worldPosition.x - prePositionLeft.x)/10;
                float newPosY = (worldPosition.y - prePositionLeft.y)/10;
                Vector3 newPos = new Vector3(newPosX,newPosY,0);

                // Debug.Log("draging");
                bool dragPossible = burningFuel(newPos.magnitude);
                if(dragPossible)
                {
                    selectedObject.transform.Translate(newPos.x,newPos.y,0);
                    // selectedObject.transform.RotateAround()
                    // selectedObject.transform.position = new Vector3()
                }
            }
            firstTransionLeft = false;
            prePositionLeft = worldPosition;
            
        }
        else if(selectedObject != null && rightClicked){
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            if(!firstTransionRight){
                Vector3 newPos = (worldPosition - prePositionRight) * dragSpeed;
                // float newPosX = (worldPosition.x - prePositionLeft.x)/10;
                // float newPosY = (worldPosition.y - prePositionLeft.y)/10;
                // Vector3 newPos = new Vector3(newPosX,newPosY,0);

                bool dragPossible = burningFuel(newPos.magnitude * 2);
                if(dragPossible)
                {
                    selectedObject.transform.localScale += newPos;
                }
                
            }
            firstTransionRight = false;
            prePositionRight = worldPosition;
        }
        else if(selectedObject != null && scrolled){
            float rotateDegree = (newScrolle.y-prevScrolle.y) * rotatescale;

            bool dragPossible = burningFuel(.5f);
            if(dragPossible)
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                selectedObject.transform.rotation.eulerAngles.x,
                selectedObject.transform.rotation.eulerAngles.y,
                selectedObject.transform.rotation.eulerAngles.z + rotateDegree));
            }
            
            selectedObject.transform.Translate(0,0,.25f);
            selectedObject = null;
             scrolled = false;
        }
    }

    bool burningFuel(float burntFuel){
        // Debug.Log(SavePlayerInfo.isPlayer1Turn + " player1 turn");
        // Debug.Log(SavePlayerInfo.isPlayer2Turn + " player2 turn");

        if(SavePlayerInfo.isPlayer1Turn){
            if(SavePlayerInfo.player2Fuel <= 5){
                return false;
            }
            // Debug.Log( SavePlayerInfo.player2Fuel + " player1 grabing");
            SavePlayerInfo.player2Fuel -= burntFuel * burnFuelRate;
        }else if(SavePlayerInfo.isPlayer2Turn){
            if(SavePlayerInfo.player1Fuel <= 5){
                return false;
            }
            // Debug.Log("player1 grabing");
            SavePlayerInfo.player1Fuel -= burntFuel * burnFuelRate;
        }
        return true;
    }

    private RaycastHit CastRay() {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }
}
