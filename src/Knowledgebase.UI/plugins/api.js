
export const clientAppBaseUrl = (process.env.NODE_ENV == 'development' ? 'http://localhost:3000' : '');
export const apiServerBaseUrl = (process.env.NODE_ENV == 'development' ? '//localhost:58979' : '');

export default ({ app, $axios }, inject) => {
  const api = $axios.create()
  api.setBaseURL(apiServerBaseUrl + '/api/');
  api.defaults.withCredentials = true;

  // add logged-in user access token to the request
  api.interceptors.request.use(config => {
    //if (app.$auth && app.$auth.loggedIn)
    //  config.headers['common']['Authorization'] = app.$auth.$storage.getState('_token.auth0');
    return config;
  }, err => {
    return Promise.reject(err);
  })

  // handle response errors
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
