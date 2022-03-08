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
/**
 * 添加模型
 */
 function aMODEL() {
  if (true) {
    //资源全部加载后飞行 防止卡顿
    var state = false;var percent_state = false//非b3dm加载不需要使用这个
    G.IA(
      function (start) {},
      function (end) {
        state = false;
        setTimeout(() => {
          state = true;
        }, 500);
        // console.log(end)
      },
      function (percent) {
        $("#file-page-percent").html(percent)
        if(percent == "100%")percent_state = true
      }
    );
    var interva = setInterval(() => {
      if(state){
        viewer.turn(viewer,'stop')//停止自转
        G.Go(viewer,{
          h: 6.2
         ,p: -0.5402301634
         ,r: 6.28
         ,x: 120.370808
         ,y: 29.992657
         ,z: 402.19
         ,duration: 3
       })
      }
      if (state && percent_state) {
        console.log("刷新完成");
        //opacity: 0; 
        $("#file-page-container").fadeToggle(3000);
        setTimeout(() => {
          _("example_IntelligentRoamingV2")
        }, 3000);
        clearInterval(interva);
      }
    }, 1000);
  }

  // var tileset = G.aGLTF(viewer,{
  //   url: (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%8E%AF%E5%A2%83/%E9%9B%AA%E5%B1%B1/source/800003c6-0006-f100-b63f-84710c7967bb.glb',//'http://121.40.42.254:8008/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%8E%AF%E5%A2%83/%E9%9B%AA%E5%B1%B1/source/800003c6-0006-f100-b63f-84710c7967bb.glb',
  //   scale:1.0,
  //   position:{
  //     x:120.37036851640701
  //     ,y:29.994691066044325
  //     ,z:9.381325284942115
  //   },
  //   type:"山体"
  // })

  var tileset = G.aTiles(viewer, {
    type: "山体",
    url: (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%8E%AF%E5%A2%83/%E5%87%8C%E5%8E%89%E7%9A%84%E5%B1%B1%E5%8C%BA%E7%99%BD%E8%86%9C/tileset/tileset.json', 
    flyTo: true,
    heightOffset: 0,
    height: 80,
    style: {
      color: "color('white', 1)",
      show: true,
    }
  },function(){
    G.uTiles({
      tileset:tileset,
      scale:100
    })
  });

  return tileset;
}
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
            aMODEL()
            viewer.turn = G.Turn(viewer)

            viewer.turn(viewer,'play')//stop //为暂停//开始自转
      
            // _("example_DynamicDraw","drawPolyline")

            G.C.getPosition(viewer,function (p) {
              console.log(p) //取点击坐标小工具
            })
            // _viewer.scene.globe.show = false;
            _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
            _viewer.scene.highDynamicRange = true;
            _viewer.scene.globe.baseColor = new Cesium.Color.fromCssColorString("#fff");
            // _viewer.scene.sun.show = false; //在Cesium1.6(不确定)之后的版本会显示太阳和月亮，不关闭会影响展示
            _viewer.scene.moon.show = false;
            _viewer.scene.skyBox.show = false; //关闭天空盒，否则会显示天空颜色
            _viewer.scene.backgroundColor = new Cesium.Color.fromCssColorString("#000");

          },
        },Cesium);
      } else {
        alert("浏览器不支持WebGL，需更换浏览器");
      }
    }

  

  })();
};
