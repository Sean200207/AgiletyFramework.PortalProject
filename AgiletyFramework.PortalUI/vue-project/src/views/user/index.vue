<template>
    <el-card class="box-card">
        <el-row>
            <el-form class="demo-form-inline" :inline="true">
                <el-form-item label="关键字" style="color: aliceblue">
                    <div class="block">
                        <el-input v-model="searchQuery.searchString" placeholder="用户名..." />
                    </div>
                </el-form-item>
                <el-form-item style="color: aliceblue">
                    <el-button type="primary" @click="userQuery">查询</el-button>
                </el-form-item>
                <el-form-item style="color: aliceblue">
                    <el-button type="primary" @click="addUser">新增</el-button>
                </el-form-item>
            </el-form>
        </el-row>
        <el-row>
            <el-table :data="tableData" style="width: 100%" :row-class-name="tableRowClassName">
                <el-table-column type="index" label="序号" width="60" />
                <el-table-column prop="name" label="用户名称" />
                <el-table-column prop="sexTypeDescription" label="性别">
                    <template #default="scope">
                        {{ getSexType(scope.row.sex) }}
                    </template>
                </el-table-column>
                <el-table-column prop="userType" label="类型">
                    <template #default="scope">
                        {{ getUserType(scope.row.userType) }}
                    </template>
                </el-table-column>
                <el-table-column prop="status" label="状态">
                    <template #default="scope">
                        {{ getStatus(scope.row.status) }}
                    </template>
                </el-table-column>
                <!-- <el-table-column prop="status" label="状态">
                    <template #default="scope">
                        <img style="width: 30px;height: 30px;" :src="getImageUrl(scope, row, imageurl)">
                    </template>
                </el-table-column> -->
                <el-table-column label="操作" width="500">
                    <template #default="scope">
                        <el-row class="mb-12">
                            <el-button size="small" @click="Show1" type="success">查看考情</el-button>
                            <el-button size="small" @click="Show1" type="info">查看工作日志</el-button>
                            <el-button size="small" @click="Show1" type="warning">发送通知</el-button>
                            <el-button size="small" @click="Show1" type="danger">职位调动</el-button>
                        </el-row>
                    </template>
                </el-table-column>
            </el-table>
            <div class="pagination">
                <el-pagination v-model:current-page="searchQuery.currentPage" v-model:page-size="searchQuery.pageSize"
                    :page-sizes="[10, 20, 30, 40]" :small="small" :disabled="disabled" :background="background"
                    layout="total,sizes, prev, pager, next,jumper" :total="searchQuery.recordCount" 
                    @size-change="userQuery" @current-change="userQuery">
                </el-pagination>
            </div>
        </el-row>
    </el-card>



    <!-- Dialog 对话框--新增用户专用 -->
    <el-dialog v-model="dialogVisible" title="新增用户" width="40%">
        <el-form :model="userForm" :rules="userRules" ref="userFormRef" label-vidth="120px">
            <el-form-item label="用户名称" prop="name">
                <el-input v-model="userForm.name" placeholder="用户名称" />
            </el-form-item>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="用户密码" prop="password">
                        <el-input type="password" v-model="userForm.password" placeholder="用户密码" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="确认密码" prop="confirmPassword">
                        <el-input type="password" v-model="userForm.confirmPassword" placeholder="确认密码" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="性别" prop="sex">
                        <el-radio-group v-model="userForm.sex">
                            <el-radio label=1>男性</el-radio>
                            <el-radio label=0>女性</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="是否启用" prop="isEnabled">
                        <el-switch v-model="userForm.isEnabled" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="用户头像" prop="imageUrl">
                            <el-upload class="avatar-uploader" :action="imageApiUrl" :show-file-list="false"
                            :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload">
                            <img style="width: 100px; height: 100px;" v-if="userForm.imageUrl" :src="baseImageUrl"
                            class="avatar" />
                            <el-icon v-else class="avatar-uploader-icon">
                                <Plus />
                            </el-icon>
                        </el-upload>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="手机号码" prop="mobile">
                        <el-input v-model.number="userForm.mobile" placeholder="手机号码" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="电话号码" prop="phone">
                        <el-input v-model.number="userForm.phone" placeholder="电话号码" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="qq号码" prop="qq">
                        <el-input v-model="userForm.qq" placeholder="qq号码" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="微信">
                        <el-input v-model="userForm.weChat" placeholder="微信" />
                    </el-form-item>
                </el-col>
            </el-row>

            <el-form-item label="邮箱地址" prop="email">
                <el-input v-model="userForm.email" placeholder="邮箱地址" />
            </el-form-item>

            <el-form-item label="详细地址">
                <el-input v-model="userForm.address" type="textarea" placeholder="详细地址" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="dialogVisible=false">
                    取消</el-button>
                <el-button type="primary" @click="subUseForm(userFormRef)">
                    确认
                </el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script setup>

import { apiUrl } from '../../common/index';
//import { nextTick, ref, onMounted, callwithAsyncErrorHandling} from 'vue';
import { nextTick, ref, onMounted } from 'vue';
//import axios from "axios";
import service from "../../common/apiRequest"
import { ElMessage } from 'element-plus'
//中文包
//import zhCn from "element-plus/lib/locale/lang/zh-cn"

//页面加载就要去获取数据，用作绑定页面
//列表数据绑定结果
const tableData = ref([])

//查询条件绑定对象
const searchQuery = ref({
    currentPage: 1,
    pageSize: 10,
    recordCount: 0,
    searchString: ""
})

//数据应该是请求webapi
//页面加载完毕--触发，加载数据
onMounted(async () => {

    //为tableData赋值---请求请求的时api
    //tableData.value = [{ "name": "admin" }, { "name": "Sean" }]
    //数据肯定不能在这里固定写死，应该去请求Api
    //const service = axios.create({ baseURL: "http://localhost:5224/" });
    // const response = await service.get("api/User/1/2");
    // let { success, message, data, oValue } = response.data;
    // if (success == true) {
    //     tableData.value = data.dataList;
    // }

    userQuery();
});

//查询功能--查询用户--请求api服务器
let userQuery = async () => {
    var url = `/api/User/${searchQuery.value.currentPage}/${searchQuery.value.pageSize}`;
    var searchString = searchQuery.value.searchString;
    if (searchString) {
        url = `/api/User/${searchQuery.value.currentPage}/${searchQuery.value.pageSize}/${searchString}`;
    }

    let reponse = await service.get(url);

    let { success, message, data, oValue } = reponse.data;
    
    if (success = true) {
        tableData.value = data.dataList;
        searchQuery.value.recordCount = data.recordCount;
    }
    else {
        alert(message);
    }
}

const getSexType = (sex) => {
    if (sex == 1) {
        return "男性";
    }
    else {
        return "女性";
    }
}

const getUserType = (userType) => {
    if (userType == 1) {
        return "管理员";
    }
    else {
        return "普通用户";
    }
}

const getStatus = (status) => {
    if (status == 1) {
        return "正常";
    }
    else if (status == 2) {
        return "已冻结";
    }
    else if (status == 3) {
        return "已删除";
    }
}

let baseImageUrl = ref();
const imageApiUrl = `${apiUrl()}api/File`; //用作上传图片的路径
const dialogVisible = ref(false);
const userFormRef = ref();
//新增用户，表单绑定数据
let userForm = ref({});

//弹出新增用户的弹框
const addUser = async () =>{
    dialogVisible.value = true;
    await nextTick();  //等待Dom加载完成--加载完成后, userFormRef就可以代表form了
    userFormRef.value.resetFields() //
}


//文件上传前触发的行为--验证一些文件，可以在这里完成
const beforeAvatarUpload = (uploadFile) => {
    console.log(uploadFile);
}

//文件上传成功后，api响应了结果了，触发这里?
const handleAvatarSuccess = (response,uploadFile,uploadFiles)=>{
    // console.log(response);
    // console.log(uploadFile);
    // console.log(uploadFiles);
    if(response.success){
        userForm.value.imageUrl = response.data;
    }
    baseImageUrl = `${apiUrl()}${response.data}`
}

//----------表单校验

//自定义验证密码
const checkPassword = (rule,value,callback) => {
    if(!value){
        return callback(new Error('请确认密码'))
    }
    if(value != userForm.value.password){
        return callback(new Error('两次密码不一致'))
    }
    callback();
}

//验证邮箱
const checkEmail = (rule,value,callback) => {
    if(value == ''){
        callback(new Error('请确认填写邮箱'))
    }else{
        if(value !== '请确认填写邮箱'){
            var reg = /^[A-za-z0-9\u4e00-\u9fa5]+@[a-zA-z0-9_-]+(\.[a-zA-z0-9_-]+)+$/;
            if(!reg.test(value)){
                callback(new Error('邮箱地址格式不对'))
            }
        }
        callback();
    }
    
}

//引用验证规则
const userRules = ref ({
    name: [{required: true,message: '请输入用户名', trigger: 'blur'}],
    password:[{required: true,message: '请输入密码', trigger: 'blur'}],
    confirmPassword: [{validator: checkPassword,trigger: 'blur'}],
    sex: [{required: true,message: '请选择性别', trigger: 'blur'}],
    isEnabled: [{required: true,message: '你确认是否启用', trigger: 'blur'}],
    imageUrl: [{required: true,message: '请选择头像', trigger: 'blur'}],
    mobile: [
        {required: true,message: '请输入手机号', trigger: 'blur'},
        {type: 'number',message: '手机号必须为数字',trigger: 'biur'}
    ],
    phone: [{type: 'number',message: '电话号码必须为数字', trigger: 'blur'}],
    email: [{validator: checkEmail,trigger: 'blur'}]
});

//提交新增用户信息
const subUseForm = async(uform) => {
    //这里开始触发校验规则
    await uform.validate(async(valid,fields) =>{
        //console.log(valid);
        if(valid){ //如果这里true  校验成功
            let reponse = await service.post(`/api/User`,userForm.value);
            //let reponse = await service.post(`/api/User`,{imageurl:"你好"});
            let {data,success,message} = reponse.data;
            if(success){
                userQuery();
                dialogVisible.value = false;
                ElMessage({
                    type: 'success',
                    message: message,
                })
            }else{
                ElMessageBox.alert(message);
            }
        }
    });
};
</script>