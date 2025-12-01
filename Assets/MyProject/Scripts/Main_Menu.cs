using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // <--- Importante para trocar de cena!

public class MainMenu : MonoBehaviour
{
    public string nomeDaCenaDoJogo = "SampleScene"; // O nome EXATO da sua cena de jogo
    public GameObject painelCreditos; // Arraste o painel de créditos aqui

    public void Jogar()
    {
        // Carrega a cena do jogo
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    public void AbrirCreditos()
    {
        painelCreditos.SetActive(true);
    }

    public void FecharCreditos()
    {
        painelCreditos.SetActive(false);
    }

    public void SairDoJogo()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }
}
