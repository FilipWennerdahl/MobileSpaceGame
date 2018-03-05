using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper{


    public static float UnitToPixel(float units) {
        return (Camera.main.pixelHeight / (2 * Camera.main.orthographicSize)) * units;
    }

    public static float PixelToUnit(float pixels) {
        return ((2 * Camera.main.orthographicSize) / Camera.main.pixelHeight) * pixels;
    }

    public static float UnitScreenWidth() {
        return PixelToUnit(Screen.width);
    }

    public static float UnitScreenHeight() {
        return PixelToUnit(Screen.height);
    }

}
