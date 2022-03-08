var script = ['<!-- 初始化三维库 --><script type="text/javascript" src="/lib/index.js" libpath="../" include="Taoist"></script>'];
script.forEach(element => {
  document.writeln(element);
});
//example方法
const _ = (params, options, _viewer) => {
  _viewer = _viewer ?? this.viewer;
  window.Gear_X = new Gear(_viewer)
  if (_viewer instanceof Cesium.Viewer) return Gear_X[params](options);
};

window.onload = () => {
  (function () {
    
    init();
    function init() {
      //初始化地球0
      if (G.U.webglReport()) {
        //判断浏览器是否支持WebGL
        G.create3D({
          id: "mapBox",
          showGroundAtmosphere: true,
          debug: false,
          success: function (_viewer) {
            window.viewer = _viewer;
            G.sTime(_viewer);
            _("example_altitude"); //添加杭州高程

            const rain = _("example_RainwaterSystem_Shaders"); //雨
            setTimeout(() => {
              rain.show(false)

              const snow = _("example_snowSystem_Shaders"); //雪
              setTimeout(() => {
                snow.show(false)
                _("example_Fogging"); //雾
              }, 6000);
            }, 8000);
           
          
    
    
            _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
            _viewer.scene.highDynamicRange = true;
            _viewer.scene.globe.baseColor = new Cesium.Color.fromCssColorString("#171744");
            // _viewer.scene.sun.show = false; //在Cesium1.6(不确定)之后的版本会显示太阳和月亮，不关闭会影响展示
            _viewer.scene.moon.show = false;
            _viewer.scene.skyBox.show = false; //关闭天空盒，否则会显示天空颜色
            _viewer.scene.backgroundColor = new Cesium.Color.fromCssColorString("#171744");

          },
        },Cesium);
      } else {
        alert("浏览器不支持WebGL，需更换浏览器");
      }
    }

  

  })();
};
