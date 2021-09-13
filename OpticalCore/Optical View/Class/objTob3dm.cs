using Arctron.Obj23dTiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Optical_View.Class
{
    class objTob3dm
    {
        //[Obsolete]
        public string Obj_WriteTileset(String ObjFile,String outputDir = "tileset")
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            var gisPosition = new GisPosition();
            return TilesConverter.WriteTilesetFile(ObjFile, outputDir, gisPosition);
            //File.Exists(Path.Combine(outputDir, "tileset.json"))
        }
    }
}
