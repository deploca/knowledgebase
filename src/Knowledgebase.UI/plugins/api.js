import Vue from 'vue'
import axios from 'axios'

const api = axios.create({
  baseURL: (process.env.NODE_ENV == 'development' ? '//localhost:58979' : '') + '/api/',
});

export default ({ }, inject) => {
  api.interceptors.response.use(null, err => {
    err.serverStatusCode = err.response.status;
    err.serverMessage = err.response.data.message;

    err.showNotification = (context) => {
      var message = context.$t(`errors.${err.serverMessage}`)
      context.$bvToast.toast(message, {
        title: context.$t('errors.ERROR_HAPPEND'),
        variant: 'danger',
      })
    };

    if (err.serverStatusCode == 401) {
      context.$bvToast.toast(err.response.headers['WWW-Authenticate'], {
        title: context.$t('errors.ERROR_HAPPEND'),
        variant: 'danger',
      })
    } else if (err.serverStatusCode == 403) {
      context.$bvToast.toast(context.$t('errors.NOT_ALLOWED'), {
        title: context.$t('errors.ERROR_HAPPEND'),
        variant: 'danger',
      })
    }

    return Promise.reject(err);
  })
  inject('api', api)
}

export const setApiHeader = (key, value) => {
  api.defaults.headers.common[key] = value;
}
