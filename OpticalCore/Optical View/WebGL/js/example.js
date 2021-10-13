'use strict';
(function (window) {
    window.mousePosition = function(ev) {
        if (ev.pageX || ev.pageY) {//firefox、chrome等浏览器
            return { x: ev.pageX, y: ev.pageY };
        }
        return {// IE浏览器
            x: ev.clientX + document.body.scrollLeft - document.body.clientLeft,
            y: ev.clientY + document.body.scrollTop - document.body.clientTop
        };
    }
    class  OpticalCor_Logic{
        constructor() {
            this.socket; this.begin; 
      
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

        //socket通信 
        WebSocketint(port) {
           var socket;
            try {
                socket = new WebSocket('ws://127.0.0.1:' + port); 
            } catch (e) {
            
                console.log('error' + e);
                return;
            }
            socket.onopen = sOpen;
            socket.onerror = sError;
            socket.onmessage = sMessage;
            socket.onclose = sClose;
            function sOpen() {
                console.log('connect success!');

                socket.send(JSON.stringify({
                    type:"client",
                    msg:"success"
                }));
            }
            function sError(e) {
                console.log("error ", e);
            }
            function sMessage(messageEvent) {
                //messageEvent:MessageEvent 对象			
                var data = messageEvent.data;//来自服务器的数据
                var origin = messageEvent.origin;//服务器的地址

                console.log(data, origin);
            }
            function sClose(e) {
                console.log("connect closed:" + e.code);
            }
            this.socket = socket;
            return true;
        }
        //通信发送信息
        SocketintSend(str) {
            console.log(str,'<=【】' )
            socket.send(str);
        }
        //结束通信
        SocketintClose() {
            socket.close();
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
                        //开启hdr
                        _viewer.scene.highDynamicRange = true;

                        _viewer.scene.globe.enableLighting = true;

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
        example_addBaseLayer() {
            OpticalCore.addBaseLayer(window.GIS, {
                name: '影像底图',
                type: 'mapbox',//www_google sl_geoq // 被墙了暂时无法使用
                layer: 'blue',//satellite
                cn: true,
                brightness: 1
            });
        }
        //建筑分层
        example_BuildingStratification(path){
            // var socket = this.socket
            $("#mapBox").append(`<div class="tree well infoview" style="max-height: 666px;overflow: auto;-ms-overflow-style: none;scrollbar-width: none;">
                <ul>
                    <li id="tree_ul">
                        <span><i class="icon-folder-open"></i> `+path+`</span> 
                    </li>
                </ul>
            </div>`)
            //#region 加载建筑信息
            this.build_build()
            // socket.send('client success');

        }
        build_build() {
            
            var data = GIS.Socket_Response
            data = data.list.datas;
            console.log(data,GIS.Socket_Response)
            window.GIS.Building = data;
            function uuid() {
                var temp_url = URL.createObjectURL(new Blob());
                var uuid = temp_url.toString(); // blob:https://xxx.com/b250d159-e1b6-4a87-9002-885d90033be3
                URL.revokeObjectURL(temp_url);
                return uuid.substr(uuid.lastIndexOf("/") + 1);
            }
            function add3DTiles(element,flyTo,id){
                var mod =  OpticalCore.add3DTiles(window.GIS, {
                    name: "模型",
                    id:id,
                    url:  element.uri,//gis.crcr.top:9732
                    flyTo: flyTo,//视野转跳
                    height:10
                },{
                    color: "color('white', 1)",
                    show: true
                });
                mod.name = "build_build"
                mod.element = element;
            }

            var n1 = []; var n2 = [];
            // function  px_() {
            //     for (let index = 0; index < px.length; index++) {
            //         const e = px[index];
            //         if(e.name1 == 1){
            //             n1.push(e)
            //         }
            //         if(e.name1 == 2){
            //             n2.push(e)
            //         }
                
            //     }
            // }px_()
            var compare = function (prop) {
                return function (obj1, obj2) {
                    var val1 = obj1[prop];
                    var val2 = obj2[prop];if (val1 < val2) {
                        return -1;
                    } else if (val1 > val2) {
                        return 1;
                    } else {
                        return 0;
                    }            
                } 
            }
            n1=(n1.sort(compare("name2")));
            n2=(n2.sort(compare("name2")));

            var n1_min = Math.min.apply(Math, n1.map(function(o) {return o.index}))
            for (let i = 0; i < n1.length; i++) {
                n1[i].index = n1_min + i;
            }
            var n2_min = Math.min.apply(Math, n2.map(function(o) {return o.index}))
            for (let i = 0; i < n2.length; i++) {
                n2[i].index = n2_min + i;
            }
            n1.forEach(element => {
                data[element.index] = element.data
            });
            n2.forEach(element => {
                data[element.index] = element.data
            });

            //#endregion
                
            function makeTree(parentObj, treeJson) {
                var ulObj = $(`<ul></ul>`);
                for (var i = 0; i < treeJson.length; i++) {
                    var childHtml = `<li style="display: none;">`;
                    var aHtml;

                    var str = treeJson[i].text;
                    var _uuid = uuid();
            
                    if(treeJson[i].uri != null){
                        add3DTiles(treeJson[i],i == 0 ? true : false,_uuid)
                        aHtml = `<span><i  class="icon-minus-sign" data-id="` + _uuid + `"></i> `+ str +`</span>  <a id="`+_uuid+`" onclick="treeOnclick(this)"  href="#">可视</a>`
                    }else{
                        aHtml = `<span><i  class="icon-minus-sign" data-id="` + _uuid + `"></i> `+ str +`</span>`
                    }
                    childHtml += aHtml;
                    childHtml += "</li>";

                    if(treeJson[i].id != null && treeJson[i].id != '5')  {
                        var childObj = $(childHtml);
                        if (treeJson[i].children != null && treeJson[i].children.length > 0) {
                            makeTree(childObj, treeJson[i].children);
                        }
                        $(ulObj).append(childObj);
                    }  
                }
                $(parentObj).append($(ulObj));

            };
            
            
            function toTree(data) {
                let result = []
                if (!Array.isArray(data)) {
                return result
                }
                data.forEach(item => {
                    delete item.children;
                });
                let map = {};
                data.forEach(item => {
                    map[item.id] = item;
                });
                data.forEach(item => {
                let parent = map[item.pId];
                if (parent) {
                    (parent.children || (parent.children = [])).push(item);
                } else {
                    result.push(item);
                }
                });
                return result;
            }
            // var tree = toTree(data);
            console.log(data)
            makeTree($("#tree_ul"),data)
        
            $(function(){
                $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');
                $('.tree li.parent_li > span').on('click', function (e) {
                    var children = $(this).parent('li.parent_li').find(' > ul > li');
                    if (children.is(":visible")) {
                        children.hide('fast');
                        $(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
                    } else {
                        children.show('fast');
                        $(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
                    }
                    e.stopPropagation();
                });
            });

                
         
         
            return  data;
        }
        //移动高亮模型
        MobileDetection(_viewer){
            var viewer = _viewer == null ? window.GIS :_viewer;
            
            function mouseMove(ev) {
                ev = ev || window.event;
                var mousePos = mousePosition(ev);
                var pick = new Cesium.Cartesian2(mousePos.x, mousePos.y);
                    
                var mod = viewer.scene.pick(pick);//选取当前的entity
                if (mod && Cesium.defined(mod.primitive))
                { 
                    mod = mod.primitive
                       
                    for (var i = 0; i < viewer.scene.primitives.length; i++) {
                        var model = viewer.scene.primitives.get(i)
                        if(model.style_ != null){
                            if(!model.show_){
                                model.style = model.style_;
                            }
                        }
                    }
                   
                    mod.style_ = mod.style;
                      
                    var defaultStyle = new Cesium.Cesium3DTileStyle({
                        color: "color('white', 0.5)"
                    });
                    mod.style = defaultStyle

                }
               
            }
            document.onmousemove = mouseMove;
        }
        BuildPull_out() {
            this.MobileDetection()
            var handlers = new Cesium.ScreenSpaceEventHandler( window.GIS.scene.canvas);
          
            handlers.setInputAction(function (movement) {
                var modle = window.GIS.scene.pick(movement.position);//选取当前的entity;
                if(modle && Cesium.defined(modle.tileset))
                { 
                    modle = modle.tileset
                    // var modle = VMSDS.core.QueryModel_Scene(window.VMSDS.GIS,mod.id)
                    if(Cesium.defined(modle))
                    { 
                        var viewer = app_viewer()
                        add_mod(viewer,modle)
                        function add_mod(viewer,e) {
                            var model = OpticalCore.add3DTiles(viewer, {
                                name:"Overview_model",
                                url: modle.url,
                                duration: 0,
                                flyTo:true,//视野转跳
                                height:10
                            },{
                                color: "color('white', 1)",
                                show: true
                            });
                            model.element = e;
                        }

                        window.mousePosition = function(){
                            return {// IE浏览器
                                x: 0,
                                y: 0
                            };
                        } 
                    }
                   
                }
            }, Cesium.ScreenSpaceEventType.LEFT_CLICK);
            

            function app_viewer() {
                var viewer = control.addOverview("Overview");
                $(".overview-div").append(`
                <div class="btn-group mb-2" style="position: absolute;bottom: 10%;left: 50%;z-index: 999;">
                    <button type="button" class="btn btn-light">复位</button>
                </div>
                `);
            
                viewer.scene.globe.show = false;
                $($($("#Overview").find(".btn-group")[0]).find("button")[0]).click(function () {
                    
                    var model = OpticalCore.QueryModel_Scene_x(viewer,{"name":"Overview_model"});
                    if(model.length <= 0){return;}
                    viewer.flyTo(model[0], {
                        offset : {
                            heading : Cesium.Math.toRadians(0.0),
                            pitch : Cesium.Math.toRadians(-25),
                            range : 0
                        },
                        duration: 0
                    });
                
                })
                var btn = $($($("#Overview").find(".btn-group")[0]).find("button")[0]);
                $(btn).html("正在转跳中..");
                $(btn).attr('disabled',true);
                setTimeout(() => {
                    setTimeout(() => {
                        $(btn).attr('disabled',false);
                        $(btn).html("复位")
                    }, (1 * 1000 + 500));
                
                }, 1000);
                return viewer;
            }
            
       
        }

        add3DTiles(url){
            var model = OpticalCore.add3DTiles(window.GIS, {
                url:url,
                duration: 0,
                flyTo:  true ,//视野转跳
                height:10
            },{
                color: "color('white', 1)",
                show: true
            });
        }
    }
    window.Logic = OpticalCor_Logic;

})(window);
