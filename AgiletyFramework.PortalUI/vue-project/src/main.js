//import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

// import './assets/main.css' //去掉默认样式
import './style.css'  //引入自定义样式

import ElementPlus from 'element-plus' //引入elementplus的组件
import 'element-plus/dist/index.css'  //引入elementplus的样式文件
import * as ElementPlusIconsVue from '@element-plus/icons-vue'  //图标

//需要npm install pinia-plugin-persist  --支持本地数据持久化保存
import piniaPluginPersist from 'pinia-plugin-persist'

const app = createApp(App)

//ElementPlus 图标文件生效
for(const [key,component] of Object.entries(ElementPlusIconsVue)){
    app.component(key,component)
}

app.use(createPinia().use(piniaPluginPersist));

app.use(router)

app.use(ElementPlus); //全局引入Elementplus

app.mount('#app')
