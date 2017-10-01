using System.Windows.Forms;
using Newtonsoft.Json;

namespace hoTools.Utils.Json
{
    /// <summary>
    /// Item to specify the Port Alignment
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PortAlignmentItem 
    {

        public string Type { get; }

        public string XLeft{ get; }
        public string YLeft { get; }
        public string AlignmentLeft { get; }
        public string RotationLeft { get; }

        public string XRight { get; }
        public string YRight { get; }
        public string AlignmentRight { get; }
        public string RotationRight { get; }

        public string XTop { get; }
        public string YTop { get; }
        public string AlignmentTop { get; }
        public string RotationTop { get; }

        public string XBottom { get; }
        public string YBottom { get; }
        public string AlignmentBottom { get; }
        public string RotationBottom { get; }





        /// <summary>
        /// Item to specify the style of an EA DiagramObject
        /// </summary>
        // ReSharper disable once ClassNeverInstantiated.Global
        [JsonConstructor]
        public PortAlignmentItem(string type, 
            string xLeft, string yLeft, string alignmentLeft, string rotationLeft,
            string xRight, string yRight, string alignmentRight, string rotationRight,
            string xTop, string yTop, string alignmentTop, string rotationTop,
            string xBottom, string yBottom, string alignmentBottom, string rotationBottom
            )

        {
            Type = type;
            XLeft = xLeft;
            YLeft = yLeft;
            AlignmentLeft = alignmentLeft;
            RotationLeft = rotationLeft;

            XRight = xRight;
            YRight = yRight;
            AlignmentRight = alignmentRight;
            RotationRight = rotationRight;  

            XTop = xTop;
            YTop = yTop;
            AlignmentTop = alignmentTop;
            RotationTop = rotationTop;

            XBottom = xBottom;
            YBottom = yBottom;
            AlignmentBottom = alignmentBottom;
            RotationBottom = rotationBottom;


        }
    }
}

