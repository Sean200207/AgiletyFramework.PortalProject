<!-- 这里是做布局-->

<template>
    <div id="module">
        <el-container>
            <!--菜单栏-->
            <navMenu />
            <el-container>
                <el-header>
                    <!-- 面包屑 -->
                    <breadcrumb />
                    <div v-if="mainStore().$state.user" style="display: inline-block;float: right;">
                        <div style="display: inline-block; overflow: hidden; height: 36px;">
                            <span>你好,欢迎你！{{ mainStore().$state.user.userName }}</span>
                        </div>
                    </div>
                    <!-- 头像 -->
                    <!-- <el-image style="width: 33px; height: 33px; padding: 10px;"
                        :src="imageUrlFilter(mainStore().$state.user.imageUrl)" :fit="fit" /> -->
                    <!-- 退出按钮 -->
                    <el-button class="exit" type="primary" @click="goBack()">退出</el-button>
                </el-header>
                <!-- 内容区域 -->
                <el-main>
                    <RouterView></RouterView>
                </el-main>
            </el-container>
        </el-container>
    </div>
</template>

<script setup>
import {
    Document,
    Menu as IconMenu,
    Location,
    Setting,
} from '@element-plus/icons-vue'

import navMenu from "../components/navMenu.vue"
import breadcrumb from "../components/breadcrumb.vue"
import mainStore from "../stores/counter";
import { apiUrl } from "../common/index";
import { computed } from "vue"

const handleOpen = (key, keyPath) => {
    console.log(key, keyPath)
}
const handleClose = (key, keyPath) => {
    console.log(key, keyPath)
}

//图片文件格式化
const imageUrlFilter = computed(() => item => {
    return `${apiUrl()}${item}`;
})

//退出  清除用户信息，跳转到登录页面
let goBack = () => {

    //清空保存在pinia中的数据
    mainStore().$patch({
        menulist:[],
        accessToken: null,
        refreshToken: null,
        user: null
    })

    //跳转到登录页
    router.push("/")
}

</script>