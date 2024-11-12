using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public enum Algorithm { Perlin, Automata, Graphs };
    public Algorithm CurrentAlgorithm = Algorithm.Perlin;

    [Header("Perlin Noise")]
    public GameObject PerlinNoise_SettingsTab;
    public GameObject PerlinNoise_MapTab;
    public GameObject PerlinNoise_NoiseTab;

    [Header("Celular Automata")]
    public GameObject CelularAutomata_SettingsTab;
    public GameObject CelularAutomata_MapTab;
    public GameObject CelularAutomata_ParametersTab;

    [Header("Procedual Graphs")]
    public GameObject ProcedualGraphs_SettingsTab;
    public GameObject ProcedualGraphs_MapTab;
    public GameObject ProcedualGraphs_NoiseTab;
    public GameObject ProcedualGraphs_ParametersTab;

    public TMP_InputField ProcedualGraphs_SeedInput;
    public int ProcedualGraphs_Seed;
    public TMP_InputField ProcedualGraphs_MapSizeInput;
    public int ProcedualGraphs_MapSize;
    public TMP_InputField ProcedualGraphs_CavernCountInput;
    public int ProcedualGraphs_CavernCount;
    public TMP_InputField ProcedualGraphs_DistanceBetweenCavernsInput;
    public int ProcedualGraphs_DistanceBetweenCaverns;

    public void Start()
    {
        ProcedualGraphs_SeedInput.text = ProcedualGraphs_Seed.ToString();
        ProcedualGraphs_MapSizeInput.text = ProcedualGraphs_MapSize.ToString();
        ProcedualGraphs_CavernCountInput.text = ProcedualGraphs_CavernCount.ToString();
        ProcedualGraphs_DistanceBetweenCavernsInput.text = ProcedualGraphs_DistanceBetweenCaverns.ToString();
    }

    public void SwitchAlgorithm(TMP_Dropdown dropdown)
    {
        int newAlgorithm = dropdown.value;
        if ((int)CurrentAlgorithm == newAlgorithm) return;
        CurrentAlgorithm = (Algorithm)(newAlgorithm);

        switch (CurrentAlgorithm)
        {
            case Algorithm.Perlin:
                PerlinNoise_SettingsTab.SetActive(true);
                CelularAutomata_SettingsTab.SetActive(false);
                ProcedualGraphs_SettingsTab.SetActive(false);
                break;
            case Algorithm.Automata:
                PerlinNoise_SettingsTab.SetActive(false);
                CelularAutomata_SettingsTab.SetActive(true);
                ProcedualGraphs_SettingsTab.SetActive(false);
                break;
            case Algorithm.Graphs:
                PerlinNoise_SettingsTab.SetActive(false);
                CelularAutomata_SettingsTab.SetActive(false);
                ProcedualGraphs_SettingsTab.SetActive(true);
                break;
        }
    }


    //Perlin Noise Menu

    public void PerlinNoiseSwitchTab(int Number)
    {
        switch (Number)
        {
            case 0:
                PerlinNoise_MapTab.SetActive(true);
                PerlinNoise_NoiseTab.SetActive(false);
                break;
            case 1:
                PerlinNoise_MapTab.SetActive(false);
                PerlinNoise_NoiseTab.SetActive(true);
                break;
        }
    }


    //Celular Automata Menu

    public void CelularAutomataSwitchTab(int Number)
    {
        switch (Number)
        {
            case 0:
                CelularAutomata_MapTab.SetActive(true);
                CelularAutomata_ParametersTab.SetActive(false);
                break;
            case 1:
                CelularAutomata_MapTab.SetActive(false);
                CelularAutomata_ParametersTab.SetActive(true);
                break;
        }
    }


    //Procedual Graphs Menu

    public void ProcedualGraphsSwitchTab(int Number)
    {
        switch (Number)
        {
            case 0:
                ProcedualGraphs_MapTab.SetActive(true);
                ProcedualGraphs_NoiseTab.SetActive(false);
                ProcedualGraphs_ParametersTab.SetActive(false);
                break;
            case 1:
                ProcedualGraphs_MapTab.SetActive(false);
                ProcedualGraphs_NoiseTab.SetActive(true);
                ProcedualGraphs_ParametersTab.SetActive(false);
                break;
            case 2:
                ProcedualGraphs_MapTab.SetActive(false);
                ProcedualGraphs_NoiseTab.SetActive(false);
                ProcedualGraphs_ParametersTab.SetActive(true);
                break;
        }
    }

    public void ProcedualGraphsUpdateValues()
    {
        int TempSeed = int.Parse(ProcedualGraphs_SeedInput.text);
        if (TempSeed <= int.MinValue || TempSeed >= int.MaxValue)
            ProcedualGraphs_SeedInput.text = ProcedualGraphs_Seed.ToString();
        else
            ProcedualGraphs_Seed = TempSeed;

        int TempMapSize = int.Parse(ProcedualGraphs_MapSizeInput.text);
        if (TempMapSize <= 0 || TempMapSize >= 50000)
            ProcedualGraphs_MapSizeInput.text = ProcedualGraphs_MapSize.ToString();
        else
            ProcedualGraphs_MapSize = TempMapSize;

        int TempCavernCount = int.Parse(ProcedualGraphs_CavernCountInput.text);
        if (TempCavernCount <= 0 || TempCavernCount >= 500)
            ProcedualGraphs_CavernCountInput.text = ProcedualGraphs_CavernCount.ToString();
        else
            ProcedualGraphs_CavernCount = TempCavernCount;

        int TempDistanceBetweenCaverns = int.Parse(ProcedualGraphs_DistanceBetweenCavernsInput.text);
        if (TempDistanceBetweenCaverns <= 0 || TempDistanceBetweenCaverns >= 5000)
            ProcedualGraphs_DistanceBetweenCavernsInput.text = ProcedualGraphs_DistanceBetweenCaverns.ToString();
        else
            ProcedualGraphs_DistanceBetweenCaverns = TempDistanceBetweenCaverns;
    }

    public void GenerateProceduralGraphs()
    {
        FindObjectOfType<GraphGenerator>().Generate(ProcedualGraphs_Seed, ProcedualGraphs_MapSize, ProcedualGraphs_CavernCount, ProcedualGraphs_DistanceBetweenCaverns);
    }

    //General

    public void Generate()
    {
        switch(CurrentAlgorithm)
        {
            case Algorithm.Perlin:
                break;
            case Algorithm.Automata:
                break;
            case Algorithm.Graphs:
                GenerateProceduralGraphs();
                break;
        }
    }
}
