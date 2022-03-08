  
var script = [
//编辑器资源
// ,'<link rel=stylesheet href="/lib/plugins/codemirror-5.14.2/doc/docs.css">'
// ,'<link rel="stylesheet" href="/lib/plugins/codemirror-5.14.2/lib/codemirror.css">'
// ,'<link rel="stylesheet" href="/lib/plugins/codemirror-5.14.2/addon/hint/show-hint.css">'
// ,'<script src="/lib/plugins/codemirror-5.14.2/lib/codemirror.js"></script>'
// ,'<script src="/lib/plugins/codemirror-5.14.2/addon/hint/show-hint.js"></script>'
// ,'<script src="/lib/plugins/codemirror-5.14.2/addon/hint/javascript-hint.js"></script>'
// ,'<script src="/lib/plugins/codemirror-5.14.2/mode/javascript/javascript.js"></script>'
// ,'<script src="/lib/plugins/codemirror-5.14.2/mode/markdown/markdown.js"></script>'
//<!-- 初始化三维库 -->  <!-- 实列 -->
,'<script src="./example/1-1.0.0-你好地球.js"></script>'
];
script.forEach(element => {
  document.writeln(element);
});
const uri = './example/'
const version = "1.0.0"


function init(e) {
  
  //编译器对象初始化
  Get("index.js",function (data) {
    // document.getElementById("code").innerHTML = data;
    _editor = init_editor(data)
    if(e)e()//最后一个get防止异步变量无法同步
  },"text")

}


window.onload = () => {
  //初始化交互信息
  init();

}

/**
 * Get请求
 * @param {*} url 请求地址
 * @param {*} event 回调方法
 * @param {*} textType 返回类型
 */
 const Get = (url,event,textType) => {
  var request=new XMLHttpRequest();
  var method = "GET";
  var TextType = textType ?? "JSON"
  request.open(method,url);
  request.send(null);
  request.onreadystatechange = function(){
    if (request.readyState==4&&(request.status==200 || request.status==304))
    {
      if(event)
      {
        let data = request.responseText
        switch (TextType) {
          case "JSON":
            data = eval('(' + data + ')')
            break;
          default:
            break;
        }
        event(data);
      }
    }
  }
}
