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

        modelImporter.animationType = ModelImporterAnimationType.Generic;
        modelImporter.avatarSetup = ModelImporterAvatarSetup.NoAvatar;
        modelImporter.skinWeights = ModelImporterSkinWeights.Standard;
        
    }

#endregion
}
