
<!--
        这个组件是用来封装左边菜单的部分-->
<template>
    <el-aside width="200px" class="aside">
        <el-menu :default-active="route.path" exact class="el-menu-vertical-demo" router>
            <el-menu-item class="menu" index="/index" style="padding: 0px;">
                <div style="width: 80%;padding-bottom: 30px;height: 85%;padding-left: 10px;">
                    <el-image id="myImage" style="width: 260px;" :src="zhaoxiImage" /> 图片
                </div>
            </el-menu-item>
            <el-menu-item class="menu" index="/index">
                <el-icon>
                    <component is="House"></component>
                </el-icon>
                <span>首页</span>
            </el-menu-item>
            <NavItem v-for="v in items" :key="v.webUrl" :item="v" :basePath="v.webUrl" />
        </el-menu>
    </el-aside>
</template>


<script setup>

import { reactive, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'; //导入路由
import NavItem from './navItem.vue' //子组件
import mainStore from '../stores/counter'
//import zhaoxiImage from '../assets/image/123.png'

const route = useRoute();

const items = computed(() => mainStore().$state.menulist);


// const items = reactive([
//     {
//         menuText: '系统日志',
//         webUrl: '/log',
//         icon: 'MoreFilled'
//     },
//     {
//         menuText: '用户管理',
//         webUrl: '/user',
//         icon: 'UserFilled'
//     },
//     {
//         menuText: '菜单管理',
//         webUrl: '/menu',
//         icon: 'Menu'
//     },
//     {
//         menuText: '系统管理',
//         webUrl: '/system',
//         icon: 'Grid',
//         child: [
//             { menuText: '系统日志', webUrl: '/system/log', icon: 'Setting' }
//         ]
//     }
// ])

</script>