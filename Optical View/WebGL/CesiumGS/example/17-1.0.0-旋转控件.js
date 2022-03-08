var script = ['<!-- 初始化三维库 --><script type="text/javascript" src="/lib/index.js" libpath="../" include="Taoist"></script>'];
script.forEach(element => {
  document.writeln(element);
});
window.onload = () =>{
  (
    function () {
      //example方法
      const _ = (params, options, _viewer) => {
        _viewer = _viewer ?? this.viewer;
        window.Gear_X = new Gear(_viewer)
        if (_viewer instanceof Cesium.Viewer) return Gear_X[params](options);
      };
      init()
      function init() {
         //初始化地球0
        if (G.U.webglReport()) {//判断浏览器是否支持WebGL
          G.create3D({
              id: 'mapBox',
              showGroundAtmosphere: true,
              debug: false,
              success: function (_viewer) {
                var viewer = window.viewer = _viewer
                G.BaseLayer(viewer,{
                  name: '影像底图',
                  type: 'mapbox',//www_google sl_geoq
                  layer: 'satellite',
                  // crs: '4326',
                  brightness: 0
                })//添加底图


                //大气特效
                G.E.AtmosphericEffects(viewer);
                G.sTime(viewer,'2021-10-08T16:00:43.52Z')
                G.Go(viewer,{
                   h: 4.74
                  ,p: -0.6963533564
                  ,r: 0
                  ,x: -80.595996
                  ,y: 28.599491
                  ,z: 400.63
                  ,duration : 0
                })
                
                var LaunchBottom = new Cesium.Model.fromGltf({
                  url: (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%8E%AF%E5%A2%83/%E5%8F%91%E5%B0%84%E5%9F%BA%E5%9C%B0-1(%E5%AE%9E%E6%99%AF)/spacex-launch-pad-complex/Landscape_0.glb',
                  modelMatrix:  Cesium.Transforms.headingPitchRollToFixedFrame(
                      Cesium.Cartesian3.fromDegrees(-80.6,28.6,130),
                      new Cesium.HeadingPitchRoll(
                          Cesium.Math.toRadians(50), 
                          Cesium.Math.toRadians(0), 
                          Cesium.Math.toRadians(0)
                      ),
                      Cesium.Ellipsoid.WGS84,
                      Cesium.Transforms.localFrameToFixedFrameGenerator(
                          "north",
                          "west"
                      )
                  ),
                  color:Cesium.Color.fromCssColorString('#fff').withAlpha(Number(80) / 100)
                  ,
                  scale: 0.06/ 2
                })
                //发射场位于北纬41.118N，东经100.316E
                G.aGLTF(_viewer,LaunchBottom)
              
             
                 
                //发射场
                var LaunchSite = new Cesium.Model.fromGltf({
                  url: (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%8E%AF%E5%A2%83/%E5%8F%91%E5%B0%84%E5%9F%BA%E5%9C%B0-1(%E5%AE%9E%E6%99%AF)/spacex-launch-pad-complex/spacex-launch-pad-complex-max.glb',
                  modelMatrix:  Cesium.Transforms.headingPitchRollToFixedFrame(
                      Cesium.Cartesian3.fromDegrees(-80.6,28.6,130),
                      new Cesium.HeadingPitchRoll(
                          Cesium.Math.toRadians(50), 
                          Cesium.Math.toRadians(0), 
                          Cesium.Math.toRadians(0)
                      ),
                      Cesium.Ellipsoid.WGS84,
                      Cesium.Transforms.localFrameToFixedFrameGenerator(
                          "north",
                          "west"
                      )
                  ),
                  color:Cesium.Color.fromCssColorString('#fff').withAlpha(Number(100) / 100)
                  ,
                  scale: 0.06/ 2
                })
                
                G.aGLTF(viewer,LaunchSite)
               
                //黑色的猎鹰9号
                let SPACE_X = G.aGLTF(viewer,{
                  url:  (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%89%A9%E4%BD%93/%E7%81%AB%E7%AE%AD/%E7%8C%8E%E9%B9%B0%E4%B9%9D%E5%8F%B7/scene.gltf',
                  scale:0.06 / 2,//5
                  position:{
                    x: -80.60008139822472,
                    y: 28.600085733171827,
                    z: 26,
                  }
                })
              
                var TC = new G.C.TranslationController(viewer)
                SPACE_X.readyPromise.then(m => {
                  TC.add(m)
                })
              }
          },Cesium);
        } else {
            alert('浏览器不支持WebGL，需更换浏览器')
        }

      }
     
  })()
}
