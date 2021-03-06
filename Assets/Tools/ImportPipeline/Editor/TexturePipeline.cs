﻿using System.Collections;
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
    private string bgSpriteAssetPath = "Assets/Sprites/Backgrounds";
    private string levelSpriteAssetPath = "Assets/Sprites/LevelPieces";

    private void OnPreprocessTexture()
    {
        Debug.LogFormat("OnPostprocessTexture, The Path is {0}", assetPath);
        if (assetPath.StartsWith(bgSpriteAssetPath))
        {
            PreprocessBgSprites();
        }
        else if (assetPath.StartsWith(levelSpriteAssetPath))
        {
            PreprocessLevelSprites();
        }
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
        //using an instance of the TextureImporterSettings class: access the asset settings & make desired modifications
        TextureImporterSettings texSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(texSettings); //start with ReadTextureSettings() & end with SetTextureSettings() to avoid unexpected results
        texSettings.spriteAlignment = (int)SpriteAlignment.BottomLeft; //how a Sprite's graphic rectangle is aligned with its pivot point
        texSettings.mipmapEnabled = false; //turn off generate mip maps
        texSettings.spriteGenerateFallbackPhysicsShape = false; //turn off generate physics shape
        importer.SetTextureSettings(texSettings);
    }

    private void PreprocessLevelSprites()
    {
        TextureImporter importer = assetImporter as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        TextureImporterSettings texSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(texSettings);
        texSettings.spriteAlignment = (int)SpriteAlignment.Center;
        texSettings.mipmapEnabled = false;
        importer.SetTextureSettings(texSettings);
    }

}
