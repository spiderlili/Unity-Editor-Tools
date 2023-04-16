using UnityEngine;
using UnityEditor;

public class EnvironmentArtPostProcessor : AssetPostprocessor
{
#region Paths

    private const string ProjectTexturesPath = "Assets/Textures/";
    private const string ProjectEnvironmentsPath = "Assets/Models/Environment/";

#endregion

#region ModelImporters

    private void OnPreprocessModel()
    {
        if (!assetPath.Contains(ProjectEnvironmentsPath)) {
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
        modelImporter.importTangents = ModelImporterTangents.None;
        modelImporter.swapUVChannels = false;
        modelImporter.generateSecondaryUV = false;
        
        modelImporter.animationType = ModelImporterAnimationType.None;
        
        // Material import settings
        modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportStandard; // Import materials for normal models
        modelImporter.useSRGBMaterialColor = true; // In Gamma space
        modelImporter.materialLocation = ModelImporterMaterialLocation.External; // Create Material folder at the same level as model
        modelImporter.materialName = ModelImporterMaterialName.BasedOnTextureName;
        modelImporter.materialSearch = ModelImporterMaterialSearch.Local;
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
    
#endregion
}
