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
        
                _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
                _viewer.scene.globe.show = false;//隐藏球体
                _viewer.scene.highDynamicRange = true;
                _viewer.scene.globe.baseColor = new Cesium.Color.fromCssColorString("#171744");
                // _viewer.scene.sun.show = false; //在Cesium1.6(不确定)之后的版本会显示太阳和月亮，不关闭会影响展示
                _viewer.scene.moon.show = false;
                _viewer.scene.skyBox.show = false; //关闭天空盒，否则会显示天空颜色
                _viewer.scene.backgroundColor = new Cesium.Color.fromCssColorString("#171744");
                G.BaseLayer(viewer,{
                  name: '影像底图',
                  type: 'mapbox',//www_google sl_geoq
                  layer: 'satellite',
                  // crs: '4326',
                  brightness: 0
                })//添加底图


                // G.sTime(_viewer);
                // G.C.getPosition(viewer,function (p) {
                //   console.log(p) //取点击坐标小工具
                // })
                G.Go(viewer,{
                   h: 5.87
                  ,p: -0.8375692229
                  ,r: 0
                  ,x: 120.811001
                  ,y: 30.285893
                  ,z: 3027.17
                  ,duration: 2
                })

                // //日照
                _("example_runshineAnalysis");
                var tileset = G.aTiles(viewer, {
                  type: "房间",
                  url: (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%8E%AF%E5%A2%83/%E6%B2%99%E6%BC%A0%E7%81%B02p/de-dust2-cs-map/tileset/tileset.json', 
                  flyTo: false,
                  heightOffset: 0,
                  height: 0,
                  style: {
                    color: "color('white', 1)",
                    show: true,
                  }
                },function(){
                  G.uTiles({
                    tileset:tileset,
                    scale:1
                  })
                });
                _('example_FlyingGame',tileset)
                
              }
          },Cesium);
        } else {
            alert('浏览器不支持WebGL，需更换浏览器')
        }

      }
     
  })()
}
