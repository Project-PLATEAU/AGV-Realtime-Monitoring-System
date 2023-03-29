/*         INFINITY CODE         */
/*   https://infinity-code.com   */

using System.Collections.Generic;
using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    /// <summary>
    /// Example of use Online Maps Drawing API.
    /// </summary>
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/DrawingAPI_Example")]
    public class DrawingAPI_Example : MonoBehaviour
    {
        private void Start()
        {
            List<Vector2> line = new List<Vector2>
            {
                //Geographic coordinates
                new Vector2(34.66428012f, 135.3962966f),
                new Vector2(34.6642334316667f, 135.396597438333f),
                new Vector2(34.6642144833333f, 135.396719288333f),
                new Vector2(34.6641684116667f, 135.397017686667f)
            };

            List<Vector2> poly = new List<Vector2>
            {
                //Geographic coordinates
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(2, 2),
                new Vector2(0, 1)
            };

            // Draw line
            OnlineMapsDrawingElementManager.AddItem(new OnlineMapsDrawingLine(line, Color.green, 5));

            // Draw filled transparent poly
            OnlineMapsDrawingElementManager.AddItem(new OnlineMapsDrawingPoly(poly, Color.red, 1, new Color(1, 1, 1, 0.5f)));

            // Draw filled rectangle
            // (position, size, borderColor, borderWidth, backgroundColor)
            OnlineMapsDrawingElementManager.AddItem(new OnlineMapsDrawingRect(new Vector2(2, 2), new Vector2(1, 1), Color.green, 1, Color.blue));
        }
    }
}