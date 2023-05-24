using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ViewModels.DesignPlus
{
    public class DesignPlusViewModel
    {
        public string FirstPageImageByte64String { get; set; }
        public string FirstPageImageMapTextFilePath { get; set; }
        public string GeometricBounds
        {
            get
            {
                var stringToReturn = "";
                if (File.Exists(FirstPageImageMapTextFilePath) == true)
                {
                    StreamReader reader = new StreamReader(FirstPageImageMapTextFilePath);
                    string thisLine = "";
                    while (reader.EndOfStream == false)
                    {
                        thisLine = reader.ReadLine();
                        if (thisLine.Split("$").Count() > 2)
                        {
                            string geometricBoundsWithDecimals = thisLine.Split("$")[0];
                            string[] geometricBoundsParts = geometricBoundsWithDecimals.Split(",");
                            string Y1PosCoord = Math.Round((decimal)Decimal.Parse(geometricBoundsParts.ElementAt(0)) * (decimal)1.39, 0).ToString();
                            string X1PosCoord = Math.Round((decimal)Decimal.Parse(geometricBoundsParts.ElementAt(1)) * (decimal)1.39, 0).ToString();
                            string Y2PosCoord = Math.Round((decimal)Decimal.Parse(geometricBoundsParts.ElementAt(2)) * (decimal)1.39, 0).ToString();
                            string X2PosCoord = Math.Round((decimal)Decimal.Parse(geometricBoundsParts.ElementAt(3)) * (decimal)1.39, 0).ToString();

                            stringToReturn = String.Format("{0},{1},{2},{3}", X1PosCoord, Y1PosCoord, X2PosCoord, Y2PosCoord);
                        }

                    }
                }
                return stringToReturn;
            }
        }

        public string ImgFileWidth
        {
            get
            {
                var stringToReturn = "";
                if (File.Exists(FirstPageImageMapTextFilePath) == true)
                {
                    StreamReader reader = new StreamReader(FirstPageImageMapTextFilePath);
                    string thisLine = "";
                    while (reader.EndOfStream == false)
                    {
                        thisLine = reader.ReadLine();
                        if (thisLine.Split("$").Count() > 2)
                        {
                            stringToReturn = Math.Round((decimal)Decimal.Parse(thisLine.Split("$").ElementAt(2))).ToString();
                            //stringToReturn = Math.Round((decimal)Decimal.Parse(thisLine.Split("$").ElementAt(3))).ToString();
                        }
                        else
                        {
                            stringToReturn = Math.Round((decimal)Decimal.Parse(thisLine.Split("$").ElementAt(0))).ToString();
                            //stringToReturn = Math.Round((decimal)Decimal.Parse(thisLine.Split("$").ElementAt(1))).ToString();
                        }

                    }
                }
                return stringToReturn;
            }

        }
        public string ImgFileHeight
        {
            get
            {
                var stringToReturn = "";
                if (File.Exists(FirstPageImageMapTextFilePath) == true)
                {
                    StreamReader reader = new StreamReader(FirstPageImageMapTextFilePath);
                    string thisLine = "";
                    while (reader.EndOfStream == false)
                    {
                        thisLine = reader.ReadLine();
                        if (thisLine.Split("$").Count() > 2)
                        {
                            stringToReturn = Math.Round((decimal)Decimal.Parse(thisLine.Split("$").ElementAt(3))).ToString();
                        }
                        else
                        {
                            stringToReturn = Math.Round((decimal)Decimal.Parse(thisLine.Split("$").ElementAt(1))).ToString();
                        }

                    }
                }
                return stringToReturn;
            }
        }
    }
}

