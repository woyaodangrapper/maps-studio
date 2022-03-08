var script = ['<!-- 初始化三维库 --><script type="text/javascript" src="/lib/index.js" libpath="../" include="Taoist"></script>'];
script.forEach(element => {
  document.writeln(element);
});
/**
 * 添加模型
 * @param {*} url 模型链接
 */
function a3DTiles(flyTo, url, _uuid) {
  //添加模型
  var tileset = G.aTiles(viewer, {
    type: "建筑",
    id: _uuid, //生成随机id_
    url: url.replace("http://127.0.0.1:55328", "http://121.40.42.254:8008"), //http://127.0.0.1:64158
    flyTo: false,
    heightOffset: 0,
    height: 10,
    style: {
      // color: "color('white', 0.3)",
      show: true,
    }
  });

  if (flyTo) {
    //资源全部加载后飞行 防止卡顿
    var state = false;
    G.IA(
      function (start) {},
      function (end) {
        state = false;
        setTimeout(() => {
          state = true;
        }, 500);

        // console.log(end)
      }
    );

    var interva = setInterval(() => {
      if (state) {
        G.Go(viewer,{
          h: 5.38
          ,p: -0.6465056676
          ,r: 0
          ,x: 120.267558
          ,y: 30.293665
          ,z: 93.62
          ,duration: 0
        })

        console.log("刷新完成");
        clearInterval(interva);
      }
    }, 1000);
  }

  return tileset;
}
//摄像头坐标
var arr = [
  { id: "3FD815955E4811ECA56200163E0132C0", camera: "{y: 30.293971, x: 120.266944, z: 25.89, h: 6.09, p: -0.6545561479,r: 0}", name: "外江围墙周界枪机", xyz: { x: 120.26691918084657, y: 30.294126148932428, z: 14 }, type: "摄像头-枪机" },
  { id: "3FDA05DE5E4811ECA56200163E0132C0", camera: "{y: 30.295453, x: 120.266359, z: 8.23, h: 5.44, p: -0.0454660071, r: 0}", name: "上游桥球机", xyz: { x: 120.26623582547457, y: 30.29553302082086, z: 9 }, type: "摄像头-球机" },
  { id: "3FDBCFFA5E4811ECA56200163E0132C0", camera: "{y: 30.293642, x: 120.266796, z: 26.38, h: 6.14, p: -0.5349894957,r: 0}", name: "观潮亭球机1", xyz: { x: 120.26676747291567, y: 30.293810978379664, z: 15.5 }, type: "摄像头-球机" },
  { id: "3FDD9A165E4811ECA56200163E0132C0", camera: "{y: 30.294102, x: 120.267266, z: 21.07, h: 5.17, p: -0.5261846026,r: 0}", name: "排涝闸枪机", xyz: { x: 120.26715833643064, y: 30.294147111476033, z: 14.5 }, type: "摄像头-枪机" },
  { id: "3FDF3E055E4811ECA56200163E0132C0", camera: "{y: 30.293824, x: 120.266396, z: 20.07, h: 0.27, p: -0.3614052486,r: 0}", name: "外江围墙枪机", xyz: { x: 120.26643836108585, y: 30.293971814580434, z: 14.5 }, type: "摄像头-枪机" },
  { id: "3FE12E4E5E4811ECA56200163E0132C0", camera: "{y: 30.29361, x: 120.266407, z: 38.46, h: 0.57, p: -0.8675042858, r: 0}", name: "观潮亭球机2", xyz: { x: 120.26650670572847, y: 30.293744236576636, z: 17.5 }, type: "摄像头-球机" },
  { id: "3FE31E975E4811ECA56200163E0132C0", camera: "{y: 30.295262, x: 120.266348, z: 31.2, h: 4.82, p: -0.4372911854, r: 0}", name: "鹰眼子机", xyz: { x: 120.26612001582494, y: 30.29529883612534, z: 19.5 }, type: "摄像头-球机" },
  { id: "3FE4E8B35E4811ECA56200163E0132C0", camera: "{y: 30.293762, x: 120.266285, z: 26.39, h: 1.3, p: -0.4014737007, r: 0}", name: "观潮亭球机3", xyz: { x: 120.26650467330701, y: 30.29380046498245, z: 17.5 }, type: "摄像头-球机" },
  { id: "3FE6B2CF5E4811ECA56200163E0132C0", camera: "{y: 30.293597, x: 120.266443, z: 22.83, h: 0.33, p: -0.5972511612,r: 0}", name: "配水闸枪机", xyz: { x: 120.26650305264293, y: 30.29373479011233, z: 13 }, type: "摄像头-枪机" },
  { id: "3FE6C2CF5E4811ECA56200163E0132C0", camera: "{y: 30.295262, x: 120.266294, z: 20.1, h: 4.85, p: -0.0152468164, r: 0}", name: "鹰眼球机全景", xyz: { x: 120.26612001582494, y: 30.29529883612534, z: 20 }, type: "摄像头-球机" },
  { id: "3FE6D2CF5E4811ECA56200163E0132C0", camera: "{y: 30.293974, x: 120.266938, z: 19.8, h: 6.2, p: -0.3571406111,  r: 0}", name: "外江围墙热成像枪机", xyz: { x: 120.26691918084657, y: 30.294126148932428, z: 14.5 }, type: "摄像头-枪机" },
];
window.onload = () => {
  (function () {
    //example方法
    const _ = (params, options, _viewer) => {
      _viewer = _viewer ?? this.viewer;
      if (_viewer instanceof Cesium.Viewer) return new Gear(_viewer)[params](options);
    };
    init();

    function init() {
      //初始化地球0
      if (G.U.webglReport()) {
        //判断浏览器是否支持WebGL
        G.create3D(
          {
            id: "mapBox",
            showGroundAtmosphere: true,
            debug: false,
            success: function (_viewer) {
              window.viewer = _viewer;
              G.sTime(_viewer);
              _("example_runshineAnalysis"); //日照

              _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
              _viewer.scene.globe.show = false;
              _viewer.scene.highDynamicRange = true;
              _viewer.scene.globe.baseColor = new Cesium.Color.fromCssColorString("#171744");
              // _viewer.scene.sun.show = false; //在Cesium1.6(不确定)之后的版本会显示太阳和月亮，不关闭会影响展示
              _viewer.scene.moon.show = false;
              _viewer.scene.skyBox.show = false; //关闭天空盒，否则会显示天空颜色
              _viewer.scene.backgroundColor = new Cesium.Color.fromCssColorString("#171744");

              //初始化建筑模型
              G.U.Get("http://121.40.42.254:8003/sys/get/gis_", function (response) {
                _build(response, a3DTiles);
              });

              //区域管理线
              _SolidWall()
            },
          },
          Cesium
        );
      } else {
        alert("浏览器不支持WebGL，需更换浏览器");
      }
    }

    //建筑
    function _build(data) {
      var data = data.list.datas;
      function makeTree(treeJson) {
        for (var i = 0; i < treeJson.length; i++) {
          if (treeJson[i].uri != null) {
            a3DTiles(i == 0 ? false : true, treeJson[i].uri, treeJson[i].uri).type = "建筑";
          }
          if (treeJson[i].id != null) {
            if (treeJson[i].children != null && treeJson[i].children.length > 0) {
              makeTree(treeJson[i].children);
            }
          }
        }
      }
      makeTree(data);
    }

    //区域管理线
    function _SolidWall(){
      _("example_SolidWall")
    }
  })();
};
