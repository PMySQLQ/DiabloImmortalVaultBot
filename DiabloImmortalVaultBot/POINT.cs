using System.Runtime.InteropServices;

namespace DiabloImmortalVaultBot;

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;

    public POINT(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static implicit operator System.Drawing.Point(POINT p) => new(p.X, p.Y);

    public static implicit operator POINT(System.Drawing.Point p) => new(p.X, p.Y);
}