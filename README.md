# XY.OAuth-CSharp
第三方登录插件.NET版

##使用
首先,从NuGet上安装"XY.OAuth"
然后在项目配置文件的根节点下"configuration"的子节点"appSettings"中添加如下下配置信息：
``` xml
<!--第三方登录配置 Start-->
<!-- 客户端ID -->
<add key="Sina_Client_ID" value=""/>
<!-- 密钥 -->
<add key="Sina_Client_Secret" value=""/>
<!-- 回掉地址 -->
<add key="Sina_Callback_URL" value=""/>

<!-- 客户端ID -->
<add key="QQ_Client_ID" value=""/>
<!-- 密钥 -->
<add key="QQ_Client_Secret" value=""/>
<!-- 回掉地址 -->
<add key="QQ_Callback_URL" value=""/>

<!-- 客户端ID -->
<add key="Baidu_Client_ID" value=""/>
<!-- API -->
<add key="Baidu_Client_API" value=""/>
<!-- 密钥 -->
<add key="Baidu_Client_Secret" value=""/>
<!-- 回掉地址 -->
<add key="Baidu_Callback_URL" value=""/>
<!--第三方登录配置 End-->
```

---

##支持
当前版本支持.NET3.5,.NET4.0,.NET4.5

---

##说明：
项目中的XY.OAuth.Web为测试项目，是ASP.NET MVC4搭建

初次开源，有不足的地方，欢迎大家指正。

[![QQ](http://im-img.qq.com/home/img/qlogo.png)](http://sighttp.qq.com/authd?IDKEY=e696ee8677e2ebf15105d237d8f62834bd6b47cace4daf6b)

[![微博](http://img.t.sinajs.cn/t6/style/images/global_nav/WB_logo.png)](http://weibo.com/178969939)


