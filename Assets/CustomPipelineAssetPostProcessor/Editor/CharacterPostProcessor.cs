using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CharacterPostProcessor : AssetPostprocessor
{
#region Paths
    
    private const string ProjectCharactersPath = "Assets/Models/Characters/"; // Special settings for normal maps
    
#endregion
    
#region ModelImporters

    private void OnPreprocessModel()
    {
        if (!assetPath.Contains(ProjectCharactersPath)) {
            return;
        }
        
        ModelImporter modelImporter = (ModelImporter)assetImporter;
        string fileAssetPathName = Path.GetFileName(assetPath);
        // modelImporter.globalScale = 1f; // Set in DCC
        modelImporter.useFileScale = true;
        modelImporter.importBlendShapes = false;
        modelImporter.importVisibility = false;
        modelImporter.importCameras = false;
        modelImporter.importLights = false;
        modelImporter.preserveHierarchy = false;
        modelImporter.sortHierarchyByName = false;
        modelImporter.meshCompression = ModelImporterMeshCompression.Medium;
        modelImporter.isReadable = false;
        modelImporter.optimizeMeshPolygons = true; // Optimise polygon order
        modelImporter.optimizeMeshVertices = true; // Optimise vertex order
        modelImporter.addCollider = false;
        modelImporter.keepQuads = false;
        modelImporter.weldVertices = true;
        modelImporter.indexFormat = ModelImporterIndexFormat.Auto;
        modelImporter.importNormals = ModelImporterNormals.Import;
        modelImporter.importBlendShapeNormals = modelImporter.importNormals;
        modelImporter.normalCalculationMode = ModelImporterNormalCalculationMode.AreaAndAngleWeighted;
        modelImporter.normalSmoothingSource = ModelImporterNormalSmoothingSource.PreferSmoothingGroups;
        modelImporter.normalSmoothingAngle = 60;
        modelImporter.importTangents = ModelImporterTangents.CalculateMikk;
        modelImporter.swapUVChannels = false;
        modelImporter.generateSecondaryUV = false;

        // Rig settings
        modelImporter.animationType = ModelImporterAnimationType.Generic;
        modelImporter.avatarSetup = ModelImporterAvatarSetup.NoAvatar;
        modelImporter.skinWeights = ModelImporterSkinWeights.Standard;
        
        // Animation settings
        if (fileAssetPathName.Contains("@")) {
            modelImporter.importConstraints = false;
            modelImporter.importAnimation = true;
            modelImporter.resampleCurves = true;
            modelImporter.animationCompression = ModelImporterAnimationCompression.Optimal;
            modelImporter.importAnimatedCustomProperties = false;
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.None; // Do not import materials for animation
        } else {
            modelImporter.importConstraints = false;
            modelImporter.importAnimation = false;
            
            // Material import settings
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportStandard; // Import materials for normal models
            modelImporter.useSRGBMaterialColor = true; // In Gamma space
            modelImporter.materialLocation = ModelImporterMaterialLocation.External; // Create Material folder at the same level as model
            modelImporter.materialName = ModelImporterMaterialName.BasedOnTextureName;
            modelImporter.materialSearch = ModelImporterMaterialSearch.Local;

        }
    }

#endregion
    
#region MaterialImporters

    // OnPostprocessMaterial is only triggered when the material is created for the first time
    private void OnPostprocessMaterial(Material material)
    {
        Shader PBRCharacterShader = Shader.Find("Unlit/PBR");
        if (PBRCharacterShader != null) {
            material.shader = PBRCharacterShader;
        }
    }

#endregion
}
