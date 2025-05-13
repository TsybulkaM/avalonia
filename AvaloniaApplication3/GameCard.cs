// Поместите этот класс внутри вашего namespace AvaloniaApplication3

using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AvaloniaApplication3;

public class GameCard
{
    public string ImageId { get; private set; }
    private Uri ImageSourceUri { get; set; }
    
    private Bitmap ImageBitmap { get; set; }
    
    public bool IsRevealed { get; set; }
    public bool IsMatched { get; set; }
    public Button CardButton { get; set; }

    public GameCard(Uri imageSourceUri)
    {
        ImageSourceUri = imageSourceUri;
        ImageBitmap = new Bitmap(AssetLoader.Open(ImageSourceUri));
        ImageId = imageSourceUri.AbsoluteUri;
        IsRevealed = false;
        IsMatched = false;
    }
    
    public void ShowFace()
    {
        try
        {
            var imageControl = new Image
            {
                Source = ImageBitmap,
                Stretch = Stretch.Uniform
            };
            
            CardButton.Content = new Viewbox
            {
                Child = imageControl,
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.Both
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading img for {ImageId}: {ex.Message}");
            CardButton.Content = "X";
        }
    }
    
    public void ShowBack()
    {
        var textBlock = new TextBlock
        {
            Text = "?",
            FontSize = 48,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        CardButton.Content = new Viewbox
        {
            Child = textBlock,
            Stretch = Stretch.Uniform,
            StretchDirection = StretchDirection.Both
        };
    }
}