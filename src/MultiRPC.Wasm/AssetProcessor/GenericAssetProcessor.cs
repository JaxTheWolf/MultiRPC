﻿using System;
using MultiRPC.Core;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;

namespace MultiRPC.AssetProcessor
{
    /// <summary>
    /// Processor with a common <see cref="IAssetProcessor.ProcessAsset(Stream, object[])"/>, if you need to use a custom <see cref="IAssetProcessor.ProcessAsset(Stream, object[])"/> that can't be made by changing <see cref="MakeImageSource(Stream, object[])"/> then inheritance from <see cref="IAssetProcessor"/>
    /// </summary>
    class GenericAssetProcessor : IAssetProcessor
    {
        public const string DefaultExtension = "png";

        public static IFileSystemAccess FileSystem { get; } = ServiceManager.ServiceProvider.GetService<IFileSystemAccess>();

        //TODO: Add some kind of caching
        public virtual string AssetTarget => "N/A";

        public virtual Task<Stream> GetAsset(string assetPath, params object[] args) => throw new NotImplementedException();

        public async Task<object> ProcessAsset(Stream assetStream, params object[] args) => await MakeImageSource(assetStream, args);

        public virtual async Task<ImageSource> MakeImageSource(Stream assetStream, params object[] args)
        {
            // Set the SVG source to the selected file and give it the size of the button
            BitmapImage bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(assetStream);
            if (args?.Length == 2)
            {
                bitmapImage.DecodePixelHeight = (int)args[0];
                bitmapImage.DecodePixelWidth = (int)args[1];
            }

            return bitmapImage;
        }
    }
}
