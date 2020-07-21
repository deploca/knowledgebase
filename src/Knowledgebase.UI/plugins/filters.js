import Vue from 'vue'

export default ({ app }, inject) => {
  const dateTimeHelpers = app.$dateTime

  Vue.filter('formatDate', (value) => {
    return value ? dateTimeHelpers.formatDate(value) : ''
  })

  Vue.filter('formatDateTime', (value) => {
    return value ? dateTimeHelpers.formatDateTime(value) : ''
  })
  
}
