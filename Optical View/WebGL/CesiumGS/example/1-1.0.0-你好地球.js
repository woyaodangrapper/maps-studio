window.onload = () =>{
  (function () {
    //初始化地球0
    if (G.U.webglReport()) {//判断浏览器是否支持WebGL
        G.create3D({
          id: 'mapBox',
          showGroundAtmosphere: true,
          debug: false,
          success: function (_viewer) {
            _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
            G.BaseLayer(_viewer,{
              name: '影像底图',
              type: 'mapbox',//www_google sl_geoq
              layer: 'satellite',
              // crs: '4326',
              brightness: 0
            })
          }
      },Cesium);

    } else {
        alert('浏览器不支持WebGL，需更换浏览器')
    }
  })()
}
