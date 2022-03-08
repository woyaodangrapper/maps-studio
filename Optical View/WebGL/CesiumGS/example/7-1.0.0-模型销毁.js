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
        viewer.flyTo(tileset, {
          offset: {
            heading: Cesium.Math.toRadians(0.0),
            pitch: Cesium.Math.toRadians(-25),
            range: 0,
          },
          duration: 3,
        });

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

              _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
              // _viewer.scene.globe.show = false;
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
              //初始化摄像头模型
              _camera();
              //销毁模型
              _byeModel();
              //日照
              _("example_runshineAnalysis");

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

    //添加摄像头
    function _camera() {
      const URI = "/assets/model/";

      let CAMERA_X_GLTFS = [];
      for (let index = 0; index < arr.length; index++) {
        const element = arr[index];
        var CAMERA_XPOSITION_GLTF = {
          x: element.xyz.x,
          y: element.xyz.y,
          z: element.xyz.z,
          h: 0,
          p: 0,
          r: 0,
        };

        let URL = "/assets/images/" + element.type + ".svg";
        //添加摄像头标签
        var Img = G.aImg(viewer, {
          id: URL,
          type: "标签",
          url: URL,
          x: element.xyz.x,
          y: element.xyz.y,
          z: element.xyz.z,
        });
        Img.type = "标签";

        //调整摄像头朝向
        switch (element.name) {
          case "外江围墙周界枪机":
          case "外江围墙热成像枪机":
            CAMERA_XPOSITION_GLTF.h = Cesium.Math.toRadians(70);
            break;
          case "外江围墙枪机":
            CAMERA_XPOSITION_GLTF.h = Cesium.Math.toRadians(-100);
            break;
          case "排涝闸枪机":
            CAMERA_XPOSITION_GLTF.h = Cesium.Math.toRadians(20);
            break;
          case "配水闸枪机":
            CAMERA_XPOSITION_GLTF.h = Cesium.Math.toRadians(0);
            break;
          default:
            break;
        }
        var CAMERA_URL_GLTF = URI + element.type + ".glb";

        var CAMERA_X_GLTF = G.aGLTF(viewer, {
          url: CAMERA_URL_GLTF,
          scale: element.type == "摄像头-球机" ? 5 : 0.08,
          position: CAMERA_XPOSITION_GLTF,
        });

        CAMERA_X_GLTF.object = element;
        CAMERA_X_GLTF.type = "摄像头";
        CAMERA_X_GLTFS.push(CAMERA_X_GLTF);

        /**添加栏杆 */
        if (element.name !== "鹰眼子机" && element.name !== "鹰眼球机全景" && element.name !== "上游桥球机" && element.name !== "配水闸枪机") {
          var xyz = Cesium.Cartesian3.fromDegrees(element.xyz.x, element.xyz.y, 10);
          var hpr = new Cesium.HeadingPitchRoll(0, 0, 0);
          var fixedFrameTransform = Cesium.Transforms.localFrameToFixedFrameGenerator("north", "west");
          if (element.name === "观潮亭球机2" || element.name === "观潮亭球机3") {
            if (element.name == "观潮亭球机2") {
              xyz = Cesium.Cartesian3.fromDegrees(120.26650285574493, 30.293744031454484, 13);
            } else {
              xyz = Cesium.Cartesian3.fromDegrees(120.26650047169053, 30.293801239323717, 13);
            }
            hpr = new Cesium.HeadingPitchRoll(Cesium.Math.toRadians(180), 0, 0);
          }
          if (element.name === "观潮亭球机1") {
            //x: 120.26677215412022, y: 30.2938095977817
            xyz = Cesium.Cartesian3.fromDegrees(120.26677215412022, 30.2938095977817, 11);
          }
          viewer.scene.primitives.add(
            Cesium.Model.fromGltf({
              url: URI + "栏杆.glb",
              scale: 1.2,
              modelMatrix: Cesium.Transforms.headingPitchRollToFixedFrame(xyz, hpr, Cesium.Ellipsoid.WGS84, fixedFrameTransform),
            })
          ).type = "栏杆";
        }
      }
      //模型加载完成的回调
      CAMERA_X_GLTFS[CAMERA_X_GLTFS.length - 1].readyPromise
        .then(function () {
          //then 判断最后一个模型加载完成的回调
          //模型发光
          setSelected(postProcessStage, CAMERA_X_GLTFS);
        })
        .otherwise(function (error) {
          console.log(error);
        });

      //单机事件 点击模型后变色发光(选中效果)
      var handler = new Cesium.ScreenSpaceEventHandler(viewer.scene.canvas);
      handler.setInputAction(function (movement) {
        var model = viewer.scene.pick(movement.position); //选取当前的entity

        CAMERA_X_GLTFS.forEach(element => {
          var selectedColor = new Cesium.Color.fromCssColorString("#fff").withAlpha(0.95);
          element.color = selectedColor;
        });
        var selected = false;
        if (model && Cesium.defined(model.primitive)) {
          selected = model.primitive;
          // colors: ['White', 'Red', 'Green', 'Blue', 'Yellow', 'Gray'],
          var selectedColor = Cesium.Color["Blue".toUpperCase()]; //new Cesium.Color(0, 1, 0, 1);
          selected.color = selectedColor;
        }

        if (model && Cesium.defined(model.id)) {
          selected = model.id;
        }
        if (selected instanceof Cesium.Cesium3DTileset || !selected) {
          return;
        }
        if (selected.object.type.indexOf("摄像头") != -1) {
          var wsc = {
            name: "点击摄像头事件",
            id: selected.object.id,
            name: selected.object.name,
            shapeType: "摄像头",
          };
          console.log(wsc, "*");
        }

        // VMSDS.core.Highlight(Gltf)
      }, Cesium.ScreenSpaceEventType.LEFT_CLICK);

      let setSelected = (postProcessStage, pickeds) => {
        postProcessStage.selected = [];
        postProcessStage.enabled = false;
        let pickObjects = [];

        for (let index = 0; index < pickeds.length; index++) {
          const picked = pickeds[index];

          if (picked) {
            let primitive = picked;
            let pickIds = primitive._pickIds;
            let pickId = picked.pickId;

            if (!pickId && !pickIds && picked.content) {
              pickIds = picked.content._model._pickIds;
            }

            if (!pickId) {
              if (picked.id) {
                pickId = pickIds.find(pickId => {
                  return pickId.object == picked;
                });
              } else if (pickIds) {
                pickId = pickIds[0];
              }
            }

            if (pickId) {
              let pickObject = {
                pickId: pickId,
              };
              pickObjects.push(pickObject);
            } else {
              console.log("未找到pickId");
            }
          }
        }
        postProcessStage.selected = pickObjects;
        postProcessStage.enabled = !postProcessStage.enabled;
      };
      let postProcessStage = G.E.createUnrealBloomStage("unrealBloom");
      postProcessStage.enabled = false;
      viewer.postProcessStages.add(postProcessStage);
    }

    //销毁模型
    function _byeModel() {
      // setTimeout(() => {
      //   destroy();
      // }, 5000);

      var i = 0;
      var Interval = setInterval(() => {
        if (i++ >= arr.length - 1) {
          destroy();
          clearInterval(Interval);
          //刷新指引
          new Gear()["setTitle"]({
            text: "销毁模型完成,具体清除已经输出至控制台",
            title: "销毁模型",
          });
        } else {
          _("example_positioning_camera", arr[i].id);
          new Gear()["setTitle"]({
            text: arr[i].type,
            title: "正在浏览:" + arr[i].name,
          });
        }
      }, 5000);

      function destroy() {
        var 摄像头 = G.Query_X(viewer, {
          type: "摄像头",
        });
        var 建筑 = G.Query_X(viewer, {
          type: "建筑",
        });
        var 标签 = G.Query_X(viewer, {
          type: "标签",
        });
        var 栏杆 = G.Query_X(viewer, {
          type: "栏杆",
        });

        /**删除摄像头 */
        var arr = [];
        new Array(摄像头, 栏杆, 标签).forEach(elements => {
          for (let index = 0; index < elements.length; index++) {
            const element = elements[index];

            var state = true;
            var code = 0;
            var msg = "success";
            try {
              var state = G.dMod(viewer, element);
            } catch (error) {
              msg = error;
              code = 500;
            }

            arr.push({
              code: code,
              msg: msg,
              type: !element.object ? element.type : element.object.type,
              id: !element.object ? element.id : element.object.id,
              data: {
                status: state,
              },
            });
          }
        });
        console.table(arr);
        console.log("模型销毁报告");
      }
    }
  })();
};
