var script = ['<!-- 初始化三维库 --><script type="text/javascript" src="/lib/index.js" libpath="../" include="Taoist"></script>'];
script.forEach((element) => {
  document.writeln(element);
});

window.onload = () => {
  const _ = (_viewer, params) => {
    if (_viewer instanceof Cesium.Viewer) return new Gear(_viewer)[params]();
  };
  /**
   * 添加模型
   * @param {*} url 模型链接
   */
  function a3DTiles(flyTo, url, _uuid) {
    //添加模型
    G.aTiles(
      viewer,
      {
        id: _uuid, //生成随机id_
        url: url.replace('http://127.0.0.1:55328', 'http://121.40.42.254:8008'), //http://127.0.0.1:64158
        flyTo,
        heightOffset: 0,
        height: 10,
        style: {
          color: "color('white', 1)",
          show: true,
        }
      }
    );
  }
  (function () {
    //初始化地球0
    if (G.U.webglReport()) {
      //判断浏览器是否支持WebGL
      G.create3D(
        {
          id: 'mapBox',
          showGroundAtmosphere: true,
          debug: false,
          success: function (_viewer) {
            window.viewer = _viewer;
            G.sTime(_viewer); //时间初始化 默认是12点

            // _(_viewer, 'example_addBaseLayer'); //添加底图

            G.U.Get('http://121.40.42.254:8003/sys/get/gis_', function (response) {
              _build(response, a3DTiles);
            });

            _viewer.cesiumWidget.screenSpaceEventHandler.removeInputAction(Cesium.ScreenSpaceEventType.LEFT_DOUBLE_CLICK);
            // _viewer.scene.globe.show = false;
            _viewer.scene.highDynamicRange = true;
            _viewer.scene.globe.baseColor = new Cesium.Color.fromCssColorString('#171744');
            // _viewer.scene.sun.show = false; //在Cesium1.6(不确定)之后的版本会显示太阳和月亮，不关闭会影响展示
            _viewer.scene.moon.show = false;
            _viewer.scene.skyBox.show = false; //关闭天空盒，否则会显示天空颜色
            _viewer.scene.backgroundColor = new Cesium.Color.fromCssColorString('#171744');
          },
        },
        Cesium
      );
    } else {
      alert('浏览器不支持WebGL，需更换浏览器');
    }

    /**
     * 初始化树结构（如果有的话
     * @param {*} response 接口数据
     * @param {*} a3DTiles 回调
     * @returns
     */
    function _build(response, a3DTiles) {
      $('#mapBox').append(
        `<div class="tree well infoview" style="cursor:pointer;max-height: 666px;overflow: auto;box-shadow:0px 0px 10px rgba(12, 12, 12, 0.2);min-width: 233px;">
                <ul>
                    <li id="tree_ul">
                        <span><i class="icon-folder-open"></i> ` +
          '建筑' +
          `</span> 
                    </li>
                </ul>
              </div>`
      );
      var data = response;
      data = data.list.datas;

      var n1 = [];
      var n2 = [];
      var compare = function (prop) {
        return function (obj1, obj2) {
          var val1 = obj1[prop];
          var val2 = obj2[prop];
          if (val1 < val2) {
            return -1;
          } else if (val1 > val2) {
            return 1;
          } else {
            return 0;
          }
        };
      };
      n1 = n1.sort(compare('name2'));
      n2 = n2.sort(compare('name2'));

      var n1_min = Math.min.apply(
        Math,
        n1.map(function (o) {
          return o.index;
        })
      );
      for (let i = 0; i < n1.length; i++) {
        n1[i].index = n1_min + i;
      }
      var n2_min = Math.min.apply(
        Math,
        n2.map(function (o) {
          return o.index;
        })
      );
      for (let i = 0; i < n2.length; i++) {
        n2[i].index = n2_min + i;
      }
      n1.forEach((element) => {
        data[element.index] = element.data;
      });
      n2.forEach((element) => {
        data[element.index] = element.data;
      });

      function makeTree(parentObj, treeJson) {
        var ulObj = $(`<ul></ul>`);
        for (var i = 0; i < treeJson.length; i++) {
          var childHtml = `<li style="display: none;">`;
          var aHtml;

          var str = treeJson[i].text;
          var _uuid = _(viewer, 'uuid'); //生成随机id;
          if (treeJson[i].uri != null) {
            a3DTiles(i == 0 ? false : true, treeJson[i].uri, _uuid);
           
            aHtml = `<span><i  class="icon-minus-sign" data-id="` + _uuid + `"></i> ` + str + `</span>  <a id="` + _uuid + `" onclick="treeOnclick(this)"  href="#">可视</a>`;
          } else {
            aHtml = `<span><i  class="icon-minus-sign" data-id="` + _uuid + `"></i> ` + str + `</span>`;
          }
          childHtml += aHtml;
          childHtml += '</li>';

          if (treeJson[i].id != null && treeJson[i].id != '5') {
            var childObj = $(childHtml);
            if (treeJson[i].children != null && treeJson[i].children.length > 0) {
              makeTree(childObj, treeJson[i].children);
            }
            $(ulObj).append(childObj);
          }
        }
        $(parentObj).append($(ulObj));
      }
      // var tree = toTree(data);
      makeTree($('#tree_ul'), data);

      $(function () {
        $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');
        $('.tree li.parent_li > span').on('click', function (e) {
          var children = $(this).parent('li.parent_li').find(' > ul > li');
          if (children.is(':visible')) {
            children.hide('fast');
            $(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
          } else {
            children.show('fast');
            $(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
          }
          e.stopPropagation();
        });
      });

      return data;
    }
  })();
};
