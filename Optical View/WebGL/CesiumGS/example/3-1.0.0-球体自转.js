
  
var script = ['<!-- 初始化三维库 --><script type="text/javascript" src="/lib/index.js" libpath="../" include="Taoist"></script>'];
script.forEach(element => {
  document.writeln(element);
});
/**
 * 初始化交互
 * @param {*} viewer 球体对象
 */
function init(viewer) {
  const turn = G.Turn(viewer)
  G.BaseLayer(viewer,{
    name: '影像底图',
    type: 'mapbox',//www_google sl_geoq
    layer: 'satellite',
    // crs: '4326',
    brightness: 0
  })//添加底图
  turn(viewer,'play')//stop //为暂停//开始自转
  //  G.Turn.Release(viewer)//销毁

}


window.onload = () =>{
  (function () {
    //初始化地球0
    if (G.U.webglReport()) {//判断浏览器是否支持WebGL
      G.create3D({
        id: 'mapBox',
        showGroundAtmosphere: true,
        debug: false,
        success: function (_viewer) {

          init(_viewer)
          _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
          // _viewer.scene.globe.show = false;
          _viewer.scene.highDynamicRange = true;
          _viewer.scene.globe.baseColor =new Cesium.Color.fromCssColorString("#171744");
          // _viewer.scene.sun.show = false; //在Cesium1.6(不确定)之后的版本会显示太阳和月亮，不关闭会影响展示
          _viewer.scene.moon.show = false;
          _viewer.scene.skyBox.show = false;//关闭天空盒，否则会显示天空颜色
          _viewer.scene.backgroundColor =
            new Cesium.Color.fromCssColorString("#171744");

        }
      },Cesium);
    } else {
        alert('浏览器不支持WebGL，需更换浏览器')
    }

  })()
}
