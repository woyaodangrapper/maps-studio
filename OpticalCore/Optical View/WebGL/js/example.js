'use strict';
(function (window) {
    class  OpticalCor_Logic{
        constructor() {
           
      
        }
        //检测浏览器webgl支持
        webglReport() {
            //获取浏览器类型及版本
            function getExplorerInfo() {
                var explorer = window.navigator.userAgent.toLowerCase();
                //ie 
                if (explorer.indexOf("msie") >= 0) {
                    var ver = Number(explorer.match(/msie ([\d]+)/)[1]);
                    return { type: "IE", version: ver };
                }
                //firefox 
                else if (explorer.indexOf("firefox") >= 0) {
                    var ver = Number(explorer.match(/firefox\/([\d]+)/)[1]);
                    return { type: "Firefox", version: ver };
                }
                //Chrome
                else if (explorer.indexOf("chrome") >= 0) {
                    var ver = Number(explorer.match(/chrome\/([\d]+)/)[1]);
                    return { type: "Chrome", version: ver };
                }
                //Opera
                else if (explorer.indexOf("opera") >= 0) {
                    var ver = Number(explorer.match(/opera.([\d]+)/)[1]);
                    return { type: "Opera", version: ver };
                }
                //Safari
                else if (explorer.indexOf("Safari") >= 0) {
                    var ver = Number(explorer.match(/version\/([\d]+)/)[1]);
                    return { type: "Safari", version: ver };
                }
                return { type: explorer, version: -1 };
            }
            var exinfo = getExplorerInfo();
            if (exinfo.type == "IE" && exinfo.version < 11) {
                return false;
            }

            try {
                var glContext;
                var canvas = document.createElement('canvas');
                var requestWebgl2 = typeof WebGL2RenderingContext !== 'undefined';
                if (requestWebgl2) {
                    glContext = canvas.getContext('webgl2') || canvas.getContext('experimental-webgl2') || undefined;
                }
                if (glContext == null) {
                    glContext = canvas.getContext('webgl') || canvas.getContext('experimental-webgl') || undefined;
                }
                if (glContext == null) {
                    return false;
                }
            } catch (e) {
                return false;
            }
            return true;
        }

        //抛出全部方法
        findProperties(obj,...arg){
        
            function getProperty(new_obj){
        
            if(new_obj.__proto__ === null){ //说明该对象已经是最顶层的对象
                return [];
            }
        
            let properties = Object.getOwnPropertyNames(new_obj);
        
            let arr = [];  
            
            arg.forEach((v)=>{
            
                const newValue = properties.filter((property)=>{
                    return property.startsWith(v);
                })
            
                if(newValue.length>0){
                    arr = arr.concat(newValue);
                }
            
            })
        
            return [...arr,...getProperty(new_obj.__proto__)];
        
            }
        
            return getProperty(obj);   
        
        
        }
        //初始化地球
        example_Init() {
            if (this.webglReport()) {//判断浏览器是否支持WebGL
                OpticalCore.initialization({
                    id: 'mapBox',
                    success: function (_viewer) {
                        window.GIS = _viewer;
                        _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
                        //do something...
                        // 访问球体对象（viewer.scene.globe） 并设置颜色
                        // _viewer.scene.globe.baseColor = new Cesium.Color.fromBytes(9, 21, 30, 1);
                        //_viewer.scene.backgroundColor = new Cesium.Color.fromCssColorString("#fff0");

                        _viewer.scene.sun.show = false; //在Cesium1.6(不确定)之后的版本会显示太阳和月亮，不关闭会影响展示
                        _viewer.scene.moon.show = false;
                        _viewer.scene.skyBox.show = false;//关闭天空盒，否则会显示天空颜色
                        _viewer.scene.backgroundColor = new Cesium.Color.fromCssColorString("#2D2D30");
                        _viewer.scene.globe.show = false; //不显示地球，这条和地球透明度选一个就可以


                    }
                });
            } else {
                //提示用户浏览器不支持WebGL，需更换浏览器
            }
        }
     //加载底图
    example_addBaseLayer(type,url) {
        var config = null;
        switch (Number(type)) {
            case 1:

                var urlStr = url;
                var index = Number(urlStr);
                if (!isNaN(index))//如果url是默认数字的话 调用内置的在线地图资源
                {
                    // alert("是数字");
                    switch (index) {
                        case 1:
                            config = {
                                name: '影像底图',
                                type: 'mapbox',//www_google sl_geoq
                                layer: 'satellite',
                                // crs: '4326',
                                brightness: 1
                            }  
                            break;
                        case 2:
                            config = {
                                name: '影像底图',
                                type: 'mapbox',//www_google sl_geoq
                                layer: 'navigation',
                                // crs: '4326',
                                brightness: 0
                            } 
                            break;
                        case 3:
                            config = {
                                name: '影像底图',
                                type: 'mapbox',//www_google sl_geoq
                                layer: 'blue',
                                // crs: '4326',
                                brightness: 0
                            } 
                            break; 
                        case 4:
                            config = {
                                name: '影像底图',
                                type: 'tdt',//www_google sl_geoq
                                layer: 'blue',
                                brightness: 0
                            } 
                            break; 
                        case 5:
                            config = {
                                name: '影像底图',
                                type: 'tdt',//www_google sl_geoq
                                layer: 'satellite',
                                brightness: 0
                            } 
                            break; 
                        default:
                            break;
                    }
                }else{
                    config = {
                        name: '影像底图',
                        type: 'arcgis_cache',
                        url: url + '/L{arc_z}/R{arc_y}/C{arc_x}.png'
                    }
                }

               
                break;  
            case 2:
                var provider = new Cesium.CesiumTerrainProvider({
                    url: url,
                    requestWaterMask : true,//开启法向量
                    requestVertexNormals : true//开启水面特效
                });
                window.GIS.terrainProvider =provider;
                break;
            default:
             
                break;
        }
        if(Number(type) !== 2)
            if(config !== null)
            {

            }
            else
            console.warn("无法查找到对应地图底图配置",{type,url})
               
    }
       
        
    }
    window.OpticalCor_Logic = OpticalCor_Logic;

})(window);
