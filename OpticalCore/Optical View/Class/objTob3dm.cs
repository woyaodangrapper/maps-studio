using Arctron.Obj23dTiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Optical_View.Class
{
    class objTob3dm
    {
        //文件(F)编辑(E) 格式(O) 查看(M) 帮助(H)
        //ENU: 30. 294856.120.266620
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
