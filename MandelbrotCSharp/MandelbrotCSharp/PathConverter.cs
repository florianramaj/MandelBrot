using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MandelbrotCSharp
{
    public class PathConverter : IValueConverter
    {
        /// <summary>
        /// This method converts points into path geometry.
        /// </summary>
        /// <param name="value">The points of the function.</param>
        /// <param name="targetType">This input parameter is the target type.</param>
        /// <param name="parameter">A parameter as type object.</param>
        /// <param name="culture">The culture info parameter.</param>
        /// <returns>Returns the points as type geometry.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PathGeometry geometry = new PathGeometry();
            List<Point> pointsOfFunction = (List<Point>)value;
            List<LineSegment> linesegments = new List<LineSegment>();
            Point start;

            if (pointsOfFunction != null && pointsOfFunction.Count > 0)
            {
                start = pointsOfFunction[0];

                for (int i = 1; i < pointsOfFunction.Count; i++)
                {
                    linesegments.Add(new LineSegment(pointsOfFunction[i], true));
                }

                PathFigure figure = new PathFigure(start, linesegments, false);
                geometry.Figures.Add(figure);
            }

            return geometry;
        }

        /// <summary>
        /// Converts a object back to his old value.
        /// </summary>
        /// <param name="value">The value that should be converted back.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">A parameter as type object.</param>
        /// <param name="culture">The culture info parameter.</param>
        /// <returns>It returns the back converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
