import { defineStore } from "pinia"



const store = defineStore('main', {

  //状态管理
  persist: {
    //开启
    enabled: true,
    strategies: [
      {
        //指定key，这个名称会在浏览器本地存储中生成对应的name
        key: "site",
        //自定义存储方式，默认是sessionStorage
        storage: localStorage,
        //要存储的对象，默认是所有
        //paths:["UserMenus"]
      }
    ]
  },
  state() {  //保存的数据
    return {
      menulist: [],
      accessToken: '',
      refreshToken: '',
      user: {}
    }
  },
  getters: {

  },
  actions: {
    //全局赋值菜单数据的方法
    initMenu(menu) {
      this.menulist = menu;
    }
  }
})

export default store
