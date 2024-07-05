<template>
    <!--无子级-->
    <el-menu-item class="menu" :index="basePath" v-if="!item.child || item.child.length <= 0">
        <le-icon>
            <component :is="item.icon"></component>
        </le-icon>
        <span>{{ item.menuText }}</span>
    </el-menu-item>
    <!--有子级-->
    <el-sub-menu class="menu" :index="basePath" v-else>
        <template #title>
            <el-icon>
                <component :is="item.icon"></component>
            </el-icon>
            <span>{{ item.menuText }}</span>
        </template>
        <NavItem v-for="sub in item.child" :key="sub.webUrl" :item="sub" :basePath="sub.webUrl" />
    </el-sub-menu>
</template>

<script setup>

import { ref, reactive } from 'vue'

const { item, basePath } = defineProps({
    item: {
        type: Object,
    },
    basePath: {
        type: String
    }
})
</script>