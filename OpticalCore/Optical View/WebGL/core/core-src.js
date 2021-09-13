'use strict';
(function (window) {
    /**
     * 创建一个地球
     * @param options
     * @returns viewer
     */
    function initialization(options) {

        Cesium.Ion.defaultAccessToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJiNGZiOTc1NS0zZmZlLTQ4MzUtODFlMS00ZDI2NWE5YTFkZjIiLCJpZCI6MTgwMDUsInNjb3BlcyI6WyJhc3IiLCJnYyJdLCJpYXQiOjE1NzMxMTcwODd9.WPytI-wsAoBmC7NLmz01l0GcYoh3bvTES7z1yZQgGMM'

        //初始化部分参数 如果没有就默认设为false;
        var args = ["geocoder", "homeButton", "sceneModePicker", "baseLayerPicker", "navigationHelpButton", "animation", "timeLine", "fullscreenButton", "vrButton", "infoBox", "selectionIndicator", "shadows"];
        for (var i = 0; i < args.length; i++) {
            if (!options[args[i]]) {
                options[args[i]] = false;
            }
        }
        options["shouldAnimate"] = true;//飞行漫游启动 viewer动画效果
        var container = options["id"];
        //创建viewer
        var viewer = new Cesium.Viewer(container, options);
        /**对Cesium的改造 *******************************************************************/
        //隐藏Cesium原有的一些控件，默认只剩一个球
        _hideCesiumElement();

        //设置鼠标的样式，在使用滚轮及右键对地球缩放或旋转时在鼠标位置添加一个图标
        MouseStyle(viewer, container);

        //解决限定相机进入地下 https://github.com/AnalyticalGraphicsInc/cesium/issues/5837
        viewer.camera.changed.addEventListener(function () {
            if (viewer.camera._suspendTerrainAdjustment && viewer.scene.mode === Cesium.SceneMode.SCENE3D) {
                viewer.camera._suspendTerrainAdjustment = false;
                viewer.camera._adjustHeightForTerrain();
            }
        });

        viewer.scene.globe.depthTestAgainstTerrain = true;
        //开启hdr
        viewer.scene.highDynamicRange = true;
        //光
        viewer.scene.globe.enableLighting = true;
        //移除默认的bing影像图层
        viewer.imageryLayers.removeAll();
        viewer.clock.currentTime = Cesium.JulianDate.fromDate(new Date());

        viewer.config = options;
        /************************回调函数 */
        //加载成功后回调函数
        if (options.success) {
            options.success(viewer);
        }

        return viewer;
    }

    //隐藏Cesium原有的一些控件
    function _hideCesiumElement() {
        $('.cesium-viewer-toolbar').hide();
        $('.cesium-viewer-animationContainer').hide();
        $('.cesium-viewer-timelineContainer').hide();
        $('.cesium-viewer-bottom').hide();
    }

    //设置鼠标的样式
    function MouseStyle(viewer, container) {
        //修改视图默认鼠标操作方式
        viewer.scene.screenSpaceCameraController.zoomEventTypes = [Cesium.CameraEventType.WHEEL, Cesium.CameraEventType.PINCH];
        viewer.scene.screenSpaceCameraController.tiltEventTypes = [Cesium.CameraEventType.MIDDLE_DRAG, Cesium.CameraEventType.PINCH, Cesium.CameraEventType.RIGHT_DRAG];

        $('#' + container).append('<div class="cesium-mousezoom"><div class="zoomimg"/></div>');
        var handler = new Cesium.ScreenSpaceEventHandler(viewer.scene.canvas);

        //按住鼠标右键
        handler.setInputAction(function (event) {
            handler.removeInputAction(Cesium.ScreenSpaceEventType.MOUSE_MOVE);
            $('.cesium-mousezoom').addClass('cesium-mousezoom-visible');
            $('.cesium-mousezoom').css({
                top: event.position.y + 'px',
                left: event.position.x + 'px'
            });
        }, Cesium.ScreenSpaceEventType.RIGHT_DOWN);
        //抬起鼠标右键
        handler.setInputAction(function (event) {
            handler.setInputAction(function (evnet) {
                $('.cesium-mousezoom').css({
                    top: evnet.endPosition.y + 'px',
                    left: evnet.endPosition.x + 'px'
                });
            }, Cesium.ScreenSpaceEventType.MOUSE_MOVE);
            $('.cesium-mousezoom').removeClass('cesium-mousezoom-visible');
        }, Cesium.ScreenSpaceEventType.RIGHT_UP);

        //按住鼠标中键
        handler.setInputAction(function (event) {
            handler.removeInputAction(Cesium.ScreenSpaceEventType.MOUSE_MOVE);
            $('.cesium-mousezoom').addClass('cesium-mousezoom-visible');
            $('.cesium-mousezoom').css({
                top: event.position.y + 'px',
                left: event.position.x + 'px'
            });
        }, Cesium.ScreenSpaceEventType.MIDDLE_DOWN);
        //抬起鼠标中键
        handler.setInputAction(function (event) {
            handler.setInputAction(function (evnet) {
                $('.cesium-mousezoom').css({
                    top: evnet.endPosition.y + 'px',
                    left: evnet.endPosition.x + 'px'
                });
            }, Cesium.ScreenSpaceEventType.MOUSE_MOVE);
            $('.cesium-mousezoom').removeClass('cesium-mousezoom-visible');
        }, Cesium.ScreenSpaceEventType.MIDDLE_UP);

        //滚轮滚动
        handler.setInputAction(function (evnet) {
            handler.setInputAction(function (evnet) {
                $('.cesium-mousezoom').css({
                    top: evnet.endPosition.y + 'px',
                    left: evnet.endPosition.x + 'px'
                });
            }, Cesium.ScreenSpaceEventType.MOUSE_MOVE);
            $('.cesium-mousezoom').addClass('cesium-mousezoom-visible');
            setTimeout(function () {
                $('.cesium-mousezoom').removeClass('cesium-mousezoom-visible');
            }, 200);
        }, Cesium.ScreenSpaceEventType.WHEEL);
    }
    //创建地图底图
    function createImageryProvider(config) {
        var options = {};
        for (var key in config) {
            var value = config[key];
            if (value == null) return;

            switch (key) {
                case 'crs':
                    if (value == '4326' || value.toUpperCase() == 'EPSG4326') {
                        options.tilingScheme = new Cesium.GeographicTilingScheme({
                            numberOfLevelZeroTilesX: config.numberOfLevelZeroTilesX || 2,
                            numberOfLevelZeroTilesY: config.numberOfLevelZeroTilesY || 1
                        });
                    } else {
                        options.tilingScheme = new Cesium.WebMercatorTilingScheme({
                            numberOfLevelZeroTilesX: config.numberOfLevelZeroTilesX || 2,
                            numberOfLevelZeroTilesY: config.numberOfLevelZeroTilesY || 1
                        });
                    }
                    break;
                case 'rectangle':
                    options.rectangle = Cesium.Rectangle.fromDegrees(value.xmin, value.ymin, value.xmax, value.ymax);
                    break;
                default:
                    options[key] = value;
                    break;
            }
        }

        if (options.proxy) {
            options.url = new Cesium.Resource({
                url: options.url,
                proxy: options.proxy
            });
        }

        var layer;
        switch (options.type) {
            case 'image':
                layer = new Cesium.SingleTileImageryProvider(options);
                break;
            case 'xyz':
            case 'tile':
                options.customTags = {
                    "z&1": function z1(imageryProvider, x, y, level) {
                        return level + 1;
                    }
                };
                layer = new Cesium.UrlTemplateImageryProvider(options);
                break;
            case 'wms':
                layer = new Cesium.WebMapServiceImageryProvider(options);
                break;
            case 'wmts':
                layer = new Cesium.WebMapTileServiceImageryProvider(options);
                break;
            case "arcgis":
            case "arcgis_tile":
            case "arcgis_dynamic":
                layer = new Cesium.ArcGisMapServerImageryProvider(options);
                break;
            case "arcgis_cache":
                if (!Cesium.UrlTemplateImageryProvider.prototype.padLeft0) {
                    Cesium.UrlTemplateImageryProvider.prototype.padLeft0 = function (numStr, n) {
                        numStr = String(numStr);
                        var len = numStr.length;
                        while (len < n) {
                            numStr = "0" + numStr;
                            len++;
                        }
                        return numStr;
                    };
                }
                options.customTags = {
                    //小写
                    arc_x: function arc_x(imageryProvider, x, y, level) {
                        return imageryProvider.padLeft0(x.toString(16), 8);
                    },
                    arc_y: function arc_y(imageryProvider, x, y, level) {
                        return imageryProvider.padLeft0(y.toString(16), 8);
                    },
                    arc_z: function arc_z(imageryProvider, x, y, level) {
                        return imageryProvider.padLeft0(level.toString(), 2);
                    },
                    //大写
                    arc_X: function arc_X(imageryProvider, x, y, level) {
                        return imageryProvider.padLeft0(x.toString(16), 8).toUpperCase();
                    },
                    arc_Y: function arc_Y(imageryProvider, x, y, level) {
                        return imageryProvider.padLeft0(y.toString(16), 8).toUpperCase();
                    },
                    arc_Z: function arc_Z(imageryProvider, x, y, level) {
                        return imageryProvider.padLeft0(level.toString(), 2).toUpperCase();
                    }
                };
                layer = new Cesium.UrlTemplateImageryProvider(options);
                break;
            case "www_gaode":
                //高德
                var _url;
                switch (options.layer) {
                    case "vec":
                    default:
                        //style=7是立体的，style=8是灰色平面的
                        _url = 'http://' + (options.bigfont ? 'wprd' : 'webrd') + '0{s}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={x}&y={y}&z={z}';
                        break;
                    case "img_d":
                        _url = 'http://webst0{s}.is.autonavi.com/appmaptile?style=6&x={x}&y={y}&z={z}';
                        break;
                    case "img_z":
                        _url = 'http://webst0{s}.is.autonavi.com/appmaptile?x={x}&y={y}&z={z}&lang=zh_cn&size=1&scale=1&style=8';
                        break;
                    case "time":
                        var time = new Date().getTime();
                        _url = 'http://tm.amap.com/trafficengine/mapabc/traffictile?v=1.0&;t=1&x={x}&y={y}&z={z}&&t=' + time;
                        break;
                }

                layer = new Cesium.UrlTemplateImageryProvider({
                    url: options.proxy ? new Cesium.Resource({ url: _url, proxy: options.proxy }) : _url,
                    subdomains: ['1', '2', '3', '4'],
                    maximumLevel: 18
                });
                break;
            case "www_google":
                //谷歌国内
                var _url;

                if (config.crs == '4326' || config.crs == 'wgs84') {
                    //wgs84   无偏移
                    switch (options.layer) {
                        default:
                        case "img_d":
                            _url = 'http://www.google.cn/maps/vt?lyrs=s&x={x}&y={y}&z={z}';
                            break;
                    }
                } else {
                    //有偏移
                    switch (options.layer) {
                        case "vec":
                        default:
                            _url = 'http://mt{s}.google.cn/vt/lyrs=m@207000000&hl=zh-CN&gl=CN&src=app&x={x}&y={y}&z={z}&s=Galile';
                            break;
                        case "img_d":
                            _url = 'http://mt{s}.google.cn/vt/lyrs=s&hl=zh-CN&gl=CN&x={x}&y={y}&z={z}&s=Gali';
                            break;
                        case "img_z":
                            _url = 'http://mt{s}.google.cn/vt/imgtp=png32&lyrs=h@207000000&hl=zh-CN&gl=cn&x={x}&y={y}&z={z}&s=Galil';
                            break;
                        case "ter":
                            _url = 'http://mt{s}.google.cn/vt/lyrs=t@131,r@227000000&hl=zh-CN&gl=cn&x={x}&y={y}&z={z}&s=Galile';
                            break;
                    }
                }

                layer = new Cesium.UrlTemplateImageryProvider({
                    url: options.proxy ? new Cesium.Resource({ url: _url, proxy: options.proxy }) : _url,
                    subdomains: ['1', '2', '3'],
                    maximumLevel: 20
                });
                break;
            case "www_osm":
                //OSM开源地图
                var _url = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
                layer = new Cesium.UrlTemplateImageryProvider({
                    url: options.proxy ? new Cesium.Resource({ url: _url, proxy: options.proxy }) : _url,
                    subdomains: "abc",
                    maximumLevel: 18
                });
                break;
            case "www_geoq":
                //智图开源地图
                var _url = 'https://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineCommunity/MapServer/tile/{z}/{y}/{x}';
                layer = new Cesium.UrlTemplateImageryProvider({
                    url: options.proxy ? new Cesium.Resource({ url: _url, proxy: options.proxy }) : _url,
                    subdomains: "abc",
                    maximumLevel: 18
                });
                break; 
            case "thematic_geoq":
                //智图水系开源地图
                var _url = 'http://thematic.geoq.cn/arcgis/rest/services/ThematicMaps/WorldHydroMap/MapServer/tile/{z}/{y}/{x}';
                layer = new Cesium.UrlTemplateImageryProvider({
                    url: options.proxy ? new Cesium.Resource({ url: _url, proxy: options.proxy }) : _url,
                    subdomains: "abc",
                    maximumLevel: 18
                }); 
            case "sl_geoq":
                //智图深蓝开源地图
                var _url = 'https://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineStreetPurplishBlue/MapServer/tile/{z}/{y}/{x}';
                layer = new Cesium.UrlTemplateImageryProvider({
                    url: options.proxy ? new Cesium.Resource({ url: _url, proxy: options.proxy }) : _url,
                    subdomains: "abc",
                    maximumLevel: 18
                });
                break;
            case "local":
                //本地
                var _url = options.url + '/{z}/{y}/{x}.png';
                layer = new Cesium.UrlTemplateImageryProvider({
                    url: options.proxy ? new Cesium.Resource({ url: _url, proxy: options.proxy }) : _url,
                    subdomains: "abc",
                    maximumLevel: 18
                });
                break;
            case "tdt":
                //天地图
                var _url
                // 添加mapbox自定义地图实例 mapbox://styles/1365508153/ckmy004lc1bsj17n94k80cfik
                switch (options.layer) {
                    case 'satellite':
                        _url = "http://t0.tianditu.gov.cn/img_c/wmts?tk=d97070ed5b0f397ed2dd8317bcbb486d"
                        break;
                    case 'navigation': 
                       
                        break;
                    case 'blue':
                        _url = "http://services.arcgisonline.com/arcgis/rest/services/Canvas/World_Dark_Gray_Base/MapServer?tk=d97070ed5b0f397ed2dd8317bcbb486d";
                        break;       
                    case 'terrain':
                        break;       
                }
             

                layer = new Cesium.UrlTemplateImageryProvider({
                    url: _url,
                    subdomains: "abc",
                    maximumLevel: 18
                });
                break;
            case "mapbox":
                //mapboxgl的底图
                var style;
                // 添加mapbox自定义地图实例 mapbox://styles/1365508153/ckmy004lc1bsj17n94k80cfik
                switch (options.layer) {
                    case 'satellite':
                        style =  'ckmy0yizu18bx17pdcfh81ikn';
                        break;
                    case 'navigation': 
                        style =  'ckmy0li0j1cd717la2xd0mamg';
                        break;
                    case 'blue':
                        style =  'ckmy004lc1bsj17n94k80cfik';
                        break;       
                    case 'terrain':
                        style =  'ckn9dnm5b2m9a17o0nijfqbl3';
                }
                var url = 'https://api.mapbox.com/styles/v1'
                switch (options.cn) {
                    case false:
                        url =  'https://api.mapbox.com/styles/v1'
                        break;
                    case true: 
                        url =  'http://api.mapbox.cn/styles/v1'
                        break;
                  
                }


                var layer=new Cesium.MapboxStyleImageryProvider({
                    url:'https://api.mapbox.com/styles/v1',
                    username:'1365508153',
                    styleId: style,
                    accessToken: 'pk.eyJ1IjoiMTM2NTUwODE1MyIsImEiOiJja214ejg5ZWMwZGhqMnJxa3F3MjVuaTJqIn0.ERt-vJ_qoD10EP5CvwsEzQ',
                    scaleFactor:true,
                });
                break;
        }
        layer.config = options;
        layer.brightness = options.brightness;
        return layer;
    }
        /**
     * 添加3D图层
     * @param viewer
     * @param options
     */
    function add3DTiles(viewer, options,Style) {
        console.log(options.duration)
        var tileset = viewer.scene.primitives.add(new Cesium.Cesium3DTileset(options));
        tileset.readyPromise.then(function () {
            var boundingSphere = tileset.boundingSphere;
            if (options.flyTo) {
                // viewer.camera.viewBoundingSphere(boundingSphere, new Cesium.HeadingPitchRange(0.0, -0.5, boundingSphere.radius));
                var timer = setInterval(() => {
                    if(viewer.clock.multiplier == 1){
                        viewer.flyTo(tileset, {
                            offset : {
                                heading : Cesium.Math.toRadians(0.0),
                                pitch : Cesium.Math.toRadians(-25),
                                range : 0
                            },
                            duration: options.duration == undefined ? 3 : options.duration
                        });
                        clearInterval(timer)
                    }
                }, 500);
                
                //console.log('自动定位', new Cesium.HeadingPitchRange(0.0, -0.5, boundingSphere.radius));
            }
            viewer.camera.lookAtTransform(Cesium.Matrix4.IDENTITY);
            //此处为调整模型高度的函数
            var heightOffset = options.height;
            //heightOffset += 10;
            var cartographic = Cesium.Cartographic.fromCartesian(boundingSphere.center);
            var heightOffset = options.height;
            var surface = Cesium.Cartesian3.fromRadians(cartographic.longitude, cartographic.latitude,options.heightOffset == null ? 0 : options.heightOffset);
            var offset = Cesium.Cartesian3.fromRadians(cartographic.longitude, cartographic.latitude, heightOffset);
            var translation = Cesium.Cartesian3.subtract(offset, surface, new Cesium.Cartesian3());
            tileset.modelMatrix = Cesium.Matrix4.fromTranslation(translation);
        }).otherwise(function (error) {
            console.log('add3DTiles', error);
        });
        var defaultStyle = new Cesium.Cesium3DTileStyle({
            color: Style.color
        });
        tileset.style = defaultStyle;
        tileset.show = Style.show;
        tileset.id = options.id;
        tileset.name = options.name;


        if(options.ca){
            //添加模型高低颜色差
            tileset.style = new Cesium.Cesium3DTileStyle({
                color: {
                    conditions: [
                        ['${floor} >= 300', 'rgba(45, 0, 75, 0.5)'],
                        ['${floor} >= 200', 'rgb(102, 71, 151)'],
                        ['${floor} >= 100', 'rgb(170, 162, 204)'],
                        ['${floor} >= 50', 'rgb(224, 226, 238)'],
                        ['${floor} >= 25', 'rgb(252, 230, 200)'],
                        ['${floor} >= 10', 'rgb(248, 176, 87)'],
                        ['${floor} >= 5', 'rgb(198, 106, 11)'],
                        ['true', 'rgb(127, 59, 8)']
                    ]
                }
            });

            
        }

        

        return tileset;
    }
    //返回根据自定义查询 针对3DTiles/scene 查询模型v
    function QueryModel_Scene_x(viewer, type) {
        var _arr = [];
        for (var i = 0; i < viewer.scene.primitives.length; i++) {
            if(viewer.scene.primitives.get(i)[Object.keys(type)[0]] === type[Object.keys(type)[0]]){
                 _arr.push(viewer.scene.primitives.get(i))
            }
            
        }
        return _arr;
    }
    //根据自定义查询entities下模型 并且返回
    function QueryModel_Entities_x (viewer,type) {
        var entitys = viewer.entities._entities._array;
        var _arr = [];
        for (var i = 0; i < entitys.length; i++) {
            if(entitys[i][Object.keys(type)[0]] === type[Object.keys(type)[0]]){
                 _arr.push(entitys[i])
            }
            
        }
        return _arr;
    }
    /**
     * 添加地图底图图层
     * @param viewer
     * @param options
     */
     function addBaseLayer(viewer, options) {
        var imageryProvider = createImageryProvider(options);
        var imageryOption = {
            show: true,
            alpha: this._opacity
        };
        if (options.rectangle && options.rectangle.xmin && options.rectangle.xmax && options.rectangle.ymin && options.rectangle.ymax) {
            var xmin = options.rectangle.xmin;
            var xmax = options.rectangle.xmax;
            var ymin = options.rectangle.ymin;
            var ymax = options.rectangle.ymax;
            var rectangle = Cesium.Rectangle.fromDegrees(xmin, ymin, xmax, ymax);
            this.rectangle = rectangle;
            imageryOption.rectangle = rectangle;
        }
        if (options.brightness)
            imageryOption.brightness = options.brightness;
        if (options.minimumTerrainLevel)
            imageryOption.minimumTerrainLevel = options.minimumTerrainLevel;
        if (options.maximumTerrainLevel)
            imageryOption.maximumTerrainLevel = options.maximumTerrainLevel;
        var layer = new Cesium.ImageryLayer(imageryProvider, imageryOption);
        layer.config = options;
        viewer.imageryLayers.add(layer);

        return layer;
    }
    window.OpticalCore = {
        QueryModel_Entities_x,
        QueryModel_Scene_x,
        add3DTiles,
        initialization,//初始化地球
        addBaseLayer,//添加底图
    };

    
    //添加鹰眼控件
    function addOverview(container,father) {
        $("#" + container).html('<div class="overview-div"><div class="overview-close"></div><div id="eye" class="overview-map"></div></div>');
        $(".overview-div").append('<div class="overview-narrow"></div>');
        $(".overview-div").append('<div class="overview-enlarge"></div>');
        //1.创建双球
        var viewer = new Cesium.Viewer('eye',{
            fullscreenButton: false,
            orderIndependentTranslucency: false,
            contextOptions: {
                webgl: {
                    alpha: true,
                }
            },
        })

        viewer.scene.globe.depthTestAgainstTerrain = true;
        //开启hdr
        viewer.scene.highDynamicRange = true;

        viewer.scene.globe.enableLighting = true;
        //移除默认的bing影像图层
        viewer.imageryLayers.removeAll();
        viewer.clock.currentTime = Cesium.JulianDate.fromDate(new Date("2019/10/04 06:00:00"));

        //是否关闭大气效果
        viewer.scene.globe.showGroundAtmosphere = true;

        // VMSDS.effect.AtmosphericEffects(viewer);
        $(".overview-div").animate({width:'90%'});
        $(".overview-div").animate({height:'74%'});
        $("#eye").animate({width:'94%'});
        $("#eye").animate({height:'85%'});

        $(".overview-close").click(function () {
            $(".overview-div").animate({opacity:0},100,"linear",function(){
                $(".overview-div").remove();
            });

            window.mousePosition = function(ev) {
                if (ev.pageX || ev.pageY) {//firefox、chrome等浏览器
                    return { x: ev.pageX, y: ev.pageY };
                }
                return {// IE浏览器
                    x: ev.clientX + document.body.scrollLeft - document.body.clientLeft,
                    y: ev.clientY + document.body.scrollTop - document.body.clientTop
                };
            }
        })
        $(".overview-narrow").click(function () {
            $(".overview-div").animate({height:'20%'},100,"linear",function(){
            });
            $(".overview-div").animate({width:'21%'},100,"linear",function(){
                $(".overview-narrow").css('display','none');
                $(".overview-enlarge").css('display','block');
            });
        })
        $(".overview-enlarge").click(function () {
            $(".overview-div").animate({height:'85%'},100,"linear",function(){
            });
            $(".overview-div").animate({width:'94%'},100,"linear",function(){
                $(".overview-narrow").css('display','block');
                $(".overview-enlarge").css('display','none');
            });
        })
        viewer.scene.sun.show = false; //在Cesium1.6(不确定)之后的版本会显示太阳和月亮，不关闭会影响展示
        viewer.scene.moon.show = false;
        viewer.scene.skyBox.show = false;//关闭天空盒，否则会显示天空颜色
        viewer.scene.backgroundColor = new Cesium.Color(0.0, 0.0, 0.0, 0.0);


        //2.设置鹰眼图中球属性
        let control = viewer.scene.screenSpaceCameraController;
        control.enableRotate = true;
        control.enableTranslate = true;
        control.enableZoom = true;
        control.enableTilt = true;
        control.enableLook = false;

        $('.cesium-viewer-toolbar').hide();
        $('.cesium-viewer-animationContainer').hide();
        $('.cesium-viewer-timelineContainer').hide();
        $('.cesium-viewer-bottom').hide();
        
        let syncViewer = function () {
            viewer.camera.flyTo({
                destination: _changeViewerHeight(viewer,father.camera.position),
                orientation: {
                    heading: father.camera.heading,
                    pitch: father.camera.pitch,
                    roll: father.camera.roll
                },
                duration: 0.0
            });
        };
        if(father){
            //3. 同步
            father.entities.add({
                position: Cesium.Cartesian3.fromDegrees(0, 0),
                label: {
                    text: new Cesium.CallbackProperty(function () {
                        syncViewer();
                        return "";
                    }, true)
                }
            });
        }
        function _changeViewerHeight(viewer,position) {
            var positions = new Array();
            positions.push(position);
            var formatPos = viewer.positionHandler.formatPositon(position);
            var addedHeight = 1000;
            if (formatPos.z > 200) {
                addedHeight = 2000;
            } else if (formatPos.z > 1000) {
                addedHeight = formatPos.z * 2;
            }
            var positions_ = viewer.positionHandler.addPositionsHeight(positions, addedHeight);
            return positions_[0];
        }
        return viewer;
    }
   
    window.control = {
        addOverview,
       
    };;

})(window);
