using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//AssetPostprocessor is a class that will trigger several events when an asset is imported to the project 
//depending on whether the asset is a texture / 3D model / audio
//goal: set up TextureType, GenerateMipMaps, Pivot when importing textures
public class TexturePipeline : AssetPostprocessor
{
    //detect when a texture is imported. OnPreprocessTexture is triggered before the importing process initiates
    //add code related to configuring the settings of the imported assets to OnPreprocessTexture()
    private void OnPreprocessTexture()
    {
        Debug.LogFormat("OnPostprocessTexture, The Path is {0}", assetPath);
    }

    //OnPostprocessTexture is called until the asset is imported
    void OnPostprocessTexture(Texture2D texture)
    {
        Debug.LogFormat("OnPostprocessTexture, The Path is {0}", assetPath);
    }

    private void PreprocessBgSprites()
    {
        //assetImporter is part of the AssetPostprocessor class: gives access to the properties of the imported assets
        //must cast the assetImporter variable to a TextureImporter when dealing with textures. in other scenarios: use ModelImporter / AudioImporter
        TextureImporter importer = assetImporter as TextureImporter;

        importer.textureType = TextureImporterType.Sprite;
        TextureImporterSettings texSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(texSettings);
        texSettings.spriteAlignment = (int)SpriteAlignment.BottomLeft;
        texSettings.mipmapEnabled = false;
        importer.SetTextureSettings(texSettings);
    }
}
