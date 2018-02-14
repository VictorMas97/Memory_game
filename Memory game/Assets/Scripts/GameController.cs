using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [SerializeField] private GameObject[] tokens;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GUIText successCountText;
    [SerializeField] private GUIText movCountText;
    [SerializeField] private GUIText gameOverText;

    private int rows = 4;
    private int columns = 6;
    private int movementCount;
    private int momentaryMovementCount;
    private int successCount;
    private GameObject[,] tokensBoard;
    private GameObject instance;
    private RaycastHit2D auxTarget;//Crea un objeto objetivo de laser
    private TokenController token1 = null;
    private TokenController token2 = null;


    void Start ()
    {
        gameOverText.text = "";
        restartButton.SetActive(false);
        exitButton.SetActive(false);

        movementCount = 0;
        momentaryMovementCount = 0;
        successCount = 0;

        UpdateMovementCount();
        UpdateSuccessCount();
        boardCreation();
        barkTokens();
        printBoard();        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Cuando el botón izquierdo del ratón este prsionado
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Crea el laser
            RaycastHit2D target = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity); //Crea un objeto objetivo de laser

            Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.magenta); //Dibuja un laser magenta desde su origen y con direccción

            switch (momentaryMovementCount) //Crea el objeto en la posición y rotación dicha
            {
                case 0:
                    if (target) //Si el laser choca con algun objeto llama al metodo para realizar la animación 
                    {

                        if (target.transform.rotation == Quaternion.identity) //Si el la rotación del objetivo es (0,0,0), actualiza el marcador
                        {
                            token1 = target.transform.GetComponent<TokenController>();
                            token1.flip();
                            momentaryMovementCount++;
                            movementCount++;
                            UpdateMovementCount();
                            auxTarget = target;                        
                        }
                    }

                    break;
                case 1:

                    if (target) //Si el laser choca con algun objeto llama al metodo para realizar la animación 
                    {                        
                       
                        if (target.transform.rotation == Quaternion.identity) //Si el la rotación del objetivo es (0,0,0), actualiza el marcador
                        {
                            token2 = target.transform.GetComponent<TokenController>();
                            token2.flip();
                            movementCount++;
                            UpdateMovementCount();
                            checkTokens(auxTarget, target);
                            momentaryMovementCount = 0;
                        }
                    }

                    break;
            }
        }

        if (successCount == 12) //Condicíon de victoria: Si todas el contador de aciertor llega a 12 están dadas las vueltas
        {
            GameOver();
        }
    }

    void boardCreation () //Se crea el tablero
    {
        tokensBoard = new GameObject[rows, columns]; //Se crea una matriz de 2D con las filas y columnas que posee el tablero
        int assignedNnumber = 0;         

        for (int rowsCounter = 0; rowsCounter < rows; rowsCounter++) //Bucle que recorre las filas
        {
            for (int columsCounter = 0; columsCounter < columns; columsCounter++) //Bucle que recore las columnas
            {
                tokensBoard[rowsCounter, columsCounter] = tokens[assignedNnumber];

                if (columsCounter % 2 != 0) //Si la posición de las columnas son impares la variable "assignedNnumber" incrementa en 1
                {
                    assignedNnumber++;
                }
            }
        }
	}

    void barkTokens() //Se barajan las fichas
    {
        GameObject temporaryToken = null;
        int posRows1 = 0;
        int posRows2 = 0;
        int posColums1 = 0;
        int posColums2 = 0;

        for (int i = 0; i < 200; i++) //Bucle que realiza 200 vueltas
        {
            posRows1 = Random.Range(0, tokensBoard.GetLength(0));
            posRows2 = Random.Range(0, tokensBoard.GetLength(0));
            posColums1 = Random.Range(0, tokensBoard.GetLength(1));
            posColums2 = Random.Range(0, tokensBoard.GetLength(1));
            temporaryToken = tokensBoard[posRows1, posColums1]; //Guardamos lo que hay en la posición random 1 en la variable "temporaryNumber"
            tokensBoard[posRows1, posColums1] = tokensBoard[posRows2, posColums2]; //Guardamos lo que hay en la posición random 2 en la random 1
            tokensBoard[posRows2, posColums2] = temporaryToken; //Guardamos lo que hay en la variable "temporaryNumber" en la posición random 2
        }
    }

    void printBoard () //Se pinta el tablero de forma gráfica
    {
        Vector3 tokenPosition = new Vector3(-25, 15, 0); //Creamos un vector 3 que indica la posición de la ficha

        for (int rowsCounter = 0; rowsCounter < rows; rowsCounter++) //Bucle que recorre las filas
        {
            for (int columsCounter = 0; columsCounter < columns; columsCounter++) //Bucle que recore las columnas
            {
                Instantiate(tokensBoard[rowsCounter, columsCounter], tokenPosition, Quaternion.identity); //Crea el objeto 
                tokenPosition.x += 10;
            }

            tokenPosition.y -= 10;
            tokenPosition.x = -25;
        }
    }

    void checkTokens (RaycastHit2D target1, RaycastHit2D target2) //Comprueba si dos fichas son iguales
    {
        if (target1.transform.name == target2.transform.name) //Si los nobres de los objetivos del rayo son iguales se suma 1 al contador de aciertos
        {
            successCount++;
            UpdateSuccessCount();
        }
        else //Y si no sus nombres no coinciden se dan la vuelta
        {        
            token1.contraryFlip();
            token2.contraryFlip();
        }
    }

    void UpdateMovementCount() //Actualiza el contador de movimientos
    {
        movCountText.text = "Movements: " + movementCount;
    }

    void UpdateSuccessCount() //Actualiza el contador de aciertos
    {
        successCountText.text = "Successes: " + successCount + "/12";
    }

    public void GameOver() //Muestra el texto de "Game Over!"
    {
        gameOverText.text = "Game Over!";
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void Restart() //El juego vuelve a empezar
    {
        SceneManager.LoadScene("Main");
    }

    public void Exit() //Se sale de la aplicación
    {
        Application.Quit();
    }
}