/*第三方类库加载管理js，方便切换lib*/
(function () {
    var r = new RegExp("(^|(.*?\\/))(include-lib\.js)(\\?|$)"),
        s = document.getElementsByTagName('script'), targetScript;
    for (var i = 0; i < s.length; i++) {
        var src = s[i].getAttribute('src');
        if (src) {
            var m = src.match(r);
            if (m) {
                targetScript = s[i];
                break;
            }
        }
    }

    //当前版本号,用于清理浏览器缓存
    var cacheVersion = Date.parse(new Date());

    // cssExpr 用于判断资源是否是css
    var cssExpr = new RegExp('\\.css');

    function inputLibs(list) {
        if (list == null || list.length == 0) return;

        for (var i = 0, len = list.length; i < len; i++) {
            var url = list[i];
            if (cssExpr.test(url)) {
                var css = '<link rel="stylesheet" href="' + url + '?time=' + cacheVersion + '">';
                document.writeln(css);
            } else {
                var script = '<script type="text/javascript" src="' + url + '?time=' + cacheVersion + '"><' + '/script>';
                document.writeln(script);
            }
          
        }
    }

    //加载类库资源文件
    function load() {
        var arrInclude = (targetScript.getAttribute('include') || "").split(",");
        var libpath = (targetScript.getAttribute('libpath') || "");
        if (libpath.lastIndexOf('/') != libpath.length - 1)
            libpath += "/";

        var libsConfig = {
            'VMSDS': [     
                // libpath + "assets/js/vmsds-auxiliary.js",
                libpath + "lib/Cesium/Cesium.js",
                libpath + "lib/Cesium/Widgets/widgets.css",

                libpath + "core/index.js",
                libpath + "core/core-src.js",
                libpath + "core/mousezoom.css",

                libpath + "js/example.js",
            ],
        };

        for (var i in arrInclude) {
            var key = arrInclude[i];
            inputLibs(libsConfig[key]);
        }
    }
    load();
})();