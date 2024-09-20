using System;
using CoreGraphics;
using UIKit;

namespace Xamarin.MarkdownParser.Ios;

public class RoundedBox : UIView
{
    public UIColor FillColor { get; set; }

    public RoundedBox()
    {
        SetColors();
    }

    public RoundedBox(Foundation.NSCoder coder) : base(coder)
    {
        SetColors();
    }

    public RoundedBox(Foundation.NSObjectFlag t) : base(t)
    {
        SetColors();
    }

    public RoundedBox(IntPtr handle) : base(handle)
    {
        SetColors();
    }

    public RoundedBox(CoreGraphics.CGRect frame) : base(frame)
    {
        SetColors();
    }

    private void SetColors()
    {
        BackgroundColor = UIColor.Clear;
    }

    public override void Draw(CGRect rect)
    {
        // draw on screen: https://docs.microsoft.com/en-us/xamarin/ios/platform/graphics-animation-ios/core-graphics
        var rectanglePath = UIBezierPath.FromRoundedRect(rect, 50.0f);

        FillColor?.SetFill();

        rectanglePath.Fill();
    }
}