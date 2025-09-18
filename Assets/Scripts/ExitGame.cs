using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Chamado pelo botão "Exit Game"
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");

        // Fecha o jogo
        Application.Quit();

        // No editor, apenas para testar
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}