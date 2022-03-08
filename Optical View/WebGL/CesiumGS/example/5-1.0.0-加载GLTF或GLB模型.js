   
var script = ['<!-- 初始化三维库 --><script type="text/javascript" src="/lib/index.js" libpath="../" include="Taoist"></script>'];
script.forEach(element => {
  document.writeln(element);
});
 /**
 * 初始化交互
 * @param {*} viewer 球体对象
 */
function init(viewer) {

    //定位坐标
    G.Go(viewer,{
      h: 2.69
      ,p: -0.5393882234
      ,r: 6.28
      ,x: 116.225243
      ,y: 39.550147
      ,z: 1123.53
      ,duration : 0
    })

   G.sTime(viewer)

    if (true) {
      //资源全部加载后飞行 防止卡顿
      var state = false;//var percent_state = false//非b3dm加载不需要使用这个
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
          // if(percent == "100%")percent_state = true
        }
      );
      var interva = setInterval(() => {
        if (state) {// && percent_state
          console.log("刷新完成");
          $("#file-page-container").fadeToggle(3000);
          clearInterval(interva);
        }
      }, 1000);
    }
  
   //环境模型
    var GLTF = G.aGLTF(viewer,{
    url:  'http://121.40.42.254:8008/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%89%A9%E4%BD%93/%E4%B8%AD%E5%9B%BD%E9%A3%8E/chinese-shirt-agisoftclotheschallenge/a.glb',
    scale:100,
    position:{
       x: 116.2317
      ,y: 39.5427
      ,z: 500
    }
  })

  setTimeout(() => {
      //定位坐标
      G.Go(viewer,{
        h: 2.69
        ,p: -0.5393882234
        ,r: 6.28
        ,x: 116.23080758191139
        ,y: 39.54239260122578
        ,z: 1376.54
      ,duration : 0
     })
   
     setTimeout(() => {
      viewer.clock.multiplier =  2
      var Rotate = G.Rotate(viewer)
      Rotate({x: 116.23080758191139,y: 39.54239260122578,z: 540},viewer)//pitch:0,
     }, 200);
  }, 6000);
  ///模型特效
  let setSelected = (postProcessStage,pickeds) =>{
    postProcessStage.selected = []
    postProcessStage.enabled = false
    let pickObjects = []

    for (let index = 0; index < pickeds.length; index++) {
      const picked = pickeds[index];

      if (picked) {
        
        let primitive = picked
        let pickIds = primitive._pickIds;
        let pickId = picked.pickId;

        if (!pickId && !pickIds && picked.content) {
            pickIds = picked.content._model._pickIds;
        }
        
        if (!pickId) {
            if (picked.id) {
                pickId = pickIds.find(pickId => {
                    return pickId.object == picked;
                })
            } else if (pickIds) {
                pickId = pickIds[0]
            }
        }

        if (pickId) {
            let pickObject = {
                pickId: pickId
            }
            pickObjects.push(pickObject)
        } else {
          console.log('未找到pickId')
        }
      }
    }
    postProcessStage.selected = pickObjects
    postProcessStage.enabled = !postProcessStage.enabled
  }
  let postProcessStage = G.E.createUnrealBloomStage( 'unrealBloom' )
  postProcessStage.enabled = false
  viewer.postProcessStages.add(postProcessStage);

  GLTF.readyPromise.then(function () {
    var selectedColor = new Cesium.Color.fromCssColorString("#fff");//new Cesium.Color(1, 1, 1, 1);
    GLTF.color = selectedColor;
    setSelected(postProcessStage,[GLTF])
  }).otherwise(function (error) {
      console.log(error);
  });
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
          
            _viewer.scene.globe.baseColor = new Cesium.Color.fromCssColorString("#000");

          }
      },Cesium);
    } else {
        alert('浏览器不支持WebGL，需更换浏览器')
    }
  })()
}
