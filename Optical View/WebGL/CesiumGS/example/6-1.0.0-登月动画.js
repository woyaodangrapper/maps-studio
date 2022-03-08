window.onload = () =>{
  (
  
    function () {

      let _LEM = {}; G.U.Get((WEBGL_DEBUG?local:server) + "/AP11.json",function (data) {
        _LEM = data
        init()
      })
    
      //初始化绘制类
      function initDraw(viewer) {
        this.DynamicDraw = new G.D.DynamicDrawer(viewer, {
          id: '666',
          _style: {
            labelTransparency: 100, //字体透明度
            outlineColor: '#3462d0', //字体边框颜色
            fillColor: '#fff', //字体颜色
            PolylineColor: '#fff',
            PolylineWitch: 3, //线条宽度
            shapeColor: '#fff',
            shapeTransparency: 50, //形状透明度
            borderWitch: 2, //边框宽度
            borderColor: '#3462d0',
            borderTransparency: 80, //边框透明度
          },
        });
      }
      //画
      window.DD = (type) => {
        initDraw(window.viewer)
        function _(positions) {
          var p = [];
          positions.forEach((element) => {
            var cartographic = Cesium.Cartographic.fromCartesian(element);
            var lng = Cesium.Math.toDegrees(cartographic.longitude);
            var lat = Cesium.Math.toDegrees(cartographic.latitude);
            var height = cartographic.height; //模型高度
            var mapPosition = { x: lng, y: lat, z: height };

            p.push(mapPosition);
          });
          return p;
        }

        switch (type) {
          case 'drawPolyline': //画折线
            this.DynamicDraw.drawPolyline(function (positions) {
              console.log(_(positions));
            });
            break;
          case 'drawPolygon': //画多边形
            this.DynamicDraw.drawPolygon(function (positions) {
              console.log(_(positions));
            });
            break;
          case 'drawRectangle': //画矩形
            this.DynamicDraw.drawRectangle(function (positions) {
              console.log(_(positions));
            });
            break;
          case 'drawCircle': //画圆
            this.DynamicDraw.drawCircle(function (positions) {
              console.log(_(positions));
            });
            break;

          default:
            break;
        }
      }
      function initBox(viewer,entity) {

     
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
        let postProcessStage = G.EFFECT.createUnrealBloomStage( 'unrealBloom' )
        postProcessStage.enabled = false
        viewer.postProcessStages.add(postProcessStage);
    
        var cartographic = Cesium.Cartographic.fromCartesian(new Cesium.Cartesian3(-861729.1799009074, 4734414.184475191, 4172484.3779525114));
        var lng = Cesium.Math.toDegrees(cartographic.longitude);
        var lat = Cesium.Math.toDegrees(cartographic.latitude);
        var height = cartographic.height;//模型高度
        var SPACE_X_BASE_MapPosition = { x: lng, y: lat, z: height }
        var SPACE_X_BASE_position = {
          x:SPACE_X_BASE_MapPosition.x,
          y:SPACE_X_BASE_MapPosition.y,
          z:140,//280
          h:-148.05388154940792,
          p:-71.81713733847417,
          r:60.609705301466434
        }
        var SPACE_X_BASE = G.aGLTF(viewer,{
          url: (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%89%A9%E4%BD%93/%E7%81%AB%E7%AE%AD/%E7%81%AB%E7%AE%AD%E5%96%B7%E5%8F%A3/scene.gltf',
          scale:5,//5
          position:SPACE_X_BASE_position
        }) 


        
        var SPACE_X_position = {
          x:100.31569904188999,
          y:41.11799996109391,
          z:140,//280
          h:0,
          p:0,
          r:0
        }
        // var SPACE_X = G.aGLTF(viewer,{
        //   url: (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%89%A9%E4%BD%93/%E7%81%AB%E7%AE%AD/%E6%9B%B4%E5%A5%BD%E7%9A%84%E9%BE%99%E9%A3%9E%E8%88%B9/spacex-its-mars-lander.glb',
        //   scale:5,//5
        //   position:SPACE_X_position
        // }) 
      
        // G.BOLT.Editing(viewer,GLTF)
        var TC = new G.BOLT.TranslationController(viewer)

        SPACE_X_BASE.readyPromise.then(m => {
          viewer.clock.multiplier = .0;
          try {
            TC.add(m)
            // setInterval(() => {
            //   console.log(G.PH.UaternionInverse(SPACE_X_BASE.modelMatrix))
            // }, 2000);
          } catch (error) {
            console.error(error)
          }
        })
        // GLTF.readyPromise.then(function () {
        //   setSelected(postProcessStage,[GLTF])
        // }).otherwise(function (error) {
        //     console.log(error);
        // });

        
        viewer.scene.postUpdate.addEventListener(function (scene, time) {
          if (!Cesium.defined(entity.position)) {
            return;
          }
   
          var m =  SPACE_X_BASE.modelMatrix
          // 计算中心处的变换矩阵
          var m1 = Cesium.Transforms.eastNorthUpToFixedFrame(Cesium.Matrix4.getTranslation(m, new Cesium.Cartesian3()), Cesium.Ellipsoid.WGS84, new Cesium.Matrix4());
          // 矩阵相除
          var m3 = Cesium.Matrix4.multiply(Cesium.Matrix4.inverse(m1, new Cesium.Matrix4()), m, new Cesium.Matrix4());
          // 得到旋转矩阵
          var mat3 = Cesium.Matrix4.getMatrix3(m3, new Cesium.Matrix3());
          // 计算四元数
          var q = Cesium.Quaternion.fromRotationMatrix(mat3);
          // 计算旋转角(弧度)
          var hpr = Cesium.HeadingPitchRoll.fromQuaternion(q);


          var origin = entity.position.getValue(time)
          var cartographic = Cesium.Cartographic.fromCartesian(origin);
          var height = cartographic.height;//模型高度
          var modelMatrix = Cesium.Transforms.headingPitchRollToFixedFrame(Cesium.Cartesian3.fromDegrees(SPACE_X_BASE_MapPosition.x , SPACE_X_BASE_MapPosition.y, height+310), hpr);
          SPACE_X_BASE.modelMatrix = modelMatrix;
        });


        return SPACE_X_BASE
      }
      // this.prototype
      const Layer = {
        add: (viewer)=>{
          G.BaseLayer(viewer,{
            name: '影像底图',
            type: 'mapbox',//www_google sl_geoq
            layer: 'satellite',
            // crs: '4326',
            brightness: 0
          })
        },
        provider: {
            add: (viewer)=>{G._TP(viewer)}
        },
        LEFT_CLICK:(viewer)=>{  G.BOLT.getPosition(viewer,function (p) {
          console.log(p) //取点击坐标小工具
        })},
        GLTF:{
          add:(viewer,url,x,y,z,scale,h)=>{
            G.aGLTF(viewer,{
              url: url,
              scale:scale??1.0,
              position:{
                x:x,
                y:y,
                z:z,
                h:h??0
              }
            })
          },
          addx:(viewer,obj)=>{
            return G.aGLTF(viewer,obj)
          },
          addv2:(viewer,url,x,y,h)=>{
            G.aMODEL(viewer,{
              url: url,
              scale:1.0,
              position:{
                x:x,
                y:y,
                z:h
              }
            })
          }
        }
    
      }
      function IntelligentRoamingDynamicLine(viewer, list) {

        var arr = []
        list.forEach(element => {
          arr.push(element.x,element.y,element.z)
        });

        var alp = 1;
        var num = 0;
        viewer.entities.add({
          type: 'IntelligentRoaming',
          polyline: {
            positions: Cesium.Cartesian3.fromDegreesArrayHeights(arr),
            width: 26,
            material: new Cesium.PolylineGlowMaterialProperty({
              //发光线
              glowPower: 0.1,
              color: new Cesium.CallbackProperty(function () {
                if (!alp && !num) {
                  console.log('1', alp);
                }
                if (num % 2 === 0) {
                  alp -= 0.005;
                } else {
                  alp += 0.005;
                }
  
                if (alp <= 0.2) {
                  num++;
                } else if (alp >= 1) {
                  num++;
                }
                return Cesium.Color.ORANGE.withAlpha(alp);
                //entity的颜色透明 并不影响材质，并且 entity也会透明
              }, false),
            }),
            clampToGround: false,
          },
        });
      }
      //行走速度控制
      function IntelligentRoaming_Speed(value) {
        // const viewer = viewer;
        viewer.clock.multiplier = value.multiplier;
        viewer.clock.worldSpeedCache = viewer.clock.multiplier;
      }

      //漫游
      function FRP_X(options) {
        var dc = new G.M.DrawCurve(Cesium, viewer);

        // let _this = this

        // _options.stopTime = 200//模型结束时间

        var timer = options.timer ?? 100; //模型行走数度

        var FineBezierTimer = options.FineBezierTimer ?? 0.5; //算法路径速度

        var multiplier = options.multiplier ?? 1; //当前世界速度 (可整体提高行走速度 必要也可以暂停模型)

        var nodeTime = options.nodeTime == null ? 0 : options.nodeTime * 1000; //节点停留时间

        viewer.RoamingStatus == true; //漫游防冲突
      
        var pointTime = "16:00:00"//起始时间
      


        //初始化路线参数
        var xyList = options.positions;
        
        var xy_FB = [];
        //加工 xy_FB数据添加类型以及时差
        for (let i = 0; i < xyList.length; i++) {
          //起始点
          var mod = xyList[i][0];
          var q = Cesium.Cartesian3.fromDegrees(mod.x, mod.y, mod.z);
          q.time = timer;
          q.index = i;
          q.type = mod.type;
          xy_FB.push(q);

          const _element = xyList[i];
          //曲线计算
          for (let index = 0; index < _element.length; index++) {
            const element1 = xyList[i][index];
            const element2 = xyList[i][index + 1];
            const element3 = xyList[i][index + 2];

            if (element3 == null) {
              break;
            }
            var a = element1.x - element2.x;
            var b = element1.y - element2.y;
            var c = element2.x + a / 40;
            var d = element2.y + b / 40;

            var e = element2.x - element3.x;
            var f = element2.y - element3.y;
            var g = element2.x - e / 40;
            var h = element2.y - f / 40;

          
            var line = []
          
            line = Cesium.Cartesian3.fromDegreesArray([c, d, element2.x, element2.y, element2.x, element2.y, g, h])

            line[0].time = FineBezierTimer;

            line[0].type = element2.type; //以中心的类型作为评判

            line[line.length - 1].time = timer;

            console.log()
            
            line.forEach((element) => {
              var point = {};
              var cartographic = Cesium.Cartographic.fromCartesian(element);
              
              point.x = Cesium.Math.toDegrees(cartographic.longitude);
              point.y = Cesium.Math.toDegrees(cartographic.latitude);
              point.z = cartographic.height + Number(element2.z);
              console.log( point )
              // point.z = cartographic.height;
              // viewer.entities.add({
              //   position: Cesium.Cartesian3.fromDegrees(point.x,point.y,point.z),
              //   point: {
              //     pixelSize: 10,//大小
              //     color: Cesium.Color.YELLOW,
              //     outlineColor: Cesium.Color.RED,//边框颜色
              //     outlineWidth: 3,//宽 边框
              //   },
              //   label: {
              //       text: "x",
              //       fillColor: Cesium.Color.YELLOW,
              //       style: Cesium.LabelStyle.FILL_AND_OUTLINE,
              //       outlineWidth: 5,
              //       outlineColor: Cesium.Color.RED
              //   }
              // });

              var Cartesian3 = Cesium.Cartesian3.fromDegrees(point.x, point.y, point.z);
              element.x = Cartesian3.x;
              element.y = Cartesian3.y;
              element.z = Cartesian3.z;

              element.index = i;
              xy_FB.push(element);
            });

          }

          //终点
          var q = Cesium.Cartesian3.fromDegrees(xyList[i][xyList[i].length - 1].x, xyList[i][xyList[i].length - 1].y, xyList[i][xyList[i].length - 1].z);
          q.index = i;
          q.type = xyList[i][xyList[i].length - 1].type;
          xy_FB.push(q);
        }
        var index;

        var PatrolPoint = []; //巡视点

        function getTimeList(list,timer) {
          var FlightRoamingData = []; //人物漫游时路线数据存储
          var timer = timer??'04:00:00';
          var mm = timer; //一截路的时长
          for (let index = 0; index < list.length; index++) {
            if (Cesium.defined(list[index].time)) {
              mm = list[index].time;
            }

            const element = list[index];

            FlightRoamingData.push({
              id: 'roaming_' + index,
              x: element.x,
              y: element.y,
              z: element.z,
              index: element.index,
              type: element.type,
              time: ISODateString(new Date()), //设置漫游起始时间或当前时间
              ss: 20, // 停留的时长
            });
            var hour = timer.split(':')[0];
            var min = timer.split(':')[1];
            var sec = timer.split(':')[2];
            var s = Number(hour * 3600) + Number(min * 60) + Number(sec); //加当前相机时间
            function formatTime(s) {
              var t;
              if (s > -1) {
                var hour = Math.floor(s / 3600);
                var min = Math.floor(s / 60) % 60;
                var sec = s % 60;
                if (hour < 10) {
                  t = '0' + hour + ':';
                } else {
                  t = hour + ':';
                }

                if (min < 10) {
                  t += '0';
                }
                t += min + ':';
                if (sec < 10) {
                  t += '0';
                }
                t += sec.toFixed(2);
              }
              return t;
            }
            timer = formatTime(s + mm);
            function ISODateString(d) {
              function pad(n) {
                return n < 10 ? '0' + n : n;
              }
              return d.getUTCFullYear() + '-' + pad(d.getUTCMonth() + 1) + '-' + pad(d.getUTCDate()) + 'T' + timer + 'Z';
            }
          }
          return FlightRoamingData;
        }

        //时差转换成为时间戳
        var RFD = getTimeList(xy_FB,pointTime); //xyFineBezier

        var timestamp = []; //判断段落节点
        var PatrolPoint = []; //标记点集合
    
        //初始化标记点
        var index = 999;var aFRarr = []//还原aFR接口需要的数据结构


        RFD.forEach((element) => {
          var cartographic = Cesium.Cartographic.fromCartesian(element);
          var lng = Cesium.Math.toDegrees(cartographic.longitude);
          var lat = Cesium.Math.toDegrees(cartographic.latitude);
          var mapPosition = { x: lng, y: lat, z: cartographic.height };
  
          if (index != element.index) {
            index = element.index;
            timestamp.push(element);
          }
          
          //判断是否为标记点
          if (element.type == 'key') {
            PatrolPoint.push(element);
          }
          aFRarr.push({
            x: mapPosition.x,
            y: mapPosition.y,
            z: mapPosition.z,
            time: element.time,
          });
          
        });

     
        // IntelligentRoamingDynamicLine(viewer, aFRarr);//发光线
        var uri = (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%89%A9%E4%BD%93/%E7%81%AB%E7%AE%AD/'
        uri += "%E7%8C%8E%E9%B9%B0%E4%B9%9D%E5%8F%B7/"
        var _options = {positions: aFRarr} 
        _options.url =  uri + 'scene.gltf'
        _options.scale = 0.06 / 2;
        console.log(aFRarr)
        var entity = G.aFR(viewer,_options,pointTime)
        // entity.path = {
        //   show: true,
        //   leadTime: 0,
        //   width: 26,
        //   resolution: 0.1,
        //   material: new Cesium.PolylineGlowMaterialProperty({
        //     //发光线
        //     glowPower: 0.1,
        //     color: Cesium.Color.GREEN.withAlpha(1),
        //   }),
        // };
        //~~~~~~~~~~~~~~~~~~~~平滑插值~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // entity.position.setInterpolationOptions({
        //   interpolationDegree:2, //1 2 5
        //   interpolationAlgorithm: Cesium.HermitePolynomialApproximation
        // })
        /**
         * 调整模型方向
         */
        let _position = entity.position;

        var Heading = 60;
        function Direction(longitude, latitude, direction) {
            var center = Cesium.Cartesian3.fromDegrees(longitude, latitude, direction);
            var heading = Cesium.Math.toRadians(direction);
            var pitch = 0;
            var roll = 0;
            var hpr = new Cesium.HeadingPitchRoll(heading, pitch, roll);
            var orientation = Cesium.Transforms.headingPitchRollQuaternion(center, hpr);
            return orientation;
        }
        function Turn(viewer,cartesian3){
        
            var ellipsoid = viewer.scene.globe.ellipsoid;
            var cartographic = ellipsoid.cartesianToCartographic(cartesian3);
            var lat = Cesium.Math.toDegrees(cartographic.latitude);
            var lng = Cesium.Math.toDegrees(cartographic.longitude);
            var alt = cartographic.height;
            return { x: lng, y: lat, z: alt };
        }

        deviation = Cesium.Math.toDegrees(Heading).toFixed(6)
        const turn = Turn(viewer,_position.getValue(new Date()))
        var orientation = Direction(turn.x,turn.y, Cesium.Math.toDegrees(Heading).toFixed(2));;
        entity.orientation = orientation
        // viewer.clock.multiplier = multiplier;
        // console.log(multiplier)
        return entity
      }

      function init() {
         //初始化地球0
        if (G.U.webglReport()) {//判断浏览器是否支持WebGL
          G.create3D({
              id: 'mapBox',
              showGroundAtmosphere: true,
              debug: false,
              success: function (_viewer) {
                var viewer = window.viewer = _viewer
                Layer.add(viewer)//添加底图
                //大气特效
                G.E.AtmosphericEffects(viewer);
                    
                G.Go(viewer,{
                   h: 4.74
                  ,p: -0.6963533564
                  ,r: 0
                  ,x: -80.595996
                  ,y: 28.599491
                  ,z: 400.63
                  ,duration : 0
                })
                //海里换算
                for (let index = 0; index < _LEM.EE.List.length; index++) {
                  _LEM.EE.List[index].z = (_LEM.EE.List[index].z * 1852).toFixed(2)
                }
                let rp = {positions: [
                    _LEM.EE.List
                  ],
                  multiplier:1
                }
                const entity = FRP_X(rp)
                // viewer.trackedEntity = entity;
                if (true) {
                  var viewModel = {
                    rate: 1.0
                  };
          
                  Cesium.knockout.track(viewModel);
                  var toolbar = document.getElementById('toolbar');
                  Cesium.knockout.applyBindings(viewModel, toolbar);
                  for (var name in viewModel) {
                    if (viewModel.hasOwnProperty(name)) {
                      Cesium.knockout.getObservable(viewModel, name).subscribe(updateMaterial);
                    }
                  }
                  function updateMaterial() {
                    IntelligentRoaming_Speed({ multiplier: Number(viewModel.rate) });
                  }
                }

                return
             
                // G.sTime(viewer,'2021-10-08T16:00:43.52Z')//+12
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
                Layer.GLTF.addx(_viewer,LaunchBottom)
              
                //黑色的猎鹰9号
                G.aGLTF(viewer,{
                  url:  (WEBGL_DEBUG?local:server) + '/%E4%BA%BA%E7%89%A9%E7%8E%AF%E6%A8%A1%E5%9E%8B/%E7%89%A9%E4%BD%93/%E7%81%AB%E7%AE%AD/%E7%8C%8E%E9%B9%B0%E4%B9%9D%E5%8F%B7/scene.gltf',
                  scale:0.06 / 2,//5
                  position:{
                    x: -80.60008139822472,
                    y: 28.600085733171827,
                    z: 26,
                  }
                })
                 
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
                
                Layer.GLTF.addx(viewer,LaunchSite)
               
               
                // return
                var SPACE_X_ = {
                  x: 100.31569904188999,
                  y: 41.11799996109391,
                  z: 80,
                  
                }
                var SPACE_X_AAA = G.aGLTF(viewer,{
                  url:  uri + 'aaa' + '.glb',
                  scale:35,//5
                  position:SPACE_X_
                }) 

            
                var SPACE_X_BBB = G.aGLTF(viewer,{
                  url:  uri + 'bbb' + '.glb',
                  scale:35,//5
                  position:SPACE_X_
                }) 

              
                var SPACE_X_CCC = G.aGLTF(viewer,{
                  url:  uri + 'ccc' + '.glb',
                  scale:35,//5
                  position:SPACE_X_
                }) 
                  

                var SPACE_X_DDD = G.aGLTF(viewer,{
                  url:  uri + 'ddd' + '.glb',
                  scale:35,//5
                  position:SPACE_X_
                }) 
                return

                viewer.scene.postUpdate.addEventListener(function (scene, time) {
                  if (!Cesium.defined(entity.position) || !entity.position.getValue(time)) {
                    return;
                  }
                    console.log(origin)
                  var hpr = new Cesium.HeadingPitchRoll(0, 0, 0);
                  var origin = entity.position.getValue(time)
                  var modelMatrix = Cesium.Transforms.headingPitchRollToFixedFrame(origin, hpr);
                  SPACE_X_BBB.modelMatrix = modelMatrix;
                  SPACE_X_CCC.modelMatrix = modelMatrix;
                  SPACE_X_DDD.modelMatrix = modelMatrix;
                });
        
              
            
              
              
                // initBox(_viewer,entity)

                


                Layer.LEFT_CLICK(_viewer)

                // setInterval(() => {
                //   console.log( G.BOLT.getCameraView(_viewer))
                // }, 30000);
                // var setvisible = G.EFFECT.runshineAnalysis();
                // setvisible(_viewer, 'play'); //stop//日照分析

                
                // Layer.provider.add(_viewer)//添加高程
                
        
                // {x: 121.03577759595257, y: 29.712778780221058, z: 113.58550007303276, name: '经纬度'}
              
                
              }
          },Cesium);
        } else {
            alert('浏览器不支持WebGL，需更换浏览器')
        }

      }
     
  })()
}
