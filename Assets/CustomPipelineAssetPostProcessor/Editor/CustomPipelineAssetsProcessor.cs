using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CustomPipelineAssetsProcessor : AssetPostprocessor
{
#region Paths

    private const string ProjectTexturesPath = "Assets/Textures";
    private const string ProjectCharactersTexturesPath = "Assets/Textures/Characters"; // Special settings for normal maps

#endregion

#region ModelImporters

    private void OnPreprocessModel()
    {
        ModelImporter modelImporter = (ModelImporter)assetImporter;
        modelImporter.globalScale = 1f;
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
        modelImporter.importTangents = ModelImporterTangents.None;
        modelImporter.swapUVChannels = false;
        modelImporter.generateSecondaryUV = false;
    }

#endregion
    
#region TextureImporters
    
    private void OnPreprocessTexture()
    {
        if (!assetPath.Contains(ProjectTexturesPath)) {
            return;
        }

        TextureImporter importer = (TextureImporter)assetImporter;
        importer.textureType = TextureImporterType.Default;
        importer.npotScale = TextureImporterNPOTScale.ToNearest;
        importer.isReadable = false; // Optimistaion
        importer.streamingMipmaps = false;
        importer.mipmapEnabled = false; // Disable mipmaps for 2D games, enable mipmaps for 3D games
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.anisoLevel = 1; // No filtering applied 
        importer.maxTextureSize = 1024;

        // Apply specific texture compression settings per platform
        TextureImporterPlatformSettings textureImporterPlatformSettingsDefault = importer.GetDefaultPlatformTextureSettings(); // Get default settings
        textureImporterPlatformSettingsDefault.maxTextureSize = 1024;
        textureImporterPlatformSettingsDefault.resizeAlgorithm = TextureResizeAlgorithm.Bilinear;
        textureImporterPlatformSettingsDefault.format = TextureImporterFormat.Automatic;
        textureImporterPlatformSettingsDefault.crunchedCompression = true;
        textureImporterPlatformSettingsDefault.compressionQuality = 100;
        
        TextureImporterPlatformSettings textureImporterPlatformSettingsStandalone = importer.GetPlatformTextureSettings("Standalone"); // Get PC / Apple default settings
        textureImporterPlatformSettingsStandalone.overridden = true;
        textureImporterPlatformSettingsStandalone.maxTextureSize = 2048;
        textureImporterPlatformSettingsStandalone.resizeAlgorithm = TextureResizeAlgorithm.Bilinear;
        textureImporterPlatformSettingsStandalone.format = TextureImporterFormat.ARGB32; // Uncompressed
        textureImporterPlatformSettingsStandalone.crunchedCompression = false;
        
        importer.SetPlatformTextureSettings(textureImporterPlatformSettingsStandalone);

        // importer.textureCompression = TextureImporterCompression.CompressedHQ; // Only works when format is set to automatic - NOT recommended!

        // sRGBTexture setting does not matter if in Gamma space, default should be true
        // docs.unity3d.com/Manual/LinearRendering-LinearTextures.html
        importer.sRGBTexture = true;
        
        if (importer.DoesSourceTextureHaveAlpha()) {
            importer.alphaSource = TextureImporterAlphaSource.FromInput;
            importer.alphaIsTransparency = true;

        } else {
            importer.alphaSource = TextureImporterAlphaSource.None;
        }
    }

    private void CharacterTexturesPostProcessor()
    {
        if (!assetPath.Contains(ProjectCharactersTexturesPath)) {
            return;
        }

        TextureImporter importer = (TextureImporter)assetImporter;
        
        // Requires naming convention to contain "_Normal" suffix to apply the right settings to normal maps
        string name = Path.GetFileName(assetPath);
        if (name.Contains("_Normal")) {
            importer.textureType = TextureImporterType.NormalMap;
        } else {
            importer.textureType = TextureImporterType.Default;
        }
        
        // Apply specific texture compression settings per platform
        TextureImporterPlatformSettings textureImporterPlatformSettingsDefault = importer.GetDefaultPlatformTextureSettings(); // Get default settings
        textureImporterPlatformSettingsDefault.maxTextureSize = 512;
        textureImporterPlatformSettingsDefault.resizeAlgorithm = TextureResizeAlgorithm.Bilinear;
        textureImporterPlatformSettingsDefault.format = TextureImporterFormat.Automatic;
        textureImporterPlatformSettingsDefault.crunchedCompression = true;
        textureImporterPlatformSettingsDefault.compressionQuality = 100;
        
        TextureImporterPlatformSettings textureImporterPlatformSettingsStandalone = importer.GetPlatformTextureSettings("Standalone"); // Get PC / Apple default settings
        textureImporterPlatformSettingsStandalone.overridden = true;
        textureImporterPlatformSettingsStandalone.maxTextureSize = 512;
        textureImporterPlatformSettingsStandalone.resizeAlgorithm = TextureResizeAlgorithm.Bilinear;
        textureImporterPlatformSettingsStandalone.format = TextureImporterFormat.RGBA32; // Uncompressed
        textureImporterPlatformSettingsStandalone.crunchedCompression = false;
        
        importer.SetPlatformTextureSettings(textureImporterPlatformSettingsStandalone);
        
        importer.npotScale = TextureImporterNPOTScale.ToNearest;
        importer.isReadable = false; // Optimistaion
        importer.streamingMipmaps = false;
        importer.mipmapEnabled = false; // Disable mipmaps for 2D games, enable mipmaps for 3D games
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.anisoLevel = 1; // No filtering applied 

        // sRGBTexture setting does not matter if in Gamma space, default should be true
        // docs.unity3d.com/Manual/LinearRendering-LinearTextures.html
        importer.sRGBTexture = true;
        
        importer.alphaSource = importer.DoesSourceTextureHaveAlpha() ? TextureImporterAlphaSource.FromInput : TextureImporterAlphaSource.None;
        importer.alphaIsTransparency = false; // Use alpha as black and white map info channel
    }
    
#endregion
}
