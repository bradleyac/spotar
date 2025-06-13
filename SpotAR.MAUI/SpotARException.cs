using System;

namespace SpotAR.MAUI;

public class SpotARException : ApplicationException
{
    public SpotARException() : base()
    {
    }

    public SpotARException(string? message) : base(message)
    {
    }
}
