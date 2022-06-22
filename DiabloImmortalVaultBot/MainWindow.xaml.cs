﻿namespace DiabloImmortalVaultBot;

public partial class MainWindow
{
    private readonly AppSettings _appSettings;

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


    public MainWindow(IOptions<AppSettings> settings)
    {
        InitializeComponent();
        _appSettings = settings.Value;
    }

    private void BtnStartClick(object sender, RoutedEventArgs e)
    {
        Task.Run(ClickBot);
    }

    private void ClickBot()
    {
        SetCursorPos(_appSettings.XPos, _appSettings.YPos);
        while (true)
        {
            try
            {
                if(_appSettings.ForcerCursorPosition)
                    SetCursorPos(_appSettings.XPos, _appSettings.YPos);

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

    private Color GetPixelColor()
    {
        var bitmap = new Bitmap((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);
        var graphics = Graphics.FromImage(bitmap);
        graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
        var currentPixelColor = bitmap.GetPixel(_appSettings.XPos, _appSettings.YPos);
        return currentPixelColor;
    }

    private static void Click()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }
}