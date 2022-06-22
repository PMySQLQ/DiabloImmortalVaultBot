using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiabloImmortalVaultBot;

public partial class MainWindow
{
    private const int XPos = 1800; //enter button
    //private const int XPos = 1350; //find party button
    private const int YPos = 856;

    #region user32.dll
    private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT point);
    #endregion


    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnStartClick(object sender, RoutedEventArgs e)
    {
        SetCursorPos(XPos, YPos);
        Thread.Sleep(50);
        Task.Run(ClickBot);
    }

    private static Color GetPixelColor()
    {
        var bitmap = new Bitmap((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);
        var graphics = Graphics.FromImage(bitmap);
        graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
        var currentPixelColor = bitmap.GetPixel(XPos, YPos);
        return currentPixelColor;
    }

    private static void ClickBot()
    {
        while (true)
        {
            try
            {
                var pixelColor = GetPixelColor();
                var diff = pixelColor.R - pixelColor.G - pixelColor.B;

                if (diff >= 0)
                {
                    Click();
                    break;
                }
            }
            catch
            {
                //ignore
            }
            Thread.Sleep(50);
        }
    }

    private static void Click()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }
}