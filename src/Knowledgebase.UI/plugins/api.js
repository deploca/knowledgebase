import Vue from 'vue'
import axios from 'axios'

const api = axios.create({
  baseURL: (process.env.NODE_ENV == 'development' ? '//localhost:58979' : '') + '/api/',
});

export default ({ app, notification }, inject) => {
  api.interceptors.response.use(null, err => {
    if (err.response.status == 401) {
      Vue.prototype.$notification.error({
        message: err.message,
        description: err.response.headers['WWW-Authenticate'],
      })
    } else if (err.response.status == 403) {
      Vue.prototype.$notification.error({
        message: err.message,
      })
    }
    return Promise.reject(err);
  })
  inject('api', api)
}

export const setApiHeader = (key, value) => {
  api.defaults.headers.common[key] = value;
}
