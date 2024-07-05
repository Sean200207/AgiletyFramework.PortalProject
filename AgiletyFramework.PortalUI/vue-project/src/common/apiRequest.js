

import axios from "axios";
import { apiUrl } from '../common/index'
import mainStore from '../stores/counter'

const service = axios.create({ baseURL: apiUrl() });


//请求拦截器--在axios 发起请求之前要做的事  aop思想
service.interceptors.request.use(config => {

    //可以在这里配置token
    var token = mainStore().accessToken;

    //这里就是配置axios 请求api的时候，带上token
    config.headers.Authorization = 'Bearer ' + token;
    return config;
});


//响应拦截器
service.interceptors.response.use(async response => {

    
    return response;
});

export default service;