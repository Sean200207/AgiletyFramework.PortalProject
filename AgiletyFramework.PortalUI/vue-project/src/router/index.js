import { createRouter, createWebHistory } from 'vue-router'
import viewIndex from '../views/index.vue'
import axios from '../common/apiRequest'
import mainStore from '../stores/counter'

var routeArray = [
  {
    path: '/',
    name: 'login',
    meta: { title: '登录' },
    component: () => import("../views/login/index.vue")
  },
  {
    path: '/home',
    name: 'home',
    component: viewIndex,
    children: [
      {
        path: '/index',
        name: 'index',
        meta: { title: '首页' },
        component: () => import("../views/home/index.vue")
      },
      // {
      //   path: '/user',
      //   name: 'user',
      //   meta: { title: '用户管理' },
      //   component: () => import("../views/user/index.vue")
      // },
      // {
      //   path: '/log',
      //   name: 'log',
      //   meta: { title: '日志列表' },
      //   component: () => import("../views/log/index.vue")
      // },
      // {
      //   path: '/menu',
      //   name: 'menu',
      //   meta: { title: '菜单管理' },
      //   component: () => import("../views/menu/index.vue")
      // }
    ]
  }
];
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routeArray
})

const SettingUserRouter = async () => {
  //用作保存路由数据的数组---来自于api接口 "Menu/GetRouteTreeSelectList"
  let routeData = [];
  //开始请求api 获取路由数据
  //let reponse = await axios.get("Menu/GetRouteTreeSelectList")
  let reponse = await axios.get("Menu");
  let { success, data } = reponse.data;
  if (success) {
    console.log("开始初始化路由信息");
    routeData = data;
    mainStore().initMenu(data);  //这里把路由信息保存到浏览器的本地
  }
  else {
    //console.error("动态路由数据错了~");
    routeData = [];
  }
  //开始配置路由信息
  //读取当前项目中Views文件夹下的vue后缀的文件
  const modules = import.meta.glob('../views/*/*.vue')

  //开始加载路由对应的Vue组件
  var initRoute = (routelist) =>{
    var returnRoteArray = [];
    routelist.forEach(item =>{
      let obj ={
        name: item.webUrlName,
        meta:{
          title: item.menuText,
          id: item.id
        },
        path:item.webUrl,
        component:modules[item.vueFilePath],
        children:[]
      };
      if(item.children && item.children.length > 0){
        obj.children = initRoute(item.children);
        returnRoteArray.push(obj);
      }
      else{
        returnRoteArray.push(obj);
      }
    });
    return returnRoteArray;
  }

  //调用方法，获取路由节点信息
  let treeRoutelist = initRoute(routeData);

  //开始动态添加路由节点-添加到全局状态
  treeRoutelist.forEach(item => {
    router.addRoute('home',item)
  });
};

//需要在beforeEach里面使用pinia，否则无法成功初始化
router.beforeEach(async (to, from, next) => {
  if (to.path != "/login") {  //login 不属于动态菜单和路由的数据
    await SettingUserRouter();
    //console.log(router.getRoutes());
  }
  // if (mainStore().$state.accessToken == "" || !mainStore().$state.accessToken) {
  //   if (to.path != "/login") {
  //     next("/login")
  //   }
  // }

  //由于统一配置了404页面的原因，这里要判断一下当前是否已经进入404页面
  if (to.name == "notfound") {
    //由于是在导航里动态添加的路由，刷新页面时无法读取（刷新页面时没有跳转所以没有触发导航机制）
    //所以要进行手动跳转到动态添加的路由，但是前提是跳转的path在路由中存在才行
    if (router.getRoutes().find(p => p.path == to.path)) {
      //经过一些列判断后没问题则跳转
      next(to.path)
      return;
    }
  }
  next()
})
export default router
