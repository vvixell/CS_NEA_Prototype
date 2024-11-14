using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public enum Algorithm { Perlin, Automata, Graphs };
    public Algorithm CurrentAlgorithm = Algorithm.Perlin;


    [Header("Perlin Noise")]
    public GameObject PerlinNoise_SettingsTab;
    public GameObject PerlinNoise_MapTab;
    public GameObject PerlinNoise_NoiseTab;

    public TMP_InputField PerlinNoise_SeedInput;
    public int PerlinNoise_Seed;

    public TMP_InputField PerlinNoise_MapSizeInput;
    public int PerlinNoise_MapSize;

    public TMP_InputField PerlinNoise_ScaleInput;
    public float PerlinNoise_Scale;

    public Slider PerlinNoise_OctavesInput;
    public TextMeshProUGUI PerlinNoise_OctavesText;
    public int PerlinNoise_Octaves;

    public Slider PerlinNoise_PersistenceInput;
    public TextMeshProUGUI PerlinNoise_PersistenceText;
    public float PerlinNoise_Persistence;

    public Slider PerlinNoise_LacunarityInput;
    public TextMeshProUGUI PerlinNoise_LacunarityText;
    public float PerlinNoise_Lacunarity;

    public Slider PerlinNoise_CuttOffInput;
    public TextMeshProUGUI PerlinNoise_CuttOffText;
    public float PerlinNoise_CuttOff;

    public Toggle PerlinNoise_FallOffInput;
    public bool PerlinNoise_FallOff;


    [Header("Celular Automata")]
    public GameObject CelularAutomata_SettingsTab;
    public GameObject CelularAutomata_MapTab;
    public GameObject CelularAutomata_ParametersTab;


    [Header("Procedural Graphs")]
    public GameObject ProceduralGraphs_SettingsTab;
    public GameObject ProceduralGraphs_MapTab;
    public GameObject ProceduralGraphs_NoiseTab;
    public GameObject ProceduralGraphs_ParametersTab;
    public GameObject ProceduralGraphs_VisualsTab;

    //Map Tab
    public TMP_InputField ProceduralGraphs_SeedInput;
    public int ProceduralGraphs_Seed;

    public TMP_InputField ProceduralGraphs_MapSizeInput;
    public int ProceduralGraphs_MapSize;

    //Noise Tab

    public TMP_InputField ProceduralGraphs_ScaleInput;
    public float ProceduralGraphs_Scale;

    public Slider ProceduralGraphs_OctavesInput;
    public TextMeshProUGUI ProceduralGraphs_OctavesText;
    public int ProceduralGraphs_Octaves;

    public Slider ProceduralGraphs_PersistenceInput;
    public TextMeshProUGUI ProceduralGraphs_PersistenceText;
    public float ProceduralGraphs_Persistence;

    public Slider ProceduralGraphs_LacunarityInput;
    public TextMeshProUGUI ProceduralGraphs_LacunarityText;
    public float ProceduralGraphs_Lacunarity;

    //Paramaters Tab

    public TMP_InputField ProceduralGraphs_CavernCountInput;
    public int ProceduralGraphs_CavernCount;

    public TMP_InputField ProceduralGraphs_DistanceBetweenCavernsInput;
    public int ProceduralGraphs_DistanceBetweenCaverns;

    public Slider ProceduralGraphs_RandomnessInput;
    public TextMeshProUGUI ProceduralGraphs_RandomnessText;
    public float ProceduralGraphs_Randomness;

    public Toggle ProceduralGraphs_ExistingPathWeightInput;
    public bool ProceduralGraphs_ExistingPathWeight;

    public Slider ProceduralGraphs_ExistingPathWeightMultiplierInput;
    public TextMeshProUGUI ProceduralGraphs_ExistingPathWeightMultiplierText;
    public float ProceduralGraphs_ExistingPathWeightMultiplier;

    //Visuals Tab

    public Toggle ShowDiscSamplingPointsInput;
    public bool ShowDiscSamplingPoints;

    public Toggle ShowRandomCavernsInput;
    public bool ShowRandomCaverns;

    public Toggle ShowTriangulationInput;
    public bool ShowTriangulation;

    public Toggle ShowEdgeWeightsInput;
    public bool ShowEdgeWeights;

    public Toggle ShowCaveLinesInput;
    public bool ShowCaveLines;


    public void Start()
    {;
        PerlinNoise_SeedInput.text = PerlinNoise_Seed.ToString();
        PerlinNoise_MapSizeInput.text = PerlinNoise_MapSize.ToString();
        PerlinNoise_ScaleInput.text = PerlinNoise_Scale.ToString();
        PerlinNoise_OctavesInput.value = PerlinNoise_Octaves;
        PerlinNoise_OctavesText.text = PerlinNoise_Octaves.ToString();
        PerlinNoise_PersistenceInput.value = PerlinNoise_Persistence;
        PerlinNoise_PersistenceText.text = PerlinNoise_Persistence.ToString();
        PerlinNoise_LacunarityInput.value = PerlinNoise_Lacunarity;
        PerlinNoise_LacunarityText.text = PerlinNoise_Lacunarity.ToString();
        PerlinNoise_CuttOffInput.value = PerlinNoise_CuttOff;
        PerlinNoise_CuttOffText.text = PerlinNoise_CuttOff.ToString();
        PerlinNoise_FallOffInput.isOn = PerlinNoise_FallOff;

        ProceduralGraphs_SeedInput.text = ProceduralGraphs_Seed.ToString();
        ProceduralGraphs_MapSizeInput.text = ProceduralGraphs_MapSize.ToString();
        ProceduralGraphs_CavernCountInput.text = ProceduralGraphs_CavernCount.ToString();
        ProceduralGraphs_DistanceBetweenCavernsInput.text = ProceduralGraphs_DistanceBetweenCaverns.ToString();
        ProceduralGraphs_ScaleInput.text = ProceduralGraphs_Scale.ToString();
        ProceduralGraphs_OctavesInput.value = ProceduralGraphs_Octaves;
        ProceduralGraphs_OctavesText.text = ProceduralGraphs_Octaves.ToString();
        ProceduralGraphs_PersistenceInput.value = ProceduralGraphs_Persistence;
        ProceduralGraphs_PersistenceText.text = ProceduralGraphs_Persistence.ToString();
        ProceduralGraphs_LacunarityInput.value = ProceduralGraphs_Lacunarity;
        ProceduralGraphs_LacunarityText.text = ProceduralGraphs_Lacunarity.ToString();
        ProceduralGraphs_RandomnessInput.value = ProceduralGraphs_Randomness;
        ProceduralGraphs_RandomnessText.text = Math.Round(ProceduralGraphs_Randomness, 2, MidpointRounding.AwayFromZero).ToString();
        ProceduralGraphs_ExistingPathWeightInput.isOn = ProceduralGraphs_ExistingPathWeight;
        ProceduralGraphs_ExistingPathWeightMultiplierInput.value = ProceduralGraphs_ExistingPathWeightMultiplier;
        ProceduralGraphs_ExistingPathWeightMultiplierText.text = Math.Round(ProceduralGraphs_ExistingPathWeightMultiplier, 2, MidpointRounding.AwayFromZero).ToString();
        
        ShowDiscSamplingPointsInput.isOn = ShowDiscSamplingPoints;
        ShowRandomCavernsInput.isOn = ShowRandomCaverns;
        ShowTriangulationInput.isOn = ShowTriangulation;
        ShowEdgeWeightsInput.isOn = ShowEdgeWeights;
        ShowCaveLinesInput.isOn = ShowCaveLines;
    }

    public void SwitchAlgorithm(TMP_Dropdown dropdown)
    {
        int newAlgorithm = dropdown.value;
        if ((int)CurrentAlgorithm == newAlgorithm) return;
        CurrentAlgorithm = (Algorithm)(newAlgorithm);

        FindObjectOfType<NoiseAlgo>().DestroyImage();
        FindObjectOfType<GraphGenerator>().DestroyGraph();

        switch (CurrentAlgorithm)
        {
            case Algorithm.Perlin:
                PerlinNoise_SettingsTab.SetActive(true);
                CelularAutomata_SettingsTab.SetActive(false);
                ProceduralGraphs_SettingsTab.SetActive(false);
                break;
            case Algorithm.Automata:
                PerlinNoise_SettingsTab.SetActive(false);
                CelularAutomata_SettingsTab.SetActive(true);
                ProceduralGraphs_SettingsTab.SetActive(false);
                break;
            case Algorithm.Graphs:
                PerlinNoise_SettingsTab.SetActive(false);
                CelularAutomata_SettingsTab.SetActive(false);
                ProceduralGraphs_SettingsTab.SetActive(true);
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

    public void GenerateNoise()
    {
        FindObjectOfType<NoiseAlgo>().Generate(
            PerlinNoise_Seed, PerlinNoise_MapSize,
            new NoiseSettings(PerlinNoise_Scale, PerlinNoise_Octaves, PerlinNoise_Persistence, PerlinNoise_Lacunarity),
            PerlinNoise_CuttOff,
            PerlinNoise_FallOff
            );
    }

    public void PerlinNoiseUpdateValues()
    {
        int TempSeed = int.Parse(PerlinNoise_SeedInput.text);
        if (TempSeed <= int.MinValue || TempSeed >= int.MaxValue)
            PerlinNoise_SeedInput.text = PerlinNoise_Seed.ToString();
        else
            PerlinNoise_Seed = TempSeed;

        int TempMapSize = int.Parse(PerlinNoise_MapSizeInput.text);
        if (TempMapSize <= 0 || TempMapSize >= 50000)
            PerlinNoise_MapSizeInput.text = PerlinNoise_MapSize.ToString();
        else
            PerlinNoise_MapSize = TempMapSize;

        float TempScale = float.Parse(PerlinNoise_ScaleInput.text);
        if (TempScale <= 0 || TempScale >= 500)
            PerlinNoise_ScaleInput.text = PerlinNoise_Scale.ToString();
        else
            PerlinNoise_Scale = TempScale;

        int TempOctaves = (int)PerlinNoise_OctavesInput.value;
        if (TempOctaves <= 0 || TempOctaves >= 7)
            PerlinNoise_OctavesInput.value = PerlinNoise_Octaves;
        else
            PerlinNoise_Octaves = TempOctaves;

        float TempPersistence = PerlinNoise_PersistenceInput.value;
        if (TempPersistence <= 0 || TempPersistence > 4)
            PerlinNoise_PersistenceInput.value = PerlinNoise_Persistence;
        else
            PerlinNoise_Persistence = TempPersistence;

        float TempLacunarity = PerlinNoise_LacunarityInput.value;
        if (TempLacunarity <= 0 || TempLacunarity > 4)
            PerlinNoise_LacunarityInput.value = PerlinNoise_Lacunarity;
        else
            PerlinNoise_Lacunarity = TempLacunarity;

        float TempCuttOff = PerlinNoise_CuttOffInput.value;
        if (TempCuttOff < 0 || TempCuttOff > 1)
            PerlinNoise_CuttOffInput.value = PerlinNoise_CuttOff;
        else
            PerlinNoise_CuttOff = TempCuttOff;

        PerlinNoise_FallOff = PerlinNoise_FallOffInput.isOn;

        PerlinNoise_SeedInput.text = PerlinNoise_Seed.ToString();
        PerlinNoise_MapSizeInput.text = PerlinNoise_MapSize.ToString();
        PerlinNoise_ScaleInput.text = PerlinNoise_Scale.ToString();
        PerlinNoise_OctavesInput.value = PerlinNoise_Octaves;
        PerlinNoise_OctavesText.text = PerlinNoise_Octaves.ToString();
        PerlinNoise_PersistenceInput.value = PerlinNoise_Persistence;
        PerlinNoise_PersistenceText.text = Math.Round(PerlinNoise_Persistence,2).ToString();
        PerlinNoise_LacunarityInput.value = PerlinNoise_Lacunarity;
        PerlinNoise_LacunarityText.text = Math.Round(PerlinNoise_Lacunarity,2).ToString();
        PerlinNoise_CuttOffInput.value = PerlinNoise_CuttOff;
        PerlinNoise_CuttOffText.text = Math.Round(PerlinNoise_CuttOff,2).ToString();
        PerlinNoise_FallOffInput.isOn = PerlinNoise_FallOff;
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
                ProceduralGraphs_MapTab.SetActive(true);
                ProceduralGraphs_NoiseTab.SetActive(false);
                ProceduralGraphs_ParametersTab.SetActive(false);
                ProceduralGraphs_VisualsTab.SetActive(false);
                break;
            case 1:
                ProceduralGraphs_MapTab.SetActive(false);
                ProceduralGraphs_NoiseTab.SetActive(true);
                ProceduralGraphs_ParametersTab.SetActive(false);
                ProceduralGraphs_VisualsTab.SetActive(false);
                break;
            case 2:
                ProceduralGraphs_MapTab.SetActive(false);
                ProceduralGraphs_NoiseTab.SetActive(false);
                ProceduralGraphs_ParametersTab.SetActive(true);
                ProceduralGraphs_VisualsTab.SetActive(false);
                break;
            case 3:
                ProceduralGraphs_MapTab.SetActive(false);
                ProceduralGraphs_NoiseTab.SetActive(false);
                ProceduralGraphs_ParametersTab.SetActive(false);
                ProceduralGraphs_VisualsTab.SetActive(true);
                break;
        }
    }

    public void ProcedualGraphsUpdateValues()
    {
        int TempSeed = int.Parse(ProceduralGraphs_SeedInput.text);
        if (TempSeed <= int.MinValue || TempSeed >= int.MaxValue)
            ProceduralGraphs_SeedInput.text = ProceduralGraphs_Seed.ToString();
        else
            ProceduralGraphs_Seed = TempSeed;

        int TempMapSize = int.Parse(ProceduralGraphs_MapSizeInput.text);
        if (TempMapSize <= 0 || TempMapSize >= 50000)
            ProceduralGraphs_MapSizeInput.text = ProceduralGraphs_MapSize.ToString();
        else
            ProceduralGraphs_MapSize = TempMapSize;

        int TempCavernCount = int.Parse(ProceduralGraphs_CavernCountInput.text);
        if (TempCavernCount <= 0 || TempCavernCount >= 500)
            ProceduralGraphs_CavernCountInput.text = ProceduralGraphs_CavernCount.ToString();
        else
            ProceduralGraphs_CavernCount = TempCavernCount;

        int TempDistanceBetweenCaverns = int.Parse(ProceduralGraphs_DistanceBetweenCavernsInput.text);
        if (TempDistanceBetweenCaverns <= 0 || TempDistanceBetweenCaverns >= 5000)
            ProceduralGraphs_DistanceBetweenCavernsInput.text = ProceduralGraphs_DistanceBetweenCaverns.ToString();
        else
            ProceduralGraphs_DistanceBetweenCaverns = TempDistanceBetweenCaverns;

        float TempScale = float.Parse(ProceduralGraphs_ScaleInput.text);
        if (TempScale <= 0 || TempScale > 50000)
            ProceduralGraphs_ScaleInput.text = ProceduralGraphs_Scale.ToString();
        else
            ProceduralGraphs_Scale = TempScale;

        int TempOctaves = (int)ProceduralGraphs_OctavesInput.value;
        if (TempOctaves <= 0 || TempOctaves >= 5)
            ProceduralGraphs_OctavesInput.value = ProceduralGraphs_Octaves;
        else
            ProceduralGraphs_Octaves = TempOctaves;

        float TempPersistence = ProceduralGraphs_PersistenceInput.value;
        if (TempPersistence <= 0 || TempPersistence > 4)
            ProceduralGraphs_PersistenceInput.value = ProceduralGraphs_Persistence;
        else
            ProceduralGraphs_Persistence = TempPersistence;

        float TempLacunarity = ProceduralGraphs_LacunarityInput.value;
        if (TempLacunarity <= 0 || TempLacunarity > 4)
            ProceduralGraphs_LacunarityInput.value = ProceduralGraphs_Lacunarity;
        else
            ProceduralGraphs_Lacunarity = TempLacunarity;

        ShowDiscSamplingPoints = ShowDiscSamplingPointsInput.isOn;
        ShowRandomCaverns = ShowRandomCavernsInput.isOn;
        ShowTriangulation = ShowTriangulationInput.isOn;
        ShowEdgeWeights = ShowEdgeWeightsInput.isOn;
        ShowCaveLines = ShowCaveLinesInput.isOn;
        ProceduralGraphs_RandomnessText.text = Math.Round(ProceduralGraphs_Randomness, 2).ToString();
        ProceduralGraphs_Randomness = ProceduralGraphs_RandomnessInput.value;
        ProceduralGraphs_ExistingPathWeight = ProceduralGraphs_ExistingPathWeightInput.isOn;
        ProceduralGraphs_ExistingPathWeightMultiplier = ProceduralGraphs_ExistingPathWeightMultiplierInput.value;
        ProceduralGraphs_ExistingPathWeightMultiplierText.text = Math.Round(ProceduralGraphs_ExistingPathWeightMultiplier, 2, MidpointRounding.AwayFromZero).ToString();

        ProceduralGraphs_ScaleInput.text = ProceduralGraphs_Scale.ToString();
        ProceduralGraphs_OctavesInput.value = ProceduralGraphs_Octaves;
        ProceduralGraphs_OctavesText.text = ProceduralGraphs_Octaves.ToString();
        ProceduralGraphs_PersistenceInput.value = ProceduralGraphs_Persistence;
        ProceduralGraphs_PersistenceText.text = Math.Round(ProceduralGraphs_Persistence, 2).ToString();
        ProceduralGraphs_LacunarityInput.value = ProceduralGraphs_Lacunarity;
        ProceduralGraphs_LacunarityText.text = Math.Round(ProceduralGraphs_Lacunarity, 2).ToString();

        ProceduralGraphs_SeedInput.text = ProceduralGraphs_Seed.ToString();
        ProceduralGraphs_MapSizeInput.text = ProceduralGraphs_MapSize.ToString();
        ProceduralGraphs_CavernCountInput.text = ProceduralGraphs_CavernCount.ToString();
        ProceduralGraphs_DistanceBetweenCavernsInput.text = ProceduralGraphs_DistanceBetweenCaverns.ToString();
        
    }

    public void GenerateProceduralGraphs()
    {
        FindObjectOfType<GraphGenerator>().Generate(
            ProceduralGraphs_Seed,
            ProceduralGraphs_MapSize,
            ProceduralGraphs_CavernCount,
            ProceduralGraphs_DistanceBetweenCaverns,
            new NoiseSettings(ProceduralGraphs_Scale, ProceduralGraphs_Octaves, ProceduralGraphs_Persistence, ProceduralGraphs_Lacunarity),
            ProceduralGraphs_Randomness,
            ProceduralGraphs_ExistingPathWeight,
            ProceduralGraphs_ExistingPathWeightMultiplier,
            ShowDiscSamplingPoints,
            ShowRandomCaverns,
            ShowTriangulation,
            ShowEdgeWeights,
            ShowCaveLines
            );
    }

    //General

    public void Generate()
    {
        switch(CurrentAlgorithm)
        {
            case Algorithm.Perlin:
                GenerateNoise();
                break;
            case Algorithm.Automata:
                break;
            case Algorithm.Graphs:
                GenerateProceduralGraphs();
                break;
        }
    }
    public void GenerateRandSeed()
    {
        int NewSeed = new System.Random().Next(int.MinValue, int.MaxValue);

        switch (CurrentAlgorithm)
        {
            case Algorithm.Perlin:
                PerlinNoise_Seed = NewSeed;
                PerlinNoise_SeedInput.text = NewSeed.ToString();
                GenerateNoise();
                break;
            case Algorithm.Automata:
                break;
            case Algorithm.Graphs:
                ProceduralGraphs_Seed = NewSeed;
                ProceduralGraphs_SeedInput.text = NewSeed.ToString();
                GenerateProceduralGraphs();
                break;
        }
    }
}
